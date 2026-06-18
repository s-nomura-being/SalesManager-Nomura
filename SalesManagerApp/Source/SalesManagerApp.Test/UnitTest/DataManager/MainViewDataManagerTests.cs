using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common; // DbConnection用
using Microsoft.Data.Sqlite; // または System.Data.SQLite
using SalesManagerApp.Data;
using DatabaseLib.Table;
using SalesManagerApp.DBControl;

namespace SalesManagerApp.Test.UnitTest.DataManager
{
    [TestFixture]
    public class MainViewDataManagerTests
    {
        private MainViewDataManager _target;
        private string _originalConnectionString;

        // インメモリDBの揮発を防ぐための維持用コネクション
        private DbConnection _keepAliveConnection;

        [SetUp]
        public void SetUp()
        {
            // 1. 既存の接続文字列を退避
            _originalConnectionString = Properties.Settings.Default.DbConnectionString;

            // 2. 共有キャッシュモードのインメモリDB接続文字列を設定
            // ※Microsoft.Data.Sqlite の場合: "Data Source=SharedTestDB;Mode=Memory;Cache=Shared"
            // ※System.Data.SQLite の場合: "FullUri=file::memory:?cache=shared;"
            string inMemoryConnectionString = "Data Source=SharedTestDB;Mode=Memory;Cache=Shared";
            Properties.Settings.Default.DbConnectionString = inMemoryConnectionString;

            // 3. テスト中、DBが破棄されないようにConnectionを開きっぱなしにする
            _keepAliveConnection = new SqliteConnection(inMemoryConnectionString);
            _keepAliveConnection.Open();

            // テスト対象のインスタンス生成
            _target = new MainViewDataManager();
        }

        [TearDown]
        public void TearDown()
        {
            // 接続文字列を元に戻す
            Properties.Settings.Default.DbConnectionString = _originalConnectionString;

            // 維持用コネクションを閉じる（ここでインメモリDBのデータが破棄・解放されます）
            if (_keepAliveConnection != null)
            {
                _keepAliveConnection.Close();
                _keepAliveConnection.Dispose();
            }
        }

        [Test]
        public void TS001_GetMainViewData_商品データが存在する場合計算とソートが行われること()
        {
            // Arrange
            // インメモリDBに対して前提データを投入する
            var productCtrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);

            //店舗、区分マスタ登録
            var store = new List<StoreMaster>()
            {
                new StoreMaster(){ Id = 1, Name = "東京本店", Created_at = DateTime.Now, },
                new StoreMaster(){ Id = 2, Name = "大阪支店", Created_at = DateTime.Now, },
            };
            var category = new List<CategoryMaster>()
            {
                new CategoryMaster(){ Id = 1, Category = "区分1", Created_at = DateTime.Now, },
                new CategoryMaster(){ Id = 2, Category = "区分2", Created_at = DateTime.Now, },
            };
            var store_ctrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            var cate_ctrl = new CategoryMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            store_ctrl.SetStoreMaster(store);
            cate_ctrl.SetCategoryMaster(category);

            //商品マスタ登録
            var product = new List<ProductMaster>()
            {
                new ProductMaster(){ Id = 1, Name = "商品A", UnitPrice = 200, CategoryId = 1, Created_at = DateTime.Now, },
                new ProductMaster(){ Id = 2, Name = "商品B", UnitPrice = 100, CategoryId = 2, Created_at = DateTime.Now, }
            };
            var pro_ctrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            pro_ctrl.SetProductMaster(product);

            //販売情報登録
            var sales = new List<SalesResult>()
            {
                new SalesResult(){Id = 1, StoreId = 1, ProductId = 1, Quantity = 10, SaleDate = new DateTime(2026,4,4), SalesAmount = 1000 },
                new SalesResult(){Id = 2, StoreId = 2, ProductId = 2, Quantity = 5, SaleDate = new DateTime(2026,4,4), SalesAmount = 500 },
            };
            var sale_ctrl = new SalesResultControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
           sale_ctrl.SetSalesResult(sales);

