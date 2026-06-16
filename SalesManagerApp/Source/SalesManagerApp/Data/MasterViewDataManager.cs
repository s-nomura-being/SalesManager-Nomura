using DatabaseLib.Table;
using SalesManagerApp.DBControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerApp.Data
{
    /// <summary>
    /// 各マスタ管理画面で使用するデータを管理するクラス
    /// </summary>
    public class MasterViewDataManager
    {
        /// <summary>店舗マスタコントロール</summary>
        StoreMasterControl _storectrl;
        /// <summary>商品マスタコントロール</summary>
        ProductMasterControl _productctrl;
        /// <summary>区分マスタコントロール</summary>
        CategoryMasterControl _categoryctrl;

        /// <summary>
        /// 各マスタ管理画面用データ管理クラス
        /// </summary>
        public MasterViewDataManager()
        {
            //コントロールの初期化
            _storectrl = new StoreMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            _productctrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
            _categoryctrl = new CategoryMasterControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
        }

        /// <summary>
        /// 店舗マスタデータ取得
        /// </summary>
        /// <param name="lst_data">店舗マスタデータリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool GetStoreMasterData(out List<StoreMasterData> lst_data)
        {
            bool result = false;

            //データ作成用
            lst_data = new List<StoreMasterData>();

            try
            {
                //店舗マスタデータを取得
                List<StoreMaster> lst_store = new List<StoreMaster>();
                result = _storectrl.GetStoreMaster(out lst_store);

                foreach (var store in lst_store)
                {
                    StoreMasterData data = new StoreMasterData();

                    data.ID = store.Id;
                    data.StoreName = store.Name;

                    lst_data.Add(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
            }

            return result;
        }

        /// <summary>
        /// 店舗マスタデータ登録
        /// </summary>
        /// <param name="lst_data">店舗マスタデータリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetStoreMasterData(List<StoreMasterData> lst_data)
        {
            bool result = false;

            try
            {
                //画面データをテーブルデータに変換
                List<StoreMaster> lst_master = new List<StoreMaster>();
                foreach (var data in lst_data)
                {
                    StoreMaster master = new StoreMaster();
                    //ID
                    master.Id = data.ID;
                    //店舗名
                    master.Name = data.StoreName;
                    lst_master.Add(master);
                }

                //テーブルデータを登録
                result = _storectrl.SetStoreMaster(lst_master);
            }
            catch (Exception e)
            {

            }

            return result;
        }

        /// <summary>
        /// 商品マスタデータ取得
        /// </summary>
        /// <param name="lst_data">商品マスタデータリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool GetProductMasterData(out List<ProductMasterData> lst_data)
        {
            bool result = false;

            //データ作成用
            lst_data = new List<ProductMasterData>();

            try
            {
                //商品マスタデータを取得
                List<ProductMaster> lst_product = new List<ProductMaster>();
                result = _productctrl.GetProductMaster(out lst_product);

                foreach (var product in lst_product)
                {
                    ProductMasterData data = new ProductMasterData();

                    data.ID = product.Id;
                    data.ProductName = product.Name;
                    data.UnitPrice = product.UnitPrice;
                    data.CategoryID = product.Category.Id;

                    lst_data.Add(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
            }

            return result;
        }

        /// <summary>
        /// 商品マスタデータ登録
        /// </summary>
        /// <param name="lst_data">商品マスタデータリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetProductMasterData(List<ProductMasterData> lst_data)
        {
            bool result = false;

            try
            {
                //画面データをテーブルデータに変換
                List<ProductMaster> lst_master = new List<ProductMaster>();
                foreach (var data in lst_data)
                {
                    ProductMaster master = new ProductMaster();
                    master.Id = data.ID;
                    master.Name = data.ProductName;
                    master.UnitPrice = data.UnitPrice;
                    master.CategoryId = data.CategoryID;

                    lst_master.Add(master);
                }

                //テーブルデータを登録
                result = _productctrl.SetProductMaster(lst_master);
            }
            catch (Exception e)
            {

            }

            return result;
        }

        /// <summary>
        /// 区分マスタデータ取得
        /// </summary>
        /// <param name="lst_data">区分マスタデータリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool GetCategoryMasterData(out List<CategoryMasterData> lst_data)
        {
            bool result = false;

            //データ作成用
            lst_data = new List<CategoryMasterData>();

            try
            {
                //区分マスタデータを取得
                List<CategoryMaster> lst_category = new List<CategoryMaster>();
                result = _categoryctrl.GetCategoryMaster(data=>data.Delete_flag == false,out lst_category);

                foreach (var category in lst_category)
                {
                    CategoryMasterData data = new CategoryMasterData();

                    data.ID = category.Id;
                    data.CategoryName = category.Category;

                    lst_data.Add(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
            }

            return result;
        }

        /// <summary>
        /// 区分マスタデータ登録
        /// </summary>
        /// <param name="lst_data">店舗マスタデータリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetCategoryMasterData(List<CategoryMasterData> lst_data)
        {
            bool result = false;

            try
            {
                //画面データをテーブルデータに変換
                List<CategoryMaster> lst_master = new List<CategoryMaster>();
                foreach (var data in lst_data)
                {
                    CategoryMaster master = new CategoryMaster();
                    //ID
                    master.Id = data.ID;
                    //店舗名
                    master.Category = data.CategoryName;
                    lst_master.Add(master);
                }

                //テーブルデータを登録
                result = _categoryctrl.SetCategoryMaster(lst_master);
            }
            catch (Exception e)
            {

            }

            return result;
        }
    }
}
