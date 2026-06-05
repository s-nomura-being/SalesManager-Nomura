using DatabaseLib.Table;
using SalesManagerApp.DBControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesManagerApp.Data
{
    /// <summary>
    /// メイン画面で使用するデータを管理するクラス
    /// </summary>
    public class MainViewDataManager
    {
        /// <summary>商品マスタコントロール</summary>
        private ProductMasterControl _productctrl;

        /// <summary>
        /// メイン画面用データ管理クラス
        /// </summary>
        public MainViewDataManager() 
        {
            //コントロールの初期化
            _productctrl = new ProductMasterControl(DatabaseLib.E_DBType.SQLite);
        }

        /// <summary>
        /// メイン画面表示用データ取得
        /// </summary>
        /// <param name="lst_lastweekdata">先週の商品売上データ</param>
        /// <param name="lst_stockstatusdata">在庫状況データ</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool GetMainViewData(out List<LastWeekProductSalesData> lst_lastweekdata, out List<StockStatusData> lst_stockstatusdata)
        {
            bool result = false;

            lst_lastweekdata = new List<LastWeekProductSalesData>();
            lst_stockstatusdata = new List<StockStatusData>();

            try
            {
                //商品マスタ保持用リスト
                List<ProductMaster> lst_product = new List<ProductMaster>();

                //商品マスタを取得
                //TODO：現在日時から先週分のデータに対象を絞り込む必要あり
                _productctrl.GetProductMaster(out lst_product);

                //商品マスタデータが取得できたら各データを作成
                if (0 < lst_product.Count)
                {
                    //先週の商品売上データ作成
                    lst_lastweekdata = CreateProductSalesData(lst_product);
                    //在庫状況データ作成
                    lst_stockstatusdata = CreateStockStatusData(lst_product);

                    //作成成功
                    result = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
                throw new Exception("表示用データの作成に失敗");
            }

            return result;
        }

        /// <summary>
        /// 先週の商品売上データ作成
        /// </summary>
        /// <param name="lst_product">商品マスタデータリスト</param>
        /// <returns>先週の商品売上データ</returns>
        private List<LastWeekProductSalesData> CreateProductSalesData(List<ProductMaster> lst_product)
        {
            //データ作成用リスト
            List<LastWeekProductSalesData> lst_lastweekdata = new List<LastWeekProductSalesData>();

            try
            {
                //商品マスタデータを分解して先週の商品売上データを作成する
                foreach (var product in lst_product)
                {
                    //1商品の販売数を算出
                    int totalsalescount = product.SalesResults.Sum(sales => sales.Quantity);

                    //1行分のデータ用オブジェクト作成
                    LastWeekProductSalesData lastweekdata = new LastWeekProductSalesData();

                    //商品名
                    lastweekdata.ProductName = product.Name;

                    //先週販売数(商品毎の合計販売数)
                    lastweekdata.LastWeekSales = totalsalescount;

                    //総在庫数(商品毎の合計在庫数)
                    lastweekdata.TotalStock = product.InventoryInfos.Sum(inventory => inventory.Stock);

                    //累計売上金額(販売数*商品単価)
                    lastweekdata.TotalSales = totalsalescount * product.UnitPrice;

                    //作成した先週実績データをリストに追加
                    lst_lastweekdata.Add(lastweekdata);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
                throw;
            }

            return lst_lastweekdata;
        }

        /// <summary>
        /// 在庫状況データ作成
        /// </summary>
        /// <param name="lst_product">商品マスタデータリスト</param>
        /// <returns>在庫状況データ</returns>
        private List<StockStatusData> CreateStockStatusData(List<ProductMaster> lst_product)
        {
            //データ作成用リスト
            List<StockStatusData> lst_stockstate = new List<StockStatusData>();

            try
            {
                //商品マスタデータを分解して商品毎の在庫状況データを作成する
                foreach (var product in lst_product)
                {
                    //1商品の販売数を算出
                    int totalsalescount = product.SalesResults.Sum(sales => sales.Quantity);

                    //1行分のデータ用オブジェクト作成
                    StockStatusData stockstate = new StockStatusData();

                    //商品名
                    stockstate.ProductName = product.Name;

                    //現在在庫数
                    stockstate.Stock = product.InventoryInfos.Sum(inventory => inventory.Stock);

                    //販売後在庫数
                    stockstate.AfterStock = totalsalescount;

                    //発注通知
                    //TODO：発注タイミングを設定ファイルから取得
                    stockstate.Notice = stockstate.Stock <= 5 ? "発注！" : "";

                    //作成した先週実績データをリストに追加
                    lst_stockstate.Add(stockstate);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
                throw;
            }

            return lst_stockstate;
        }
    }
}
