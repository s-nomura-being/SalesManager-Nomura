using SalesManagerApp.Data;
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
    /// 売上集計画面フォームクラス
    /// </summary>
    public partial class TotalSalesView : Form
    {
        /// <summary>データ管理オブジェクト</summary>
        private TotalSalesViewDataManager _manager;

        /// <summary>グリッド並び替えフラグ</summary>
        private bool isAscending = true;

        /// <summary>
        /// 集計期間レコード
        /// </summary>
        /// <param name="Display">表示用データ</param>
        /// <param name="Value">実際の値</param>
        public record PeriodItem(string Display, E_Period Value);

        /// <summary>
        /// 売上集計画面
        /// </summary>
        public TotalSalesView(Form owner = null)
        {
            InitializeComponent();

            //データ管理クラス生成
            _manager = new TotalSalesViewDataManager();

            //親画面設定
            if (null != owner) Owner = owner;

            //集計期間をコンボボックスに設定
            var period_list = new[]
            {
                //new PeriodItem("全期間", E_Period.None),
                new PeriodItem("週間", E_Period.Weekly),
                new PeriodItem("月間", E_Period.Monthly),
                new PeriodItem("年間", E_Period.Yearly),
            };
            cmb_Period.DataSource = period_list;
            //表示用のデータと処理用のデータを設定する
            cmb_Period.DisplayMember = nameof(PeriodItem.Display);
            cmb_Period.ValueMember = nameof(PeriodItem.Value);

            //データ作成用
            List<TotalSalesData> lst_total = new List<TotalSalesData>();
            //取得したデータをグリッドビューに表示する
            dgv_TotalSales.DataSource = lst_total;

            //グリッドビュー設定
            //行、列幅自動調整
            dgv_TotalSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv_TotalSales.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            //[累計売上金額]列を余剰分引き伸ばし
            dgv_TotalSales.Columns[nameof(TotalSalesData.TotalSales)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        /// <summary>
        /// 売上集計画面読み込み時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TotalSalesView_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 集計実行ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CalcTotal_Click(object sender, EventArgs e)
        {
            //TODO：入力チェック

            //コンボボックスで選択中のオブジェクトを集計期間に変換する
            if (cmb_Period.SelectedItem is PeriodItem period)
            {
                //データ作成用
                List<TotalSalesData> lst_total = new List<TotalSalesData>();

                //指定された期間、日時で集計データを取得する
                _manager.GetTotalSalesViewData(period.Value, out lst_total);

                //取得したデータをグリッドビューに表示する
                dgv_TotalSales.DataSource = lst_total;
            }
        }

        /// <summary>
        /// 詳細表示ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ShowDetail_Click(object sender, EventArgs e)
        {
            //絞り込み条件作成

            if (null == dgv_TotalSales.CurrentRow)
            {
                //表示
                var view_detail = new DetailSalesView(this);
                view_detail.Show();
            }
            else
            {
                var row_data = dgv_TotalSales.CurrentRow.DataBoundItem as TotalSalesData;

                if (null != row_data)
                {
                    //表示
                    var view_detail = new DetailSalesView(this, row_data.StartDate, row_data.EndDate);
                    view_detail.Show();
                }
            }

        }

        /// <summary>
        /// 一覧表出力ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Output_Click(object sender, EventArgs e)
        {
            //出力処理
            Output();
        }

        /// <summary>
        /// 集計一覧出力メニュー押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mn_OutputTotalSales_Click(object sender, EventArgs e)
        {
            //出力処理
            Output();
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
                saveFileDialog.FileName = "集計結果" + DateTime.Now.ToString("yyyyMMdd");

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
        /// グリッドヘッダークリック時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_TotalSales_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //クリックした列の並び替え
            dgv_TotalSales.SordGridColumn<TotalSalesData>(e.ColumnIndex, ref isAscending);
        }

        /// <summary>
        /// 行描画時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_TotalSales_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //行番号を入れる
            dgv_TotalSales.InsertRowHeaderNumber(e);
        }
    }
}
