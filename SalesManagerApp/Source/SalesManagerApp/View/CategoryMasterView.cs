using DocumentFormat.OpenXml.Drawing.Charts;
using FileControlLib.Processor;
using SalesManagerApp.Data;
using SalesManagerApp.FileIO;
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
    /// 区分マスタ管理画面フォームクラス
    /// </summary>
    public partial class CategoryMasterView : Form
    {
        MasterViewDataManager _manager;

        /// <summary>
        /// 区分マスタ管理画面
        /// </summary>
        public CategoryMasterView(Form owner = null)
        {
            InitializeComponent();

            //親画面設定
            if (null != owner) Owner = owner;

            //データ管理クラス生成
            _manager = new MasterViewDataManager();
        }

        /// <summary>
        /// 区分マスタ管理画面読み込み時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryMasterView_Load(object sender, EventArgs e)
        {
            //データ作成用
            List<CategoryMasterData> lst_category = new List<CategoryMasterData>();
            //区分マスタデータ取得
            _manager.GetCategoryMasterData(out lst_category);
            //取得したデータをグリッドビューに表示する
            dgv_CategoryMaster.DataSource = new BindingList<CategoryMasterData>(lst_category);

            //グリッドビュー設定
            //行、列幅自動調整
            dgv_CategoryMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgv_CategoryMaster.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //[区分名]列を余剰分引き伸ばし
            dgv_CategoryMaster.Columns[nameof(CategoryMasterData.CategoryName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //ID列を読み取り専用に設定
            dgv_CategoryMaster.Columns[nameof(CategoryMasterData.ID)].ReadOnly = true;
        }

        /// <summary>
        /// 保存ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            //グリッドに登録されているデータを取得
            var list = dgv_CategoryMaster.DataSource as BindingList<CategoryMasterData>;
            if (null == list) return;

            bool result = _manager.SetCategoryMasterData(list.ToList());

            if (true == result)
            {
                //ファイル出力命令
                Output();

                MessageBox.Show("Save");
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
        /// 新規行作成 デフォルト値取得時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_CategoryMaster_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            //グリッドに登録されているデータを取得
            var list = dgv_CategoryMaster.DataSource as BindingList<CategoryMasterData>;

            int id = 1;
            //データがある場合
            if (list != null && list.Count > 0)
            {
                //IDの最大値 +1した値が次のID
                id = list.Max(product => product.ID) + 1;
            }

            //ID列に値をセット
            e.Row.Cells[nameof(CategoryMasterData.ID)].Value = id;
        }

        /// <summary>
        /// 不正値検出時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_CategoryMaster_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
        private void dgv_CategoryMaster_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // e.FormattedValue が null の場合を考慮して修正
            if (e.FormattedValue == null || string.IsNullOrEmpty(e.FormattedValue.ToString())) return;

            //グリッドに登録されているデータを取得
            var list = dgv_CategoryMaster.DataSource as BindingList<CategoryMasterData>;
            if (null == list) return;

            //入力値を取得
            var input_data = e.FormattedValue.ToString();

            //登録データから入力値が重複しているデータがあるか検索
            bool isDuplicate = list.Where((category, idx) => idx != e.RowIndex).Any(category => category.CategoryName == input_data);
            if (isDuplicate)
            {
                //重複ありの場合エラー
                MessageBox.Show("データ重複");
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// ファイル出力
        /// </summary>
        /// <returns></returns>
        private bool Output()
        {
            bool result = false;

            try
            {
                //グリッドに登録されているデータを取得
                var list = dgv_CategoryMaster.DataSource as BindingList<CategoryMasterData>;
                if (null == list) return false;

                //ファイル保存処理
                var file_manager = new CategoryMasterViewFileManager(new ExcelFileProcessor());
                file_manager.SaveFile(list.ToList());

                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
                throw;
            }

            return result;
        }
    }
}
