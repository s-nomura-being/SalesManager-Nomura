using DocumentFormat.OpenXml.Drawing.Charts;
using SalesManagerApp.Data;
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
    /// 店舗マスタ管理画面フォームクラス
    /// </summary>
    public partial class StoreMasterView : Form
    {
        MasterViewDataManager _manager;

        /// <summary>
        /// 店舗マスタ管理画面
        /// </summary>
        public StoreMasterView(Form owner = null)
        {
            InitializeComponent();

            //親画面設定
            if (null != owner) Owner = owner;

            //データ管理クラス生成
            _manager = new MasterViewDataManager();
        }

        /// <summary>
        /// 店舗マスタ管理画面読み込み時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StoreMasterView_Load(object sender, EventArgs e)
        {
            //データ作成用
            List<StoreMasterData> lst_store = new List<StoreMasterData>();
            //店舗マスタデータ取得
            _manager.GetStoreMasterData(out lst_store);
            //取得したデータをグリッドビューに表示する
            dgv_StoreMaster.DataSource = new BindingList<StoreMasterData>(lst_store);

            //グリッドビュー設定
            //行、列幅自動調整
            dgv_StoreMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv_StoreMaster.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //[店舗名]列を余剰分引き伸ばし
            dgv_StoreMaster.Columns[nameof(StoreMasterData.StoreName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //ID列を読み取り専用に設定
            dgv_StoreMaster.Columns[nameof(StoreMasterData.ID)].ReadOnly = true;
        }

        /// <summary>
        /// 保存ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            //グリッドに登録されているデータを取得
            var list = dgv_StoreMaster.DataSource as BindingList<StoreMasterData>;
            if (null == list) return;

            bool result = _manager.SetStoreMasterData(list.ToList());

            if(true == result)
            {
                MessageBox.Show("Save");

                //ファイル出力命令
            }
            else
            {
                MessageBox.Show("Save Errrrr");
            }
        }

        /// <summary>
        /// キャンセルボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 不正値検出時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_StoreMaster_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (null != e.Exception)
            {
                MessageBox.Show("error", "err");

                e.ThrowException = false;
                e.Cancel = true;
            }
        }

        /// <summary>
        /// セル 入力値検証時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_StoreMaster_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // e.FormattedValue が null の場合を考慮して修正
            if (e.FormattedValue == null || string.IsNullOrEmpty(e.FormattedValue.ToString())) return;

            //グリッドに登録されているデータを取得
            var list = dgv_StoreMaster.DataSource as BindingList<StoreMasterData>;
            if (null == list) return;

            //入力値を取得
            var input_data = e.FormattedValue.ToString();

            //登録データから入力値が重複しているデータがあるか検索
            bool isDuplicate = list.Where((store, idx) => idx != e.RowIndex).Any(store => store.StoreName == input_data);
            if (isDuplicate)
            {
                //重複ありの場合エラー
                MessageBox.Show("データ重複");
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// 新規行作成 デフォルト値取得時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_StoreMaster_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            //グリッドに登録されているデータを取得
            var list = dgv_StoreMaster.DataSource as BindingList<StoreMasterData>;

            int id = 1;
            //データがある場合
            if (list != null && list.Count > 0)
            {
                //IDの最大値 +1した値が次のID
                id = list.Max(product => product.ID) + 1;
            }

            //ID列に値をセット
            e.Row.Cells[nameof(StoreMasterData.ID)].Value = id;
        }
    }
}
