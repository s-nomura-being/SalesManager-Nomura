using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using Microsoft.Data.Sqlite; // または System.Data.SQLite
using SalesManagerApp.Data;
using DatabaseLib.Table;
using SalesManagerApp.DBControl;

namespace SalesManagerApp.Test.UnitTest.DataManager
{
    [TestFixture]
    public class TotalSalesViewDataManagerTests
    {
        private TotalSalesViewDataManager _target;
        private string _originalConnectionString;
        private DbConnection _keepAliveConnection;

        [SetUp]
        public void SetUp()
        {
            _originalConnectionString = Properties.Settings.Default.DbConnectionString;

            // インメモリDB(共有キャッシュ)の設定
            string inMemoryConnectionString = "Data Source=SharedTotalSalesDB;Mode=Memory;Cache=Shared";
            Properties.Settings.Default.DbConnectionString = inMemoryConnectionString;

            _keepAliveConnection = new SqliteConnection(inMemoryConnectionString);
            _keepAliveConnection.Open();

            // ※実際の環境に合わせて、テーブル作成(Init処理)の呼び出しを追加してください
            // var db = new DatabaseLib.DataBaseControl(inMemoryConnectionString); db.Init();

            _target = new TotalSalesViewDataManager();

            // テストデータの準備
            PrepareTestData();
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
                new ProductMaster(){ Id = 1, Name = "商品A", UnitPrice = 100, CategoryId = 1, Created_at = DateTime.Now, },
            };
            var pro_ctrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            pro_ctrl.SetProductMaster(product);
            //販売情報登録
            var sales = new List<SalesResult>()
            {
                // 2025年 12月
                new SalesResult { Id = 1, StoreId = 1, ProductId = 1, SaleDate = new DateTime(2025, 12, 10), Quantity = 1, SalesAmount = 100 },
                // 2026年 1月 第1週 (月曜始まりと想定)
                new SalesResult { Id = 2, StoreId = 1, ProductId = 1, SaleDate = new DateTime(2026, 1, 5), Quantity = 2, SalesAmount = 200 },
                new SalesResult { Id = 3, StoreId = 1, ProductId = 1, SaleDate = new DateTime(2026, 1, 8), Quantity = 3, SalesAmount = 300 },
                // 2026年 1月 第2週
                new SalesResult { Id = 4, StoreId = 1, ProductId = 1, SaleDate = new DateTime(2026, 1, 15), Quantity = 5, SalesAmount = 500 }
            };
            var sale_ctrl = new SalesResultControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            sale_ctrl.SetSalesResult(sales);
        }

        [Test]
        public void TS007_GetTotalSalesViewData_Weekly指定で週ごとに集計されること()
        {
            // Act: 現在のメソッドシグネチャに合わせて2引数で呼び出し
            _target.GetTotalSalesViewData(E_Period.Weekly, out var resultData);

            // Assert
            // 2025/12/10の週, 2026/1/5の週, 2026/1/15の週 の3レコードになる想定
            Assert.That(resultData.Count, Is.EqualTo(3));

            // 2026/1/5の週 (1/5と1/8の合算) の確認
            var week1Data = resultData.FirstOrDefault(d => d.StartDate.Date == new DateTime(2026, 1, 5));
            Assert.That(week1Data, Is.Not.Null);
            Assert.That(week1Data.DataCount, Is.EqualTo(2));
            Assert.That(week1Data.TotalSalesCount, Is.EqualTo(5)); // 2 + 3
            Assert.That(week1Data.TotalSales, Is.EqualTo(500)); // 200 + 300
        }

        [Test]
        public void TS008_GetTotalSalesViewData_Monthly指定で月ごとに集計されること()
        {
            // Act
            _target.GetTotalSalesViewData(E_Period.Monthly, out var resultData);

            // Assert
            // 2025年12月, 2026年1月 の2レコード
            Assert.That(resultData.Count, Is.EqualTo(2));

            var janData = resultData.FirstOrDefault(d => d.Period == "2026年1月");
            Assert.That(janData, Is.Not.Null);
            Assert.That(janData.TotalSalesCount, Is.EqualTo(10)); // 2 + 3 + 5
            Assert.That(janData.TotalSales, Is.EqualTo(1000)); // 200 + 300 + 500
        }

        [Test]
        public void TS009_GetTotalSalesViewData_Yearly指定で年ごとに集計されること()
        {
            // Act
            _target.GetTotalSalesViewData(E_Period.Yearly, out var resultData);

            // Assert
            Assert.That(resultData.Count, Is.EqualTo(2)); // 2025年, 2026年

            var data2025 = resultData.FirstOrDefault(d => d.Period == "2025年");
            Assert.That(data2025.TotalSales, Is.EqualTo(100));

            var data2026 = resultData.FirstOrDefault(d => d.Period == "2026年");
            Assert.That(data2026.TotalSales, Is.EqualTo(1000));
        }

        [Test]
        public void TS010_GetTotalSalesViewData_None指定でfalseと空リストが返ること()
        {
            // Act
            bool result = _target.GetTotalSalesViewData(E_Period.None, out var resultData);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(resultData, Is.Empty);
        }

        [Test]
        public void TS011_GetTotalSalesViewData_処理成功時に戻り値がtrueになること()
        {
            // Act
            bool result = _target.GetTotalSalesViewData(E_Period.Yearly, out _);

            // Assert
            Assert.That(result, Is.True, "データの取得に成功した場合はtrueを返すこと");
        }
    }
}