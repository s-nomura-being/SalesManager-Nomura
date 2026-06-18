using NUnit.Framework;
using SalesManagerApp.DBControl;
using DatabaseLib.Table;
using System.Linq;
using System.Collections.Generic;
using System;

namespace SalesManagerApp.Tests
{
    [TestFixture]
    public class FilterExtensionsTests
    {
        // 共通データ作成用メソッド
        private IQueryable<SalesResult> GetSampleSalesData()
        {
            return new List<SalesResult>
            {
                new SalesResult { Store = new StoreMaster { Name = "店舗A" }, Product = new ProductMaster { Name = "商品1", Category = new CategoryMaster { Category = "区分1" } }, SaleDate = new DateTime(2026, 6, 1) },
                new SalesResult { Store = new StoreMaster { Name = "店舗B" }, Product = new ProductMaster { Name = "商品2", Category = new CategoryMaster { Category = "区分2" } }, SaleDate = new DateTime(2026, 6, 10) }
            }.AsQueryable();
        }

        [Test]
        public void TS58_CreateSalesFilter_店舗名絞り込みの検証()
        {
            var query = GetSampleSalesData();
            var filter = new FilterInfo(new List<string> { "店舗A" }, new List<string>(), new List<string>());

            // 実行（クエリ式が構築される）
            var result = query.CreateSalesFilter(filter).ToList();

            // 検証
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Store.Name, Is.EqualTo("店舗A"));
        }

        [Test]
        public void TS59_CreateSalesFilter_期間絞り込みの検証()
        {
            var query = GetSampleSalesData();
            // 2026/6/5 ～ 2026/6/15 の範囲
            var filter = new FilterInfo(new List<string>(), new List<string>(), new List<string>(),
                                        new DateTime(2026, 6, 5), new DateTime(2026, 6, 15));

            var result = query.CreateSalesFilter(filter).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().SaleDate, Is.EqualTo(new DateTime(2026, 6, 10)));
        }

        [Test]
        public void TS60_CreateInventoryFilter_複数条件の絞り込み検証()
        {
            var data = new List<InventoryInfo>
            {
                new InventoryInfo { Store = new StoreMaster { Name = "店舗A" }, Product = new ProductMaster { Name = "商品1", Category = new CategoryMaster { Category = "区分1" } } },
                new InventoryInfo { Store = new StoreMaster { Name = "店舗B" }, Product = new ProductMaster { Name = "商品2", Category = new CategoryMaster { Category = "区分2" } } }
            }.AsQueryable();

            // 店舗Aかつ商品1のみを抽出
            var filter = new FilterInfo(new List<string> { "店舗A" }, new List<string> { "商品1" }, new List<string>());

            var result = data.CreateInventoryFilter(filter).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Store.Name, Is.EqualTo("店舗A"));
            Assert.That(result.First().Product.Name, Is.EqualTo("商品1"));
        }
    }
}