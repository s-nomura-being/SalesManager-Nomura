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
    /// 在庫情報のコントロールクラス
    /// </summary>
    public class InventoryInfoControl
    {
        /// <summary>DBコントローラー</summary>
        private DBController _controller;

        /// <summary>
        /// 在庫情報のコントロールクラス
        /// </summary>
        /// <param name="dbType">DBの種類</param>
        public InventoryInfoControl(E_DBType dbType, string connection_string)
        {
            //DBコントローラー生成
            _controller = new DBController(dbType, connection_string);

            //テーブルの初期化
            _controller.Init();
        }

        /// <summary>
        /// 在庫情報複数登録
        /// </summary>
        /// <param name="inventoryinfos">在庫情報データのリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetInventoryInfo(List<InventoryInfo> inventoryinfos)
        {
            bool result = false;

            try
            {
                //セット対象のデータチェック
                if (null == inventoryinfos)
                {
                    Console.WriteLine("Error: 在庫情報がnullです。");
                    return false;
                }

                //データ数分ループして登録する
                foreach (var storemaster in inventoryinfos)
                {
                    //在庫情報登録
                    result &= SetInventoryInfo(storemaster);
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
        /// 在庫情報登録
        /// </summary>
        /// <param name="inventoryinfo">在庫情報データ</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetInventoryInfo(InventoryInfo inventoryinfo)
        {
            bool result = false;

            try
            {
                //セット対象のデータチェック
                if (null == inventoryinfo)
                {
                    Console.WriteLine("Error: 在庫情報がnullです。");
                    return false;
                }

                if(true == CheckUpdateInventory(inventoryinfo))
                {
                    //在庫情報を更新
                    result = _controller.UpdateRecord(inventoryinfo, inventoryinfo.StoreId, inventoryinfo.ProductId);
                }
                else
                {
                    //在庫情報を挿入
                    result = _controller.InsertRecord(inventoryinfo);
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
        /// 挿入か、更新かチェック
        /// </summary>
        /// <param name="inventory">区分マスタデータ</param>
        /// <returns>更新対象の場合はtrue、それ以外はfalse</returns>
        private bool CheckUpdateInventory(InventoryInfo inventory)
        {
            bool update = false;

            try
            {
                //IDが一致するレコードがあるかチェックする
                var categoryMasters = _controller.SelectRecord<InventoryInfo>(data => data.StoreId == inventory.StoreId && data.ProductId == inventory.ProductId);
                //一致するレコードがあれば更新、なければ挿入
                update = categoryMasters.ToList().Count > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            return update;
        }

        /// <summary>
        /// 在庫情報全取得
        /// </summary>
        /// <param name="inventoryinfoList"></param>
        /// <returns></returns>
        public bool GetInventoryInfo(out List<InventoryInfo> inventoryinfoList)
        {
            bool result = false;
            inventoryinfoList = new List<InventoryInfo>();

            try
            {
                //在庫情報データを全て取得する(店舗ID、商品IDから店舗名、商品名も同時に取得)
                inventoryinfoList = _controller.SelectRecord<InventoryInfo>()
                                                .Include(data => data.Store)
                                                .Include(data => data.Product)
                                                .Include(data => data.Product.Category)
                                                .Include(data => data.Product.SalesResults)
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
        /// 条件に一致する在庫情報取得
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="inventoryinfoList"></param>
        /// <returns></returns>
        public bool GetInventoryInfo(Expression<Func<InventoryInfo, bool>> condition, out List<InventoryInfo> inventoryinfoList)
        {
            bool result = false;
            inventoryinfoList = new List<InventoryInfo>();

            try
            {
                //条件と一致する在庫情報データを取得する(店舗ID、商品IDから店舗名、商品名も同時に取得)
                inventoryinfoList = _controller.SelectRecord(condition)
                                                .Include(data => data.Store)
                                                .Include(data => data.Product)
                                                .Include(data => data.Product.Category)
                                                .Include(data => data.Product.SalesResults)
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
        /// 条件に一致する在庫情報取得
        /// </summary>
        /// <param name="filter">フィルター情報</param>
        /// <param name="inventoryinfoList"></param>
        /// <returns></returns>
        public bool GetInventoryInfo(FilterInfo filter, out List<InventoryInfo> inventoryinfoList)
        {
            bool result = false;
            inventoryinfoList = new List<InventoryInfo>();

            try
            {
                //ベースとなるIQueryableを取得
                var query = _controller.SelectRecord<InventoryInfo>();
                //フィルター情報を適用
                query = query.CreateInventoryFilter(filter);
                //条件と一致する在庫情報データを取得する(店舗ID、商品IDから店舗名、商品名も同時に取得)
                inventoryinfoList = query.Include(data => data.Store)
                                            .Include(data => data.Product)
                                            .Include(data => data.Product.Category)
                                            .Include(data => data.Product.SalesResults)
                                            .ToList();
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            return result;
        }
    }
}