            //在庫情報登録
            var inv = new List<InventoryInfo>()
            {
                new InventoryInfo(){ StoreId = 1, ProductId = 1, Stock = 20 },
                new InventoryInfo(){ StoreId = 2, ProductId = 2, Stock = 10 },
            };
            var inv_ctrl = new InventoryInfoControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            inv_ctrl.SetInventoryInfo(inv);

            // Act
            bool result = _target.GetMainViewData(out var lastWeekData, out var stockData);

            // Assert
            Assert.That(result, Is.True, "データの取得に成功すること");

            // 売上データの検証 (商品ID昇順でソートされていること)
            Assert.That(lastWeekData.Count, Is.EqualTo(2), "商品数分のデータが作成されること");
            Assert.That(lastWeekData[0].ProductID, Is.EqualTo(1), "商品IDの昇順でソートされること");
            Assert.That(lastWeekData[1].ProductID, Is.EqualTo(2));

            // 累計金額・在庫の計算確認
            Assert.That(lastWeekData[0].TotalSales, Is.EqualTo(2000), "10個 * 200円 = 2000円であること");
            Assert.That(lastWeekData[0].TotalStock, Is.EqualTo(20), "総在庫数が一致すること");
        }

        [Test]
        public void TS002_GetMainViewData_商品データが0件の場合falseが返却されること()
        {
            // Arrange
            // インメモリDBは空の状態のまま実行する

            // Act
            bool result = _target.GetMainViewData(out var lastWeekData, out var stockData);

            // Assert
            Assert.That(result, Is.False, "0件の場合はfalseを返すこと");
            Assert.That(lastWeekData, Is.Empty, "リストは空であること");
            Assert.That(stockData, Is.Empty, "リストは空であること");
        }

