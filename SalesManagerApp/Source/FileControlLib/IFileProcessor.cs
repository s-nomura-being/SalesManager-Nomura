using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileControlLib
{
    public interface IFileProcessor
    {
        /// <summary>拡張子</summary>
        string Extension { get; }

        /// <summary>
        /// ファイルを保存する
        /// </summary>
        /// <param name="filePath">保存するファイルのパス</param>
        /// <param name="data">保存するデータのリスト</param>
        /// <returns>保存に成功したかどうか</returns>
        bool SaveFile<T>(string filePath, List<T> data);

        /// <summary>
        /// ファイルを読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <param name="data">読み込んだデータのリスト</param>
        /// <returns>読み込みに成功したかどうか</returns>
        bool ReadFile<T>(string filePath, out List<T> data);
    }

    internal class DummyDataType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
