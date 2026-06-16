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
    /// 商品マスタのコントロールクラス
    /// </summary>
    public class ProductMasterControl
    {
        /// <summary>DBコントローラー</summary>
        private DBController _controller;

        /// <summary>
        /// 商品マスタのコントロールクラス
        /// </summary>
        /// <param name="dbType">DBの種類</param>
        public ProductMasterControl(E_DBType dbType, string connection_string)
        {
            //DBコントローラー生成
            _controller = new DBController(dbType, connection_string);

            //テーブルの初期化
            _controller.Init();
        }

        /// <summary>
        /// 商品マスタ複数登録
        /// </summary>
        /// <param name="productMasters">商品マスタデータのリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetProductMaster(List<ProductMaster> productMasters)
        {
            bool result = true;

            try
            {
                //セット対象のデータチェック
                if (null == productMasters)
                {
                    Console.WriteLine("Error: 商品マスタがnullです。");
                    return false;
                }

                //データ数分ループして登録する
                foreach (var productmaster in productMasters)
                {
                    //商品マスタ登録
                    result &= SetProductMaster(productmaster);
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
        /// 商品マスタ登録
        /// </summary>
        /// <param name="productMaster">商品マスタデータ</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetProductMaster(ProductMaster productMaster)
        {
            bool result = false;

            try
            {
                //セット対象のデータチェック
                if (null == productMaster)
                {
                    Console.WriteLine("Error: 商品マスタがnullです。");
                    return false;
                }

                //挿入か更新か判定する
                if (true == CheckUpdateProductMaster(productMaster))
                {
                    //商品マスタを更新
                    result = _controller.UpdateRecord(productMaster, productMaster.Id);
                }
                else
                {
                    //商品マスタを挿入
                    result = _controller.InsertRecord(productMaster);
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
        /// 商品名の重複チェック
        /// </summary>
        /// <param name="productMaster">比較元の商品マスタ</param>
        /// <returns>重複が存在する場合はtrue、それ以外はfalse</returns>
        private bool ExistsProductMaster(ProductMaster productMaster)
        {
            bool duplicate = false;

            try
            {
                //商品名が一致するレコードを取得
                var productMasters = _controller.SelectRecord<ProductMaster>(data => data.Name == productMaster.Name);
                //取得レコードが1つでもあれば重複
                duplicate = productMasters.ToList().Count > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            return duplicate;
        }

        /// <summary>
        /// 挿入か、更新かチェック
        /// </summary>
        /// <param name="productMaster">商品マスタデータ</param>
        /// <returns>更新対象の場合はtrue、それ以外はfalse</returns>
        private bool CheckUpdateProductMaster(ProductMaster productMaster)
        {
            bool update = false;

            try
            {
                //IDが一致するレコードがあるかチェックする
                var productMasters = _controller.SelectRecord<ProductMaster>(data => data.Id == productMaster.Id);
                //一致するレコードがあれば更新、なければ挿入
                update = productMasters.ToList().Count > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            return update;
        }

        /// <summary>
        /// 商品マスタ全取得
        /// </summary>
        /// <param name="productMasterList"></param>
        /// <returns></returns>
        public bool GetProductMaster(out List<ProductMaster> productMasterList)
        {
            bool result = false;
            productMasterList = new List<ProductMaster>();

            try
            {
                //商品マスタデータを全て取得する(商品毎の販売実績、在庫情報、カテゴリも同時に取得)
                productMasterList = _controller.SelectRecord<ProductMaster>()
                                        .Include(data => data.SalesResults)
                                        .Include(data => data.InventoryInfos)
                                        .Include(data => data.Category)
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
        /// 条件に一致する商品マスタ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="productMasterList"></param>
        /// <returns></returns>
        public bool GetProductMaster(Expression<Func<ProductMaster, bool>> condition, out List<ProductMaster> productMasterList)
        {
            bool result = false;
            productMasterList = new List<ProductMaster>();

            try
            {
                //条件と一致する商品マスタデータを取得する(商品毎の販売実績、在庫情報、カテゴリも同時に取得)
                productMasterList = _controller.SelectRecord(condition)
                                        .Include(data => data.SalesResults)
                                        .Include(data => data.InventoryInfos)
                                        .Include(data => data.Category)
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
