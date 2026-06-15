using FileControlLib;
using SalesManagerApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerApp.FileIO
{
    public class StoreMasterViewFileManager
    {
        private readonly IFileProcessor _fileprocesser;

        /// <summary>保存ファイル名</summary>
        private const string SAVE_FILENAME = "master/店舗マスタ";

        /// <summary>
        /// ファイルの読み書き操作を行うクラス
        /// <para>
        /// 指定されたファイル形式に応じたファイル処理クラスを使用して、ファイルの読み書きを行う
        /// </para>
        /// </summary>
        /// <param name="fileprocesser"></param>
        public StoreMasterViewFileManager(IFileProcessor fileprocesser)
        {
            _fileprocesser = fileprocesser;
        }

        /// <summary>
        /// ファイルを読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <param name="data">読み込んだデータのリスト</param>
        /// <returns>読み込みに成功したかどうか</returns>
        public bool ReadFile(string filePath, out List<StoreMasterData> data)
        {
            return _fileprocesser.ReadFile(filePath, out data);
        }

        /// <summary>
        /// ファイルを保存する
        /// </summary>
        /// <param name="filePath">保存するファイルのパス</param>
        /// <param name="data">保存するデータのリスト</param>
        /// <returns>保存に成功したかどうか</returns>
        public bool SaveFile(List<StoreMasterData> data)
        {
            string filePath = Path.Combine(Application.StartupPath, string.Concat(SAVE_FILENAME + _fileprocesser.Extension));
            return _fileprocesser.SaveFile(filePath, data);
        }
    }
}
