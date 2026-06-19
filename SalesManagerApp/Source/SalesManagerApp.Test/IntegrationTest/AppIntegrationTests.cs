using DatabaseLib.Table;
// プロジェクトの実際のおよび必要な名前空間を定義
using FileControlLib;
using FileControlLib.Processor;
using Moq;
using NUnit.Framework;
using SalesManagerApp.Data;
using SalesManagerApp.DBControl;
using SalesManagerApp.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesManagerApp.Tests.Integration
{
    [TestFixture]
    public class AppIntegrationTests
    {
        private const string PRODUCT_CSV = "product.csv";
        private const string INV_CSV = "inventory.cs";
        private const string SALE_CSV = "sales.csv";

        private MainViewDataManager _dataManager;
        private string _originalConnectionString;
        private int _originalNoticeMust;
        private int _originalNoticeLow;

        private string db_filename;

        [SetUp]
        public void SetUp()
        {
            // 既存のアプリケーション設定をバックアップ
            _originalConnectionString = Properties.Settings.Default.DbConnectionString;
            _originalNoticeMust = Properties.Settings.Default.NoticeCount_Must;
            _originalNoticeLow = Properties.Settings.Default.NoticeCount_Low;

            // テスト用のDB接続文字列を設定 (インメモリSQLiteまたはテスト用ファイル)
            db_filename = $"Data Source=test_database{Guid.NewGuid()}.db;";
            Properties.Settings.Default.DbConnectionString = db_filename;

            // 通知のしきい値を設定
            Properties.Settings.Default.NoticeCount_Must = 5;
            Properties.Settings.Default.NoticeCount_Low = 10;

            // 事前にテスト用DBのテーブルをクリーンアップする処理をここに記述します
            CleanupTestDatabase();

            // MainViewDataManagerの初期化
            _dataManager = new MainViewDataManager();
        }

        [TearDown]
        public void TearDown()
        {
            // アプリケーション設定を元に戻す
            Properties.Settings.Default.DbConnectionString = _originalConnectionString;
            Properties.Settings.Default.NoticeCount_Must = _originalNoticeMust;
            Properties.Settings.Default.NoticeCount_Low = _originalNoticeLow;
        }

        private void CleanupTestDatabase()
        {
            //テスト用ファイルを削除
            if(true == File.Exists(db_filename))
            {
                File.Delete(db_filename);
            }
            if (true == File.Exists(PRODUCT_CSV))
            {
                File.Delete(PRODUCT_CSV);
            }
            if (true == File.Exists(INV_CSV))
            {
                File.Delete(INV_CSV);
            }
            if (true == File.Exists(SALE_CSV))
            {
                File.Delete(SALE_CSV);
            }
        }

        [Test]
        public void IT01_CSVファイル手動読込とメイン画面即時反映シナリオ()
        {
            // ==========================================
            // 1. Arrange: ファイル読み込みモックとデータの準備
            // ==========================================
            var mockFileProcessor = new Mock<IFileProcessor>();

            //店舗マスタ事前登録
            var store = new StoreMaster() { Id = 1, Name = "テスト店舗" };
            var store_ctrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            store_ctrl.SetStoreMaster(store);

            // CSVから読み込まれた想定のデータを作成
            var productCsvData = new List<ProductFileData>
            {
                new ProductFileData { ProductId = 1, ProductName = "テスト商品A", UnitPrice = 1000, Category = "テスト区分A" },
                new ProductFileData { ProductId = 2, ProductName = "テスト商品B", UnitPrice = 500, Category = "テスト区分B" }
            };

            var inventoryCsvData = new List<InventoryFileData>
            {
                new InventoryFileData { StoreId = 1, ProductId = 1, Stock = 15 }, // 商品A: 在庫15
                new InventoryFileData { StoreId = 1, ProductId = 2, Stock = 4 }   // 商品B: 在庫4
            };

            // 売上は「先週(今日含む過去7日間)」のデータとする
            var saleCsvData = new List<SaleFileData>
            {
                // 商品Aの売上 (合計販売数: 3)
                new SaleFileData { SaleDate = DateTime.Today, StoreId = 1, ProductId = 1, Quantity = 2 },
                new SaleFileData { SaleDate = DateTime.Today.AddDays(-1), StoreId = 1, ProductId = 1, Quantity = 1 },
                
                // 商品Bの売上 (合計販売数: 2)
                new SaleFileData { SaleDate = DateTime.Today, StoreId = 1, ProductId = 2, Quantity = 2 }
            };

            // モックの振る舞い定義
            var outProducts = productCsvData;
            mockFileProcessor.Setup(m => m.ReadFile(PRODUCT_CSV, out outProducts)).Returns(true);

            var outInventories = inventoryCsvData;
            mockFileProcessor.Setup(m => m.ReadFile(INV_CSV, out outInventories)).Returns(true);

            var outSales = saleCsvData;
            mockFileProcessor.Setup(m => m.ReadFile(SALE_CSV, out outSales)).Returns(true);

            // ==========================================
            // 2. Act: データのDB保存と、メイン画面用データの取得
            // ==========================================

            // IFileProcessorから取得したデータをDataManagerへ渡してDB保存 (正常に保存できるか確認)
            bool isProductSaved = _dataManager.SetProductMasterFileData(outProducts);
            bool isInventorySaved = _dataManager.SetInventoryFileData(outInventories);
            bool isSaleSaved = _dataManager.SetSaleFileData(outSales);

            // メイン画面用データの取得
            bool isViewDataRetrieved = _dataManager.GetMainViewData(out List<LastWeekProductSalesData> lastWeekSalesData, out List<StockStatusData> stockStatusData);

            // ==========================================
            // 3. Assert: 結果の検証
            // ==========================================
            Assert.That(isProductSaved, Is.True, "商品マスタデータのDB登録に失敗しました。");
            Assert.That(isInventorySaved, Is.True, "在庫データのDB登録に失敗しました。");
            Assert.That(isSaleSaved, Is.True, "売上データのDB登録に失敗しました。");
            Assert.That(isViewDataRetrieved, Is.True, "メイン画面表示用データの作成・取得に失敗しました。");

            // 先週の商品売上データの検証 (商品Aについて)
            var prodASales = lastWeekSalesData.FirstOrDefault(d => d.ProductID == 1);
            Assert.That(prodASales, Is.Not.Null, "商品Aの売上データが含まれていません。");
            Assert.That(prodASales.ProductName, Is.EqualTo("テスト商品A"));
            Assert.That(prodASales.LastWeekSales, Is.EqualTo(3), "商品Aの先週販売数が一致しません(2 + 1 = 3)。");
            Assert.That(prodASales.TotalStock, Is.EqualTo(15), "商品Aの総在庫数が一致しません。");
            Assert.That(prodASales.TotalSales, Is.EqualTo(3000), "商品Aの累計売上金額が一致しません(販売数3 * 単価1000)。");

            // 在庫状況データの検証 (商品Aについて)
            // 商品A: 在庫15 - 販売数3 = 販売後在庫12 (しきい値 Low=10以上なので、通常状態)
            var prodAStock = stockStatusData.FirstOrDefault(d => d.ProductName == "テスト商品A");
            Assert.That(prodAStock, Is.Not.Null);
            Assert.That(prodAStock.Stock, Is.EqualTo(15));
            Assert.That(prodAStock.AfterStock, Is.EqualTo(12));
            Assert.That(prodAStock.Notice, Is.EqualTo(string.Empty));
            Assert.That(prodAStock.StatusType, Is.EqualTo(E_StockStatusType.None));

            // 在庫状況データの検証 (商品Bについて)
            // 商品B: 在庫4 - 販売数2 = 販売後在庫2 (しきい値 Must=5以下なので、「要発注」)
            var prodBStock = stockStatusData.FirstOrDefault(d => d.ProductName == "テスト商品B");
            Assert.That(prodBStock, Is.Not.Null);
            Assert.That(prodBStock.Stock, Is.EqualTo(4));
            Assert.That(prodBStock.AfterStock, Is.EqualTo(2));
            Assert.That(prodBStock.Notice, Is.EqualTo("要発注"));
            Assert.That(prodBStock.StatusType, Is.EqualTo(E_StockStatusType.Must));
        }

        [Test]
        public void IT02_アラートしきい値変更とメイン画面アラート動的再判定シナリオ()
        {
            // ==========================================
            // 1. Arrange: 初期設定とデータ準備
            // ==========================================
            // 初期のしきい値を設定 (要発注: 5以下, 在庫少: 10以下)
            Properties.Settings.Default.NoticeCount_Must = 5;
            Properties.Settings.Default.NoticeCount_Low = 10;

            //店舗マスタ事前登録
            var store = new StoreMaster() { Id = 1, Name = "テスト店舗" };
            var store_ctrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            store_ctrl.SetStoreMaster(store);

            // 商品データと在庫データを準備 (販売後在庫が「8」になるように設定)
            var productCsvData = new List<ProductFileData>
            {
                new ProductFileData { ProductId = 1, ProductName = "テスト商品A", UnitPrice = 1000, Category = "テスト区分" }
            };

            var inventoryCsvData = new List<InventoryFileData>
            {
                new InventoryFileData { StoreId = 1, ProductId = 1, Stock = 8 }
            };

            // データをDBに登録
            _dataManager.SetProductMasterFileData(productCsvData);
            _dataManager.SetInventoryFileData(inventoryCsvData);

            // ==========================================
            // 2. Act 1: 変更前のメイン画面データ取得
            // ==========================================
            bool isInitialViewDataRetrieved = _dataManager.GetMainViewData(out _, out List<StockStatusData> initialStockData);

            var initialTarget = initialStockData.FirstOrDefault(d => d.ProductName == "テスト商品A");

            // ==========================================
            // 3. Assert 1: 変更前の状態検証
            // ==========================================
            // 在庫8 は「5以下」ではないが「10以下」であるため、「在庫少」扱いになること
            Assert.That(isInitialViewDataRetrieved, Is.True, "変更前の表示用データ取得に失敗しました。");
            Assert.That(initialTarget, Is.Not.Null, "商品Aの在庫データが含まれていません。");
            Assert.That(initialTarget.AfterStock, Is.EqualTo(8));
            Assert.That(initialTarget.Notice, Is.EqualTo("在庫が少ない"));
            Assert.That(initialTarget.StatusType, Is.EqualTo(E_StockStatusType.Low));

            // ==========================================
            // 4. Act 2: アプリ設定の変更 (要発注のしきい値を 10 へ引き上げ)
            // ==========================================
            Properties.Settings.Default.NoticeCount_Must = 10;

            // 設定変更後、再度メイン画面のデータを取得する
            bool isUpdatedViewDataRetrieved = _dataManager.GetMainViewData(
                out _,
                out List<StockStatusData> updatedStockData);

            var updatedTarget = updatedStockData.FirstOrDefault(d => d.ProductName == "テスト商品A");

            // ==========================================
            // 5. Assert 2: 変更後の状態検証
            // ==========================================
            // 在庫8 は新しい要発注のしきい値「10以下」に該当するため、「要発注」扱いに切り替わること
            Assert.That(isUpdatedViewDataRetrieved, Is.True, "変更後の表示用データ取得に失敗しました。");
            Assert.That(updatedTarget, Is.Not.Null, "商品Aの在庫データが含まれていません。");
            Assert.That(updatedTarget.AfterStock, Is.EqualTo(8));
            Assert.That(updatedTarget.Notice, Is.EqualTo("要発注"), "しきい値変更後の発注通知テキストが切り替わっていません。");
            Assert.That(updatedTarget.StatusType, Is.EqualTo(E_StockStatusType.Must), "しきい値変更後のStatusTypeが切り替わっていません。");
        }

        [Test]
        public void IT03_売上集計から詳細ドリルダウン画面遷移シナリオ()
        {
            // ==========================================
            // 1. Arrange: 初期設定とテスト用売上データの準備
            // ==========================================
            // 商品マスタや店舗マスタのセットアップ (詳細表示で名称解決するため)
            var productCsvData = new List<ProductFileData>
            {
                new ProductFileData { ProductId = 1, ProductName = "テスト商品A", UnitPrice = 1000, Category = "テスト区分A" },
                new ProductFileData { ProductId = 2, ProductName = "テスト商品B", UnitPrice = 500, Category = "テスト区分B" }
            };

            var store = new List<StoreMaster>
            {
                new StoreMaster { Id = 1, Name = "東京本店" },
                new StoreMaster { Id = 2, Name = "大阪支店" }
            };
            var store_ctrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            store_ctrl.SetStoreMaster(store);

            _dataManager.SetProductMasterFileData(productCsvData);

            // 異なる月の販売実績データを登録 (2026年4月 と 2026年5月)
            var saleCsvData = new List<SaleFileData>
            {
                // 4月のデータ (1件)
                new SaleFileData { SaleDate = new DateTime(2026, 4, 15), StoreId = 1, ProductId = 1, Quantity = 2 },
                
                // 5月のデータ (2件)
                new SaleFileData { SaleDate = new DateTime(2026, 5, 10), StoreId = 1, ProductId = 1, Quantity = 5 },
                new SaleFileData { SaleDate = new DateTime(2026, 5, 20), StoreId = 2, ProductId = 2, Quantity = 3 }
            };
            _dataManager.SetSaleFileData(saleCsvData);

            // ==========================================
            // 2. Act 1: 売上集計画面での月間集計の実行
            // ==========================================
            // ※TotalSalesViewDataManager のインスタンス化
            var totalSalesManager = new TotalSalesViewDataManager();

            // 月間(Monthly)で集計データを取得
            bool isTotalSuccess = totalSalesManager.GetTotalSalesViewData(E_Period.Monthly, out List<TotalSalesData> totalSalesData);

            // 5月の集計行を選択したと仮定 (ドリルダウン対象)
            var selectedMayData = totalSalesData.FirstOrDefault(d => d.StartDate.Year == 2026 && d.StartDate.Month == 5);

            // ==========================================
            // 3. Act 2: ドリルダウンによる画面遷移と詳細データの取得
            // ==========================================
            // 選択した行の期間情報(StartDate, EndDate)を引き継いでフィルターを作成
            // (店舗や商品の指定は空リストとし、期間のみで絞り込み)
            var drillDownFilter = new FilterInfo(new List<string>(), new List<string>(), new List<string>(), selectedMayData?.StartDate, selectedMayData?.EndDate);

            var detailSalesManager = new DetailSalesViewDataManager();

            // フィルター条件を渡して売上詳細一覧データを取得
            bool isDetailSuccess = detailSalesManager.GetDetailSalesViewData(out List<DetailSalesData> detailSalesData, drillDownFilter);

            // ==========================================
            // 4. Assert: 取得された詳細データの検証
            // ==========================================
            Assert.That(isTotalSuccess, Is.True, "月間集計データの取得に失敗しました。");
            Assert.That(selectedMayData, Is.Not.Null, "5月の集計データが生成されていません。");

            Assert.That(isDetailSuccess, Is.True, "詳細一覧データの取得に失敗しました。");
            Assert.That(detailSalesData, Is.Not.Null);

            // 5月分のデータのみ(2件)が抽出されていることの確認
            Assert.That(detailSalesData.Count, Is.EqualTo(2), "絞り込み条件が正しく適用されず、データ件数が一致しません。");

            // 抽出されたデータがすべて5月のものであることの確認
            bool allMayData = detailSalesData.All(d => d.SalesDate.Year == 2026 && d.SalesDate.Month == 5);
            Assert.That(allMayData, Is.True, "5月以外のデータが詳細一覧に含まれています。");
        }

        [Test]
        public void IT04_複数条件絞り込み適用とフィルタリング状態でのExcel出力シナリオ()
        {
            // ==========================================
            // 1. Arrange: テストデータの準備 (マスタと売上)
            // ==========================================
            // 商品・店舗・区分の名称が詳細画面で解決される前提でマスタを登録します。
            // ※必要に応じて、SetCategoryMasterData等のControlメソッドを利用してDBに登録してください。
            var productCsvData = new List<ProductFileData>
            {
                new ProductFileData { ProductId = 1, ProductName = "おにぎり", UnitPrice = 150, Category = "食品" },
                new ProductFileData { ProductId = 2, ProductName = "洗剤", UnitPrice = 400, Category = "日用品" }
            };
            _dataManager.SetProductMasterFileData(productCsvData);

            var store = new List<StoreMaster>
            {
                new StoreMaster { Id = 1, Name = "新宿店" },
                new StoreMaster { Id = 2, Name = "渋谷店" }
            };
            var store_ctrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            store_ctrl.SetStoreMaster(store);

            // 販売実績データ (条件の組み合わせパターンを用意)
            var saleCsvData = new List<SaleFileData>
            {
                // ① 新宿店 × 食品 (抽出対象)
                new SaleFileData { SaleDate = new DateTime(2026, 6, 1), StoreId = 1, ProductId = 1, Quantity = 2 },
                // ② 新宿店 × 日用品 (区分が異なるため除外)
                new SaleFileData { SaleDate = new DateTime(2026, 6, 2), StoreId = 1, ProductId = 2, Quantity = 1 },
                // ③ 渋谷店 × 食品 (店舗が異なるため除外)
                new SaleFileData { SaleDate = new DateTime(2026, 6, 3), StoreId = 2, ProductId = 1, Quantity = 3 }
            };
            _dataManager.SetSaleFileData(saleCsvData);

            // ==========================================
            // 2. Act 1: 複合条件での絞り込み実行
            // ==========================================
            // 新宿店の「食品」のみを絞り込む FilterInfo を作成
            var targetStores = new List<string> { "新宿店" };
            var targetCategories = new List<string> { "食品" };
            var emptyProducts = new List<string>();

            var filterInfo = new FilterInfo(targetStores, emptyProducts, targetCategories);

            var detailSalesManager = new DetailSalesViewDataManager();

            // 絞り込み条件を適用してデータを取得
            bool isDataRetrieved = detailSalesManager.GetDetailSalesViewData(out List<DetailSalesData> filteredData, filterInfo);

            // ==========================================
            // 3. Assert 1: 絞り込み結果の検証
            // ==========================================
            Assert.That(isDataRetrieved, Is.True, "詳細データの取得に失敗しました。");
            Assert.That(filteredData, Is.Not.Null);
            Assert.That(filteredData.Count, Is.EqualTo(1), "複合絞り込みが正しく適用されていません。");

            var targetData = filteredData.First();
            Assert.That(targetData.StoreName, Is.EqualTo("新宿店"));
            Assert.That(targetData.Category, Is.EqualTo("食品"));
            Assert.That(targetData.ProductName, Is.EqualTo("おにぎり"));

            // ==========================================
            // 4. Act 2: ファイル出力の実行 (Mock連携)
            // ==========================================
            // ファイル操作クラスのモックを用意
            var mockFileProcessor = new Mock<IFileProcessor>();
            mockFileProcessor.Setup(m => m.SaveFile(It.IsAny<string>(), It.IsAny<List<DetailSalesData>>()))
                             .Returns(true);

            // FileManagerをモックをインジェクトして初期化 (コンストラクタ注入等を想定)。
            var detailFileManager = new DetailSalesViewFileManager(mockFileProcessor.Object);

            string exportFilePath = @"C:\Temp\ExportedSales.xlsx";
            bool isFileSaved = detailFileManager.SaveFile(exportFilePath, filteredData);

            // ==========================================
            // 5. Assert 2: ファイル出力連携の検証
            // ==========================================
            Assert.That(isFileSaved, Is.True, "ファイルの保存処理が失敗しました。");

            // SaveFileメソッドが、「指定したパス」と「絞り込まれたデータリスト」を引数として
            // 正確に1回呼び出されたことを検証する (画面の表示内容と出力内容が一致する担保)
            mockFileProcessor.Verify(m => m.SaveFile(
                It.Is<string>(path => path == exportFilePath),
                It.Is<List<DetailSalesData>>(list => list.Count == 1 && list[0].ProductName == "おにぎり")
            ), Times.Once, "抽出されたデータがファイルプロセッサに正しく引き渡されていません。");
        }

        [Test]
        public void IT05_在庫情報絞り込みと在庫金額自動計算_Excel出力シナリオ()
        {
            // ==========================================
            // 1. Arrange: テストデータの準備 (商品、在庫、販売実績)
            // ==========================================
            // 商品データの登録 (単価の計算用)
            var productCsvData = new List<ProductFileData>
            {
                new ProductFileData { ProductId = 1, ProductName = "ノート", UnitPrice = 200, Category = "文具" },
                new ProductFileData { ProductId = 2, ProductName = "ペン", UnitPrice = 150, Category = "文具" }
            };
            _dataManager.SetProductMasterFileData(productCsvData);

            // 店舗データ登録
            var store = new List<StoreMaster>
            {
                new StoreMaster { Id = 1, Name = "東京店" },
                new StoreMaster { Id = 2, Name = "大阪店" }
            };
            var store_ctrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            store_ctrl.SetStoreMaster(store);

            // 在庫データの登録
            var inventoryCsvData = new List<InventoryFileData>
            {
                // 東京店の在庫
                new InventoryFileData { StoreId = 1, ProductId = 1, Stock = 50 }, // ノート: 50個
                new InventoryFileData { StoreId = 1, ProductId = 2, Stock = 30 }, // ペン: 30個
                // 大阪店の在庫 (絞り込み除外用)
                new InventoryFileData { StoreId = 2, ProductId = 1, Stock = 100 }
            };
            _dataManager.SetInventoryFileData(inventoryCsvData);

            // 販売実績データの登録 (最終販売日の計算用)
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);
            var saleCsvData = new List<SaleFileData>
            {
                // 東京店 - ノート (最終販売日は今日)
                new SaleFileData { SaleDate = yesterday, StoreId = 1, ProductId = 1, Quantity = 5 },
                new SaleFileData { SaleDate = today, StoreId = 1, ProductId = 1, Quantity = 2 },
                
                // 東京店 - ペン (最終販売日は昨日)
                new SaleFileData { SaleDate = yesterday, StoreId = 1, ProductId = 2, Quantity = 10 }
            };
            _dataManager.SetSaleFileData(saleCsvData);

            // ==========================================
            // 2. Act 1: 絞り込み条件の適用とデータ取得
            // ==========================================
            // 「東京店」のみに絞り込む FilterInfo を作成
            // ※名称やIDでの絞り込み実装に合わせて引数は調整してください
            var targetStores = new List<string> { "東京店" };
            var emptyList = new List<string>();
            var filterInfo = new FilterInfo(targetStores, emptyList, emptyList);

            var inventoryManager = new InventoryViewDataManager();

            // データを取得
            bool isDataRetrieved = inventoryManager.GetInventoryViewData(out List<InventoryListData> inventoryList, filterInfo);

            // ==========================================
            // 3. Assert 1: 計算結果（在庫金額・最終販売日）の検証
            // ==========================================
            Assert.That(isDataRetrieved, Is.True, "在庫一覧データの取得に失敗しました。");
            Assert.That(inventoryList, Is.Not.Null);
            // 東京店のデータのみ(ノートとペン)抽出されていること
            Assert.That(inventoryList.Count, Is.EqualTo(2), "絞り込み条件が正しく適用されていません。");

            // ノートの検証
            var noteData = inventoryList.FirstOrDefault(d => d.ProductName == "ノート");
            Assert.That(noteData, Is.Not.Null);
            Assert.That(noteData.Stock, Is.EqualTo(50), "ノートの現在在庫数が一致しません。");
            Assert.That(noteData.UnitPrice, Is.EqualTo(200), "ノートの単価がマスタと一致しません。");
            Assert.That(noteData.StockValue, Is.EqualTo(10000), "ノートの在庫金額(50 * 200)の計算が間違っています。");
            Assert.That(noteData.LastSalesDate, Is.EqualTo(today), "ノートの最終販売日が最新の販売日と一致しません。");

            // ペンの検証
            var penData = inventoryList.FirstOrDefault(d => d.ProductName == "ペン");
            Assert.That(penData, Is.Not.Null);
            Assert.That(penData.StockValue, Is.EqualTo(4500), "ペンの在庫金額(30 * 150)の計算が間違っています。");
            Assert.That(penData.LastSalesDate, Is.EqualTo(yesterday), "ペンの最終販売日が一致しません。");

            // ==========================================
            // 4. Act 2: ファイル出力の実行 (Mock連携)
            // ==========================================
            var mockFileProcessor = new Mock<IFileProcessor>();
            mockFileProcessor.Setup(m => m.SaveFile(It.IsAny<string>(), It.IsAny<List<InventoryListData>>()))
                             .Returns(true);

            // InventoryInfoViewFileManager をモックを渡して初期化
            var inventoryFileManager = new InventoryInfoViewFileManager(mockFileProcessor.Object);

            string exportFilePath = @"C:\Temp\ExportedInventory.xlsx";
            bool isFileSaved = inventoryFileManager.SaveFile(exportFilePath, inventoryList);

            // ==========================================
            // 5. Assert 2: ファイル出力連携の検証
            // ==========================================
            Assert.That(isFileSaved, Is.True, "ファイルの保存処理が失敗しました。");

            // 画面で計算・抽出された通りのデータリストが保存処理に渡されていることを検証
            mockFileProcessor.Verify(m => m.SaveFile(
                It.Is<string>(path => path == exportFilePath),
                It.Is<List<InventoryListData>>(list =>
                    list.Count == 2 &&
                    list.Any(x => x.ProductName == "ノート" && x.StockValue == 10000))
            ), Times.Once, "計算済みの在庫リストがファイルプロセッサに正しく引き渡されていません。");
        }

        [Test]
        public void IT06_各種マスタデータのインライン編集_保存と他画面への名称即時反映シナリオ()
        {
            // ==========================================
            // 1. Arrange: 初期データ(マスタ・在庫)の準備
            // ==========================================
            // 初期のマスタデータを登録 (ここでは MainViewDataManager を利用して初期投入)
            var productCsvData = new List<ProductFileData>
            {
                new ProductFileData { ProductId = 1, ProductName = "お茶", UnitPrice = 150, Category = "飲料" }
            };
            _dataManager.SetProductMasterFileData(productCsvData);
            // 店舗データ登録
            var store = new List<StoreMaster>
            {
                new StoreMaster { Id = 1, Name = "東京店" },
            };
            var store_ctrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            store_ctrl.SetStoreMaster(store);

            // 在庫データの登録 (変更が在庫画面等にも波及するか確認するため)
            var inventoryCsvData = new List<InventoryFileData>
            {
                new InventoryFileData { StoreId = 1, ProductId = 1, Stock = 20 }
            };
            _dataManager.SetInventoryFileData(inventoryCsvData);

            // ==========================================
            // 2. Act 1: 変更前のメイン画面データ確認
            // ==========================================
            bool isBeforeRetrieved = _dataManager.GetMainViewData(out _, out List<StockStatusData> beforeStockData);

            var beforeTarget = beforeStockData.FirstOrDefault(d => d.ProductName == "お茶");

            // ==========================================
            // 3. Assert 1: 変更前の状態検証
            // ==========================================
            Assert.That(isBeforeRetrieved, Is.True);
            Assert.That(beforeTarget, Is.Not.Null, "初期登録した商品「お茶」が取得できません。");

            // ==========================================
            // 4. Act 2: マスタ管理画面での名称変更と保存
            // ==========================================
            // MasterViewDataManager のインスタンス化
            var masterManager = new MasterViewDataManager();

            // 商品マスタデータの取得
            masterManager.GetProductMasterData(out List<ProductMasterData> productMasters);
            var editTarget = productMasters.FirstOrDefault(p => p.ProductName == "お茶");
            Assert.That(editTarget, Is.Not.Null, "マスタデータから対象商品が取得できません。");

            // UIグリッド上でのインライン編集をシミュレート
            editTarget.ProductName = "特選お茶";

            // 変更内容をDBへ保存
            bool isSaved = masterManager.SetProductMasterData(productMasters);
            Assert.That(isSaved, Is.True, "商品マスタの保存処理に失敗しました。");

            // ==========================================
            // 5. Act 3: 他画面(メイン画面と在庫画面)のデータ再取得
            // ==========================================
            // ① メイン画面の再取得
            bool isAfterMainRetrieved = _dataManager.GetMainViewData(out _, out List<StockStatusData> afterStockData);

            // ② 在庫情報画面の再取得 (絞り込みなしで全件取得)
            var inventoryManager = new InventoryViewDataManager();
            var emptyFilter = new FilterInfo(new List<string>(), new List<string>(), new List<string>());
            bool isAfterInvRetrieved = inventoryManager.GetInventoryViewData(out List<InventoryListData> inventoryList, emptyFilter);

            // ==========================================
            // 6. Assert 2: 変更の即時反映検証
            // ==========================================
            Assert.That(isAfterMainRetrieved, Is.True);
            Assert.That(isAfterInvRetrieved, Is.True);

            // メイン画面の在庫状況データにおいて、名称が書き換わっていること
            var afterMainTarget = afterStockData.FirstOrDefault(d => d.ProductName == "特選お茶");
            var oldMainTarget = afterStockData.FirstOrDefault(d => d.ProductName == "お茶");
            Assert.That(afterMainTarget, Is.Not.Null, "メイン画面に新しい商品名が反映されていません。");
            Assert.That(oldMainTarget, Is.Null, "メイン画面に古い商品名が残っています。");

            // 在庫情報確認画面のデータにおいて、名称が書き換わっていること
            var afterInvTarget = inventoryList.FirstOrDefault(d => d.ProductName == "特選お茶");
            var oldInvTarget = inventoryList.FirstOrDefault(d => d.ProductName == "お茶");
            Assert.That(afterInvTarget, Is.Not.Null, "在庫画面に新しい商品名が反映されていません。");
            Assert.That(oldInvTarget, Is.Null, "在庫画面に古い商品名が残っています。");
        }

        [Test]
        public void IT07_異常フォーマットCSVの読込とトランザクション検証()
        {
            // ==========================================
            // 1. Arrange: 読み込み前の正常な状態を構築
            // ==========================================
            // 事前に正常な商品データ(ID:1)を1件登録しておく
            var initialProductCsvData = new List<ProductFileData>
            {
                new ProductFileData { ProductId = 1, ProductName = "正常商品", UnitPrice = 100, Category = "テスト区分" }
            };

            // 正常データの登録実行
            bool isInitialSaved = _dataManager.SetProductMasterFileData(initialProductCsvData);
            Assert.That(isInitialSaved, Is.True, "事前データの登録に失敗しました。");

            // マスタ管理クラスを用いて、現在のDB登録件数を取得・保持しておく
            var masterManager = new MasterViewDataManager();
            masterManager.GetProductMasterData(out List<ProductMasterData> initialMasters);
            int initialRecordCount = initialMasters.Count;

            // ==========================================
            // 2. Act: 不正なデータを含むリストでの読込実行
            // ==========================================
            // 2件目は正常だが、1件目に一意制約違反（既存のID:1と重複）などの異常データを含める
            // ※ 実装環境のDB制約（SQLite等）に引っかかるような不正データを意図的に設定します
            var invalidProductCsvData = new List<ProductFileData>
            {
                new ProductFileData { ProductId = 1, ProductName = "重複エラー商品", UnitPrice = 200, Category = "テスト区分" },
                new ProductFileData { ProductId = 2, ProductName = "新規商品", UnitPrice = 300, Category = "テスト区分" }
            };

            bool isInvalidSaved = false;
            Exception caughtException = null;

            try
            {
                // 不正データの登録を実行
                isInvalidSaved = _dataManager.SetProductMasterFileData(invalidProductCsvData);
            }
            catch (Exception ex)
            {
                // SetProductMasterFileData 内で例外が rethrow される仕様のため、Catch して保持
                caughtException = ex;
            }

            // ==========================================
            // 3. Assert: エラーハンドリングとトランザクションの検証
            // ==========================================
            // 例外がスローされたか、または戻り値として false が返ったこと（= 処理が成功していないこと）を検証
            bool isFailedAsExpected = (isInvalidSaved == false) || (caughtException != null);
            Assert.That(isFailedAsExpected, Is.True, "例外が発生せず、かつ保存処理が true を返しました。エラーとして適切にハンドリングされていません。");

            // DBの状態を確認し、ロールバックされている（＝2件目の「新規商品」も登録されていない）ことを検証
            masterManager.GetProductMasterData(out List<ProductMasterData> afterMasters);

            Assert.That(afterMasters.Count, Is.EqualTo(initialRecordCount), "トランザクションが破綻し、中途半端にデータが登録されています（ロールバックされていません）。");

            // 初期データが上書きされたり破損していないことの確認
            var targetProduct = afterMasters.FirstOrDefault(p => p.ID == 1);
            Assert.That(targetProduct, Is.Not.Null);
            Assert.That(targetProduct.ProductName, Is.EqualTo("正常商品"), "エラー発生時に既存データが不正に上書きされています。");
        }

        [Test]
        public void IT08_ファイル排他制御_ロック_時のエラーハンドリング()
        {
            // ==========================================
            // 1. Arrange: テストデータの準備とファイルの排他ロック
            // ==========================================
            // テスト実行ディレクトリにテスト用のファイルを定義
            string testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "locked_sales.csv");

            // 出力用のダミーデータを用意
            var dummySalesData = new List<DetailSalesData>
            {
                new DetailSalesData { StoreName = "テスト店舗", ProductName = "テスト商品", SalesCount = 1, Sales = 100 }
            };

            // 実体となるCsvFileProcessorを用意
            // ※UI層と繋がったManagerクラス(DetailSalesViewFileManager等)がある場合は、それにプロセッサを注入して使用します
            var fileProcessor = new CsvFileProcessor();

            bool isFileSaved = true;

            // ==========================================
            // 2. Act: ロック状態での保存実行
            // ==========================================
            try
            {
                // FileShare.None を指定してストリームを開き、他プロセス（またはスレッド）からの
                // アクセスを完全にブロックする状態（= ユーザーがExcelで開いている状態）をシミュレート
                using (var lockStream = new FileStream(testFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    // ロックを維持したまま、ファイル保存処理を実行
                    isFileSaved = fileProcessor.SaveFile(testFilePath, dummySalesData);
                }
            }
            finally
            {
                // テスト後のクリーンアップ処理
                if (File.Exists(testFilePath))
                {
                    File.Delete(testFilePath);
                }
            }

            // ==========================================
            // 3. Assert: クラッシュ回避とエラーハンドリングの検証
            // ==========================================
            // CsvFileProcessor.WriteCSV 内で例外が正しく catch され、クラッシュせずに
            // is_success の初期値である false が呼び出し元まで返ってきていることを検証
            Assert.That(isFileSaved, Is.False, "ファイルがロックされているにもかかわらず、保存処理が成功(true)を返しました。");
        }
    }
}