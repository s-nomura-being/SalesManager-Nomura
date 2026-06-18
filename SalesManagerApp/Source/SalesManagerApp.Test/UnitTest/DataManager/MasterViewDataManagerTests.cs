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
    public class MasterViewDataManagerTests
    {
        private MasterViewDataManager _target;
        private string _originalConnectionString;
        private DbConnection _keepAliveConnection;

        [SetUp]
        public void SetUp()
        {
            _originalConnectionString = Properties.Settings.Default.DbConnectionString;

            // 共有キャッシュモードのインメモリDB設定
            string inMemoryConnectionString = "Data Source=SharedMasterDB;Mode=Memory;Cache=Shared";
            Properties.Settings.Default.DbConnectionString = inMemoryConnectionString;

            _keepAliveConnection = new SqliteConnection(inMemoryConnectionString);
            _keepAliveConnection.Open();

            // ※実際の環境に合わせて、テーブル作成(Init処理)の呼び出しを追加してください
            // var db = new DatabaseLib.DataBaseControl(inMemoryConnectionString); db.Init();

            _target = new MasterViewDataManager();
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

        [Test]
        public void TS025_GetStoreMasterData_店舗データが正しく取得できること()
        {
            // Arrange
            var ctrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            ctrl.SetStoreMaster(new List<StoreMaster>
            {
                new StoreMaster { Id = 1, Name = "店舗A" },
                new StoreMaster { Id = 2, Name = "店舗B" }
            });

            // Act
            bool result = _target.GetStoreMasterData(out var resultData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(resultData.Count, Is.EqualTo(2));
            Assert.That(resultData.Any(d => d.ID == 1 && d.StoreName == "店舗A"), Is.True);
        }

        [Test]
        public void TS026_SetStoreMasterData_店舗データが正しく登録されること()
        {
            // Arrange
            var inputData = new List<StoreMasterData>
            {
                new StoreMasterData { ID = 10, StoreName = "新規店舗X" }
            };

            // Act
            bool result = _target.SetStoreMasterData(inputData);

            // Assert
            Assert.That(result, Is.True);

            var ctrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            ctrl.GetStoreMaster(out var dbData);
            Assert.That(dbData.Any(d => d.Id == 10 && d.Name == "新規店舗X"), Is.True);
        }

        [Test]
        public void TS027_GetProductMasterData_商品データが区分IDを含めて正しく取得できること()
        {
            // Arrange
            // 区分マスタと商品マスタの関連データを準備
            var categoryCtrl = new CategoryMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            categoryCtrl.SetCategoryMaster(new List<CategoryMaster> { new CategoryMaster { Id = 5, Category = "テスト区分" } });

            var productCtrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            productCtrl.SetProductMaster(new List<ProductMaster>
            {
                new ProductMaster(){ Id = 1, Name = "テスト商品", UnitPrice = 1000, CategoryId = 5, Created_at = DateTime.Now }
            });

            // Act
            bool result = _target.GetProductMasterData(out var resultData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(resultData.Count, Is.EqualTo(1));
            Assert.That(resultData[0].ID, Is.EqualTo(1));
            Assert.That(resultData[0].ProductName, Is.EqualTo("テスト商品"));
            Assert.That(resultData[0].CategoryID, Is.EqualTo(5), "区分IDが正しくマッピングされていること");
        }

        [Test]
        public void TS028_SetProductMasterData_商品データが正しく登録されること()
        {
            // Arrange
            var categoryCtrl = new CategoryMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            categoryCtrl.SetCategoryMaster(new List<CategoryMaster> { new CategoryMaster { Id = 2, Category = "テスト区分" } });
            var inputData = new List<ProductMasterData>
            {
                new ProductMasterData { ID = 20, ProductName = "新規商品Y", UnitPrice = 500, CategoryID = 2 }
            };

            // Act
            bool result = _target.SetProductMasterData(inputData);

            // Assert
            Assert.That(result, Is.True);

            var ctrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            ctrl.GetProductMaster(out var dbData);
            Assert.That(dbData.Any(d => d.Id == 20 && d.Name == "新規商品Y" && d.CategoryId == 2), Is.True);
        }

        [Test]
        public void TS029_GetCategoryMasterData_論理削除されたデータが除外されること()
        {
            // Arrange
            var ctrl = new CategoryMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            ctrl.SetCategoryMaster(new List<CategoryMaster>
            {
                new CategoryMaster { Id = 1, Category = "有効区分", Delete_flag = false },
                new CategoryMaster { Id = 2, Category = "削除済み区分", Delete_flag = true }
            });

            // Act
            bool result = _target.GetCategoryMasterData(out var resultData);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(resultData.Count, Is.EqualTo(1), "Delete_flagがtrueのデータは除外されること");
            Assert.That(resultData[0].ID, Is.EqualTo(1));
            Assert.That(resultData[0].CategoryName, Is.EqualTo("有効区分"));
        }

        [Test]
        public void TS030_SetCategoryMasterData_区分データが正しく登録されること()
        {
            // Arrange
            var inputData = new List<CategoryMasterData>
            {
                new CategoryMasterData { ID = 30, CategoryName = "新規区分Z" }
            };

            // Act
            bool result = _target.SetCategoryMasterData(inputData);

            // Assert
            Assert.That(result, Is.True);

            var ctrl = new CategoryMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            ctrl.GetCategoryMaster(out var dbData);
            Assert.That(dbData.Any(d => d.Id == 30 && d.Category == "新規区分Z"), Is.True);
        }

        [Test]
        public void TS031_SetStoreMasterData_例外発生時にfalseが返却されること()
        {
            // Arrange
            Properties.Settings.Default.DbConnectionString = "Data Source=InvalidPath:::;Version=3;";
            var errorTarget = new MasterViewDataManager();

            var inputData = new List<StoreMasterData> { new StoreMasterData() };

            // Act
            bool result = errorTarget.SetStoreMasterData(inputData);

            // Assert
            Assert.That(result, Is.False, "Catchブロックを通り、falseが返ること");
        }
    }
}