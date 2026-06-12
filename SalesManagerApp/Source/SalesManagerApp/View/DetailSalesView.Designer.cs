namespace SalesManagerApp.View
{
    partial class DetailSalesView
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
            this.mn_DetailSales = new MenuStrip();
            this.tsm_File = new ToolStripMenuItem();
            this.mn_Output = new ToolStripMenuItem();
            this.lbl_Filter = new Label();
            this.pnl_Filter = new Panel();
            this.dgv_DetailList = new DataGridView();
            this.label1 = new Label();
            this.btn_Output = new Button();
            this.rdo_Period = new RadioButton();
            this.rdo_All = new RadioButton();
            this.label2 = new Label();
            this.dtp_End = new DateTimePicker();
            this.dtp_Start = new DateTimePicker();
            this.mn_DetailSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgv_DetailList).BeginInit();
            this.SuspendLayout();
            // 
            // mn_DetailSales
            // 
            this.mn_DetailSales.Items.AddRange(new ToolStripItem[] { this.tsm_File });
            this.mn_DetailSales.Location = new Point(0, 0);
            this.mn_DetailSales.Name = "mn_DetailSales";
            this.mn_DetailSales.Size = new Size(1664, 24);
            this.mn_DetailSales.TabIndex = 0;
            this.mn_DetailSales.Text = "menuStrip1";
            // 
            // tsm_File
            // 
            this.tsm_File.DropDownItems.AddRange(new ToolStripItem[] { this.mn_Output });
            this.tsm_File.Name = "tsm_File";
            this.tsm_File.Size = new Size(67, 20);
            this.tsm_File.Text = "ファイル(&F)";
            // 
            // mn_Output
            // 
            this.mn_Output.Name = "mn_Output";
            this.mn_Output.Size = new Size(163, 22);
            this.mn_Output.Text = "実績一覧出力(&O)";
            this.mn_Output.Click += this.mn_Output_Click;
            // 
            // lbl_Filter
            // 
            this.lbl_Filter.AutoSize = true;
            this.lbl_Filter.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.lbl_Filter.Location = new Point(12, 24);
            this.lbl_Filter.Name = "lbl_Filter";
            this.lbl_Filter.Size = new Size(130, 30);
            this.lbl_Filter.TabIndex = 8;
            this.lbl_Filter.Text = "絞り込み条件";
            // 
            // pnl_Filter
            // 
            this.pnl_Filter.Location = new Point(12, 158);
            this.pnl_Filter.Name = "pnl_Filter";
            this.pnl_Filter.Size = new Size(458, 612);
            this.pnl_Filter.TabIndex = 9;
            // 
            // dgv_DetailList
            // 
            this.dgv_DetailList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.dgv_DetailList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_DetailList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dgv_DetailList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_DetailList.Location = new Point(476, 57);
            this.dgv_DetailList.Name = "dgv_DetailList";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.dgv_DetailList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_DetailList.RowHeadersWidth = 150;
            this.dgv_DetailList.Size = new Size(1176, 765);
            this.dgv_DetailList.TabIndex = 10;
            this.dgv_DetailList.ColumnHeaderMouseClick += this.dgv_DetailList_ColumnHeaderMouseClick;
            this.dgv_DetailList.RowPostPaint += this.dgv_DetailList_RowPostPaint;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label1.Location = new Point(476, 24);
            this.label1.Name = "label1";
            this.label1.Size = new Size(97, 30);
            this.label1.TabIndex = 11;
            this.label1.Text = "実績一覧";
            // 
            // btn_Output
            // 
            this.btn_Output.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Output.Location = new Point(268, 777);
            this.btn_Output.Name = "btn_Output";
            this.btn_Output.Size = new Size(202, 45);
            this.btn_Output.TabIndex = 12;
            this.btn_Output.Text = "一覧表出力";
            this.btn_Output.UseVisualStyleBackColor = true;
            this.btn_Output.Click += this.btn_Output_Click;
            // 
            // rdo_Period
            // 
            this.rdo_Period.AutoSize = true;
            this.rdo_Period.Checked = true;
            this.rdo_Period.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.rdo_Period.Location = new Point(39, 68);
            this.rdo_Period.Name = "rdo_Period";
            this.rdo_Period.Size = new Size(52, 34);
            this.rdo_Period.TabIndex = 13;
            this.rdo_Period.TabStop = true;
            this.rdo_Period.Text = "　";
            this.rdo_Period.UseVisualStyleBackColor = true;
            this.rdo_Period.CheckedChanged += this.rdo_Period_CheckedChanged;
            // 
            // rdo_All
            // 
            this.rdo_All.AutoSize = true;
            this.rdo_All.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.rdo_All.Location = new Point(39, 118);
            this.rdo_All.Name = "rdo_All";
            this.rdo_All.Size = new Size(94, 34);
            this.rdo_All.TabIndex = 14;
            this.rdo_All.Text = "全期間";
            this.rdo_All.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label2.Location = new Point(231, 68);
            this.label2.Name = "label2";
            this.label2.Size = new Size(34, 30);
            this.label2.TabIndex = 20;
            this.label2.Text = "～";
            // 
            // dtp_End
            // 
            this.dtp_End.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.dtp_End.Format = DateTimePickerFormat.Short;
            this.dtp_End.Location = new Point(265, 66);
            this.dtp_End.Name = "dtp_End";
            this.dtp_End.Size = new Size(164, 35);
            this.dtp_End.TabIndex = 19;
            // 
            // dtp_Start
            // 
            this.dtp_Start.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.dtp_Start.Format = DateTimePickerFormat.Short;
            this.dtp_Start.Location = new Point(61, 66);
            this.dtp_Start.Name = "dtp_Start";
            this.dtp_Start.Size = new Size(164, 35);
            this.dtp_Start.TabIndex = 18;
            // 
            // DetailSalesView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1664, 834);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtp_End);
            this.Controls.Add(this.dtp_Start);
            this.Controls.Add(this.rdo_All);
            this.Controls.Add(this.rdo_Period);
            this.Controls.Add(this.btn_Output);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_DetailList);
            this.Controls.Add(this.pnl_Filter);
            this.Controls.Add(this.lbl_Filter);
            this.Controls.Add(this.mn_DetailSales);
            this.Name = "DetailSalesView";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "売上・販売実績一覧";
            this.Load += this.DetailSalesView_Load;
            this.mn_DetailSales.ResumeLayout(false);
            this.mn_DetailSales.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgv_DetailList).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private MenuStrip mn_DetailSales;
        private Label lbl_Filter;
        private Panel pnl_Filter;
        private DataGridView dgv_DetailList;
        private Label label1;
        private Button btn_Output;
        private ToolStripMenuItem tsm_File;
        private ToolStripMenuItem mn_Output;
        private RadioButton rdo_Period;
        private RadioButton rdo_All;
        private Label label2;
        private DateTimePicker dtp_End;
        private DateTimePicker dtp_Start;
    }
}