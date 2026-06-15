using DocumentFormat.OpenXml.Wordprocessing;
using FileControlLib.Processor;
using SalesManagerApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace SalesManagerApp.View.ViewControl
{
    public static class GridViewControl
    {
        /// <summary>
        /// グリッド初期化
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="column_name">引き伸ばし列名</param>
        public static void Init(this DataGridView grid, string column_name)
        {
            //行、列幅自動調整
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            //指定列を余剰分引き伸ばし
            grid.Columns[column_name].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        /// <summary>
        /// 並べ替え
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grid"></param>
        /// <param name="column_idx"></param>
        /// <param name="isAscending"></param>
        public static void SordGridColumn<T>(this DataGridView grid, int column_idx, ref bool isAscending)
        {
            if(grid.DataSource is List<T> lst_data)
            {
                //列のプロパティ名を取得
                string propName = grid.Columns[column_idx].DataPropertyName;
                if (string.IsNullOrEmpty(propName)) return;

                //型からプロパティ情報を取得
                var propInfo = typeof(T).GetProperty(propName);
                if (propInfo == null) return;

                //並び替え
                if (isAscending)
                {
                    lst_data = lst_data.OrderBy(data => propInfo.GetValue(data)).ToList();
                }
                else
                {
                    lst_data = lst_data.OrderByDescending(data => propInfo.GetValue(data)).ToList();
                }

                //フラグを反転
                isAscending = !isAscending;

                // グリッドの更新
                grid.DataSource = lst_data;
            }
        }

        /// <summary>
        /// 行ヘッダー挿入
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="e"></param>
        public static void InsertRowHeaderNumber(this DataGridView grid, DataGridViewRowPostPaintEventArgs e)
        {
            //表示する行番号を取得
            var rowNumber = (e.RowIndex + 1).ToString();

            //文字列の描画位置と整列方法を設定
            var stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };

            // 行ヘッダー内における描画範囲(矩形)を定義
            var headerBounds = new Rectangle(
                e.RowBounds.Left,       // 行の左端
                e.RowBounds.Top,        // 行の上端
                grid.RowHeadersWidth,   // 行ヘッダーの幅
                e.RowBounds.Height      // 行の高さ
            );

            // 行ヘッダーに行番号を描画
            e.Graphics.DrawString
            (
                rowNumber,                              // 描画する文字列(行番号)
                grid.RowHeadersDefaultCellStyle.Font,   // 使用するフォント
                SystemBrushes.ControlText,              // 使用するブラシ(標準テキスト色)
                headerBounds,                           // 描画する矩形領域
                stringFormat                            // 文字列の配置情報(中央揃え)
            );
        }

        /// <summary>
        /// クラスに設定された属性を読み取って、列の書式を自動設定する
        /// </summary>
        public static void ApplyFormatAttributes<T>(this DataGridView grid)
        {
            // クラス(T)の全プロパティを取得
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                // ① [ExcelFormat] 属性がついているかチェック
                // ※もし前回の属性の名前が違ったら書き換えてください
                var formatAttr = prop.GetCustomAttribute<ExcelFormatAttribute>();

                // 属性がついていて、かつDataGridView上にその列が存在するなら
                if (formatAttr != null && grid.Columns.Contains(prop.Name))
                {
                    // その列の書式を、属性で指定されたフォーマット(" #,##0" など)に設定！
                    grid.Columns[prop.Name].DefaultCellStyle.Format = formatAttr.Format;

                    // オマケ：数値なら右寄せにする設定を入れるとさらに綺麗になります！
                    grid.Columns[prop.Name].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }
    }
}
