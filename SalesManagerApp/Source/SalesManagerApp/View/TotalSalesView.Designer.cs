namespace SalesManagerApp.View
{
    partial class TotalSalesView
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            this.cmb_Period = new ComboBox();
            this.btn_CalcTotal = new Button();
            this.btn_Output = new Button();
            this.dgv_TotalSales = new DataGridView();
            this.ms_TotalSales = new MenuStrip();
            this.tsm_File = new ToolStripMenuItem();
            this.mn_OutputTotalSales = new ToolStripMenuItem();
            this.btn_ShowDetail = new Button();
            this.label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)this.dgv_TotalSales).BeginInit();
            this.ms_TotalSales.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmb_Period
            // 
            this.cmb_Period.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.cmb_Period.FormattingEnabled = true;
            this.cmb_Period.Location = new Point(22, 68);
            this.cmb_Period.Name = "cmb_Period";
            this.cmb_Period.Size = new Size(160, 45);
            this.cmb_Period.TabIndex = 0;
            // 
            // btn_CalcTotal
            // 
            this.btn_CalcTotal.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_CalcTotal.Location = new Point(901, 68);
            this.btn_CalcTotal.Name = "btn_CalcTotal";
            this.btn_CalcTotal.Size = new Size(159, 45);
            this.btn_CalcTotal.TabIndex = 1;
            this.btn_CalcTotal.Text = "集計実行";
            this.btn_CalcTotal.UseVisualStyleBackColor = true;
            this.btn_CalcTotal.Click += this.btn_CalcTotal_Click;
            // 
            // btn_Output
            // 
            this.btn_Output.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Output.Location = new Point(1083, 67);
            this.btn_Output.Name = "btn_Output";
            this.btn_Output.Size = new Size(169, 45);
            this.btn_Output.TabIndex = 2;
            this.btn_Output.Text = "一覧表出力";
            this.btn_Output.UseVisualStyleBackColor = true;
            this.btn_Output.Click += this.btn_Output_Click;
            // 
            // dgv_TotalSales
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.dgv_TotalSales.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_TotalSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dgv_TotalSales.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_TotalSales.Location = new Point(22, 119);
            this.dgv_TotalSales.Name = "dgv_TotalSales";
            this.dgv_TotalSales.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.dgv_TotalSales.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_TotalSales.RowHeadersWidth = 150;
            this.dgv_TotalSales.Size = new Size(1230, 495);
            this.dgv_TotalSales.TabIndex = 3;
            this.dgv_TotalSales.ColumnHeaderMouseClick += this.dgv_TotalSales_ColumnHeaderMouseClick;
            this.dgv_TotalSales.RowPostPaint += this.dgv_TotalSales_RowPostPaint;
            // 
            // ms_TotalSales
            // 
            this.ms_TotalSales.Items.AddRange(new ToolStripItem[] { this.tsm_File });
            this.ms_TotalSales.Location = new Point(0, 0);
            this.ms_TotalSales.Name = "ms_TotalSales";
            this.ms_TotalSales.Size = new Size(1264, 24);
            this.ms_TotalSales.TabIndex = 5;
            this.ms_TotalSales.Text = "menuStrip1";
            // 
            // tsm_File
            // 
            this.tsm_File.DropDownItems.AddRange(new ToolStripItem[] { this.mn_OutputTotalSales });
            this.tsm_File.Name = "tsm_File";
            this.tsm_File.Size = new Size(67, 20);
            this.tsm_File.Text = "ファイル(&F)";
            // 
            // mn_OutputTotalSales
            // 
            this.mn_OutputTotalSales.Name = "mn_OutputTotalSales";
            this.mn_OutputTotalSales.Size = new Size(163, 22);
            this.mn_OutputTotalSales.Text = "集計一覧出力(&O)";
            this.mn_OutputTotalSales.Click += this.mn_OutputTotalSales_Click;
            // 
            // btn_ShowDetail
            // 
            this.btn_ShowDetail.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_ShowDetail.Location = new Point(1083, 624);
            this.btn_ShowDetail.Name = "btn_ShowDetail";
            this.btn_ShowDetail.Size = new Size(169, 45);
            this.btn_ShowDetail.TabIndex = 6;
            this.btn_ShowDetail.Text = "詳細表示";
            this.btn_ShowDetail.UseVisualStyleBackColor = true;
            this.btn_ShowDetail.Click += this.btn_ShowDetail_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label1.Location = new Point(22, 35);
            this.label1.Name = "label1";
            this.label1.Size = new Size(97, 30);
            this.label1.TabIndex = 7;
            this.label1.Text = "集計期間";
            // 
            // TotalSalesView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1264, 681);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_ShowDetail);
            this.Controls.Add(this.dgv_TotalSales);
            this.Controls.Add(this.btn_Output);
            this.Controls.Add(this.btn_CalcTotal);
            this.Controls.Add(this.cmb_Period);
            this.Controls.Add(this.ms_TotalSales);
            this.MainMenuStrip = this.ms_TotalSales;
            this.Name = "TotalSalesView";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "売上集計";
            this.Load += this.TotalSalesView_Load;
            ((System.ComponentModel.ISupportInitialize)this.dgv_TotalSales).EndInit();
            this.ms_TotalSales.ResumeLayout(false);
            this.ms_TotalSales.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ComboBox cmb_Period;
        private Button btn_CalcTotal;
        private Button btn_Output;
        private DataGridView dgv_TotalSales;
        private MenuStrip ms_TotalSales;
        private Button btn_ShowDetail;
        private Label label1;
        private ToolStripMenuItem tsm_File;
        private ToolStripMenuItem mn_OutputTotalSales;
        private Label label4;
    }
}