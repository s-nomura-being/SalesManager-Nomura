using DatabaseLib;
using DatabaseLib.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerApp.DBControl
{
    /// <summary>
    /// 店舗マスタのコントロールクラス
    /// </summary>
    public class StoreMasterControl
    {
        /// <summary>DBコントローラー</summary>
        private DBController _controller;

        /// <summary>
        /// 店舗マスタのコントロールクラス
        /// </summary>
        /// <param name="dbType">DBの種類</param>
        public StoreMasterControl(E_DBType dbType)
        {
            //DBコントローラー生成
            _controller = new DBController(dbType);

            //テーブルの初期化
            _controller.Init();
        }

        /// <summary>
        /// 店舗マスタ複数登録
        /// </summary>
        /// <param name="storeMasters">店舗マスタデータのリスト</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetStoreMaster(List<StoreMaster> storeMasters)
        {
            bool result = false;

            try
            {
                //セット対象のデータチェック
                if (null == storeMasters)
                {
                    Console.WriteLine("Error: 店舗マスタがnullです。");
                    return false;
                }

                //データ数分ループして登録する
                foreach (var storemaster in storeMasters)
                {
                    //店舗マスタ登録
                    result &= SetStoreMaster(storemaster);
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
        /// 店舗マスタ登録
        /// </summary>
        /// <param name="storeMaster">店舗マスタデータ</param>
        /// <returns>成功した場合はtrue、それ以外はfalse</returns>
        public bool SetStoreMaster(StoreMaster storeMaster)
        {
            bool result = false;

            try
            {
                //セット対象のデータチェック
                if(null == storeMaster)
                {
                    Console.WriteLine("Error: 店舗マスタがnullです。");
                    return false;
                }

                //挿入か更新か判定する
                if (true == CheckUpdateStoreMaster(storeMaster))
                {
                    //更新日時セット
                    storeMaster.Updated_at = DateTime.Now;
                    //店舗マスタを更新
                    result = _controller.UpdateRecord(storeMaster, storeMaster.Id);
                }
                else
                {
                    //作成日時セット
                    storeMaster.Created_at = DateTime.Now;
                    //店舗マスタを挿入
                    result = _controller.InsertRecord(storeMaster);
                }
            }
            catch (Exception e)
            {
                //エラー処理
                Console.WriteLine($"Error: {e}");
            }

            return result;
        }

        /// <summary>
        /// 店舗名の重複チェック
        /// </summary>
        /// <param name="storeMaster">比較元の店舗マスタ</param>
        /// <returns>重複が存在する場合はtrue、それ以外はfalse</returns>
        private bool ExistsStoreMaster(StoreMaster storeMaster)
        {
            bool duplicate = false;

            try
            {
                //店舗名が一致するレコードを取得する
                var storeMasters = _controller.SelectRecord<StoreMaster>(data => data.Name == storeMaster.Name);
                //取得レコードが1つでもあれば重複
                duplicate = storeMasters.ToList().Count > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return duplicate;
        }

        /// <summary>
        /// 挿入か、更新かチェック
        /// </summary>
        /// <param name="storeMaster">店舗マスタデータ</param>
        /// <returns>更新対象の場合はtrue、それ以外はfalse</returns>
        private bool CheckUpdateStoreMaster(StoreMaster storeMaster)
        {
            bool update = false;

            try
            {
                //IDが一致するレコードがあるかチェックする
                var storeMasters = _controller.SelectRecord<StoreMaster>(data => data.Id == storeMaster.Id);
                //一致するレコードがあれば更新、なければ挿入
                update = storeMasters.ToList().Count > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return update;
        }

        /// <summary>
        /// 店舗マスタ全取得
        /// </summary>
        /// <param name="storeMasterList"></param>
        /// <returns></returns>
        public bool GetStoreMaster(out List<StoreMaster> storeMasterList)
        {
            bool result = false;
            storeMasterList = new List<StoreMaster>();

            try
            {
                //店舗マスタデータを全て取得する
                storeMasterList = _controller.SelectRecord<StoreMaster>().ToList();
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return result;
        }

        /// <summary>
        /// 条件に一致する店舗マスタ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="storeMasterList"></param>
        /// <returns></returns>
        public bool GetStoreMaster(Expression<Func<StoreMaster, bool>> condition, out List<StoreMaster> storeMasterList)
        {
            bool result = false;
            storeMasterList = new List<StoreMaster>();

            try
            {
                //条件と一致する店舗マスタデータを取得する
                storeMasterList = _controller.SelectRecord(condition).ToList();
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }

            return result;
        }
    }
}