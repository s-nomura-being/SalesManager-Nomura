using SalesManagerApp.View;

namespace SalesManagerApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            //ログイン画面を表示し、成功した場合のみメイン画面を起動する
            using (var login = new LoginView())
            {
                if (login.ShowDialog() != DialogResult.OK)
                {
                    //ログインせずに閉じた/キャンセルした場合はアプリを終了
                    return;
                }
            }

            Application.Run(new MainForm());
        }
    }
}