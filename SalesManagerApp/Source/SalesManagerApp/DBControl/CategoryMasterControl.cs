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
    /// 区分マスタのコントロールクラス
    /// </summary>
    public class CategoryMasterControl
    {
        /// <summary>DBコントローラー</summary>
        private DBController _controller;

        /// <summary>
        /// 区分マスタのコントロールクラス
        /// </summary>
        /// <param name="dbType">DBの種類</param>
        public CategoryMasterControl(E_DBType dbType, string connection_string)
        {
            //DBコントローラー生成
            _controller = new DBController(dbType, connection_string);

            //テーブルの初期化
            _controller.Init();
        }

        /// <summary>
        /// 区分マスタ複数登録
        /// </summary>
        /// <param name="categoryMasters">区分マスタデータのリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetCategoryMaster(List<CategoryMaster> categoryMasters)
        {
            bool result = true;

            try
            {
                //セット対象のデータチェック
                if (null == categoryMasters)
                {
                    Console.WriteLine("Error: 区分マスタがnullです。");
                    return false;
                }

                //データ数分ループして登録する
                foreach (var categorymaster in categoryMasters)
                {
                    //区分マスタ登録
                    result &= SetCategoryMaster(categorymaster);
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
        /// 区分マスタ登録
        /// </summary>
        /// <param name="categoryMaster">区分マスタデータ</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetCategoryMaster(CategoryMaster categoryMaster)
        {
            bool result = false;

            try
            {
                //セット対象のデータチェック
                if (null == categoryMaster)
                {
                    Console.WriteLine("Error: 区分マスタがnullです。");
                    return false;
                }

                //挿入か更新か判定する
                if (true == CheckUpdateCategoryMaster(categoryMaster))
                {
                    //区分マスタを更新
                    result = _controller.UpdateRecord(categoryMaster, categoryMaster.Id);
                }
                else
                {
                    //区分マスタを挿入
                    result = _controller.InsertRecord(categoryMaster);
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
        /// 区分名の重複チェック
        /// </summary>
        /// <param name="categoryMaster">比較元の区分マスタ</param>
        /// <returns>重複が存在する場合はtrue、それ以外はfalse</returns>
        private bool ExistsCategoryMaster(CategoryMaster categoryMaster)
        {
            bool duplicate = false;

            try
            {
                //区分名が一致するレコードを取得
                var categoryMasters = _controller.SelectRecord<CategoryMaster>(data => data.Category == categoryMaster.Category);
                //取得レコードが1つでもあれば重複
                duplicate = categoryMasters.ToList().Count > 0;
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
        /// <param name="categoryMaster">区分マスタデータ</param>
        /// <returns>更新対象の場合はtrue、それ以外はfalse</returns>
        private bool CheckUpdateCategoryMaster(CategoryMaster categoryMaster)
        {
            bool update = false;

            try
            {
                //IDが一致するレコードがあるかチェックする
                var categoryMasters = _controller.SelectRecord<CategoryMaster>(data => data.Id == categoryMaster.Id);
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
        /// 区分マスタ全取得
        /// </summary>
        /// <param name="categoryMasterList"></param>
        /// <returns></returns>
        public bool GetCategoryMaster(out List<CategoryMaster> categoryMasterList)
        {
            bool result = false;
            categoryMasterList = new List<CategoryMaster>();

            try
            {
                //区分マスタデータを全て取得する
                categoryMasterList = _controller.SelectRecord<CategoryMaster>()
                                                .AsNoTracking()
                                                .Include(data => data.ProductMasters)
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
        /// 条件に一致する区分マスタ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="categoryMasterList"></param>
        /// <returns></returns>
        public bool GetCategoryMaster(Expression<Func<CategoryMaster, bool>> condition, out List<CategoryMaster> categoryMasterList)
        {
            bool result = false;
            categoryMasterList = new List<CategoryMaster>();

            try
            {
                //条件と一致する区分マスタデータを取得する
                categoryMasterList = _controller.SelectRecord(condition)
                                                .AsNoTracking()
                                                .Include(data => data.ProductMasters)
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
