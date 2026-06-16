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
    /// 売上販売実績一覧画面で使用するデータを管理するクラス
    /// </summary>
    public class DetailSalesViewDataManager
    {
        /// <summary>売上販売実績コントロール</summary>
        SalesResultControl _salesresultctrl;

        /// <summary>
        /// 売上販売実績一覧画面用データ管理クラス
        /// </summary>
        public DetailSalesViewDataManager()
        {
            //コントロールの初期化
            _salesresultctrl = new SalesResultControl(DatabaseLib.E_DBType.SQLite, Properties.Settings.Default.DbConnectionString);
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
                //売上データ保持用リスト
                List<SalesResult> lst_salses = new List<SalesResult>();
                //売上データを取得する
                _salesresultctrl.GetSalesResult(out lst_salses);

                //データから店舗名、商品名、区分名を抜き出す(重複抜き)
                lst_store = lst_salses.Select(data => data.Store.Name).Distinct().ToList();
                lst_product = lst_salses.Select(data => data.Product.Name).Distinct().ToList();
                lst_category = lst_salses.Select(data => data.Product.Category.Category).Distinct().ToList();

                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
            }

            return result;
        }

        /// <summary>
        /// 売上販売実績一覧画面データ取得
        /// </summary>
        /// <param name="lst_detaildata">売上詳細一覧データリスト</param>
        /// <param name="filter_setting">絞り込み条件オブジェクト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool GetDetailSalesViewData(out List<DetailSalesData> lst_detaildata, FilterInfo? filter)
        {
            bool result = false;
            lst_detaildata = new List<DetailSalesData>();

            try
            {
                //売上詳細一覧データを作成する
                lst_detaildata = CreateDetailSalesData(filter);
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return result;
        }

        /// <summary>
        /// 売上詳細一覧データ作成
        /// </summary>
        /// <param name="filter">絞り込み条件</param>
        /// <returns>売上詳細一覧データ</returns>
        private List<DetailSalesData> CreateDetailSalesData(FilterInfo? filter)
        {
            //データ作成用リスト
            List<DetailSalesData> lst_details = new List<DetailSalesData>();

            try
            {
                //販売実績データ保持用リスト
                List<SalesResult> lst_results = new List<SalesResult>();

                //絞り込みが有効の場合、条件付きで取得
                if(null != filter)
                {
                    //絞り込み条件と一致する販売実績データを取得する
                    _salesresultctrl.GetSalesResult(filter, out lst_results);
                }
                else
                {
                    //販売実績データを取得する
                    _salesresultctrl.GetSalesResult(out lst_results);
                }

                //販売実績データを分解して詳細データを作成する
                foreach (SalesResult result_data in lst_results)
                {
                    //1行分のデータ用オブジェクト作成
                    DetailSalesData detail = new DetailSalesData();

                    //販売日
                    detail.SalesDate = result_data.SaleDate;

                    //店舗名
                    detail.StoreName = result_data.Store.Name;

                    //商品名
                    detail.ProductName = result_data.Product.Name;

                    //商品区分
                    detail.Category = result_data.Product.Category.Category;

                    //販売数
                    detail.SalesCount = result_data.Quantity;

                    //売上金額
                    detail.Sales = result_data.SalesAmount;

                    //作成した売上詳細一覧データをリストに追加
                    lst_details.Add(detail);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            //日付順
            return lst_details.OrderBy(data=>data.SalesDate).ToList();
        }
    }
}
