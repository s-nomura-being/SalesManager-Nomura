using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using DatabaseLib.Table;

namespace DatabaseLib
{
    public interface IDataBase : IDisposable
    {
        /// <summary>
        /// DB初期化処理
        /// </summary>
        void Init();

        /// <summary>
        /// データ挿入
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <param name="insert_data">挿入するデータ</param>
        /// <returns>T:成功 F:失敗</returns>
        bool InsertRecord<T>(T insert_data) where T : class;

        /// <summary>
        /// データ更新
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <param name="update_data">更新するデータ</param>
        /// <param name="id">更新対象のキー</param>
        /// <returns>T:成功 F:失敗</returns>
        bool UpdateRecord<T>(T update_data, params object[] id) where T : class;

        /// <summary>
        /// データ削除
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <param name="id">削除対象のキー</param>
        /// <returns>T:成功 F:失敗</returns>
        bool DeleteRecord<T>(int id) where T : class;

        /// <summary>
        /// 全レコード取得
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <returns>取得レコードリスト</returns>
        IQueryable<T> SelectRecord<T>() where T : class;

        /// <summary>
        /// 条件に一致するレコードを全て取得
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <param name="condition">条件式</param>
        /// <returns>取得レコードリスト</returns>
        IQueryable<T> SelectRecord<T>(Expression<Func<T, bool>> condition) where T : class;

        /// <summary>
        /// 列の値が一番大きいレコードを1つ取得
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <typeparam name="TColumn">任意の列</typeparam>
        /// <param name="selector">任意の列</param>
        /// <returns>取得レコード</returns>
        T? SelectTopRecord<T, TColumn>(Expression<Func<T, TColumn>> selector) where T : class;

        /// <summary>
        /// テーブル内レコード全削除
        /// </summary>
        /// <typeparam name="T">テーブルクラス型</typeparam>
        /// <returns>T:成功 F:失敗</returns>
        bool ClearTable<T>() where T : class;

        /// <summary>
        /// DB全削除
        /// </summary>
        /// <returns>T:成功 F:失敗</returns>
        bool DeleteDB();
    }
}
