using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileControlLib.Processor
{
    /// <summary>
    /// Excelファイルを処理する具体クラス
    /// </summary>
    public class ExcelFileProcessor : IFileProcessor
    {
        private const string SHEET_NAME = "Sheet1";

        /// <summary>拡張子</summary>
        public string Extension { get; } = ".xlsx";

        /// <summary>
        /// Excelファイルを保存する
        /// </summary>
        /// <param name="filePath">保存するファイルのパス</param>
        /// <param name="data">保存するデータのリスト</param>
        /// <returns>保存に成功したかどうか</returns>
        public bool SaveFile<T>(string filePath, List<T> data)
        {
            bool res = false;

            try
            {
                //ファイル書き込み処理
                res = WriteExcel(filePath, data);
            }
            catch (Exception e)
            {
                throw;
            }

            return res;
        }

        /// <summary>
        /// Excelファイルを読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <param name="data">読み込んだデータのリスト</param>
        /// <returns>読み込みに成功したかどうか</returns>
        public bool ReadFile<T>(string filePath, out List<T> data)
        {
            bool res = false;

            try
            {
                //ファイル読み込み処理
                data = ReadExcel<T>(filePath);
                res = true;
            }
            catch (Exception e)
            {
                throw;
            }

            return res;
        }

        /// <summary>
        /// Excelファイルを読み込む処理
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        /// <returns>読み込んだデータのリスト</returns>
        private List<T> ReadExcel<T>(string filePath)
        {
            List<T>? data = null;

            try
            {
                //ファイルの存在確認
                if (false == File.Exists(filePath))
                {
                    //ファイルが存在しない場合はエラー
                    throw new FileNotFoundException($"ファイルが見つかりません: {filePath}");
                }

                //指定ファイルをExcelワークブックとして開く
                using (var workbook = new XLWorkbook(filePath))
                {
                    //ワークブックからシート情報を取得
                    var worksheet = workbook.Worksheet(SHEET_NAME);

                    //データが入力されている範囲を取得
                    var range = worksheet.RangeUsed();

                    //データが入力されている範囲がない場合
                    if (null == range)
                    {
                        //データがないものとみなしエラー
                        throw new InvalidDataException($"ファイルにデータが含まれていません: {filePath}");
                    }

                    //データの初期化
                    data = new List<T>();

                    //データ範囲の1行目をヘッダーとして取得
                    var header_row = range.FirstRow();
                    //<列名、列番号>のディクショナリを作成
                    var headerMap = new Dictionary<string, int>();

                    //ヘッダー行のセルをループして列名と列番号をディクショナリに格納
                    foreach (var cell in header_row.Cells())
                    {
                        //セルの値をヘッダー文字列として取得し、前後の空白をトリミング
                        string header_name = cell.GetString().Trim();

                        //ヘッダー文字列が空白orNullでない場合
                        if (false == string.IsNullOrEmpty(header_name))
                        {
                            //ヘッダー文字列と列番号をディクショナリに格納
                            headerMap[header_name] = cell.Address.ColumnNumber;
                        }
                    }

                    //Tのプロパティ情報を取得し、書き込み可能なプロパティのみをリストで取得
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite).ToList();

                    //ヘッダー行以外のデータ行を取得
                    var data_rows = range.RowsUsed().Skip(1);

                    //取得データ行分ループ
                    foreach (var row in data_rows)
                    {
                        bool has_data = false;

                        //データの型にあったインスタンスを生成
                        var item = Activator.CreateInstance<T>();

                        //ジェネリック型から取得したプロパティ情報でループ
                        foreach (var property in properties)
                        {
                            //プロパティ名と一致するヘッダー列名があるか確認
                            if (headerMap.TryGetValue(property.Name, out int colKey))
                            {
                                //一致するヘッダー列がある場合、その列のセル情報を取得
                                var cell = row.Cell(colKey);

                                //セルが空でない場合
                                if (!cell.IsEmpty())
                                {
                                    //セルの値をobject型で取得
                                    var value = cell.GetValue<object>();
                                    //Null許容型のプロパティの場合は、基礎型を取得してから値を変換
                                    var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                                    //セルの値をプロパティの型に変換
                                    var convertedValue = Convert.ChangeType(value, targetType);

                                    //プロパティに変換した値をデータとしてセット
                                    property.SetValue(item, convertedValue);

                                    has_data = true;
                                }
                            }
                        }

                        // 空行でなければリストに追加
                        if (has_data)
                        {
                            //1行分のデータをリストに追加
                            data.Add(item);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
            }

            return data;
        }

        /// <summary>
        /// Excelファイルを書き込む処理
        /// </summary>
        /// <param name="filePath">保存するファイルのパス</param>
        /// <param name="data">保存するデータのリスト</param>
        private bool WriteExcel<T>(string filePath, List<T> data)
        {
            bool is_success = false;

            try
            {
                //Excelワークブックを生成
                using (var workbook = new XLWorkbook())
                {
                    //ワークブックに新しいシートを追加
                    var worksheet = workbook.Worksheets.Add(SHEET_NAME);

                    //左上セルにデータを一括書き込み
                    var range = worksheet.Cell(1, 1).InsertTable(data);
                    //テーブルデザインをデフォルトにする
                    range.Theme = XLTableTheme.None;

                    //ヘッダー行を取得
                    var header_row = worksheet.Row(1);
                    //太字設定
                    header_row.Style.Font.Bold = true;
                    //背景色を設定
                    header_row.Style.Fill.BackgroundColor = XLColor.LightGray;

                    //列インデックス
                    int colIndex = 1;
                    //保存するデータのプロパティを取得
                    var properties = typeof(T).GetProperties();
                    //プロパティ分ループ
                    foreach (var prop in properties)
                    {
                        //XLColumn-Ignoreがついているプロパティはスキップ
                        var xlAttr = prop.GetCustomAttribute<ClosedXML.Attributes.XLColumnAttribute>();
                        if (xlAttr != null && xlAttr.Ignore)
                        {
                            continue;
                        }

                        //書式属性がついているかチェック
                        var formatAttr = prop.GetCustomAttribute<ExcelFormatAttribute>();
                        if (formatAttr != null)
                        {
                            //属性がついていたら、その列に書式を適用する
                            worksheet.Column(colIndex).Style.NumberFormat.Format = formatAttr.Format;
                        }

                        // 次の列へ
                        colIndex++;
                    }
                    //列幅を自動調整
                    worksheet.Columns().AdjustToContents();
                    //指定されたパスにワークブックを保存
                    workbook.SaveAs(filePath);

                    //書き込み成功
                    is_success = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e}");
            }

            return is_success;
        }
    }

    /// <summary>
    /// 書式属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]//属性使用制限(今回はプロパティのみに制限)
    public class ExcelFormatAttribute : Attribute
    {
        public string Format { get; }

        public ExcelFormatAttribute(string format)
        {
            Format = format;
        }
    }
}
