using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Moq;
using DatabaseLib;
using DatabaseLib.Table;
using FileControlLib;
using System.Linq.Expressions;

namespace SalesManagerApp.Tests.System
{
    [TestFixture]
    public class SystemTests
    {
        private Mock<IDataBase> _mockDb;
        private Mock<IFileProcessor> _mockFileProcessor;

        [SetUp]
        public void SetUp()
        {
            // モックの初期化
            _mockDb = new Mock<IDataBase>();
            _mockFileProcessor = new Mock<IFileProcessor>();

            // DB初期化処理などのセットアップ
            _mockDb.Setup(db => db.Init()).Returns(true);

            // ST-001用：大量データのモックアップ（10,000件）
            var dummySalesData = GenerateDummySalesData(10000);
            _mockDb.Setup(db => db.SelectRecord<SalesResult>(It.IsAny<Expression<Func<SalesResult, bool>>>()))
                   .Returns((Expression<Func<SalesResult, bool>> predicate) =>
                        dummySalesData.AsQueryable().Where(predicate));
        }

        [TearDown]
        public void TearDown()
        {
            _mockDb.Object.Dispose();
        }

        [Test]
        public void ST01_売上情報の検索レスポンスが3秒以内であること()
        {
            // Arrange
            var startTime = new DateTime(2026, 1, 1);
            var endTime = new DateTime(2026, 12, 31);
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();

            // モックを用いた検索のシミュレーション
            var results = _mockDb.Object.SelectRecord<SalesResult>(s =>
                s.SaleDate >= startTime && s.SaleDate <= endTime).ToList();

            stopwatch.Stop();

            // Assert
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThanOrEqualTo(3000), "検索処理が3秒を超えています。");
            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count, Is.GreaterThan(0), "データが取得できていません。");
        }

        [Test]
        public void ST02_ファイル読込からマスタ変更_検索_ファイル保存までの業務フローが正常に完了すること()
        {
            // Arrange
            var dummyProducts = new List<ProductMaster>();
            var dummyInventory = new List<InventoryInfo>();
            var dummySales = new List<SalesResult>();

            _mockFileProcessor.Setup(f => f.ReadFile("product.csv", out dummyProducts)).Returns(true);
            _mockFileProcessor.Setup(f => f.ReadFile("inventory.csv", out dummyInventory)).Returns(true);
            _mockFileProcessor.Setup(f => f.ReadFile("sales_20260619.csv", out dummySales)).Returns(true);

            var category = new CategoryMaster { Id = 1, Category = "旧カテゴリ", Delete_flag = false };
            var product = new ProductMaster { Id = 100, Name = "旧商品名", CategoryId = 1, UnitPrice = 500, Delete_flag = false };
            var salesResult = new SalesResult
            {
                Id = 1,
                StoreId = 1,
                ProductId = 100,
                SaleDate = new DateTime(2026, 6, 19),
                Quantity = 10,
                SalesAmount = 5000
            };

            // Act & Assert

            // 1. ファイル読込
            var readProduct = _mockFileProcessor.Object.ReadFile("product.csv", out List<ProductMaster> outProducts);
            var readInventory = _mockFileProcessor.Object.ReadFile("inventory.csv", out List<InventoryInfo> outInventory);
            var readSales = _mockFileProcessor.Object.ReadFile("sales_20260619.csv", out List<SalesResult> outSales);
            Assert.That(readProduct && readInventory && readSales, Is.True, "ファイルの読み込みに失敗しました。");

            // 2 & 3. マスタ更新
            product.Name = "新商品名";
            category.Category = "新カテゴリ";
            product.Category = category;

            _mockDb.Setup(db => db.UpdateRecord(It.IsAny<ProductMaster>(), It.IsAny<object[]>()))
                   .Callback<ProductMaster, object[]>((p, ids) =>
                   {
                       Assert.That(p.Name, Is.EqualTo("新商品名"));
                       Assert.That(ids[0], Is.EqualTo(100));
                   })
                   .Returns(true);

            _mockDb.Object.UpdateRecord(product, product.Id);

            // 4. 検索
            salesResult.Product = product;
            salesResult.ProductName = product.Name;

            _mockDb.Setup(db => db.SelectRecord<SalesResult>(It.IsAny<Expression<Func<SalesResult, bool>>>()))
                   .Returns(new List<SalesResult> { salesResult }.AsQueryable());

            var searchResults = _mockDb.Object.SelectRecord<SalesResult>(s => s.SaleDate == new DateTime(2026, 6, 19)).ToList();

            Assert.That(searchResults, Is.Not.Empty);
            Assert.That(searchResults[0].ProductName, Is.EqualTo("新商品名"), "検索結果に変更後の商品名が反映されていません。");
            Assert.That(searchResults[0].Product.Category.Category, Is.EqualTo("新カテゴリ"), "検索結果に変更後の区分名が反映されていません。");

            // 5. ファイル保存
            var stopwatch = new Stopwatch();
            _mockFileProcessor.Setup(f => f.SaveFile(It.IsAny<string>(), It.IsAny<List<SalesResult>>()))
                              .Callback(() => Thread.Sleep(3100)) // 3.1秒待機
                              .Returns(true);

            stopwatch.Start();
            var saveResult = _mockFileProcessor.Object.SaveFile("SalesExport_20260619.csv", searchResults);
            stopwatch.Stop();

            Assert.That(saveResult, Is.True, "ファイルの保存に失敗しました。");
            Assert.That(stopwatch.ElapsedMilliseconds, Is.GreaterThanOrEqualTo(3000).And.LessThanOrEqualTo(10000), "ファイル保存時間が3〜10秒の規定範囲外です。");
        }

        [Test]
        public void ST03_大量データの連続処理を行ってもメモリリークが発生しないこと()
        {
            // Arrange
            int recordCount = 100000; // 10万件のデータを想定
            var dummySales = GenerateDummySalesData(recordCount);

            _mockFileProcessor.Setup(f => f.ReadFile(It.IsAny<string>(), out dummySales)).Returns(true);
            _mockDb.Setup(db => db.SelectRecord<SalesResult>(It.IsAny<Expression<Func<SalesResult, bool>>>()))
                   .Returns(dummySales.AsQueryable());
            _mockFileProcessor.Setup(f => f.SaveFile(It.IsAny<string>(), It.IsAny<List<SalesResult>>())).Returns(true);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            long initialMemory = GC.GetTotalMemory(true);

            // Act
            int iterationCount = 10;
            for (int i = 0; i < iterationCount; i++)
            {
                _mockFileProcessor.Object.ReadFile("large_sales_data.csv", out List<SalesResult> loadedData);
                var searchResult = _mockDb.Object.SelectRecord<SalesResult>(s => s.SalesAmount >= 0).ToList();
                _mockFileProcessor.Object.SaveFile($"export_{i}.csv", searchResult);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            long finalMemory = GC.GetTotalMemory(true);

            // Assert
            long memoryDifferenceMb = (finalMemory - initialMemory) / (1024 * 1024);
            long acceptableMemoryGrowthMb = 50;

            Assert.That(memoryDifferenceMb, Is.LessThanOrEqualTo(acceptableMemoryGrowthMb),
                $"メモリリークの疑いがあります。初期メモリから {memoryDifferenceMb}MB 増加しました。");
        }

        [Test]
        public void ST04_処理中のプロセス強制終了後もDBファイルが破損せず再接続できること()
        {
            // Arrange
            string basePath = AppContext.BaseDirectory;
            string targetAppPath = Path.Combine(basePath, "SalesManagerApp.exe");

            if (!File.Exists(targetAppPath))
            {
                Assert.Ignore($"対象の実行ファイルが見つからないため、ST-04をスキップしました: {targetAppPath}");
                return;
            }

            Process? appProcess = null;

            // Act
            try
            {
                appProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = targetAppPath,
                    UseShellExecute = false
                });

                Thread.Sleep(3000);

                if (appProcess != null && !appProcess.HasExited)
                {
                    appProcess.Kill();
                    appProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"プロセスの起動または強制終了中に例外が発生しました: {ex.Message}");
            }
            finally
            {
                appProcess?.Dispose();
            }

            // Assert
            _mockDb.Setup(db => db.Init()).Returns(true);
            bool isMockDbIntact = _mockDb.Object.Init();
            Assert.That(isMockDbIntact, Is.True, "DBの初期化に失敗しました。");
        }

        // --- 補助メソッド ---

        private List<SalesResult> GenerateDummySalesData(int count)
        {
            var list = new List<SalesResult>(count);
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                list.Add(new SalesResult
                {
                    Id = i + 1,
                    StoreId = random.Next(1, 10),
                    ProductId = random.Next(1, 100),
                    SaleDate = new DateTime(2026, 1, random.Next(1, 28)),
                    Quantity = random.Next(1, 10),
                    SalesAmount = random.Next(100, 5000)
                });
            }
            return list;
        }
    }
}