namespace SalesManagerApp.View
{
    partial class InventoryInfoView
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
            this.ms_InventoryInfo = new MenuStrip();
            this.tsm_File = new ToolStripMenuItem();
            this.mn_Output = new ToolStripMenuItem();
            this.lbl_Filter = new Label();
            this.pnl_Filter = new Panel();
            this.dgv_InventoryList = new DataGridView();
            this.label1 = new Label();
            this.btn_Output = new Button();
            this.ms_InventoryInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgv_InventoryList).BeginInit();
            this.SuspendLayout();
            // 
            // ms_InventoryInfo
            // 
            this.ms_InventoryInfo.Items.AddRange(new ToolStripItem[] { this.tsm_File });
            this.ms_InventoryInfo.Location = new Point(0, 0);
            this.ms_InventoryInfo.Name = "ms_InventoryInfo";
            this.ms_InventoryInfo.Size = new Size(1664, 24);
            this.ms_InventoryInfo.TabIndex = 0;
            this.ms_InventoryInfo.Text = "menuStrip1";
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
            this.mn_Output.Text = "在庫一覧出力(&O)";
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
            this.pnl_Filter.Location = new Point(12, 57);
            this.pnl_Filter.Name = "pnl_Filter";
            this.pnl_Filter.Size = new Size(458, 612);
            this.pnl_Filter.TabIndex = 9;
            // 
            // dgv_InventoryList
            // 
            this.dgv_InventoryList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.dgv_InventoryList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_InventoryList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dgv_InventoryList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_InventoryList.Location = new Point(476, 57);
            this.dgv_InventoryList.Name = "dgv_InventoryList";
            this.dgv_InventoryList.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            this.dgv_InventoryList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_InventoryList.RowHeadersWidth = 150;
            this.dgv_InventoryList.Size = new Size(1176, 662);
            this.dgv_InventoryList.TabIndex = 10;
            this.dgv_InventoryList.ColumnHeaderMouseClick += this.dgv_InventoryList_ColumnHeaderMouseClick;
            this.dgv_InventoryList.RowPostPaint += this.dgv_InventoryList_RowPostPaint;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label1.Location = new Point(476, 24);
            this.label1.Name = "label1";
            this.label1.Size = new Size(97, 30);
            this.label1.TabIndex = 11;
            this.label1.Text = "在庫一覧";
            // 
            // btn_Output
            // 
            this.btn_Output.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btn_Output.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Output.Location = new Point(268, 674);
            this.btn_Output.Name = "btn_Output";
            this.btn_Output.Size = new Size(202, 45);
            this.btn_Output.TabIndex = 12;
            this.btn_Output.Text = "一覧表出力";
            this.btn_Output.UseVisualStyleBackColor = true;
            this.btn_Output.Click += this.btn_Output_Click;
            // 
            // InventoryInfoView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1664, 731);
            this.Controls.Add(this.btn_Output);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_InventoryList);
            this.Controls.Add(this.pnl_Filter);
            this.Controls.Add(this.lbl_Filter);
            this.Controls.Add(this.ms_InventoryInfo);
            this.Name = "InventoryInfoView";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "在庫情報確認";
            this.Load += this.InventoryInfoView_Load;
            this.ms_InventoryInfo.ResumeLayout(false);
            this.ms_InventoryInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgv_InventoryList).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private MenuStrip ms_InventoryInfo;
        private Label lbl_Filter;
        private Panel pnl_Filter;
        private DataGridView dgv_InventoryList;
        private Label label1;
        private Button btn_Output;
        private ToolStripMenuItem tsm_File;
        private ToolStripMenuItem mn_Output;
    }
}