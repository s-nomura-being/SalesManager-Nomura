using DatabaseLib.Table;
using SalesManagerApp.DBControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerApp.Data
{
    /// <summary>
    /// 在庫情報確認画面で使用するデータを管理するクラス
    /// </summary>
    public class InventoryViewDataManager
    {
        /// <summary>在庫データコントロール</summary>
        InventoryInfoControl _inventoryctrl;

        /// <summary>
        /// 在庫情報確認画面用データ管理クラス
        /// </summary>
        public InventoryViewDataManager()
        {
            //コントロールの初期化
            _inventoryctrl = new InventoryInfoControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
        }

        /// <summary>
        /// 各マスタ名称取得
        /// </summary>
        /// <param name="lst_store">店舗名リスト</param>
        /// <param name="lst_product">商品名リスト</param>
        /// <param name="lst_category">区分名リスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool GetNameData(out List<string> lst_store, out List<string> lst_product, out List<string> lst_category)
        {
            bool result = false;

            lst_store = new List<string>();
            lst_product = new List<string>();
            lst_category = new List<string>();

            try
            {
                //在庫データ保持用リスト
                List<InventoryInfo> lst_inventory = new List<InventoryInfo>();
                //在庫データを取得する
                _inventoryctrl.GetInventoryInfo(out lst_inventory);

                //データから店舗名、商品名、区分名を抜き出す(重複抜き)
                lst_store = lst_inventory.Select(data => data.Store.Name).Distinct().ToList();
                lst_product = lst_inventory.Select(data => data.Product.Name).Distinct().ToList();
                lst_category = lst_inventory.Select(data => data.Product.Category.Category).Distinct().ToList();

                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
            }

            return result;
        }

        /// <summary>
        /// 在庫情報確認画面データ取得
        /// </summary>
        /// <param name="lst_detaildata">売上詳細一覧データリスト</param>
        /// <param name="filter_setting">絞り込み条件オブジェクト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool GetInventoryViewData(out List<InventoryListData> lst_detaildata, FilterInfo? filter)
        {
            bool result = false;
            lst_detaildata = new List<InventoryListData>();

            try
            {
                //在庫データを作成する
                lst_detaildata = CreateInventoryData(filter);
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return result;
        }

        /// <summary>
        /// 在庫一覧データ作成
        /// </summary>
        /// <param name="filter">絞り込み条件</param>
        /// <returns>在庫一覧データ</returns>
        private List<InventoryListData> CreateInventoryData(FilterInfo? filter)
        {
            //データ作成用リスト
            List<InventoryListData> lst_inventorydata = new List<InventoryListData>();

            try
            {
                //在庫データ保持用リスト
                List<InventoryInfo> lst_inventory = new List<InventoryInfo>();

                //絞り込みが有効の場合、条件付きで取得
                if (null != filter)
                {
                    //絞り込み条件と一致する在庫データを取得する
                    _inventoryctrl.GetInventoryInfo(filter, out lst_inventory);
                }
                else
                {
                    //在庫データを取得する
                    _inventoryctrl.GetInventoryInfo(out lst_inventory);
                }

                //販売実績データを分解して詳細データを作成する
                foreach (InventoryInfo inventory in lst_inventory)
                {
                    //1行分のデータ用オブジェクト作成
                    InventoryListData info = new InventoryListData();

                    //店舗名
                    info.StoreName = inventory.Store.Name;

                    //商品名
                    info.ProductName = inventory.Product.Name;

                    //商品区分
                    info.Category = inventory.Product.Category.Category;

                    //現在在庫数
                    info.Stock = inventory.Stock;

                    //商品単価
                    info.UnitPrice = inventory.Product.UnitPrice;

                    //在庫金額
                    info.StockValue = inventory.Stock * inventory.Product.UnitPrice;

                    //最終販売日
                    var sales_data = inventory.Product.SalesResults.MaxBy(sales => sales.SaleDate);
                    info.LastSalesDate = sales_data != null ? sales_data.SaleDate : DateTime.MinValue;

                    //作成した在庫一覧データをリストに追加
                    lst_inventorydata.Add(info);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return lst_inventorydata;
        }
    }
}
