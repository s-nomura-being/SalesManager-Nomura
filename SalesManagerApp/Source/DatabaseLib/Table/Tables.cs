using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib.Table
{
    /// <summary>
    /// テーブル定義
    /// </summary>
    public class Tables
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// 店舗マスタテーブル定義
    /// </summary>
    public class StoreMaster
    {
        /// <summary>店舗ID(主キー)</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>店舗名</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>削除フラグ</summary>
        public bool Delete_flag { get; set; } = false;

        /// <summary>作成日時</summary>
        public DateTime? Created_at {get; set; }

        /// <summary>更新日時</summary>
        public DateTime? Updated_at { get; set; }

        /// <summary>削除日時</summary>
        public DateTime? Deleted_at { get; set; }

        #region ナビゲーションプロパティ(外部キー参照用)
        /// <summary>販売実績リスト</summary>
        public List<SalesResult> SalesResults { get; set; }

        /// <summary>在庫情報リスト</summary>
        public List<InventoryInfo> InventoryInfos { get; set; }
        #endregion
    }

    /// <summary>
    /// 商品マスタテーブル定義
    /// </summary>
    public class ProductMaster
    {
        /// <summary>商品ID(主キー)</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>商品名</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>単価</summary>
        public int UnitPrice { get; set; }

        /// <summary>区分ID(外部キー)</summary>
        public int CategoryId { get; set; }

        /// <summary>削除フラグ</summary>
        public bool Delete_flag { get; set; } = false;

        /// <summary>作成日時</summary>
        public DateTime? Created_at { get; set; }

        /// <summary>更新日時</summary>
        public DateTime? Updated_at { get; set; }

        /// <summary>削除日時</summary>
        public DateTime? Deleted_at { get; set; }

        #region ナビゲーションプロパティ(外部キー参照用)
        /// <summary>区分マスタ情報</summary>
        [ForeignKey("CategoryId")]
        public CategoryMaster Category { get; set; }

        /// <summary>販売実績リスト</summary>
        public List<SalesResult> SalesResults { get; set; }

        /// <summary>在庫情報リスト</summary>
        public List<InventoryInfo> InventoryInfos { get; set; }

        /// <summary>区分名</summary>
        [NotMapped]
        public string CategoryName { get; set; } = string.Empty;
        #endregion
    }

    /// <summary>
    /// 区分マスタテーブル定義
    /// </summary>
    public class CategoryMaster
    {
        /// <summary>区分ID(主キー)</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>区分名</summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>削除フラグ</summary>
        public bool Delete_flag { get; set; } = false;

        /// <summary>作成日時</summary>
        public DateTime? Created_at { get; set; }

        /// <summary>更新日時</summary>
        public DateTime? Updated_at { get; set; }

        /// <summary>削除日時</summary>
        public DateTime? Deleted_at { get; set; }

        #region ナビゲーションプロパティ(外部キー参照用)
        /// <summary>商品マスタリスト</summary>
        public List<ProductMaster> ProductMasters { get; set; }
        #endregion
    }

    /// <summary>
    /// 販売実績テーブル定義
    /// </summary>
    public class SalesResult
    {
        /// <summary>販売実績ID(主キー)</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>販売日</summary>
        public DateTime SaleDate { get; set; }

        /// <summary>店舗ID(外部キー)</summary>
        public int StoreId { get; set; }

        /// <summary>商品ID(外部キー)</summary>
        public int ProductId { get; set; }

        /// <summary>販売数</summary>
        public int Quantity { get; set; }

        /// <summary>売上高</summary>
        public int SalesAmount { get; set; }

        #region ナビゲーションプロパティ(外部キー参照用)
        /// <summary>店舗情報</summary>
        [ForeignKey("StoreId")]
        public StoreMaster Store { get; set; }
        /// <summary>商品情報</summary>
        [ForeignKey("ProductId")]
        public ProductMaster Product { get; set; }

        /// <summary>店舗名</summary>
        [NotMapped]
        public string StoreName { get; set; } = string.Empty;

        /// <summary>商品名</summary>
        [NotMapped]
        public string ProductName { get; set; } = string.Empty;
        #endregion
    }

    /// <summary>
    /// 在庫情報テーブル定義
    /// </summary>
    public class InventoryInfo
    {
        /// <summary>店舗ID(外部キー)</summary>
        public int StoreId { get; set; }

        /// <summary>商品ID(外部キー)</summary>
        public int ProductId { get; set; }

        /// <summary>在庫数</summary>
        public int Stock { get; set; }

        #region ナビゲーションプロパティ(外部キー参照用)
        /// <summary>店舗情報</summary>
        [ForeignKey("StoreId")]
        public StoreMaster Store { get; set; }
        /// <summary>商品情報</summary>
        [ForeignKey("ProductId")]
        public ProductMaster Product { get; set; }

        /// <summary>店舗名</summary>
        [NotMapped]
        public string StoreName { get; set; } = string.Empty;

        /// <summary>商品名</summary>
        [NotMapped]
        public string ProductName { get; set; } = string.Empty;
        #endregion
    }
}
