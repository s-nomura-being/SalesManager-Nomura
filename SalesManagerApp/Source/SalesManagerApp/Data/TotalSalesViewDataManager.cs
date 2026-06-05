using DatabaseLib.Table;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SalesManagerApp.DBControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerApp.Data
{
    /// <summary>
    /// 集計期間タイプ
    /// </summary>
    public enum E_Period
    {
        /// <summary>全期間</summary>
        None = 0,
        /// <summary>週間</summary>
        Week,
        /// <summary>月間</summary>
        Month,
        /// <summary>年間</summary>
        Year,
    }

    /// <summary>
    /// 売上集計画面で使用するデータを管理するクラス
    /// </summary>
    public class TotalSalesViewDataManager
    {
        /// <summary>売上販売実績コントロール</summary>
        SalesResultControl _salesresultctrl;

        /// <summary>
        /// 売上集計画面用データ管理クラス
        /// </summary>
        public TotalSalesViewDataManager()
        {
            //コントロールの初期化
            _salesresultctrl = new SalesResultControl(DatabaseLib.E_DBType.SQLite);
        }

        /// <summary>
        /// 売上集計画面用データ取得
        /// </summary>
        /// <param name="e_period">集計期間タイプ</param>
        /// <param name="start_dt">集計開始日時</param>
        /// <param name="end_dt">集計終了日時</param>
        /// <param name="lst_totalsalesdata">売上集計データリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool GetTotalSalesViewData(E_Period e_period, DateTime start_dt, DateTime end_dt, out List<TotalSalesData> lst_totalsalesdata)
        {
            bool result = false;
            lst_totalsalesdata = new List<TotalSalesData>();

            try
            {
                //集計期間タイプ別で処理を分ける
                switch (e_period)
                {
                    //全期間
                    case E_Period.None:
                        throw new NotImplementedException();
                        break;

                    //週間
                    case E_Period.Week:
                        //週間売上集計データ作成
                        lst_totalsalesdata = CreateWeekTotalSalesData(start_dt.Year, start_dt.Month);
                        break;

                    //月間
                    case E_Period.Month:
                        //月間売上集計データ作成
                        lst_totalsalesdata = CreateMonthTotalSalesData(start_dt.Year);
                        break;

                    //年間
                    case E_Period.Year:
                        //年間売上集計データ作成
                        lst_totalsalesdata = CreateYearTotalSalesData(start_dt.Year, end_dt.Year);
                        break;

                    default:
                        throw new Exception("不正な集計期間");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return result;
        }

        /// <summary>
        /// 年間売上集計データ作成
        /// </summary>
        /// <param name="start_year">集計開始年</param>
        /// <param name="end_year">集計終了年</param>
        /// <returns>年間売上集計データ</returns>
        private List<TotalSalesData> CreateYearTotalSalesData(int start_year, int end_year)
        {
            //データ作成用リスト
            List<TotalSalesData> lst_totalsales = new List<TotalSalesData>();

            try
            {
                //販売実績データ保持用リスト
                List<SalesResult> lst_results = new List<SalesResult>();
                //集計開始年(1/1)～終了年(12/31 11:59:59)までの販売実績データを取得する
                _salesresultctrl.GetSalesResult(new DateTime(start_year, 1, 1), new DateTime(end_year, 12, 31, 23, 59, 59), out lst_results);

                //取得した販売実績データを「年単位でグループ化」、「販売数と売上金額を合計し集計データ作成」、「期間列を基準に昇順並べ替え」
                lst_totalsales = lst_results.GroupBy(data => data.SaleDate.Year)
                                            .Select(group => new TotalSalesData()
                                            {
                                                Period = $"{group.Key}年",
                                                DataCount = group.Count(),
                                                TotalSalesCount = group.Sum(group_data => group_data.Quantity),
                                                TotalSales = group.Sum(group_data => group_data.SalesAmount),
                                            })
                                            .OrderBy(data => data.Period)
                                            .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return lst_totalsales;
        }

        /// <summary>
        /// 指定年の月間売上集計データ作成
        /// </summary>
        /// <param name="target_year">集計対象年</param>
        /// <returns>月間売上集計データ</returns>
        private List<TotalSalesData> CreateMonthTotalSalesData(int target_year)
        {
            //データ作成用リスト
            List<TotalSalesData> lst_totalsales = new List<TotalSalesData>();

            try
            {
                //販売実績データ保持用リスト
                List<SalesResult> lst_results = new List<SalesResult>();

                //指定年の集計開始日時(1/1)～終了日時(12/31 11:59:59)を作成する
                DateTime start = new DateTime(target_year, 1, 1);
                DateTime end = new DateTime(target_year, 12, 31, 23, 59, 59);
                
                //指定年の販売実績データを取得する
                _salesresultctrl.GetSalesResult(start, end, out lst_results);

                //取得した販売実績データを「月単位でグループ化」、「販売数と売上金額を合計し集計データ作成」、「期間列を基準に昇順並べ替え」
                lst_totalsales = lst_results.GroupBy(data => data.SaleDate.Month)
                                            .Select(group => new TotalSalesData()
                                            {
                                                Period = $"{target_year}年{group.Key}月",
                                                DataCount = group.Count(),
                                                TotalSalesCount = group.Sum(group_data => group_data.Quantity),
                                                TotalSales = group.Sum(group_data => group_data.SalesAmount),
                                            })
                                            .OrderBy(data => data.Period)
                                            .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return lst_totalsales;
        }

        /// <summary>
        /// 指定年月の週間売上集計データ作成
        /// </summary>
        /// <param name="target_year">集計対象年</param>
        /// <param name="target_month">集計対象月</param>
        /// <returns>週間売上集計データ</returns>
        private List<TotalSalesData> CreateWeekTotalSalesData(int target_year, int target_month)
        {
            //データ作成用リスト
            List<TotalSalesData> lst_totalsales = new List<TotalSalesData>();

            try
            {
                //販売実績データ保持用リスト
                List<SalesResult> lst_results = new List<SalesResult>();

                //指定年月から、月始まりの日付を作成
                DateTime month_firstday = new DateTime(target_year, target_month, 1);
                
                //指定年月から、月の最終日を取得
                int day_in_month = DateTime.DaysInMonth(target_year, target_month);
                //指定年月・最終日から、月終わりの日付を作成
                DateTime month_endday = new DateTime(target_year, target_month, day_in_month, 23, 59, 59);

                //月始まりの日付から、週始まり(月曜日)の日付を算出
                DateTime first_month_week = GetStartOfWeek(month_firstday);
                //月終わりの日付から、週終わり(日曜日)の日付を算出
                DateTime end_month_week = GetEndOfWeek(month_endday);

                //月始まり週～月終わり週の販売実績データを取得する
                _salesresultctrl.GetSalesResult(first_month_week, end_month_week, out lst_results);

                //取得した販売実績データを「週単位でグループ化」、「販売数と売上金額を合計し集計データ作成」、「期間列を基準に昇順並べ替え」
                lst_totalsales = lst_results.GroupBy(data => GetStartOfWeek(data.SaleDate))
                                            .Select(group => new TotalSalesData()
                                            {
                                                Period = $"{group.Key:yyyy/MM/dd}の週",
                                                DataCount = group.Count(),
                                                TotalSalesCount = group.Sum(group_data => group_data.Quantity),
                                                TotalSales = group.Sum(group_data => group_data.SalesAmount),
                                            })
                                            .OrderBy(data => data.Period)
                                            .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return lst_totalsales;
        }

        /// <summary>
        /// 指定日時から週始まり(月曜日)を算出する
        /// </summary>
        /// <param name="dt">指定日時</param>
        /// <returns>週始まり(月曜日)の日時</returns>
        private DateTime GetStartOfWeek(DateTime dt)
        {
            //指定日時の曜日が、月曜日から何日ズレているか計算
            int diff_monday = dt.DayOfWeek - DayOfWeek.Monday;
            //指定日時の曜日が、月曜日から何日ズレているか計算
            int diff = (7 + (diff_monday)) % 7;
            //算出した日数を引いて月曜日の日時を返す
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// 指定日時から週終わり(日曜日)を算出する
        /// </summary>
        /// <param name="dt">指定日時</param>
        /// <returns>週終わり(日曜日)の日時</returns>
        private DateTime GetEndOfWeek(DateTime dt)
        {
            //指定日時の月曜日を取得
            DateTime startofweek = GetStartOfWeek(dt);
            //取得した月曜の日付から日数(6日23:59:59)を足して、日曜日を算出する
            return startofweek.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
        }
    }
}
