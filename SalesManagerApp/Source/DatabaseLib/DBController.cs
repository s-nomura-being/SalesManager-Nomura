using DatabaseLib.Table;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib
{
    /// <summary>
    /// 使用するDBの種類
    /// </summary>
    public enum E_DBType
    {
        SQLite = 0,
        SQL_Server,
        MySQL,

    }

    /// <summary>
    /// DB操作クラス
    /// </summary>
    public class DBController : DbContext, IDataBase
    {
        /// <summary>DB接続文字列</summary>
        private string _connection_string { get; }

        /*
            テーブルフィールドは必ず書くこと
            書かないとテーブルを作れない
            
            例：
                public DbSet<Teacher> Teachers { get; set; }
                public DbSet<Student> Students { get; set; }
        */
        public DbSet<StoreMaster> StoreMaster { get; set; }
        public DbSet<ProductMaster> ProductMaster { get; set; }
        public DbSet<CategoryMaster> CategoryMaster { get; set; }
        public DbSet<SalesResult> SalesResult { get; set; }
        public DbSet<InventoryInfo> InventoryInfo { get; set; }

        /// <summary>
        /// 使用するDBタイプ
        /// </summary>
        public E_DBType DBType { get; }

        /// <summary>
        /// DB操作クラス
        /// </summary>
        /// <param name="db_type">使用するDBの種類</param>
        public DBController(E_DBType db_type, string connection_string) : base()
        {
            //DBタイプを記憶
            DBType = db_type;
            //接続文字列記憶
            _connection_string = connection_string;
        }

        /// <summary>
        /// DBの仕様設定
        /// </summary>
        /// <param name="optionsBuilder">DBコンテキストのオプションビルダー</param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //対応するDBの仕様を設定
            switch (DBType)
            {
                case E_DBType.SQLite:
                    //SQLiteの接続先を設定する
                    optionsBuilder.UseSqlite(_connection_string);
                    break;
                case E_DBType.SQL_Server:
                    throw new NotImplementedException();
                case E_DBType.MySQL:
                    throw new NotImplementedException();
                default:
                    throw new Exception("未定義のDBタイプ");
            }
        }

        /// <summary>
        /// DBテーブル設定
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //在庫情報テーブルは店舗IDと商品IDの複合キーであることを設定する
            modelBuilder.Entity<InventoryInfo>().HasKey(key => new { key.StoreId, key.ProductId });
        }

        /// <summary>
        /// DB初期化処理
        /// </summary>
        public void Init()
        {
            //DBがなければ作成
            Database.EnsureCreated();
        }

        /// <summary>
        /// データ挿入
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <param name="insert_data">挿入するデータ</param>
        /// <returns>T:成功 F:失敗</returns>
        public bool InsertRecord<T>(T insert_data) where T : class
        {
            bool result = false;

            try
            {
                //データ追加
                //Set<T>().でテーブルデータを操作できる
                Set<T>().Add(insert_data);

                //DB保存
                SaveChanges();

                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Err:{e}");
                Debug.WriteLine($"Err:{e}");
                throw;
            }

            return result;
        }

        /// <summary>
        /// データ更新
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <param name="update_data">更新するデータ</param>
        /// <param name="ids">更新対象のキー</param>
        /// <returns>T:成功 F:失敗</returns>
        public bool UpdateRecord<T>(T update_data, params object[] ids) where T : class
        {
            bool result = false;

            try
            {
                //更新対象のレコードを取得
                //Find()でEFCoreが自動的に主キーから検索してくれる
                T? target_data = Set<T>().Find(ids);

                if(null != target_data)
                {
                    //更新対象レコードを使って、テーブルの主キー構成を取得
                    var key_info = Entry(target_data).Metadata.FindPrimaryKey();
                    if (null != key_info)
                    {
                        //取得した主キー構成からデータ操作ハンドルを取得、ハンドル数分ループ(複合キーの可能性もあるため配列となっている)
                        foreach (var property in key_info.Properties)
                        {
                            if (property.PropertyInfo != null)
                            {
                                //取得したハンドルを使って、更新対象レコードの主キー値を取得
                                var key = property.PropertyInfo.GetValue(target_data);

                                //ハンドルを使って、取得した主キーを更新用データに適用する
                                property.PropertyInfo.SetValue(update_data, key);
                            }
                        }
                    }

                    //対象データを更新
                    //update_dataのプロパティを丸ごとコピーできる
                    /*
                        Entry():
                            EntityEntry型オブジェクトを取得
                            テーブルデータ専用の監視レポートみたいなもの
                            引数のデータはDBのものと同じ？ どのプロパティが書き換えられた？を全て詰め込んであるオブジェクト
                            レコードのシステム的な情報がびっしり詰まったレポート

                        CurrentValues:
                            EntityEntry型オブジェクトから現在の値を取得する
                            DBに登録された値ではなく、メモリ上にあるデータを取得する

                        SetValues:
                            コピー元とコピー先で一致するプロパティをコピー対象とする
                    */
                    Entry(target_data).CurrentValues.SetValues(update_data);

                    //DB保存
                    SaveChanges();
                }

                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Err:{e}");
                Debug.WriteLine($"Err:{e}");
                throw;
            }

            return result;
        }

        /// <summary>
        /// データ削除
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <param name="id">削除対象のキー</param>
        /// <returns>T:成功 F:失敗</returns>
        public bool DeleteRecord<T>(int id) where T : class
        {
            bool result = false;

            try
            {
                //更新対象を取得
                T? src_data = Set<T>().Find(id);

                if (null != src_data)
                {
                    //データ削除
                    Set<T>().Remove(src_data);

                    //DB保存
                    SaveChanges();
                }

                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Err:{e}");
                Debug.WriteLine($"Err:{e}");
                throw;
            }

            return result;
        }

        /// <summary>
        /// 全レコード取得
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <returns>取得レコードリスト</returns>
        public IQueryable<T> SelectRecord<T>() where T : class
        {
            //テーブルデータを全て返す
            return Set<T>();
        }

        /// <summary>
        /// 条件に一致するレコードを全て取得
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <param name="condition">条件式</param>
        /// <returns>取得レコードリスト</returns>
        public IQueryable<T> SelectRecord<T>(Expression<Func<T, bool>> condition) where T : class
        {
            //条件に一致するテーブルデータを返す
            return Set<T>().Where(condition);
        }

        /// <summary>
        /// 列の値が一番大きいレコードを1つ取得
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <typeparam name="TColumn">任意の列</typeparam>
        /// <param name="selector">任意の列</param>
        /// <returns>取得レコード</returns>
        public T? SelectTopRecord<T, TColumn>(Expression<Func<T, TColumn>> selector) where T : class
        {
            //LINQを使ってデータの取得も可能
            return Set<T>().OrderByDescending(selector).FirstOrDefault();
        }

        /// <summary>
        /// テーブル内レコード全削除
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <returns>T:成功 F:失敗</returns>
        public bool ClearTable<T>() where T : class
        {
            bool result = false;

            try
            {
                //データ削除 この関数を使用するとSaveChangesすらいらない
                int delete_count = Set<T>().ExecuteDelete();

                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Err:{e}");
                Debug.WriteLine($"Err:{e}");
                throw;
            }

            return result;
        }

        /// <summary>
        /// DB全削除
        /// </summary>
        /// <returns>T:成功 F:失敗</returns>
        public bool DeleteDB()
        {
            bool result = false;

            try
            {
                //DB削除
                Database.EnsureDeleted();

                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Err:{e}");
                Debug.WriteLine($"Err:{e}");
                throw;
            }

            return result;
        }
    }
}
