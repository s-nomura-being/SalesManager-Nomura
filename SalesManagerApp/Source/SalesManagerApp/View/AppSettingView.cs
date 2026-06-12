using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagerApp.View
{
    /// <summary>
    /// アプリ設定画面フォームクラス
    /// </summary>
    public partial class AppSettingView : Form
    {
        /// <summary>
        /// アプリ設定画面
        /// </summary>
        public AppSettingView(Form owner = null)
        {
            InitializeComponent();

            //親画面設定
            if (null != owner) Owner = owner;
        }

        /// <summary>
        /// アプリ設定画面読み込み時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppSettingView_Load(object sender, EventArgs e)
        {
            //設定値取得
            num_Must.Value = Properties.Settings.Default.NoticeCount_Must;
            num_Low.Value = Properties.Settings.Default.NoticeCount_Low;
            cb_AutoRead.Checked = Properties.Settings.Default.AutoRead;
            txt_BackupDir.Text = Properties.Settings.Default.BackupPath;
        }

        /// <summary>
        /// 参照ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Browse_Click(object sender, EventArgs e)
        {
            //フォルダ選択ダイアログを作成
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                //ダイアログタイトル設定
                folderDialog.Description = "ファイルの保存先フォルダを選択してください。";

                //最初に開くフォルダを指定
                folderDialog.SelectedPath = @"C:\SalesManagerApp\Backup";

                //ダイアログを表示、OKなら選択パスをテキストボックスに反映
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    //選択したフォルダのフルパスを取得
                    string selectedFolderPath = folderDialog.SelectedPath;

                    //テキストボックスに取得したパスを表示する
                    txt_BackupDir.Text = selectedFolderPath;
                }
            }
        }

        /// <summary>
        /// 保存ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            //設定値保存
            Properties.Settings.Default.NoticeCount_Must = (int)num_Must.Value;
            Properties.Settings.Default.NoticeCount_Low = (int)num_Low.Value;
            Properties.Settings.Default.AutoRead = cb_AutoRead.Checked;
            Properties.Settings.Default.BackupPath = txt_BackupDir.Text;
            Properties.Settings.Default.Save();

            this.Close();
        }

        /// <summary>
        /// キャンセルボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
