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
        private const string NOTICE_TXT_MUST = "要発注";
        private const string NOTICE_TXT_LOW = "在庫が少ない";

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

                    //商品ID
                    lastweekdata.ProductID = product.Id;

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

            //ID、販売数が多い順
            return lst_lastweekdata.OrderBy(data => data.ProductID).ThenByDescending(data => data.LastWeekSales).ToList();
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
                    stockstate.Notice = GetNoticeString(stockstate.AfterStock);

                    //在庫状況
                    stockstate.StatusType = GetNoticeType(stockstate.AfterStock);

                    //作成した先週実績データをリストに追加
                    lst_stockstate.Add(stockstate);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
                throw;
            }

            //発注通知あり、販売後在庫数が少ない順
            return lst_stockstate.OrderByDescending(data => data.Notice).ThenBy(data => data.AfterStock).ToList();
        }

        /// <summary>
        /// 発注通知判定 文字列作成
        /// </summary>
        /// <param name="stock">現在の在庫数</param>
        /// <returns>発注通知テキスト</returns>
        private string GetNoticeString(int stock)
        {
            //発注通知テキスト
            string notice_text = string.Empty;

            //設定値：要発注の値を取得、比較
            if (Properties.Settings.Default.NoticeCount_Must >= stock)
            {
                //要発注テキスト
                notice_text = NOTICE_TXT_MUST;
            }
            //設定値：在庫少の値を取得、比較
            else if (Properties.Settings.Default.NoticeCount_Low >= stock)
            {
                //在庫少テキスト
                notice_text = NOTICE_TXT_LOW;
            }
            else
            {
                //通常通り
                notice_text = string.Empty;
            }

            return notice_text;
        }

        /// <summary>
        /// 発注通知判定 在庫状況設定
        /// </summary>
        /// <param name="stock">現在の在庫数</param>
        /// <returns>在庫状況</returns>
        private E_StockStatusType GetNoticeType(int stock)
        {
            //在庫状況
            var notice_type = E_StockStatusType.None;

            //設定値：要発注の値を取得、比較
            if (Properties.Settings.Default.NoticeCount_Must >= stock)
            {
                //要発注状態
                notice_type = E_StockStatusType.Must;
            }
            //設定値：在庫少の値を取得、比較
            else if (Properties.Settings.Default.NoticeCount_Low >= stock)
            {
                //在庫少状態
                notice_type = E_StockStatusType.Low;
            }
            else
            {
                //通常状態
                notice_type = E_StockStatusType.None;
            }

            return notice_type;
        }
    }
}
