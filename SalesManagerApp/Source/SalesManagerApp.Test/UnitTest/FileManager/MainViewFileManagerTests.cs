using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms; // Application.StartupPath のため必要
using SalesManagerApp.FileIO;
using SalesManagerApp.Data;
using FileControlLib;

namespace SalesManagerApp.Test.UnitTest.FileManager
{
    [TestFixture]
    public class MainViewFileManagerTests
    {
        private Mock<IFileProcessor> _mockFileProcessor;
        private MainViewFileManager _target;

        [SetUp]
        public void SetUp()
        {
            // DIにより、外部ファイルアクセスを完全にモック化
            _mockFileProcessor = new Mock<IFileProcessor>();
            _target = new MainViewFileManager(_mockFileProcessor.Object);
        }

        [Test]
        public void TS032_ReadFile_ProductFileData_読込成功時にtrueとデータが返ること()
        {
            // Arrange
            string dummyPath = "dummy_product.csv";
            var expectedData = new List<ProductFileData>
            {
                new ProductFileData { ProductId = 1, ProductName = "商品A", UnitPrice = 100, Category = "区分1" }
            };

            // out引数と戻り値の振る舞いを設定
            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out expectedData))
                .Returns(true);

            // Act
            bool result = _target.ReadFile(dummyPath, out List<ProductFileData> actualData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(actualData.Count, Is.EqualTo(1));
            Assert.That(actualData[0].ProductName, Is.EqualTo("商品A"));
        }

        [Test]
        public void TS033_ReadFile_ProductFileData_例外発生時に再スローされること()
        {
            // Arrange
            string dummyPath = "error_product.csv";
            List<ProductFileData> dummyData;

            // 例外をスローするように設定
            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out dummyData))
                .Throws(new InvalidOperationException("File Read Error"));

            // Act & Assert
            // 【修正点】out _ ではなく、out List<ProductFileData> _ と型を明示してオーバーロードを特定させる
            var ex = Assert.Throws<InvalidOperationException>(() => _target.ReadFile(dummyPath, out List<ProductFileData> _));
            Assert.That(ex.Message, Is.EqualTo("File Read Error"));
        }

        [Test]
        public void TS034_ReadFile_InventoryFileData_読込成功時にtrueとデータが返ること()
        {
            // Arrange
            string dummyPath = "dummy_inventory.csv";
            var expectedData = new List<InventoryFileData>
            {
                new InventoryFileData { StoreId = 1, ProductId = 10, Stock = 50 }
            };

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out expectedData))
                .Returns(true);

            // Act
            bool result = _target.ReadFile(dummyPath, out List<InventoryFileData> actualData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(actualData.Count, Is.EqualTo(1));
            Assert.That(actualData[0].Stock, Is.EqualTo(50));
        }

        [Test]
        public void TS035_ReadFile_SaleFileData_読込成功時にtrueとデータが返ること()
        {
            // Arrange
            string dummyPath = "dummy_sale.csv";
            var expectedData = new List<SaleFileData>
            {
                new SaleFileData { StoreId = 1, ProductId = 10, Quantity = 5, SaleDate = new DateTime(2026, 6, 18) }
            };

            _mockFileProcessor
                .Setup(m => m.ReadFile(dummyPath, out expectedData))
                .Returns(true);

            // Act
            bool result = _target.ReadFile(dummyPath, out List<SaleFileData> actualData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(actualData.Count, Is.EqualTo(1));
            Assert.That(actualData[0].Quantity, Is.EqualTo(5));
        }

        [Test]
        public void TS036_SaveFile_生成されたパスで保存処理が呼ばれること()
        {
            // Arrange
            var inputData = new List<CategoryMasterData>
            {
                new CategoryMasterData { ID = 1, CategoryName = "テスト区分" }
            };

            // プロセッサの拡張子を設定（例: .csv）
            _mockFileProcessor.SetupGet(m => m.Extension).Returns(".csv");

            // 保存先パスが期待通りになるかを計算
            string expectedPath = Path.Combine(Application.StartupPath, "data.csv");

            // 期待するパスとデータで呼び出された場合のみ true を返すように設定
            _mockFileProcessor
                .Setup(m => m.SaveFile(expectedPath, inputData))
                .Returns(true);

            // Act
            bool result = _target.SaveFile(inputData);

            // Assert
            Assert.That(result, Is.True, "正しいパスとデータでSaveFileが呼び出されればtrueになること");

            // 念のため、特定の引数で確実に呼び出されたかをVerifyで検証
            _mockFileProcessor.Verify(m => m.SaveFile(expectedPath, inputData), Times.Once);
        }
    }
}