namespace SalesManagerApp.View
{
    partial class LoginView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_UserId = new Label();
            this.lbl_Password = new Label();
            this.txt_UserId = new TextBox();
            this.txt_Password = new TextBox();
            this.btn_Login = new Button();
            this.btn_Cancel = new Button();
            this.lbl_Message = new Label();
            this.SuspendLayout();
            //
            // lbl_UserId
            //
            this.lbl_UserId.AutoSize = true;
            this.lbl_UserId.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.lbl_UserId.Location = new Point(24, 30);
            this.lbl_UserId.Name = "lbl_UserId";
            this.lbl_UserId.Size = new Size(102, 25);
            this.lbl_UserId.TabIndex = 0;
            this.lbl_UserId.Text = "ユーザーID";
            //
            // lbl_Password
            //
            this.lbl_Password.AutoSize = true;
            this.lbl_Password.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.lbl_Password.Location = new Point(24, 82);
            this.lbl_Password.Name = "lbl_Password";
            this.lbl_Password.Size = new Size(96, 25);
            this.lbl_Password.TabIndex = 2;
            this.lbl_Password.Text = "パスワード";
            //
            // txt_UserId
            //
            this.txt_UserId.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.txt_UserId.Location = new Point(150, 27);
            this.txt_UserId.Name = "txt_UserId";
            this.txt_UserId.Size = new Size(220, 32);
            this.txt_UserId.TabIndex = 1;
            //
            // txt_Password
            //
            this.txt_Password.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.txt_Password.Location = new Point(150, 79);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.PasswordChar = '*';
            this.txt_Password.Size = new Size(220, 32);
            this.txt_Password.TabIndex = 3;
            //
            // btn_Login
            //
            this.btn_Login.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Login.Location = new Point(150, 150);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new Size(105, 45);
            this.btn_Login.TabIndex = 4;
            this.btn_Login.Text = "ログイン";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += this.btn_Login_Click;
            //
            // btn_Cancel
            //
            this.btn_Cancel.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Cancel.Location = new Point(265, 150);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new Size(105, 45);
            this.btn_Cancel.TabIndex = 5;
            this.btn_Cancel.Text = "キャンセル";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += this.btn_Cancel_Click;
            //
            // lbl_Message
            //
            this.lbl_Message.AutoSize = true;
            this.lbl_Message.ForeColor = Color.Red;
            this.lbl_Message.Font = new Font("Yu Gothic UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.lbl_Message.Location = new Point(24, 122);
            this.lbl_Message.Name = "lbl_Message";
            this.lbl_Message.Size = new Size(0, 19);
            this.lbl_Message.TabIndex = 6;
            this.lbl_Message.Text = "";
            //
            // LoginView
            //
            this.AcceptButton = this.btn_Login;
            this.CancelButton = this.btn_Cancel;
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 220);
            this.Controls.Add(this.lbl_Message);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.txt_UserId);
            this.Controls.Add(this.lbl_Password);
            this.Controls.Add(this.lbl_UserId);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginView";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "ログイン";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lbl_UserId;
        private Label lbl_Password;
        private TextBox txt_UserId;
        private TextBox txt_Password;
        private Button btn_Login;
        private Button btn_Cancel;
        private Label lbl_Message;
    }
}
