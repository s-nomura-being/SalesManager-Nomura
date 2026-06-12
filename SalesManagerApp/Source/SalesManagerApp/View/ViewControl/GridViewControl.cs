using DocumentFormat.OpenXml.Wordprocessing;
using SalesManagerApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace SalesManagerApp.View.ViewControl
{
    public static class GridViewControl
    {
        public static void Init(this DataGridView grid, string column_name)
        {
            //行、列幅自動調整
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            //指定列を余剰分引き伸ばし
            grid.Columns[column_name].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

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
    }
}
