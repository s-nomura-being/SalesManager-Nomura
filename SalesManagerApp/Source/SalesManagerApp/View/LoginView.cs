using System;
using System.Windows.Forms;

namespace SalesManagerApp.View
{
    /// <summary>
    /// ログイン画面フォームクラス
    /// </summary>
    public partial class LoginView : Form
    {
        // TODO: 本来はユーザーマスタ(DB)や設定値で管理する。ここでは研修用に固定値で判定している。
        /// <summary>ログイン可能なユーザーID</summary>
        private const string LOGIN_USER_ID = "admin";

        /// <summary>ログインパスワード</summary>
        private const string LOGIN_PASSWORD = "password";

        /// <summary>
        /// ログイン画面
        /// </summary>
        public LoginView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ログインボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Login_Click(object sender, EventArgs e)
        {
            //入力値取得
            string user_id = txt_UserId.Text.Trim();
            string password = txt_Password.Text;

            //未入力チェック
            if (string.IsNullOrEmpty(user_id) || string.IsNullOrEmpty(password))
            {
                lbl_Message.Text = "ユーザーIDとパスワードを入力してください。";
                return;
            }

            //認証チェック
            if (user_id == LOGIN_USER_ID && password == LOGIN_PASSWORD)
            {
                //ログイン成功：呼び出し元に成功を返して閉じる
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                //ログイン失敗
                lbl_Message.Text = "ユーザーIDまたはパスワードが正しくありません。";
                txt_Password.Clear();
                txt_Password.Focus();
            }
        }

        /// <summary>
        /// キャンセルボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            //ログインせずに終了する
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
