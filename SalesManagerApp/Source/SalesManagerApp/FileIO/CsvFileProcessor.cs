using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsvHelper;
using System.Globalization;

namespace SalesManagerApp.FileIO
{
    /// <summary>
    /// CSVファイルを処理する具体クラス
    /// </summary>
    public class CsvFileProcessor : IFileProcessor
    {
        /// <summary>
        /// CSVファイルを保存する
        /// </summary>
        /// <param name="filePath">保存するファイルのパス</param>
        /// <param name="data">保存するデータのリスト</param>
        /// <returns>保存に成功したかどうか</returns>
        public bool SaveFile<T>(string filePath, List<T> data)
        {
            bool res = false;

            try
            {
                //CSVファイル書き込み処理実行
                res = WriteCSV(filePath, data);
            }
            catch (Exception e)
            {

                throw;
            }

            return res;
        }

        /// <summary>
        /// CSVファイルを読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <param name="data">読み込んだデータのリスト</param>
        /// <returns>読み込みに成功したかどうか</returns>
        public bool ReadFile<T>(string filePath, out List<T> data)
        {
            bool res = false;

            try
            {
                //CSVファイル読み込み処理実行
                data = ReadCSV<T>(filePath);

                if(null != data)
                {
                    res = true;
                }
                else
                {
                    //データなし
                }
            }
            catch (Exception e)
            {

                throw;
            }

            return res;
        }

        /// <summary>
        /// CSVファイルを読み込む処理
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <returns>読み込んだデータのリスト</returns>
        private List<T> ReadCSV<T>(string filePath)
        {
            List<T>? data = null;

            try
            {
                //指定ファイルから読み取りモードでファイルコントロールを生成
                using (var reader = new StreamReader(filePath))
                {
                    //CsvHelperを使用してCSVファイルの読み取りハンドルを生成
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        //読み取ったCSVファイルの内容を、指定の型のコレクションに変換して取得
                        data = csv.GetRecords<T>().ToList();
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }

            return data;
        }

        /// <summary>
        /// CSVファイルを保存する処理
        /// </summary>
        /// <param name="filePath">保存するファイルのパス</param>
        /// <param name="data">保存するデータのリスト</param>
        /// <returns>保存に成功したかどうか</returns>
        private bool WriteCSV<T>(string filePath, List<T> data)
        {
            bool is_success = false;

            try
            {
                //指定ファイルから書き込みモードでファイルコントロールを生成
                using (var writer = new StreamWriter(filePath))
                {
                    //CsvHelperを使用してCSVファイルの書き込みハンドルを生成
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        //引数のデータをCSVファイルに書き込む
                        csv.WriteRecord(data);

                        //書き込み成功
                        is_success = true;
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }

            return is_success;
        }
    }
}