        [Test]
        public void TS003_GetMainViewData_例外発生時にExceptionがスローされること()
        {
            // Arrange
            // 意図的に不正な接続文字列を設定して、内部のDBアクセスで例外を発生させる
            Properties.Settings.Default.DbConnectionString = "Data Source=Invalid:::Path;Version=3;";
            var errorTarget = new MainViewDataManager();

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => errorTarget.GetMainViewData(out _, out _));
            Assert.That(ex.Message, Is.EqualTo("表示用データの作成に失敗"), "キャッチされた例外が指定のメッセージで再スローされること");
        }

        [Test]
        public void TS004_SetProductMasterFileData_未登録の区分が含まれる場合DB処理が実行されること()
        {
            // Arrange
            var fileDataList = new List<ProductFileData>
            {
                new ProductFileData { ProductId = 1, ProductName = "テスト商品", UnitPrice = 100, Category = "新規区分" }
            };

            // Act
            bool result = _target.SetProductMasterFileData(fileDataList);

            // Assert
            Assert.That(result, Is.True, "マスタの登録に成功すること");

            // テスト用インメモリDBに区分と商品が正しく登録されたかを検証
            var categoryCtrl = new CategoryMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            categoryCtrl.GetCategoryMaster(out var categoryData);
            Assert.That(categoryData.Any(c => c.Category == "新規区分"), Is.True, "新規区分がDBに登録されていること");
        }

        [Test]
        public void TS005_SetInventoryFileData_在庫ファイルデータが正しくDBに保存されること()
        {
            // Arrange
            //店舗、商品、区分マスタ登録
            //店舗、区分マスタ登録
            var store = new List<StoreMaster>()
            {
                new StoreMaster(){ Id = 1, Name = "東京本店", Created_at = DateTime.Now, },
                new StoreMaster(){ Id = 2, Name = "大阪支店", Created_at = DateTime.Now, },
            };
            var category = new List<CategoryMaster>()
            {
                new CategoryMaster(){ Id = 1, Category = "区分1", Created_at = DateTime.Now, },
                new CategoryMaster(){ Id = 2, Category = "区分2", Created_at = DateTime.Now, },
            };
            var store_ctrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            var cate_ctrl = new CategoryMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            store_ctrl.SetStoreMaster(store);
            cate_ctrl.SetCategoryMaster(category);

            //商品マスタ登録
            var product = new List<ProductMaster>()
            {
                new ProductMaster(){ Id = 1, Name = "商品A", UnitPrice = 200, CategoryId = 1, Created_at = DateTime.Now, },
                new ProductMaster(){ Id = 2, Name = "商品B", UnitPrice = 100, CategoryId = 2, Created_at = DateTime.Now, }
            };
            var pro_ctrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            pro_ctrl.SetProductMaster(product);

            var fileDataList = new List<InventoryFileData>
            {
                new InventoryFileData { StoreId = 1, ProductId = 1, Stock = 50 },
                new InventoryFileData { StoreId = 2, ProductId = 2, Stock = 100 },
            };

            // Act
             bool result = _target.SetInventoryFileData(fileDataList);

            // Assert
            Assert.That(result, Is.True, "在庫情報の登録に成功すること");

            // インメモリDBに保存された内容を直接検証
            var inventoryCtrl = new InventoryInfoControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            inventoryCtrl.GetInventoryInfo(out var inventories);
            Assert.That(inventories.Any(i => i.ProductId == 100 && i.Stock == 50), Is.True);
        }

        [Test]
        public void TS006_GetMainViewData_先週分の売上データのみが集計されること()
        {
            // Arrange
            var productCtrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);

            // 基準日を計算（テスト実行日ベース）
            DateTime today = DateTime.Today;
            DateTime lastWeek = today.AddDays(-7);
            DateTime twoWeeksAgo = today.AddDays(-14);
            DateTime future = today.AddDays(1); // 今週または未来のデータ

            //店舗、区分マスタ登録
            var store = new List<StoreMaster>()
            {
                new StoreMaster(){ Id = 1, Name = "東京本店", Created_at = DateTime.Now, },
            };
            var category = new List<CategoryMaster>()
            {
                new CategoryMaster(){ Id = 1, Category = "区分1", Created_at = DateTime.Now, },
            };
            var store_ctrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            var cate_ctrl = new CategoryMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            store_ctrl.SetStoreMaster(store);
            cate_ctrl.SetCategoryMaster(category);

            //商品マスタ登録
            var product = new List<ProductMaster>()
            {
                new ProductMaster(){ Id = 1, Name = "期間テスト商品", UnitPrice = 100, CategoryId = 1, Created_at = DateTime.Now, },
            };
            var pro_ctrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            pro_ctrl.SetProductMaster(product);

            //販売情報登録
            var sales = new List<SalesResult>()
            {
                new SalesResult(){Id = 1, StoreId = 1, ProductId = 1, Quantity = 10, SaleDate = twoWeeksAgo, SalesAmount = 1000 },
                new SalesResult(){Id = 2, StoreId = 1, ProductId = 1, Quantity = 5, SaleDate = lastWeek, SalesAmount = 500 },
                new SalesResult(){Id = 3, StoreId = 1, ProductId = 1, Quantity = 20, SaleDate = future, SalesAmount = 2000 },
            };
            var sale_ctrl = new SalesResultControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            sale_ctrl.SetSalesResult(sales);

            //在庫情報登録
            var inv = new List<InventoryInfo>()
            {
                new InventoryInfo(){ StoreId = 1, ProductId = 1, Stock = 20 },
            };
            var inv_ctrl = new InventoryInfoControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            inv_ctrl.SetInventoryInfo(inv);

            // Act
            bool result = _target.GetMainViewData(out var lastWeekData, out var stockData);

            // Assert
            Assert.That(result, Is.True, "データの取得に成功すること");
            Assert.That(lastWeekData.Count, Is.EqualTo(1));
            Assert.That(lastWeekData[0].LastWeekSales, Is.EqualTo(5), "先週の販売数のみが集計されること");
            Assert.That(lastWeekData[0].TotalSales, Is.EqualTo(500), "先週の販売数に基づく売上金額が計算されること");
        }
    }
}