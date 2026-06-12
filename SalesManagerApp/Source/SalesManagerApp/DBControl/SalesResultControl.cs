using DatabaseLib;
using DatabaseLib.Table;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerApp.DBControl
{
    /// <summary>
    /// 販売実績のコントロールクラス
    /// </summary>
    public class SalesResultControl
    {
        /// <summary>DBコントローラー</summary>
        private DBController _controller;

        /// <summary>
        /// 販売実績のコントロールクラス
        /// </summary>
        /// <param name="dbType">DBの種類</param>
        public SalesResultControl(E_DBType dbType)
        {
            //DBコントローラー生成
            _controller = new DBController(dbType);

            //テーブルの初期化
            _controller.Init();
        }

        /// <summary>
        /// 販売実績複数登録
        /// </summary>
        /// <param name="salesresults">販売実績データのリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetSalesResult(List<SalesResult> salesresults)
        {
            bool result = false;

            try
            {
                //セット対象のデータチェック
                if (null == salesresults)
                {
                    Console.WriteLine("Error: 販売実績がnullです。");
                    return false;
                }

                //データ数分ループして登録する
                foreach (var storemaster in salesresults)
                {
                    //販売実績登録
                    result &= SetSalesResult(storemaster);
                }
            }
            catch (Exception e)
            {
                //エラー処理
                Console.WriteLine($"Error: {e.Message}");
            }

            return result;
        }

        /// <summary>
        /// 販売実績登録
        /// </summary>
        /// <param name="salesresult">販売実績データ</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetSalesResult(SalesResult salesresult)
        {
            bool result = false;

            try
            {
                //セット対象のデータチェック
                if (null == salesresult)
                {
                    Console.WriteLine("Error: 販売実績がnullです。");
                    return false;
                }

                //販売実績を挿入
                result = _controller.InsertRecord(salesresult);
            }
            catch (Exception e)
            {
                //エラー処理
                Console.WriteLine($"Error: {e.Message}");
            }

            return result;
        }

        /// <summary>
        /// 販売実績全取得
        /// </summary>
        /// <param name="salesresultList"></param>
        /// <returns></returns>
        public bool GetSalesResult(out List<SalesResult> salesresultList)
        {
            bool result = false;
            salesresultList = new List<SalesResult>();

            try
            {
                //販売実績データを全て取得する(店舗ID、商品IDから店舗マスタ、商品マスタも同時に取得)
                salesresultList = _controller.SelectRecord<SalesResult>()
                                                .Include(data => data.Product)
                                                .Include(data => data.Product.Category)
                                                .Include(data => data.Store)
                                                .OrderBy(data => data.SaleDate)
                                                .ToList();
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            return result;
        }

        /// <summary>
        /// 条件に一致する販売実績取得
        /// </summary>
        /// <param name="condition">条件式</param>
        /// <param name="salesresultList">販売実績データリスト</param>
        /// <returns></returns>
        public bool GetSalesResult(Expression<Func<SalesResult, bool>> condition, out List<SalesResult> salesresultList)
        {
            bool result = false;
            salesresultList = new List<SalesResult>();

            try
            {
                //条件と一致する販売実績データを取得する(店舗ID、商品IDから店舗マスタ、商品マスタも同時に取得)
                salesresultList = _controller.SelectRecord(condition)
                                                .Include(data => data.Product)
                                                .Include(data => data.Product.Category)
                                                .Include(data => data.Store)
                                                .OrderBy(data => data.SaleDate)
                                                .ToList();
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            return result;
        }

        /// <summary>
        /// 条件に一致する販売実績取得
        /// </summary>
        /// <param name="filter">フィルター情報</param>
        /// <param name="salesresultList">販売実績データリスト</param>
        /// <returns></returns>
        public bool GetSalesResult(FilterInfo filter, out List<SalesResult> salesresultList)
        {
            bool result = false;
            salesresultList = new List<SalesResult>();

            try
            {
                //ベースとなるIQueryableを取得
                var query = _controller.SelectRecord<SalesResult>();
                //フィルター情報を適用
                query = query.CreateSalesFilter(filter);
                //条件と一致する販売実績データを取得する(店舗ID、商品IDから店舗マスタ、商品マスタも同時に取得)
                salesresultList = query.Include(data => data.Product)
                                        .Include(data => data.Product.Category)
                                        .Include(data => data.Store)
                                        .OrderBy(data => data.SaleDate)
                                        .ToList();
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            return result;
        }


        public bool GetSalesResult(DateTime start_date, DateTime end_date,  out List<SalesResult> salesresultList)
        {
            bool result = false;
            salesresultList = new List<SalesResult>();

            try
            {
                salesresultList = _controller.SelectRecord<SalesResult>(data => data.SaleDate >= start_date && data.SaleDate <= end_date)
                                        .Include(data => data.Product)
                                        .Include(data => data.Product.Category)
                                        .Include(data => data.Store)
                                        .OrderBy(data => data.SaleDate)
                                        .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            return result;
        }

        //TODO：昇順、降順取得

        //グループ化して取得


    }
}
