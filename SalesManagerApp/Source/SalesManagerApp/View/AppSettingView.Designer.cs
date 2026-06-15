namespace SalesManagerApp.View
{
    partial class AppSettingView
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
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.btn_Save = new Button();
            this.btn_Cancel = new Button();
            this.label6 = new Label();
            this.label7 = new Label();
            this.cb_AutoRead = new CheckBox();
            this.label8 = new Label();
            this.btn_Browse = new Button();
            this.txt_BackupDir = new TextBox();
            this.num_Must = new NumericUpDown();
            this.num_Low = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)this.num_Must).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.num_Low).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label1.Location = new Point(25, 20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(215, 30);
            this.label1.TabIndex = 13;
            this.label1.Text = "発注タイミング通知設定";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label2.Location = new Point(55, 63);
            this.label2.Name = "label2";
            this.label2.Size = new Size(147, 30);
            this.label2.TabIndex = 14;
            this.label2.Text = "在庫数-要発注";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label3.Location = new Point(97, 108);
            this.label3.Name = "label3";
            this.label3.Size = new Size(105, 30);
            this.label3.TabIndex = 15;
            this.label3.Text = "在庫数-少";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label4.Location = new Point(25, 158);
            this.label4.Name = "label4";
            this.label4.Size = new Size(218, 30);
            this.label4.TabIndex = 16;
            this.label4.Text = "自動データ読み取り設定";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label5.Location = new Point(25, 257);
            this.label5.Name = "label5";
            this.label5.Size = new Size(194, 30);
            this.label5.TabIndex = 17;
            this.label5.Text = "データバックアップ設定";
            // 
            // btn_Save
            // 
            this.btn_Save.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btn_Save.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Save.Location = new Point(12, 374);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new Size(149, 51);
            this.btn_Save.TabIndex = 18;
            this.btn_Save.Text = "保存";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += this.btn_Save_Click;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btn_Cancel.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Cancel.Location = new Point(360, 374);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new Size(149, 51);
            this.btn_Cancel.TabIndex = 19;
            this.btn_Cancel.Text = "キャンセル";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += this.btn_Cancel_Click;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label6.Location = new Point(277, 63);
            this.label6.Name = "label6";
            this.label6.Size = new Size(55, 30);
            this.label6.TabIndex = 22;
            this.label6.Text = "以下";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label7.Location = new Point(277, 108);
            this.label7.Name = "label7";
            this.label7.Size = new Size(55, 30);
            this.label7.TabIndex = 23;
            this.label7.Text = "以下";
            // 
            // cb_AutoRead
            // 
            this.cb_AutoRead.AutoSize = true;
            this.cb_AutoRead.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.cb_AutoRead.Location = new Point(97, 200);
            this.cb_AutoRead.Name = "cb_AutoRead";
            this.cb_AutoRead.Size = new Size(149, 34);
            this.cb_AutoRead.TabIndex = 24;
            this.cb_AutoRead.Text = "自動読み取り";
            this.cb_AutoRead.UseVisualStyleBackColor = true;
            this.cb_AutoRead.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label8.Location = new Point(75, 297);
            this.label8.Name = "label8";
            this.label8.Size = new Size(127, 30);
            this.label8.TabIndex = 25;
            this.label8.Text = "バックアップ先";
            // 
            // btn_Browse
            // 
            this.btn_Browse.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Browse.Location = new Point(442, 297);
            this.btn_Browse.Name = "btn_Browse";
            this.btn_Browse.Size = new Size(67, 35);
            this.btn_Browse.TabIndex = 26;
            this.btn_Browse.Text = "参照";
            this.btn_Browse.UseVisualStyleBackColor = true;
            this.btn_Browse.Click += this.btn_Browse_Click;
            // 
            // txt_BackupDir
            // 
            this.txt_BackupDir.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.txt_BackupDir.Location = new Point(208, 297);
            this.txt_BackupDir.Name = "txt_BackupDir";
            this.txt_BackupDir.Size = new Size(228, 35);
            this.txt_BackupDir.TabIndex = 28;
            // 
            // num_Must
            // 
            this.num_Must.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.num_Must.Location = new Point(208, 63);
            this.num_Must.Name = "num_Must";
            this.num_Must.Size = new Size(63, 35);
            this.num_Must.TabIndex = 29;
            // 
            // num_Low
            // 
            this.num_Low.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.num_Low.Location = new Point(208, 106);
            this.num_Low.Name = "num_Low";
            this.num_Low.Size = new Size(63, 35);
            this.num_Low.TabIndex = 30;
            // 
            // AppSettingView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(521, 437);
            this.Controls.Add(this.num_Low);
            this.Controls.Add(this.num_Must);
            this.Controls.Add(this.txt_BackupDir);
            this.Controls.Add(this.btn_Browse);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cb_AutoRead);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new Size(537, 476);
            this.MinimumSize = new Size(537, 476);
            this.Name = "AppSettingView";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "アプリ設定";
            this.Load += this.AppSettingView_Load;
            ((System.ComponentModel.ISupportInitialize)this.num_Must).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.num_Low).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button btn_Save;
        private Button btn_Cancel;
        private Label label6;
        private Label label7;
        private CheckBox cb_AutoRead;
        private Label label8;
        private Button btn_Browse;
        private TextBox txt_BackupDir;
        private NumericUpDown num_Must;
        private NumericUpDown num_Low;
    }
}