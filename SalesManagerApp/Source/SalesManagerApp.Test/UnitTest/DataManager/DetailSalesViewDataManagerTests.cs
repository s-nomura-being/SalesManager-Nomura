using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using SalesManagerApp.Data;
using DatabaseLib.Table;
using SalesManagerApp.DBControl;
using FileControlLib.Processor;

namespace SalesManagerApp.Test.UnitTest.DataManager
{
    [TestFixture]
    public class DetailSalesViewDataManagerTests
    {
        private DetailSalesViewDataManager _target;
        private string _originalConnectionString;
        private DbConnection _keepAliveConnection;

        [SetUp]
        public void SetUp()
        {
            _originalConnectionString = Properties.Settings.Default.DbConnectionString;

            // 共有キャッシュモードのインメモリDB設定
            string inMemoryConnectionString = "Data Source=SharedDetailSalesDB;Mode=Memory;Cache=Shared";
            Properties.Settings.Default.DbConnectionString = inMemoryConnectionString;

            _keepAliveConnection = new SqliteConnection(inMemoryConnectionString);
            _keepAliveConnection.Open();

            // ※実際の環境に合わせて、テーブル作成(Init処理)の呼び出しを追加してください
            // var db = new DatabaseLib.DataBaseControl(inMemoryConnectionString); db.Init();

            _target = new DetailSalesViewDataManager();
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
            // ※注: 実際のSalesResultControlが結合情報を取得できるよう、各マスタへデータを登録します
            // マスタコントロールの初期化
            var categoryCtrl = new CategoryMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            var productCtrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            var salesCtrl = new SalesResultControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);

            // ダミーのデータ登録（※テスト用テーブルへのINSERTを想定）
            // 実際の実装に応じてStoreMasterControlなどがあればそれを使用してください。
            // ここではSalesResult自体にナビゲーションプロパティをセットした状態をモック代わりに登録します。

            var testSales = new List<SalesResult>
            {
                new SalesResult
                {
                    SaleDate = new DateTime(2026, 6, 18),
                    Quantity = 2, SalesAmount = 200,
                    Store = new StoreMaster { Name = "東京本店" },
                    Product = new ProductMaster
                    {
                        Name = "商品A",
                        Category = new CategoryMaster { Category = "区分1" }
                    }
                },
                new SalesResult
                {
                    SaleDate = new DateTime(2026, 6, 17),  // 日付順序テスト用（過去）
                    Quantity = 3, SalesAmount = 300,
                    Store = new StoreMaster { Name = "大阪支店" },
                    Product = new ProductMaster
                    {
                        Name = "商品B",
                        Category = new CategoryMaster { Category = "区分2" }
                    }
                },
                new SalesResult
                {
                    SaleDate = new DateTime(2026, 6, 19),
                    Quantity = 5, SalesAmount = 500,
                    Store = new StoreMaster { Name = "東京本店" }, // 重複
                    Product = new ProductMaster
                    {
                        Name = "商品A", // 重複
                        Category = new CategoryMaster { Category = "区分1" } // 重複
                    }
                }
            };

            // データセット
            salesCtrl.SetSalesResult(testSales);
        }

        [Test]
        public void TS012_GetNameData_重複が排除されたマスタ名称のリストが取得できること()
        {
            // Arrange
            PrepareTestData();

            // Act
            bool result = _target.GetNameData(out var lstStore, out var lstProduct, out var lstCategory);

            // Assert
            Assert.That(result, Is.True);

            // 東京本店が2件あるが、Distinctで1件にまとまっていること
            Assert.That(lstStore.Count, Is.EqualTo(2));
            Assert.That(lstStore, Contains.Item("東京本店"));
            Assert.That(lstStore, Contains.Item("大阪支店"));

            Assert.That(lstProduct.Count, Is.EqualTo(2));
            Assert.That(lstProduct, Contains.Item("商品A"));
            Assert.That(lstProduct, Contains.Item("商品B"));

            Assert.That(lstCategory.Count, Is.EqualTo(2));
            Assert.That(lstCategory, Contains.Item("区分1"));
            Assert.That(lstCategory, Contains.Item("区分2"));
        }

        [Test]
        public void TS013_GetNameData_データが0件の場合は空のリストが取得できること()
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
        public void TS014_GetDetailSalesViewData_filterがnullの場合全データが日付の昇順で取得できること()
        {
            // Arrange
            PrepareTestData();

            // Act
            bool result = _target.GetDetailSalesViewData(out var detailData, null);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(detailData.Count, Is.EqualTo(3));

            // 日付の昇順 (17日 -> 18日 -> 19日) にソートされていることの確認
            Assert.That(detailData[0].SalesDate, Is.EqualTo(new DateTime(2026, 6, 17)));
            Assert.That(detailData[1].SalesDate, Is.EqualTo(new DateTime(2026, 6, 18)));
            Assert.That(detailData[2].SalesDate, Is.EqualTo(new DateTime(2026, 6, 19)));
        }

        [Test]
        public void TS015_GetDetailSalesViewData_filterを指定した場合絞り込みが適用されること()
        {
            // Arrange
            PrepareTestData();

            // FilterInfoの仕様に基づき、「東京本店」のみを絞り込む条件を作成
            var filter = new FilterInfo(
                storelist: new List<string> { "東京本店" },
                productlist: new List<string>(), // 空リストは全対象と想定
                categorylist: new List<string>(),
                start: null,
                end: null
            );

            // Act
            bool result = _target.GetDetailSalesViewData(out var detailData, filter);

            // Assert
            Assert.That(result, Is.True, "データの取得に成功すること");

            // SalesResultControlの絞り込みロジックが正しく機能していれば、東京本店の2件のみが取得される
            Assert.That(detailData.Count, Is.EqualTo(2), "東京本店のデータ件数と一致すること");
            Assert.That(detailData.All(d => d.StoreName == "東京本店"), Is.True, "取得されたデータがすべて東京本店であること");
        }

        [Test]
        public void TS016_GetNameData_例外発生時はfalseが返ること()
        {
            // Arrange
            Properties.Settings.Default.DbConnectionString = "Data Source=InvalidPath:::;Version=3;";
            var errorTarget = new DetailSalesViewDataManager();

            // Act
            bool result = errorTarget.GetNameData(out var lstStore, out var lstProduct, out var lstCategory);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(lstStore, Is.Empty);
        }

        [Test]
        public void TS017_GetDetailSalesViewData_例外発生時はfalseが返ること()
        {
            // Arrange
            Properties.Settings.Default.DbConnectionString = "Data Source=InvalidPath:::;Version=3;";
            var errorTarget = new DetailSalesViewDataManager();

            // Act
            bool result = errorTarget.GetDetailSalesViewData(out var detailData, null);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(detailData, Is.Empty);
        }
    }
}