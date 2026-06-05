using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerApp.Data
{
    /// <summary>
    /// 先週の商品売上データクラス
    /// </summary>
    public class LastWeekProductSalesData
    {
        /// <summary>商品名</summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>先週販売数</summary>
        public int LastWeekSales { get; set; }

        /// <summary>総在庫数</summary>
        public int TotalStock { get; set; }

        /// <summary>累計売上金額</summary>
        public int TotalSales { get; set; }
    }

    /// <summary>
    /// 在庫状況データクラス
    /// </summary>
    public class StockStatusData
    {
        /// <summary>商品名</summary>
        public string ProductName { get; set;} = string.Empty;

        /// <summary>現在在庫数</summary>
        public int Stock { get; set; }

        /// <summary>販売後在庫</summary>
        public int AfterStock { get; set; }

        /// <summary>発注通知</summary>
        public string Notice { get; set; } = string.Empty;
    }

    /// <summary>
    /// 売上集計データクラス
    /// </summary>
    public class TotalSalesData
    {
        /// <summary>期間</summary>
        public string Period { get; set; } = string.Empty;

        /// <summary>データ数</summary>
        public int DataCount { get; set; }

        /// <summary>累計販売数</summary>
        public int TotalSalesCount { get; set; }

        /// <summary>累計売上金額</summary>
        public int TotalSales { get; set; }
    }

    /// <summary>
    /// 売上詳細一覧データクラス
    /// </summary>
    public class DetailSalesData
    {
        /// <summary>販売日</summary>
        public DateTime SalesDate { get; set; }

        /// <summary>店舗名</summary>
        public string StoreName { get; set; } = string.Empty;

        /// <summary>商品名</summary>
        public string ProductName { set; get; } = string.Empty;

        /// <summary>商品区分</summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>販売数</summary>
        public int SalesCount { get; set; }

        /// <summary>売上金額</summary>
        public int Sales { get; set; }
    }

    /// <summary>
    /// 在庫一覧データクラス
    /// </summary>
    public class InventoryListData
    {
        /// <summary>店舗名</summary>
        public string StoreName { get; set; } = string.Empty;

        /// <summary>商品名</summary>
        public string ProductName { set; get; } = string.Empty;

        /// <summary>商品区分</summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>現在在庫数</summary>
        public int Stock { get; set; }

        /// <summary>商品単価</summary>
        public int UnitPrice { get; set; }

        /// <summary>在庫金額</summary>
        public int StockValue { get; set; }

        /// <summary>最終販売日</summary>
        public DateTime LastSalesDate { get; set; }
    }
}
