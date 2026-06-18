using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using SalesManagerApp.Data;
using DatabaseLib.Table;
using SalesManagerApp.DBControl;

namespace SalesManagerApp.Test.UnitTest.DataManager
{
    [TestFixture]
    public class InventoryViewDataManagerTests
    {
        private InventoryViewDataManager _target;
        private string _originalConnectionString;
        private DbConnection _keepAliveConnection;

        [SetUp]
        public void SetUp()
        {
            _originalConnectionString = Properties.Settings.Default.DbConnectionString;

            // 共有キャッシュモードのインメモリDB設定
            string inMemoryConnectionString = "Data Source=SharedInventoryDB;Mode=Memory;Cache=Shared";
            Properties.Settings.Default.DbConnectionString = inMemoryConnectionString;

            _keepAliveConnection = new SqliteConnection(inMemoryConnectionString);
            _keepAliveConnection.Open();

            // ※実際の環境に合わせて、テーブル作成(Init処理)の呼び出しを追加してください
            // var db = new DatabaseLib.DataBaseControl(inMemoryConnectionString); db.Init();

            _target = new InventoryViewDataManager();
        }

        [TearDown]
        public void TearDown()
        {
            Properties.Settings.Default.DbConnectionString = _originalConnectionString;
            if (_keepAliveConnection != null)
            {
                _keepAliveConnection.Close();
                _keepAliveConnection.Dispose();
            }
        }

        private void PrepareTestData()
        {
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
                new ProductMaster(){ Id = 1, Name = "商品A", UnitPrice = 1500, CategoryId = 1, Created_at = DateTime.Now, },
                new ProductMaster(){ Id = 2, Name = "商品B", UnitPrice = 500, CategoryId = 2, Created_at = DateTime.Now, },
                new ProductMaster(){ Id = 3, Name = "商品C", UnitPrice = 2000, CategoryId = 1, Created_at = DateTime.Now, }
            };
            var pro_ctrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            pro_ctrl.SetProductMaster(product);

            //販売情報登録
            var sales = new List<SalesResult>()
            {
                new SalesResult(){Id = 1, StoreId = 1, ProductId = 1, Quantity = 10, SaleDate = new DateTime(2026,4,4), SalesAmount = 1000 },
                new SalesResult(){Id = 2, StoreId = 1, ProductId = 1, Quantity = 5, SaleDate = new DateTime(2026,4,6), SalesAmount = 500 },
                new SalesResult(){Id = 3, StoreId = 1, ProductId = 1, Quantity = 20, SaleDate = new DateTime(2026,4,19), SalesAmount = 2000 },
                new SalesResult(){Id = 4, StoreId = 2, ProductId = 2, Quantity = 5, SaleDate = new DateTime(2026,3,4), SalesAmount = 500 },
                new SalesResult(){Id = 5, StoreId = 2, ProductId = 2, Quantity = 50, SaleDate = new DateTime(2026,4,4), SalesAmount = 5000 },
            };
            var sale_ctrl = new SalesResultControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            sale_ctrl.SetSalesResult(sales);

            //在庫情報登録
            var inventoryCtrl = new InventoryInfoControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);

            // ダミーのデータ登録 (InventoryInfoにナビゲーションプロパティをセットした状態を構築)
            // ※実際には各コントロールを用いて正規の手順でDBへ登録してください。
            var testInventories = new List<InventoryInfo>
            {
                // 商品A: 販売実績あり（複数）
                new InventoryInfo(){ Stock = 10, StoreId = 1, ProductId = 1, },
                // 商品B: 販売実績あり
                new InventoryInfo(){ Stock = 20, StoreId = 2, ProductId = 2, },
                // 商品C: 販売実績なし
                new InventoryInfo(){ Stock = 50, StoreId = 1, ProductId = 3, },
                // 重複データ確認用 (東京本店 / 商品A / 区分1)
                new InventoryInfo(){ Stock = 5, StoreId = 1, ProductId = 1, },
            };

