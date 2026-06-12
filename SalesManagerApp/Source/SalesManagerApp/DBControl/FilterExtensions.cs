using DatabaseLib.Table;
using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerApp.DBControl
{
    /// <summary>
    /// 絞り込み条件作成用静的クラス
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// [拡張メソッド] 売上販売実績 絞り込み条件作成
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter">フィルター情報</param>
        /// <returns>絞り込み条件式</returns>
        public static IQueryable<SalesResult> CreateSalesFilter(this IQueryable<SalesResult> query, FilterInfo filter)
        {
            //店舗名で絞り込みがある場合
            if (true == filter.StoreList.Any())
            {
                //絞り込み条件に「店舗名が一致する」を追加
                query = query.Where(data => filter.StoreList.Contains(data.Store.Name));
            }
            //商品名で絞り込みがある場合
            if (true == filter.ProductList.Any())
            {
                //絞り込み条件に「商品名が一致する」を追加
                query = query.Where(data => filter.ProductList.Contains(data.Product.Name));
            }
            //区分名で絞り込みがある場合
            if (true == filter.CategoryList.Any())
            {
                //絞り込み条件に「区分名が一致する」を追加
                query = query.Where(data => filter.CategoryList.Contains(data.Product.Category.Category));
            }
            //開始時間で絞り込みがある場合
            if (true == filter.StartTime.HasValue)
            {
                //絞り込み条件に「開始時間以上」を追加
                query = query.Where(data => data.SaleDate >= filter.StartTime);
            }
            //終了時間で絞り込みがある場合
            if (true == filter.EndTime.HasValue)
            {
                //絞り込み条件に「終了時間以下」を追加
                query = query.Where(data => data.SaleDate <= filter.EndTime);
            }

            return query;
        }

        /// <summary>
        /// [拡張メソッド] 在庫情報 絞り込み条件作成
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter">フィルター情報</param>
        /// <returns>絞り込み条件式</returns>
        public static IQueryable<InventoryInfo> CreateInventoryFilter(this IQueryable<InventoryInfo> query, FilterInfo filter)
        {
            //店舗名で絞り込みがある場合
            if (true == filter.StoreList.Any())
            {
                //絞り込み条件に「店舗名が一致する」を追加
                query = query.Where(data => filter.StoreList.Contains(data.Store.Name));
            }
            //商品名で絞り込みがある場合
            if (true == filter.ProductList.Any())
            {
                //絞り込み条件に「商品名が一致する」を追加
                query = query.Where(data => filter.ProductList.Contains(data.Product.Name));
            }
            //区分名で絞り込みがある場合
            if (true == filter.CategoryList.Any())
            {
                //絞り込み条件に「区分名が一致する」を追加
                query = query.Where(data => filter.CategoryList.Contains(data.Product.Category.Category));
            }

            return query;
        }
    }
}
