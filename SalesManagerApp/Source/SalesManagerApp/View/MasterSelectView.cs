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
    /// マスタ選択画面フォームクラス
    /// </summary>
    public partial class MasterSelectView : Form
    {
        /// <summary>
        /// マスタ選択画面
        /// </summary>
        public MasterSelectView(Form owner = null)
        {
            InitializeComponent();

            //親画面設定
            if (null != owner) Owner = owner;
        }

        /// <summary>
        /// 店舗マスタ編集ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_EditStore_Click(object sender, EventArgs e)
        {
            //店舗マスタ管理画面生成・表示
            var view_storemaster = new StoreMasterView(this);
            view_storemaster.ShowDialog();
        }

        /// <summary>
        /// 商品マスタ編集ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_EditProduct_Click(object sender, EventArgs e)
        {
            //商品マスタ管理画面生成・表示
            var view_productmaster = new ProductMasterView(this);
            view_productmaster.ShowDialog();
        }

        /// <summary>
        /// 区分マスタ編集ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_EditCategory_Click(object sender, EventArgs e)
        {
            //区分マスタ管理画面生成・表示
            var view_categorymaster = new CategoryMasterView(this);
            view_categorymaster.ShowDialog();
        }
    }
}
