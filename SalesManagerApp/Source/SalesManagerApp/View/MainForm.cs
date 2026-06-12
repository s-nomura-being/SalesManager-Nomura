using SalesManagerApp.Data;
using SalesManagerApp.View.ViewControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagerApp.View
{
    /// <summary>
    /// メイン画面フォームクラス
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>データ管理オブジェクト</summary>
        MainViewDataManager _manager;

        /// <summary>先週の商品売上グリッド 並び替えフラグ</summary>
        private bool isAscending_LastWeek = true;

        /// <summary>在庫状況グリッド 並び替えフラグ</summary>
        private bool isAscending_StockStatus = true;

        /// <summary>
        /// メイン画面
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //データ管理クラス生成
            _manager = new MainViewDataManager();
        }

        /// <summary>
        /// メイン画面読み込み時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //ファイル読込命令

            //表示用データ作成
            List<LastWeekProductSalesData> lst_Lastweekdata = new List<LastWeekProductSalesData>();
            List<StockStatusData> lst_stockdata = new List<StockStatusData>();
            _manager.GetMainViewData(out lst_Lastweekdata, out lst_stockdata);

            //表示
            dgv_LastWeekSales.DataSource = lst_Lastweekdata;
            dgv_InventoryStatus.DataSource = lst_stockdata;

            //グリッドビュー設定初期化
            dgv_LastWeekSales.Init(nameof(LastWeekProductSalesData.ProductName));
            dgv_InventoryStatus.Init(nameof(StockStatusData.ProductName));
        }

        /// <summary>
        /// 在庫状況グリッドビュー セル書式設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_InventoryStatus_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //イベント発生元オブジェクトからデータグリッドビューに変換
            var dgv = (DataGridView)sender;

            //ヘッダー列、ヘッダー行は設定対象外
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            //発生元の列が発注通知の場合
            if (dgv.Columns[e.ColumnIndex].Name == nameof(StockStatusData.Notice))
            {
                //発生元の行データを在庫状況データに変換
                var rowData = dgv.Rows[e.RowIndex].DataBoundItem as StockStatusData;

                if (rowData != null)
                {
                    //行データの在庫状況タイプで分岐
                    switch (rowData.StatusType)
                    {
                        //何もなし
                        case E_StockStatusType.None:
                            //通常の色設定
                            e.CellStyle.BackColor = dgv.DefaultCellStyle.BackColor;
                            e.CellStyle.ForeColor = dgv.DefaultCellStyle.ForeColor;
                            break;

                        //在庫少
                        case E_StockStatusType.Low:
                            //背景色、文字色を変える
                            e.CellStyle.BackColor = Color.Yellow;
                            e.CellStyle.ForeColor = dgv.DefaultCellStyle.ForeColor;
                            break;

                        //要発注
                        case E_StockStatusType.Must:
                            //背景色、文字色を変える
                            e.CellStyle.BackColor = Color.Red;
                            e.CellStyle.ForeColor = Color.White;
                            break;

                        //デフォルト
                        default:
                            //通常の色設定
                            e.CellStyle.BackColor = dgv.DefaultCellStyle.BackColor;
                            e.CellStyle.ForeColor = dgv.DefaultCellStyle.ForeColor;
                            break;
                    }
                }
            }
        }

        #region ボタンイベント
        /// <summary>
        /// 売上集計ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TotalSales_Click(object sender, EventArgs e)
        {
            //売上集計画面表示
            ShowTotalSalesView();
        }

        /// <summary>
        /// 在庫一覧ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_InventoryInfo_Click(object sender, EventArgs e)
        {
            //在庫情報確認画面表示
            ShowInventoryInfoView();
        }

        /// <summary>
        /// マスタ設定ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MasterSetting_Click(object sender, EventArgs e)
        {
            //マスタ選択画面表示
            ShowMasterSelectView();
        }

        /// <summary>
        /// アプリ設定ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AppSetting_Click(object sender, EventArgs e)
        {
            //アプリ設定画面表示
            ShowAppSettingView();
        }
        #endregion

        #region メニューイベント
        /// <summary>
        /// 商品マスタファイル読込メニュー押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mn_ReadProductMaster_Click(object sender, EventArgs e)
        {
            //ファイルダイアログ表示
            string product_path = SearchAPIFile();

            //ファイル読込命令

        }

        /// <summary>
        /// 在庫データ読込メニュー押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mn_ReadInventory_Click(object sender, EventArgs e)
        {
            //ファイルダイアログ表示
            string inventory_path = SearchAPIFile();

            //ファイル読込命令

        }

        /// <summary>
        /// 売上データ読込メニュー押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mn_ReadSales_Click(object sender, EventArgs e)
        {
            //ファイルダイアログ表示
            string sales_path = SearchAPIFile();

            //ファイル読込命令

        }

        /// <summary>
        /// APIファイルを開く
        /// </summary>
        /// <returns>選択したファイルのフルパス</returns>
        private string SearchAPIFile()
        {
            string select_path = string.Empty;

            //ファイルを開くダイアログを作成
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //ダイアログのタイトルを設定
                openFileDialog.Title = "ファイルを選択してください";

                //選択できるファイルの種類（拡張子）を制限する
                openFileDialog.Filter = "CSVファイル (*.csv)|*.csv";

                //最初に開くフォルダを指定
                openFileDialog.InitialDirectory = @"C:\";

                //ダイアログを画面に表示、OKなら選択パスのファイルを返す
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //選択したファイルのフルパスを取得
                    select_path = openFileDialog.FileName;
                }
            }

            return select_path;
        }

        private bool ReadAPIFile(string path)
        {
            bool result = false;

            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
            }

            return result;
        }

        /// <summary>
        /// 売上集計メニュー押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mn_ShowTotalSales_Click(object sender, EventArgs e)
        {
            //売上集計画面表示
            ShowTotalSalesView();
        }

        /// <summary>
        /// 在庫一覧メニュー押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mn_ShowInventory_Click(object sender, EventArgs e)
        {
            //在庫情報確認画面表示
            ShowInventoryInfoView();
        }

        /// <summary>
        /// マスタ設定メニュー押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mn_ShowMasterSetting_Click(object sender, EventArgs e)
        {
            //マスタ選択画面表示
            ShowMasterSelectView();
        }

        /// <summary>
        /// アプリ設定メニュー押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mn_ShowAppSetting_Click(object sender, EventArgs e)
        {
            //アプリ設定画面表示
            ShowAppSettingView();
        }
        #endregion

        /// <summary>
        /// 売上集計画面表示
        /// </summary>
        private void ShowTotalSalesView()
        {
            //売上集計画面生成・表示
            var view_totalsales = new TotalSalesView(this);
            view_totalsales.Show();
        }

        /// <summary>
        /// 在庫情報確認画面表示
        /// </summary>
        private void ShowInventoryInfoView()
        {
            //在庫情報確認画面生成・表示
            var view_inventoryinfo = new InventoryInfoView(this);
            view_inventoryinfo.Show();
        }

        /// <summary>
        /// マスタ選択画面表示
        /// </summary>
        private void ShowMasterSelectView()
        {
            //マスタ選択画面生成・表示
            var view_masterselect = new MasterSelectView(this);
            view_masterselect.ShowDialog();
        }

        /// <summary>
        /// アプリ設定画面表示
        /// </summary>
        private void ShowAppSettingView()
        {
            //アプリ設定画面生成・表示
            var view_appsetting = new AppSettingView(this);
            view_appsetting.ShowDialog();
        }

        /// <summary>
        /// 先週の商品売上グリッド ヘッダークリック時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_LastWeekSales_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //クリックした列の並び替え
            dgv_LastWeekSales.SordGridColumn<LastWeekProductSalesData>(e.ColumnIndex, ref isAscending_LastWeek);
        }

        /// <summary>
        /// 在庫状況グリッド ヘッダークリック時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_InventoryStatus_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //クリックした列の並び替え
            dgv_InventoryStatus.SordGridColumn<StockStatusData>(e.ColumnIndex, ref isAscending_StockStatus);
        }

        /// <summary>
        /// 先週の商品売上グリッド 行描画時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_LastWeekSales_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //行番号を入れる
            dgv_InventoryStatus.InsertRowHeaderNumber(e);
        }

        /// <summary>
        /// 在庫状況グリッド 行描画時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_InventoryStatus_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //行番号を入れる
            dgv_InventoryStatus.InsertRowHeaderNumber(e);
        }
    }
}
