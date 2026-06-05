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
            _salesresultctrl = new SalesResultControl(DatabaseLib.E_DBType.SQLite);
        }

        /// <summary>
        /// 売上販売実績一覧画面データ取得
        /// </summary>
        /// <param name="lst_detaildata">売上詳細一覧データリスト</param>
        /// <param name="filter_setting">絞り込み条件オブジェクト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool GetDetailSalesViewData(out List<DetailSalesData> lst_detaildata, object filter_setting = null)
        {
            bool result = false;
            lst_detaildata = new List<DetailSalesData>();

            try
            {
                //絞り込み条件がある場合、条件設定
                if(null != filter_setting)
                {
                    //絞り込み条件データから条件式を作成する
                    //TODO：絞り込み条件の取得
                    Expression<Func<SalesResult, bool>> filter = null;
                    //filter = data => data.StoreId == 5;

                    //条件と一致する売上詳細一覧データを作成する
                    lst_detaildata = CreateDetailSalesData(filter);
                }
                else
                {
                    //売上詳細一覧データを作成する
                    lst_detaildata = CreateDetailSalesData();
                }

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
        private List<DetailSalesData> CreateDetailSalesData(Expression<Func<SalesResult, bool>> filter = null)
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

            return lst_details;
        }
    }
}
