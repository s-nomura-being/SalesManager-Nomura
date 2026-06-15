namespace SalesManagerApp.View
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            this.dgv_LastWeekSales = new DataGridView();
            this.lastWeekProductSalesDataBindingSource = new BindingSource(this.components);
            this.dgv_InventoryStatus = new DataGridView();
            this.label1 = new Label();
            this.label2 = new Label();
            this.btn_TotalSales = new Button();
            this.ms_Main = new MenuStrip();
            this.tsm_File = new ToolStripMenuItem();
            this.mn_ReadProductMaster = new ToolStripMenuItem();
            this.mn_ReadInventory = new ToolStripMenuItem();
            this.mn_ReadSales = new ToolStripMenuItem();
            this.tsm_View = new ToolStripMenuItem();
            this.mn_ShowTotalSales = new ToolStripMenuItem();
            this.mn_ShowInventory = new ToolStripMenuItem();
            this.mn_ShowMasterSetting = new ToolStripMenuItem();
            this.mn_ShowAppSetting = new ToolStripMenuItem();
            this.btn_InventoryInfo = new Button();
            this.btn_MasterSetting = new Button();
            this.btn_AppSetting = new Button();
            this.btn_ReadCSV = new Button();
            ((System.ComponentModel.ISupportInitialize)this.dgv_LastWeekSales).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.lastWeekProductSalesDataBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.dgv_InventoryStatus).BeginInit();
            this.ms_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_LastWeekSales
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.dgv_LastWeekSales.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_LastWeekSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dgv_LastWeekSales.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_LastWeekSales.Location = new Point(12, 59);
            this.dgv_LastWeekSales.Name = "dgv_LastWeekSales";
            this.dgv_LastWeekSales.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.dgv_LastWeekSales.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_LastWeekSales.RowHeadersWidth = 100;
            this.dgv_LastWeekSales.Size = new Size(1042, 304);
            this.dgv_LastWeekSales.TabIndex = 0;
            this.dgv_LastWeekSales.ColumnHeaderMouseClick += this.dgv_LastWeekSales_ColumnHeaderMouseClick;
            this.dgv_LastWeekSales.RowPostPaint += this.dgv_LastWeekSales_RowPostPaint;
            // 
            // lastWeekProductSalesDataBindingSource
            // 
            this.lastWeekProductSalesDataBindingSource.DataSource = typeof(Data.LastWeekProductSalesData);
            // 
            // dgv_InventoryStatus
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            this.dgv_InventoryStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_InventoryStatus.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            this.dgv_InventoryStatus.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_InventoryStatus.Location = new Point(12, 409);
            this.dgv_InventoryStatus.Name = "dgv_InventoryStatus";
            this.dgv_InventoryStatus.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Control;
            dataGridViewCellStyle6.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            this.dgv_InventoryStatus.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_InventoryStatus.RowHeadersWidth = 100;
            this.dgv_InventoryStatus.Size = new Size(1042, 260);
            this.dgv_InventoryStatus.TabIndex = 1;
            this.dgv_InventoryStatus.CellFormatting += this.dgv_InventoryStatus_CellFormatting;
            this.dgv_InventoryStatus.ColumnHeaderMouseClick += this.dgv_InventoryStatus_ColumnHeaderMouseClick;
            this.dgv_InventoryStatus.RowPostPaint += this.dgv_InventoryStatus_RowPostPaint;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label1.Location = new Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new Size(156, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "先週の商品売上";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label2.Location = new Point(12, 376);
            this.label2.Name = "label2";
            this.label2.Size = new Size(97, 30);
            this.label2.TabIndex = 3;
            this.label2.Text = "在庫状況";
            // 
            // btn_TotalSales
            // 
            this.btn_TotalSales.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_TotalSales.Location = new Point(1060, 137);
            this.btn_TotalSales.Name = "btn_TotalSales";
            this.btn_TotalSales.Size = new Size(192, 72);
            this.btn_TotalSales.TabIndex = 4;
            this.btn_TotalSales.Text = "売上集計";
            this.btn_TotalSales.UseVisualStyleBackColor = true;
            this.btn_TotalSales.Click += this.btn_TotalSales_Click;
            // 
            // ms_Main
            // 
            this.ms_Main.Items.AddRange(new ToolStripItem[] { this.tsm_File, this.tsm_View });
            this.ms_Main.Location = new Point(0, 0);
            this.ms_Main.Name = "ms_Main";
            this.ms_Main.Size = new Size(1264, 24);
            this.ms_Main.TabIndex = 5;
            this.ms_Main.Text = "menuStrip1";
            // 
            // tsm_File
            // 
            this.tsm_File.DropDownItems.AddRange(new ToolStripItem[] { this.mn_ReadProductMaster, this.mn_ReadInventory, this.mn_ReadSales });
            this.tsm_File.Name = "tsm_File";
            this.tsm_File.ShortcutKeyDisplayString = "";
            this.tsm_File.Size = new Size(67, 20);
            this.tsm_File.Text = "ファイル(&F)";
            // 
            // mn_ReadProductMaster
            // 
            this.mn_ReadProductMaster.Name = "mn_ReadProductMaster";
            this.mn_ReadProductMaster.Size = new Size(198, 22);
            this.mn_ReadProductMaster.Text = "商品マスタファイル読込(&P)";
            this.mn_ReadProductMaster.Click += this.mn_ReadProductMaster_Click;
            // 
            // mn_ReadInventory
            // 
            this.mn_ReadInventory.Name = "mn_ReadInventory";
            this.mn_ReadInventory.Size = new Size(198, 22);
            this.mn_ReadInventory.Text = "在庫データファイル読込(&I)";
            this.mn_ReadInventory.Click += this.mn_ReadInventory_Click;
            // 
            // mn_ReadSales
            // 
            this.mn_ReadSales.Name = "mn_ReadSales";
            this.mn_ReadSales.Size = new Size(198, 22);
            this.mn_ReadSales.Text = "売上データファイル読込(&S)";
            this.mn_ReadSales.Click += this.mn_ReadSales_Click;
            // 
            // tsm_View
            // 
            this.tsm_View.DropDownItems.AddRange(new ToolStripItem[] { this.mn_ShowTotalSales, this.mn_ShowInventory, this.mn_ShowMasterSetting, this.mn_ShowAppSetting });
            this.tsm_View.Name = "tsm_View";
            this.tsm_View.Size = new Size(58, 20);
            this.tsm_View.Text = "表示(&V)";
            // 
            // mn_ShowTotalSales
            // 
            this.mn_ShowTotalSales.Name = "mn_ShowTotalSales";
            this.mn_ShowTotalSales.Size = new Size(144, 22);
            this.mn_ShowTotalSales.Text = "売上集計(&S)";
            this.mn_ShowTotalSales.Click += this.mn_ShowTotalSales_Click;
            // 
            // mn_ShowInventory
            // 
            this.mn_ShowInventory.Name = "mn_ShowInventory";
            this.mn_ShowInventory.Size = new Size(144, 22);
            this.mn_ShowInventory.Text = "在庫一覧(&I)";
            this.mn_ShowInventory.Click += this.mn_ShowInventory_Click;
            // 
            // mn_ShowMasterSetting
            // 
            this.mn_ShowMasterSetting.Name = "mn_ShowMasterSetting";
            this.mn_ShowMasterSetting.Size = new Size(144, 22);
            this.mn_ShowMasterSetting.Text = "マスタ設定(&M)";
            this.mn_ShowMasterSetting.Click += this.mn_ShowMasterSetting_Click;
            // 
            // mn_ShowAppSetting
            // 
            this.mn_ShowAppSetting.Name = "mn_ShowAppSetting";
            this.mn_ShowAppSetting.Size = new Size(144, 22);
            this.mn_ShowAppSetting.Text = "アプリ設定(&A)";
            this.mn_ShowAppSetting.Click += this.mn_ShowAppSetting_Click;
            // 
            // btn_InventoryInfo
            // 
            this.btn_InventoryInfo.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_InventoryInfo.Location = new Point(1060, 215);
            this.btn_InventoryInfo.Name = "btn_InventoryInfo";
            this.btn_InventoryInfo.Size = new Size(192, 72);
            this.btn_InventoryInfo.TabIndex = 6;
            this.btn_InventoryInfo.Text = "在庫一覧";
            this.btn_InventoryInfo.UseVisualStyleBackColor = true;
            this.btn_InventoryInfo.Click += this.btn_InventoryInfo_Click;
            // 
            // btn_MasterSetting
            // 
            this.btn_MasterSetting.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_MasterSetting.Location = new Point(1100, 565);
            this.btn_MasterSetting.Name = "btn_MasterSetting";
            this.btn_MasterSetting.Size = new Size(152, 49);
            this.btn_MasterSetting.TabIndex = 7;
            this.btn_MasterSetting.Text = "マスタ設定";
            this.btn_MasterSetting.UseVisualStyleBackColor = true;
            this.btn_MasterSetting.Click += this.btn_MasterSetting_Click;
            // 
            // btn_AppSetting
            // 
            this.btn_AppSetting.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_AppSetting.Location = new Point(1100, 620);
            this.btn_AppSetting.Name = "btn_AppSetting";
            this.btn_AppSetting.Size = new Size(152, 49);
            this.btn_AppSetting.TabIndex = 8;
            this.btn_AppSetting.Text = "アプリ設定";
            this.btn_AppSetting.UseVisualStyleBackColor = true;
            this.btn_AppSetting.Click += this.btn_AppSetting_Click;
            // 
            // btn_ReadCSV
            // 
            this.btn_ReadCSV.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_ReadCSV.Location = new Point(1060, 59);
            this.btn_ReadCSV.Name = "btn_ReadCSV";
            this.btn_ReadCSV.Size = new Size(192, 72);
            this.btn_ReadCSV.TabIndex = 9;
            this.btn_ReadCSV.Text = "CSV読込";
            this.btn_ReadCSV.UseVisualStyleBackColor = true;
            this.btn_ReadCSV.Click += this.btn_ReadCSV_Click;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new Size(1264, 681);
            this.Controls.Add(this.btn_ReadCSV);
            this.Controls.Add(this.btn_AppSetting);
            this.Controls.Add(this.btn_MasterSetting);
            this.Controls.Add(this.btn_InventoryInfo);
            this.Controls.Add(this.btn_TotalSales);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_InventoryStatus);
            this.Controls.Add(this.dgv_LastWeekSales);
            this.Controls.Add(this.ms_Main);
            this.MainMenuStrip = this.ms_Main;
            this.MaximumSize = new Size(1280, 720);
            this.MinimumSize = new Size(300, 300);
            this.Name = "MainForm";
            this.Text = "メイン";
            this.Load += this.MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)this.dgv_LastWeekSales).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.lastWeekProductSalesDataBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.dgv_InventoryStatus).EndInit();
            this.ms_Main.ResumeLayout(false);
            this.ms_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private DataGridView dgv_LastWeekSales;
        private DataGridView dgv_InventoryStatus;
        private Label label1;
        private Label label2;
        private Button btn_TotalSales;
        private MenuStrip ms_Main;
        private Button btn_InventoryInfo;
        private Button btn_MasterSetting;
        private Button btn_AppSetting;
        private ToolStripMenuItem tsm_File;
        private ToolStripMenuItem mn_ReadProductMaster;
        private ToolStripMenuItem mn_ReadInventory;
        private ToolStripMenuItem mn_ReadSales;
        private ToolStripMenuItem tsm_View;
        private ToolStripMenuItem mn_ShowTotalSales;
        private ToolStripMenuItem mn_ShowInventory;
        private ToolStripMenuItem mn_ShowMasterSetting;
        private ToolStripMenuItem mn_ShowAppSetting;
        private BindingSource lastWeekProductSalesDataBindingSource;
        private Button btn_ReadCSV;
    }
}
