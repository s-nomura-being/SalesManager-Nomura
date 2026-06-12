using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using SalesManagerApp.Data;
using SalesManagerApp.DBControl;
using SalesManagerApp.View.ViewControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SalesManagerApp.View.TotalSalesView;

namespace SalesManagerApp.View
{
    /// <summary>
    /// 在庫情報確認画面フォームクラス
    /// </summary>
    public partial class InventoryInfoView : Form
    {
        /// <summary>データ管理オブジェクト</summary>
        InventoryViewDataManager _manager;
        /// <summary>絞り込み条件画面</summary>
        FilterControlView _filterView;

        /// <summary>グリッド並び替えフラグ</summary>
        private bool isAscending = true;

        /// <summary>
        /// 在庫情報確認画面
        /// </summary>
        public InventoryInfoView(Form owner = null)
        {
            InitializeComponent();

            //親画面設定
            if (null != owner) Owner = owner;

            //データ管理クラス生成
            _manager = new InventoryViewDataManager();

            //各マスタの名称をリストで取得
            var lst_store = new List<string>();
            var lst_product = new List<string>();
            var lst_category = new List<string>();
            _manager.GetNameData(out lst_store, out lst_product, out lst_category);

            #region 絞り込み条件画面生成
            //パネル初期化
            pnl_Filter.Controls.Clear();
            //絞り込み条件画面生成、名称リストを渡す
            _filterView = new FilterControlView(lst_store, lst_product, lst_category);
            //適用ボタンイベント登録
            _filterView.Click_Apply += this.Filter_Click_Apply;
            //作成した画面をパネルに貼り付け
            pnl_Filter.Controls.Add(_filterView);
            #endregion

            #region グリッドビュー設定
            //データ作成用
            List<InventoryListData> lst_inventory = new List<InventoryListData>();
            //ヘッダーを作成するために、データをグリッドビューに表示する
            dgv_InventoryList.DataSource = lst_inventory;
            //行、列幅自動調整
            dgv_InventoryList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv_InventoryList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //[累計売上金額]列を余剰分引き伸ばし
            dgv_InventoryList.Columns[nameof(InventoryListData.ProductName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            #endregion
        }

        /// <summary>
        /// 適用ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter_Click_Apply(object? sender, EventArgs e)
        {
            //絞り込み条件画面からチェック済みのリストを取得する
            var lst_store = new List<string>();
            var lst_product = new List<string>();
            var lst_category = new List<string>();
            _filterView.GetCheckedList(out lst_store, out lst_product, out lst_category);

            //絞り込み条件生成
            FilterInfo filter = new FilterInfo(lst_store, lst_product, lst_category);

            //データ表示
            ShowData(filter);
        }

        /// <summary>
        /// 在庫情報確認画面読み込み時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InventoryInfoView_Load(object sender, EventArgs e)
        {
            //データ表示
            ShowData();
        }

        /// <summary>
        /// 一覧表出力ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Output_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 在庫一覧出力メニュー押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mn_Output_Click(object sender, EventArgs e)
        {

        }

        private void Output()
        {
            //ファイルダイアログ表示
            string sales_path = SelectOutputDir();

            //ファイル読込命令

        }

        /// <summary>
        /// ファイル出力先選択
        /// </summary>
        /// <returns></returns>
        private string SelectOutputDir()
        {
            string select_path = string.Empty;

            // 1. ファイル保存ダイアログを作成
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "保存先を指定してください";

                // デフォルトのファイル名を設定
                saveFileDialog.FileName = "在庫一覧" + DateTime.Now.ToString("yyyyMMdd");

                // 保存できるファイルの種類（拡張子）を制限する
                saveFileDialog.Filter = "エクセルファイル (*.xlsx)|*.xlsx";

                // ダイアログを表示して、ユーザーが「保存」ボタンを押した場合のみ処理する
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // ユーザーが指定した保存先のフルパス（C:\...\aaa.csv など）を取得
                    select_path = saveFileDialog.FileName;
                }
            }

            return select_path;
        }

        /// <summary>
        /// 一覧にデータを表示
        /// </summary>
        /// <param name="filter">絞り込み条件</param>
        private void ShowData(FilterInfo? filter = null)
        {
            //データ作成用
            List<InventoryListData> lst_inventory = new List<InventoryListData>();

            //売上データを取得
            _manager.GetInventoryViewData(out lst_inventory, filter);

            //取得したデータをグリッドビューに表示する
            dgv_InventoryList.DataSource = lst_inventory;
        }

        /// <summary>
        /// グリッドヘッダークリック時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_InventoryList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //クリックした列の並び替え
            dgv_InventoryList.SordGridColumn<InventoryListData>(e.ColumnIndex, ref isAscending);
        }

        /// <summary>
        /// 行描画時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_InventoryList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //行番号を入れる
            dgv_InventoryList.InsertRowHeaderNumber(e);
        }
    }
}
