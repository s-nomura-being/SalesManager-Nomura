using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerApp.View.ViewControl
{
    /// <summary>
    /// チェックリストボックス操作クラス
    /// </summary>
    public class CheckListControl
    {
        /// <summary>制御対象のチェックリスト</summary>
        private CheckedListBox _checklist { get; }
        /// <summary>キーワードテキストボックス</summary>
        private TextBox _keywordtext { get; }
        /// <summary>チェックリストに表示する全名前データリスト</summary>
        private List<string> _all_namelist { get; }

        /// <summary>現在チェック済みの名前リスト</summary>
        private HashSet<string> _checkedNames = new HashSet<string>();

        /// <summary>チェックリスト更新中フラグ</summary>
        private bool _isUpdating = false;

        /// <summary>
        /// チェックリストボックス操作クラス
        /// </summary>
        /// <param name="listBox">チェックリストボックス</param>
        /// <param name="searchBox">キーワードテキストボックス</param>
        /// <param name="allItems">チェックリストに表示する全項目リスト</param>
        public CheckListControl(CheckedListBox listBox, TextBox searchBox, List<string> allItems)
        {
            //制御対象のコントロールを渡してもらう
            _checklist = listBox;
            _keywordtext = searchBox;
            //チェックリストに表示する項目をもらう
            _all_namelist = allItems;

            //コントロールにイベント追加
            _keywordtext.TextChanged += (s, e) => FilterCheckList(_keywordtext.Text);
            _checklist.ItemCheck += ListBox_ItemCheck;

            // 初期表示
            FilterCheckList();
        }

        /// <summary>
        /// チェック済みの名前リスト取得
        /// </summary>
        /// <returns></returns>
        public List<string> GetCheckedNames() => _checkedNames.ToList();

        /// <summary>
        /// チェック変更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            //更新中ならスルー
            if (_isUpdating) return;

            //チェック位置の名前を取得
            string item_name = _checklist.Items[e.Index]?.ToString() ?? "";
            //名前が空白ならスルー
            if (string.IsNullOrEmpty(item_name)) return;

            if (e.NewValue == CheckState.Checked)
            {
                //チェックONならチェック済みリストに追加
                _checkedNames.Add(item_name);
            }
            else
            {
                //チェックOFFならチェック済みリストから削除
                _checkedNames.Remove(item_name);
            }
        }

        /// <summary>
        /// チェックリスト キーワード絞り込み
        /// </summary>
        /// <param name="keyword">キーワード</param>
        public void FilterCheckList(string keyword = "")
        {
            //二重処理を防ぐため更新中フラグON
            _isUpdating = true;

            //一度チェックリストをクリアする
            _checklist.Items.Clear();

            //全項目からキーワードと一致するデータを取得(キーワードが空白の場合全て取得)
            var filtered = _all_namelist.Where(name => string.IsNullOrEmpty(keyword) || name.Contains(keyword));

            foreach (var name in filtered)
            {
                //取得した一致データをチェックリストに追加
                _checklist.Items.Add(name);

                //チェック済みリストに取得したデータがある場合
                if (true == _checkedNames.Contains(name))
                {
                    //チェック済みとして、上で追加したチェックリスト項目をチェックON
                    _checklist.SetItemChecked(_checklist.Items.Count - 1, true);
                }
            }

            //処理終了のため更新中フラグOFF
            _isUpdating = false;
        }

        /// <summary>
        /// チェックリスト全選択、全解除
        /// </summary>
        /// <param name="isChecked">チェック状態</param>
        public void SetAllChecked(bool isChecked)
        {
            //二重処理を防ぐため更新中フラグON
            _isUpdating = true;

            //チェックリストに登録されている項目分ループ
            for (int i = 0; i < _checklist.Items.Count; i++)
            {
                //チェックリスト項目のチェックを更新
                _checklist.SetItemChecked(i, isChecked);

                //チェック位置の名前を取得
                string item_name = _checklist.Items[i]?.ToString() ?? "";
                //名前が空白ならスルー
                if (string.IsNullOrEmpty(item_name)) return;

                if (true == isChecked)
                {
                    //チェックONならチェック済みリストに追加
                    _checkedNames.Add(item_name);
                }
                else
                {
                    //チェックOFFならチェック済みリストから削除
                    _checkedNames.Remove(item_name);
                }
            }
            _isUpdating = false;
        }
    }
}
