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
using System.Xml.Serialization;

namespace SalesManagerApp.View
{
    public partial class FilterControlView : UserControl
    {
        public event EventHandler Click_Apply;

        private CheckListControl _storelist_control;
        private CheckListControl _productlist_control;
        private CheckListControl _categorylist_control;

        private List<string> StoreList { get; }
        private List<string> ProductList { get; }
        private List<string> CategoryList { get; }


        public FilterControlView(List<string> storelist, List<string> productlist, List<string> categorylist)
        {
            InitializeComponent();

            //タグ付け
            lbl_AllSelectStore.Tag = E_FilterType.Store;
            lbl_AllSelectProduct.Tag = E_FilterType.Product;
            lbl_AllSelectCategory.Tag = E_FilterType.Category;
            lbl_AllDeselectStore.Tag = E_FilterType.Store;
            lbl_AllDeselectProduct.Tag = E_FilterType.Product;
            lbl_AllDeselectCategory.Tag = E_FilterType.Category;

            //マスタデータ取得
            StoreList = new List<string>(storelist);
            ProductList = new List<string>(productlist);
            CategoryList = new List<string>(categorylist);

            //チェックリスト操作クラス生成
            _storelist_control = new CheckListControl(chlst_Store, txt_Store, StoreList);
            _productlist_control = new CheckListControl(chlst_Product, txt_Product, ProductList);
            _categorylist_control = new CheckListControl(chlst_Category, txt_Category, CategoryList);
        }

        private void FilterControl_Load(object sender, EventArgs e)
        {
            //各チェックリスト初期化
            chlst_Store.Items.Clear();
            chlst_Product.Items.Clear();
            chlst_Category.Items.Clear();

            //処理が重くなるため、データ追加中の描画を止める
            chlst_Store.BeginUpdate();
            chlst_Product.BeginUpdate();
            chlst_Category.BeginUpdate();

            //マスタデータをチェックリストに追加
            foreach (var store in StoreList)
            {
                //ONを初期状態として追加
                chlst_Store.Items.Add(store, true);
            }
            foreach (var product in ProductList)
            {
                //ONを初期状態として追加
                chlst_Product.Items.Add(product, true);
            }
            foreach (var category in CategoryList)
            {
                //ONを初期状態として追加
                chlst_Category.Items.Add(category, true);
            }

            //止めてた描画を再開
            chlst_Store.EndUpdate();
            chlst_Product.EndUpdate();
            chlst_Category.EndUpdate();
        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            Click_Apply?.Invoke(sender, e);
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            txt_Store.Text = string.Empty;
            txt_Product.Text = string.Empty;
            txt_Category.Text = string.Empty;

            _storelist_control.SetAllChecked(true);
            _productlist_control.SetAllChecked(true);
            _categorylist_control.SetAllChecked(true);
        }

        private void AllSelect(object sender, EventArgs e)
        {
            if(sender is Label label)
            {
                if(label.Tag is E_FilterType filter_type)
                {
                    switch (filter_type)
                    {
                        case E_FilterType.Store:
                            _storelist_control.SetAllChecked(true);
                            break;

                        case E_FilterType.Product:
                            _productlist_control.SetAllChecked(true);
                            break;

                        case E_FilterType.Category:
                            _categorylist_control.SetAllChecked(true);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private void AllDeselect(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                if (label.Tag is E_FilterType filter_type)
                {
                    switch (filter_type)
                    {
                        case E_FilterType.Store:
                            _storelist_control.SetAllChecked(false);
                            break;

                        case E_FilterType.Product:
                            _productlist_control.SetAllChecked(false);
                            break;

                        case E_FilterType.Category:
                            _categorylist_control.SetAllChecked(false);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public void GetCheckedList(out List<string> lst_store, out List<string> lst_product, out List<string> lst_category)
        {
            lst_store = _storelist_control.GetCheckedNames();
            lst_product = _productlist_control.GetCheckedNames();
            lst_category = _categorylist_control.GetCheckedNames();
        }

        private enum E_FilterType
        {
            Store = 0,
            Product,
            Category,
        }
    }
}