            // データセット
            inventoryCtrl.SetInventoryInfo(testInventories);
        }

        [Test]
        public void TS018_GetNameData_重複が排除されたマスタ名称のリストが取得できること()
        {
            // Arrange
            PrepareTestData();

            // Act
            bool result = _target.GetNameData(out var lstStore, out var lstProduct, out var lstCategory);

            // Assert
            Assert.That(result, Is.True);

            Assert.That(lstStore.Count, Is.EqualTo(2));
            Assert.That(lstStore, Contains.Item("東京本店"));
            Assert.That(lstStore, Contains.Item("大阪支店"));

            Assert.That(lstProduct.Count, Is.EqualTo(3));
            Assert.That(lstProduct, Contains.Item("商品A"));
            Assert.That(lstProduct, Contains.Item("商品B"));
            Assert.That(lstProduct, Contains.Item("商品C"));

            Assert.That(lstCategory.Count, Is.EqualTo(2));
            Assert.That(lstCategory, Contains.Item("区分1"));
            Assert.That(lstCategory, Contains.Item("区分2"));
        }

        [Test]
        public void TS019_GetNameData_データが0件の場合は空のリストが取得できること()
        {
            // Arrange (データ登録なし)

            // Act
            bool result = _target.GetNameData(out var lstStore, out var lstProduct, out var lstCategory);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(lstStore, Is.Empty);
            Assert.That(lstProduct, Is.Empty);
            Assert.That(lstCategory, Is.Empty);
        }

        [Test]
        public void TS020_GetInventoryViewData_在庫金額と最終販売日が正しく計算されること()
        {
            // Arrange
            PrepareTestData();

            // Act
            bool result = _target.GetInventoryViewData(out var detailData, null);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(detailData.Count, Is.EqualTo(3)); // 全3件

            // 商品A (東京本店) の計算確認
            var dataA = detailData.First(d => d.ProductName == "商品A" && d.Stock == 5);
            Assert.That(dataA.StockValue, Is.EqualTo(7500), "在庫5 * 単価1500");
            Assert.That(dataA.LastSalesDate, Is.EqualTo(new DateTime(2026, 4, 19)), "最も新しい販売日が取得されること");
        }

        [Test]
        public void TS021_GetInventoryViewData_販売実績がない商品の最終販売日はMinValueになること()
        {
            // Arrange
            PrepareTestData();

            // Act
            _target.GetInventoryViewData(out var detailData, null);

            // Assert
            var dataB = detailData.First(d => d.ProductName == "商品C");
            Assert.That(dataB.StockValue, Is.EqualTo(100000), "在庫50 * 単価2000");
            Assert.That(dataB.LastSalesDate, Is.EqualTo(DateTime.MinValue), "販売実績がない場合はMinValueであること");
        }

        [Test]
        public void TS022_GetInventoryViewData_filterを指定した場合絞り込みが適用されること()
        {
            // Arrange
            PrepareTestData();
            var filter = new FilterInfo(
                storelist: new List<string> { "大阪支店" },
                productlist: new List<string>(),
                categorylist: new List<string>(),
                start: null,
                end: null
            );

            // Act
            bool result = _target.GetInventoryViewData(out var detailData, filter);

            // Assert
            Assert.That(result, Is.True);
            // ※InventoryInfoControl側のFilter実装が正しく機能している前提
            Assert.That(detailData.Count, Is.EqualTo(1));
            Assert.That(detailData[0].StoreName, Is.EqualTo("大阪支店"));
        }

        [Test]
        public void TS023_GetNameData_例外発生時はfalseが返ること()
        {
            // Arrange
            Properties.Settings.Default.DbConnectionString = "Data Source=InvalidPath:::;Version=3;";
            var errorTarget = new InventoryViewDataManager();

            // Act
            bool result = errorTarget.GetNameData(out var lstStore, out var lstProduct, out var lstCategory);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(lstStore, Is.Empty);
        }

        [Test]
        public void TS024_GetInventoryViewData_例外発生時はfalseが返ること()
        {
            // Arrange
            Properties.Settings.Default.DbConnectionString = "Data Source=InvalidPath:::;Version=3;";
            var errorTarget = new InventoryViewDataManager();

            // Act
            bool result = errorTarget.GetInventoryViewData(out var detailData, null);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(detailData, Is.Empty);
        }
    }
}