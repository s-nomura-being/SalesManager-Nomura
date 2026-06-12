namespace SalesManagerApp.View
{
    partial class CategoryMasterView
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
            this.btn_Save = new Button();
            this.btn_Cancel = new Button();
            this.label1 = new Label();
            this.dgv_CategoryMaster = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)this.dgv_CategoryMaster).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Save
            // 
            this.btn_Save.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btn_Save.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Save.Location = new Point(12, 597);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new Size(235, 72);
            this.btn_Save.TabIndex = 7;
            this.btn_Save.Text = "保存";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += this.btn_Save_Click;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btn_Cancel.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Cancel.Location = new Point(387, 597);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new Size(235, 72);
            this.btn_Cancel.TabIndex = 8;
            this.btn_Cancel.Text = "キャンセル";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += this.btn_Cancel_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label1.Location = new Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(103, 30);
            this.label1.TabIndex = 12;
            this.label1.Text = "区分マスタ";
            // 
            // dgv_CategoryMaster
            // 
            this.dgv_CategoryMaster.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            this.dgv_CategoryMaster.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_CategoryMaster.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dgv_CategoryMaster.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_CategoryMaster.Location = new Point(12, 42);
            this.dgv_CategoryMaster.Name = "dgv_CategoryMaster";
            this.dgv_CategoryMaster.Size = new Size(610, 549);
            this.dgv_CategoryMaster.TabIndex = 13;
            this.dgv_CategoryMaster.CellValidating += this.dgv_CategoryMaster_CellValidating;
            this.dgv_CategoryMaster.DataError += this.dgv_CategoryMaster_DataError;
            this.dgv_CategoryMaster.DefaultValuesNeeded += this.dgv_CategoryMaster_DefaultValuesNeeded;
            // 
            // CategoryMasterView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(634, 681);
            this.Controls.Add(this.dgv_CategoryMaster);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Save);
            this.Name = "CategoryMasterView";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "区分マスタ管理";
            this.Load += this.CategoryMasterView_Load;
            ((System.ComponentModel.ISupportInitialize)this.dgv_CategoryMaster).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button btn_Save;
        private Button btn_Cancel;
        private Label label1;
        private DataGridView dgv_CategoryMaster;
    }
}