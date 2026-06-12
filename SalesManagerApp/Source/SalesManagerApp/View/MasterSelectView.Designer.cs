namespace SalesManagerApp.View
{
    partial class MasterSelectView
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
            this.btn_EditStore = new Button();
            this.btn_EditProduct = new Button();
            this.btn_EditCategory = new Button();
            this.SuspendLayout();
            // 
            // btn_EditStore
            // 
            this.btn_EditStore.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_EditStore.Location = new Point(12, 12);
            this.btn_EditStore.Name = "btn_EditStore";
            this.btn_EditStore.Size = new Size(360, 72);
            this.btn_EditStore.TabIndex = 7;
            this.btn_EditStore.Text = "店舗マスタ編集";
            this.btn_EditStore.UseVisualStyleBackColor = true;
            this.btn_EditStore.Click += this.btn_EditStore_Click;
            // 
            // btn_EditProduct
            // 
            this.btn_EditProduct.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_EditProduct.Location = new Point(12, 90);
            this.btn_EditProduct.Name = "btn_EditProduct";
            this.btn_EditProduct.Size = new Size(360, 72);
            this.btn_EditProduct.TabIndex = 8;
            this.btn_EditProduct.Text = "商品マスタ編集";
            this.btn_EditProduct.UseVisualStyleBackColor = true;
            this.btn_EditProduct.Click += this.btn_EditProduct_Click;
            // 
            // btn_EditCategory
            // 
            this.btn_EditCategory.Font = new Font("Yu Gothic UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.btn_EditCategory.Location = new Point(12, 168);
            this.btn_EditCategory.Name = "btn_EditCategory";
            this.btn_EditCategory.Size = new Size(360, 72);
            this.btn_EditCategory.TabIndex = 9;
            this.btn_EditCategory.Text = "区分マスタ編集";
            this.btn_EditCategory.UseVisualStyleBackColor = true;
            this.btn_EditCategory.Click += this.btn_EditCategory_Click;
            // 
            // MasterSelectView
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(384, 252);
            this.Controls.Add(this.btn_EditCategory);
            this.Controls.Add(this.btn_EditProduct);
            this.Controls.Add(this.btn_EditStore);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MasterSelectView";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "マスタ選択";
            this.ResumeLayout(false);
        }

        #endregion

        private Button btn_EditStore;
        private Button btn_EditProduct;
        private Button btn_EditCategory;
    }
}