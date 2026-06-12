namespace SalesManagerApp.View
{
    partial class FilterControlView
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.txt_Store = new TextBox();
            this.txt_Product = new TextBox();
            this.txt_Category = new TextBox();
            this.chlst_Store = new CheckedListBox();
            this.chlst_Product = new CheckedListBox();
            this.chlst_Category = new CheckedListBox();
            this.lbl_AllSelectStore = new Label();
            this.lbl_AllDeselectStore = new Label();
            this.lbl_AllDeselectProduct = new Label();
            this.lbl_AllSelectProduct = new Label();
            this.lbl_AllDeselectCategory = new Label();
            this.lbl_AllSelectCategory = new Label();
            this.btn_Apply = new Button();
            this.btn_Clear = new Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label1.Location = new Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new Size(55, 30);
            this.label1.TabIndex = 12;
            this.label1.Text = "店舗";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label2.Location = new Point(3, 186);
            this.label2.Name = "label2";
            this.label2.Size = new Size(55, 30);
            this.label2.TabIndex = 13;
            this.label2.Text = "商品";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.label3.Location = new Point(3, 369);
            this.label3.Name = "label3";
            this.label3.Size = new Size(55, 30);
            this.label3.TabIndex = 14;
            this.label3.Text = "区分";
            // 
            // txt_Store
            // 
            this.txt_Store.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.txt_Store.Location = new Point(3, 36);
            this.txt_Store.Name = "txt_Store";
            this.txt_Store.Size = new Size(375, 35);
            this.txt_Store.TabIndex = 15;
            // 
            // txt_Product
            // 
            this.txt_Product.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.txt_Product.Location = new Point(3, 219);
            this.txt_Product.Name = "txt_Product";
            this.txt_Product.Size = new Size(375, 35);
            this.txt_Product.TabIndex = 16;
            // 
            // txt_Category
            // 
            this.txt_Category.Font = new Font("Yu Gothic UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.txt_Category.Location = new Point(3, 402);
            this.txt_Category.Name = "txt_Category";
            this.txt_Category.Size = new Size(375, 35);
            this.txt_Category.TabIndex = 17;
            // 
            // chlst_Store
            // 
            this.chlst_Store.CheckOnClick = true;
            this.chlst_Store.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.chlst_Store.FormattingEnabled = true;
            this.chlst_Store.Location = new Point(3, 77);
            this.chlst_Store.Name = "chlst_Store";
            this.chlst_Store.Size = new Size(375, 106);
            this.chlst_Store.TabIndex = 18;
            // 
            // chlst_Product
            // 
            this.chlst_Product.CheckOnClick = true;
            this.chlst_Product.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.chlst_Product.FormattingEnabled = true;
            this.chlst_Product.Location = new Point(3, 260);
            this.chlst_Product.Name = "chlst_Product";
            this.chlst_Product.Size = new Size(375, 106);
            this.chlst_Product.TabIndex = 19;
            // 
            // chlst_Category
            // 
            this.chlst_Category.CheckOnClick = true;
            this.chlst_Category.Font = new Font("Yu Gothic UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.chlst_Category.FormattingEnabled = true;
            this.chlst_Category.Location = new Point(3, 443);
            this.chlst_Category.Name = "chlst_Category";
            this.chlst_Category.Size = new Size(375, 106);
            this.chlst_Category.TabIndex = 20;
            // 
            // lbl_AllSelectStore
            // 
            this.lbl_AllSelectStore.AutoSize = true;
            this.lbl_AllSelectStore.Font = new Font("Yu Gothic UI", 12F);
            this.lbl_AllSelectStore.ForeColor = SystemColors.Highlight;
            this.lbl_AllSelectStore.Location = new Point(384, 80);
            this.lbl_AllSelectStore.Name = "lbl_AllSelectStore";
            this.lbl_AllSelectStore.Size = new Size(68, 21);
            this.lbl_AllSelectStore.TabIndex = 21;
            this.lbl_AllSelectStore.Text = "[全選択]";
            this.lbl_AllSelectStore.Click += this.AllSelect;
            // 
            // lbl_AllDeselectStore
            // 
            this.lbl_AllDeselectStore.AutoSize = true;
            this.lbl_AllDeselectStore.Font = new Font("Yu Gothic UI", 12F);
            this.lbl_AllDeselectStore.ForeColor = SystemColors.Highlight;
            this.lbl_AllDeselectStore.Location = new Point(384, 117);
            this.lbl_AllDeselectStore.Name = "lbl_AllDeselectStore";
            this.lbl_AllDeselectStore.Size = new Size(68, 21);
            this.lbl_AllDeselectStore.TabIndex = 22;
            this.lbl_AllDeselectStore.Text = "[全解除]";
            this.lbl_AllDeselectStore.Click += this.AllDeselect;
            // 
            // lbl_AllDeselectProduct
            // 
            this.lbl_AllDeselectProduct.AutoSize = true;
            this.lbl_AllDeselectProduct.Font = new Font("Yu Gothic UI", 12F);
            this.lbl_AllDeselectProduct.ForeColor = SystemColors.Highlight;
            this.lbl_AllDeselectProduct.Location = new Point(384, 300);
            this.lbl_AllDeselectProduct.Name = "lbl_AllDeselectProduct";
            this.lbl_AllDeselectProduct.Size = new Size(68, 21);
            this.lbl_AllDeselectProduct.TabIndex = 24;
            this.lbl_AllDeselectProduct.Text = "[全解除]";
            this.lbl_AllDeselectProduct.Click += this.AllDeselect;
            // 
            // lbl_AllSelectProduct
            // 
            this.lbl_AllSelectProduct.AutoSize = true;
            this.lbl_AllSelectProduct.Font = new Font("Yu Gothic UI", 12F);
            this.lbl_AllSelectProduct.ForeColor = SystemColors.Highlight;
            this.lbl_AllSelectProduct.Location = new Point(384, 263);
            this.lbl_AllSelectProduct.Name = "lbl_AllSelectProduct";
            this.lbl_AllSelectProduct.Size = new Size(68, 21);
            this.lbl_AllSelectProduct.TabIndex = 23;
            this.lbl_AllSelectProduct.Text = "[全選択]";
            this.lbl_AllSelectProduct.Click += this.AllSelect;
            // 
            // lbl_AllDeselectCategory
            // 
            this.lbl_AllDeselectCategory.AutoSize = true;
            this.lbl_AllDeselectCategory.Font = new Font("Yu Gothic UI", 12F);
            this.lbl_AllDeselectCategory.ForeColor = SystemColors.Highlight;
            this.lbl_AllDeselectCategory.Location = new Point(384, 481);
            this.lbl_AllDeselectCategory.Name = "lbl_AllDeselectCategory";
            this.lbl_AllDeselectCategory.Size = new Size(68, 21);
            this.lbl_AllDeselectCategory.TabIndex = 26;
            this.lbl_AllDeselectCategory.Text = "[全解除]";
            this.lbl_AllDeselectCategory.Click += this.AllDeselect;
            // 
            // lbl_AllSelectCategory
            // 
            this.lbl_AllSelectCategory.AutoSize = true;
            this.lbl_AllSelectCategory.Font = new Font("Yu Gothic UI", 12F);
            this.lbl_AllSelectCategory.ForeColor = SystemColors.Highlight;
            this.lbl_AllSelectCategory.Location = new Point(384, 446);
            this.lbl_AllSelectCategory.Name = "lbl_AllSelectCategory";
            this.lbl_AllSelectCategory.Size = new Size(68, 21);
            this.lbl_AllSelectCategory.TabIndex = 25;
            this.lbl_AllSelectCategory.Text = "[全選択]";
            this.lbl_AllSelectCategory.Click += this.AllSelect;
            // 
            // btn_Apply
            // 
            this.btn_Apply.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Apply.Location = new Point(3, 555);
            this.btn_Apply.Name = "btn_Apply";
            this.btn_Apply.Size = new Size(97, 45);
            this.btn_Apply.TabIndex = 27;
            this.btn_Apply.Text = "適用";
            this.btn_Apply.UseVisualStyleBackColor = true;
            this.btn_Apply.Click += this.btn_Apply_Click;
            // 
            // btn_Clear
            // 
            this.btn_Clear.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_Clear.Location = new Point(250, 555);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new Size(202, 45);
            this.btn_Clear.TabIndex = 28;
            this.btn_Clear.Text = "絞り込みクリア";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += this.btn_Clear_Click;
            // 
            // FilterControlView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_Apply);
            this.Controls.Add(this.lbl_AllDeselectCategory);
            this.Controls.Add(this.lbl_AllSelectCategory);
            this.Controls.Add(this.lbl_AllDeselectProduct);
            this.Controls.Add(this.lbl_AllSelectProduct);
            this.Controls.Add(this.lbl_AllDeselectStore);
            this.Controls.Add(this.lbl_AllSelectStore);
            this.Controls.Add(this.chlst_Category);
            this.Controls.Add(this.chlst_Product);
            this.Controls.Add(this.chlst_Store);
            this.Controls.Add(this.txt_Category);
            this.Controls.Add(this.txt_Product);
            this.Controls.Add(this.txt_Store);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FilterControlView";
            this.Size = new Size(458, 612);
            this.Load += this.FilterControl_Load;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txt_Store;
        private TextBox txt_Product;
        private TextBox txt_Category;
        private CheckedListBox chlst_Store;
        private CheckedListBox chlst_Product;
        private CheckedListBox chlst_Category;
        private Label lbl_AllSelectStore;
        private Label lbl_AllDeselectStore;
        private Label lbl_AllDeselectProduct;
        private Label lbl_AllSelectProduct;
        private Label lbl_AllDeselectCategory;
        private Label lbl_AllSelectCategory;
        private Button btn_Apply;
        private Button btn_Clear;
    }
}
