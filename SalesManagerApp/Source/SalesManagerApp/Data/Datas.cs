using ClosedXML.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Display(Name = "商品名")]
        [XLColumn(Header = "商品名")]
        public string ProductName { get; set; } = string.Empty;

        /// <summary>先週販売数</summary>
        [Display(Name = "先週販売数")]
        [XLColumn(Header = "先週販売数")]
        public int LastWeekSales { get; set; }

        /// <summary>総在庫数</summary>
        [Display(Name = "総在庫数")]
        [XLColumn(Header = "総在庫数")]
        public int TotalStock { get; set; }

        /// <summary>累計売上金額</summary>
        [Display(Name = "累計売上金額")]
        [XLColumn(Header = "累計売上金額")]
        public int TotalSales { get; set; }
    }

    /// <summary>
    /// 在庫状況データクラス
    /// </summary>
    public class StockStatusData
    {
        /// <summary>商品名</summary>
        [Display(Name = "商品名")]
        [XLColumn(Header = "商品名")]
        public string ProductName { get; set;} = string.Empty;

        /// <summary>現在在庫数</summary>
        [Display(Name = "現在在庫数")]
        [XLColumn(Header = "現在在庫数")]
        public int Stock { get; set; }

        /// <summary>販売後在庫</summary>
        [Display(Name = "販売後在庫")]
        [XLColumn(Header = "販売後在庫")]
        public int AfterStock { get; set; }

        /// <summary>発注通知</summary>
        [Display(Name = "発注通知")]
        [XLColumn(Header = "発注通知")]
        public string Notice { get; set; } = string.Empty;
    }

    /// <summary>
    /// 売上集計データクラス
    /// </summary>
    public class TotalSalesData
    {
        /// <summary>期間</summary>
        [Display(Name = "期間")]
        [XLColumn(Header = "期間")]
        public string Period { get; set; } = string.Empty;

        /// <summary>データ数</summary>
        [Display(Name = "データ数")]
        [XLColumn(Header = "データ数")]
        public int DataCount { get; set; }

        /// <summary>累計販売数</summary>
        [Display(Name = "累計販売数")]
        [XLColumn(Header = "累計販売数")]
        public int TotalSalesCount { get; set; }

        /// <summary>累計売上金額</summary>
        [Display(Name = "累計売上金額")]
        [XLColumn(Header = "累計売上金額")]
        public int TotalSales { get; set; }
    }

    /// <summary>
    /// 売上詳細一覧データクラス
    /// </summary>
    public class DetailSalesData
    {
        /// <summary>販売日</summary>
        [Display(Name = "販売日")]
        [XLColumn(Header = "販売日")]
        public DateTime SalesDate { get; set; }

        /// <summary>店舗名</summary>
        [Display(Name = "店舗名")]
        [XLColumn(Header = "店舗名")]
        public string StoreName { get; set; } = string.Empty;

        /// <summary>商品名</summary>
        [Display(Name = "商品名")]
        [XLColumn(Header = "商品名")]
        public string ProductName { set; get; } = string.Empty;

        /// <summary>商品区分</summary>
        [Display(Name = "商品区分")]
        [XLColumn(Header = "商品区分")]
        public string Category { get; set; } = string.Empty;

        /// <summary>販売数</summary>
        [Display(Name = "販売数")]
        [XLColumn(Header = "販売数")]
        public int SalesCount { get; set; }

        /// <summary>売上金額</summary>
        [Display(Name = "売上金額")]
        [XLColumn(Header = "売上金額")]
        public int Sales { get; set; }
    }

    /// <summary>
    /// 在庫一覧データクラス
    /// </summary>
    public class InventoryListData
    {
        /// <summary>店舗名</summary>
        [Display(Name = "店舗名")]
        [XLColumn(Header = "店舗名")]
        public string StoreName { get; set; } = string.Empty;

        /// <summary>商品名</summary>
        [Display(Name = "商品名")]
        [XLColumn(Header = "商品名")]
        public string ProductName { set; get; } = string.Empty;

        /// <summary>商品区分</summary>
        [Display(Name = "商品区分")]
        [XLColumn(Header = "商品区分")]
        public string Category { get; set; } = string.Empty;

        /// <summary>現在在庫数</summary>
        [Display(Name = "現在在庫数")]
        [XLColumn(Header = "現在在庫数")]
        public int Stock { get; set; }

        /// <summary>商品単価</summary>
        [Display(Name = "商品単価")]
        [XLColumn(Header = "商品単価")]
        public int UnitPrice { get; set; }

        /// <summary>在庫金額</summary>
        [Display(Name = "在庫金額")]
        [XLColumn(Header = "在庫金額")]
        public int StockValue { get; set; }

        /// <summary>最終販売日</summary>
        [Display(Name = "最終販売日")]
        [XLColumn(Header = "最終販売日")]
        public DateTime LastSalesDate { get; set; }
    }
}
