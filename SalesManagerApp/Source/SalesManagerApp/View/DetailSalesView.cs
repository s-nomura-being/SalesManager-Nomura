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

namespace SalesManagerApp.View
{
    /// <summary>
    /// 売上、販売実績一覧画面フォームクラス
    /// </summary>
    public partial class DetailSalesView : Form
    {
        /// <summary>データ管理オブジェクト</summary>
        DetailSalesViewDataManager _manager;
        /// <summary>絞り込み条件画面</summary>
        FilterControlView _filterView;

        /// <summary>グリッド並び替えフラグ</summary>
        private bool isAscending = true;

        /// <summary>
        /// 売上、販売実績一覧画面
        /// </summary>
        public DetailSalesView(Form owner = null, DateTime? start_dt = null, DateTime? end_dt = null)
        {
            InitializeComponent();

            //親画面設定
            if (null != owner) Owner = owner;

            //開始、終了日時セット
            if (null != start_dt) dtp_Start.Value = (DateTime)start_dt;
            if (null != end_dt) dtp_End.Value = (DateTime)end_dt;

            //データ管理クラス生成
            _manager = new DetailSalesViewDataManager();

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
            List<DetailSalesData> lst_detail = new List<DetailSalesData>();
            //ヘッダーを作るために、空のデータでグリッドを作成
            dgv_DetailList.DataSource = lst_detail;

            //行、列幅自動調整
            dgv_DetailList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv_DetailList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //[累計売上金額]列を余剰分引き伸ばし
            dgv_DetailList.Columns[nameof(DetailSalesData.ProductName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            #endregion
        }

        /// <summary>
        /// 売上、販売実績一覧画面読み込み時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailSalesView_Load(object sender, EventArgs e)
        {
            Filter_Click_Apply(sender, e);
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
            FilterInfo filter;
            if (true == rdo_Period.Checked)
            {
                filter = new FilterInfo(lst_store, lst_product, lst_category, dtp_Start.Value, dtp_End.Value);
            }
            else
            {
                filter = new FilterInfo(lst_store, lst_product, lst_category);
            }

            //データ表示
            ShowData(filter);
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
        /// 実績一覧出力メニュー押下
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
                saveFileDialog.FileName = "売上販売実績" + DateTime.Now.ToString("yyyyMMdd");

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
        /// 期間ラジオボタン チェック状態変化時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdo_Period_CheckedChanged(object sender, EventArgs e)
        {
            //カレンダーの有効無効を切り替える
            dtp_Start.Enabled = rdo_Period.Checked;
            dtp_End.Enabled = rdo_Period.Checked;
        }

        /// <summary>
        /// 一覧にデータを表示
        /// </summary>
        /// <param name="filter">絞り込み条件</param>
        private void ShowData(FilterInfo? filter = null)
        {
            //データ作成用
            List<DetailSalesData> lst_detail = new List<DetailSalesData>();

            //売上データを取得
            _manager.GetDetailSalesViewData(out lst_detail, filter);

            //取得したデータをグリッドビューに表示する
            dgv_DetailList.DataSource = lst_detail;
        }

        /// <summary>
        /// グリッドヘッダークリック時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_DetailList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //クリックした列の並び替え
            dgv_DetailList.SordGridColumn<DetailSalesData>(e.ColumnIndex, ref isAscending);
        }

        /// <summary>
        /// 行描画時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_DetailList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //行番号を入れる
            dgv_DetailList.InsertRowHeaderNumber(e);
        }
    }
}
