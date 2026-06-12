using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Wordprocessing;
using SalesManagerApp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagerApp.View
{
    /// <summary>
    /// 商品マスタ管理画面フォームクラス
    /// </summary>
    public partial class ProductMasterView : Form
    {
        MasterViewDataManager _manager;

        /// <summary>
        /// 商品マスタ管理画面
        /// </summary>
        public ProductMasterView(Form owner = null)
        {
            InitializeComponent();

            //親画面設定
            if (null != owner) Owner = owner;

            //データ管理クラス生成
            _manager = new MasterViewDataManager();
        }

        /// <summary>
        /// 商品マスタ管理画面読み込み時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductMasterView_Load(object sender, EventArgs e)
        {
            //グリッドビューを作成する
            var result = CreateGrid();

            //作成に失敗したら画面を閉じる
            if (false == result)
            {
                this.Close();
            }
        }

        /// <summary>
        /// データグリッドビュー初期化
        /// </summary>
        /// <returns></returns>
        private bool CreateGrid()
        {
            bool is_success = false;

            try
            {
                //データ作成用
                List<ProductMasterData> lst_product = new List<ProductMasterData>();

                //店舗マスタデータ取得
                _manager.GetProductMasterData(out lst_product);
                //取得したデータをグリッドビューに表示する
                dgv_ProductMaster.DataSource = new BindingList<ProductMasterData>(lst_product);

                //区分列を作成する
                if (dgv_ProductMaster.Columns.Contains(nameof(ProductMasterData.CategoryID)))
                {
                    //区分マスタ取得
                    List<CategoryMasterData> lst_category = new List<CategoryMasterData>();
                    _manager.GetCategoryMasterData(out lst_category);

                    //区分マスタが取得できた場合、列作成
                    if (null != lst_category && 0 < lst_category.Count)
                    {
                        //区分列の列インデックス取得
                        int category_col_idx = dgv_ProductMaster.Columns[nameof(ProductMasterData.CategoryID)].Index;
                        //自動で作成されている区分列は削除
                        dgv_ProductMaster.Columns.Remove(nameof(ProductMasterData.CategoryID));

                        //ドロップダウンリストの列を作成する
                        var cmb_Col = new DataGridViewComboBoxColumn();

                        //列名の設定
                        cmb_Col.Name = nameof(ProductMasterData.CategoryID);
                        //選択されたデータとの紐づけ設定
                        cmb_Col.DataPropertyName = nameof(ProductMasterData.CategoryID);
                        //ヘッダー文字設定
                        cmb_Col.HeaderText = TypeDescriptor.GetProperties(typeof(ProductMasterData))[nameof(ProductMasterData.CategoryID)]?.DisplayName;

                        //取得したデータを列に表示する
                        cmb_Col.DataSource = lst_category;
                        //表示テキスト設定
                        cmb_Col.DisplayMember = nameof(CategoryMasterData.CategoryName);
                        //データ実体設定
                        cmb_Col.ValueMember = nameof(CategoryMasterData.ID);

                        //作成した列をグリッドビューにセット
                        dgv_ProductMaster.Columns.Insert(category_col_idx, cmb_Col);
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resource.MSG_022);
                        return false;
                    }
                }

                //グリッドビュー設定
                //行、列幅自動調整
                dgv_ProductMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgv_ProductMaster.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                //[商品名]列を余剰分引き伸ばし
                dgv_ProductMaster.Columns[nameof(ProductMasterData.ProductName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //ID列を読み取り専用に設定
                dgv_ProductMaster.Columns[nameof(ProductMasterData.ID)].ReadOnly = true;

                is_success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
            }

            return is_success;
        }

        /// <summary>
        /// 保存ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            //グリッドに登録されているデータを取得
            var list = dgv_ProductMaster.DataSource as BindingList<ProductMasterData>;
            if (null == list) return;

            bool result = _manager.SetProductMasterData(list.ToList());

            if (true == result)
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
        /// 新規行作成 デフォルト値取得時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_ProductMaster_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            //グリッドに登録されているデータを取得
            var list = dgv_ProductMaster.DataSource as BindingList<ProductMasterData>;

            int id = 1;
            //データがある場合
            if (list != null && list.Count > 0)
            {
                //IDの最大値 +1した値が次のID
                id = list.Max(product => product.ID) + 1;
            }

            //ID列に値をセット
            e.Row.Cells[nameof(ProductMasterData.ID)].Value = id;
            e.Row.Cells[nameof(ProductMasterData.CategoryID)].Value = 1;
        }

        /// <summary>
        /// 不正値検出時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_ProductMaster_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
        private void dgv_ProductMaster_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // e.FormattedValue が null の場合を考慮して修正
            if (e.FormattedValue == null || string.IsNullOrEmpty(e.FormattedValue.ToString())) return;

            //グリッドに登録されているデータを取得
            var list = dgv_ProductMaster.DataSource as BindingList<ProductMasterData>;
            if (null == list) return;

            //入力値を取得
            var input_data = e.FormattedValue.ToString();

            //登録データから入力値が重複しているデータがあるか検索
            bool isDuplicate = list.Where((product, idx) => idx != e.RowIndex).Any(product => product.ProductName == input_data);
            if (isDuplicate)
            {
                //重複ありの場合エラー
                MessageBox.Show("データ重複");
                e.Cancel = true;
                return;
            }
        }
    }
}
