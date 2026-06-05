using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileControlLib.Processor
{
    /// <summary>
    /// JSONファイルを処理する具体クラス
    /// </summary>
    public class JsonFileProcessor : IFileProcessor
    {
        /// <summary>
        /// JSONファイルを保存する
        /// </summary>
        /// <param name="filePath">保存するファイルのパス</param>
        /// <param name="data">保存するデータのリスト</param>
        /// <returns>保存に成功したかどうか</returns>
        public bool SaveFile<T>(string filePath, List<T> data)
        {
            bool res = false;

            try
            {
                //ファイル書き込み処理実行
                res = WriteJSON(filePath, data);
            }
            catch (Exception e)
            {

                throw;
            }

            return res;
        }

        /// <summary>
        /// JSONファイルを読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <param name="data">読み込んだデータのリスト</param>
        /// <returns>読み込みに成功したかどうか</returns>
        public bool ReadFile<T>(string filePath, out List<T> data)
        {
            bool res = false;

            try
            {
                //ファイル読み込み処理実行
                data = ReadJSON<T>(filePath);

                res = true;
            }
            catch (Exception e)
            {

                throw;
            }

            return res;
        }


        

        /// <summary>
        /// JSONファイルを読み込む処理
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <returns>読み込んだデータのリスト</returns>
        private List<T> ReadJSON<T>(string filePath)
        {
            List<T>? data = null;

            try
            {
                //ファイルの存在確認
                if(false == File.Exists(filePath))
                {
                    //ファイルが存在しない場合はエラー
                    throw new FileNotFoundException("ファイルが見つかりません", filePath);
                }

                //指定ファイルの内容を全て文字列として読み込む
                string json_string = File.ReadAllText(filePath);
                //読み込んだJSON文字列をオブジェクトに変換
                data = JsonConvert.DeserializeObject<List<T>>(json_string);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
            }

            return data;
        }

        /// <summary>
        /// JSONファイルを保存する処理
        /// </summary>
        /// <param name="filePath">保存するファイルのパス</param>
        /// <param name="data">保存するデータのリスト</param>
        /// <returns>保存に成功したかどうか</returns>
        private bool WriteJSON<T>(string filePath, List<T> data)
        {
            bool is_success = false;

            try
            {
                //オブジェクトのデータをJSON文字列に変換
                string json_string = JsonConvert.SerializeObject(data, Formatting.Indented);
                //変換したJSON文字列を指定ファイルに書き込む
                File.WriteAllText(filePath, json_string);
                //書き込み成功
                is_success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
            }

            return is_success;
        }
    }
}