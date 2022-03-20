namespace EazyAlgoBridge
{
    partial class Form3
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
            this.positionGrid = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.instrument = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avgprice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ltp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.positionGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // positionGrid
            // 
            this.positionGrid.AllowUserToAddRows = false;
            this.positionGrid.AllowUserToDeleteRows = false;
            this.positionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.positionGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product,
            this.instrument,
            this.quantity,
            this.avgprice,
            this.ltp,
            this.pnl});
            this.positionGrid.Location = new System.Drawing.Point(12, 30);
            this.positionGrid.Name = "positionGrid";
            this.positionGrid.ReadOnly = true;
            this.positionGrid.RowHeadersVisible = false;
            this.positionGrid.Size = new System.Drawing.Size(650, 194);
            this.positionGrid.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(288, 245);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // product
            // 
            this.product.HeaderText = "Product";
            this.product.Name = "product";
            this.product.ReadOnly = true;
            // 
            // instrument
            // 
            this.instrument.HeaderText = "Instrument";
            this.instrument.Name = "instrument";
            this.instrument.ReadOnly = true;
            this.instrument.Width = 150;
            // 
            // quantity
            // 
            this.quantity.HeaderText = "Quantity";
            this.quantity.Name = "quantity";
            this.quantity.ReadOnly = true;
            // 
            // avgprice
            // 
            this.avgprice.HeaderText = "Avg. Price";
            this.avgprice.Name = "avgprice";
            this.avgprice.ReadOnly = true;
            // 
            // ltp
            // 
            this.ltp.HeaderText = "LTP";
            this.ltp.Name = "ltp";
            this.ltp.ReadOnly = true;
            // 
            // pnl
            // 
            this.pnl.HeaderText = "P&L";
            this.pnl.Name = "pnl";
            this.pnl.ReadOnly = true;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 280);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.positionGrid);
            this.Name = "Form3";
            this.Text = "Positions";
            ((System.ComponentModel.ISupportInitialize)(this.positionGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView positionGrid;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn product;
        private System.Windows.Forms.DataGridViewTextBoxColumn instrument;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn avgprice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ltp;
        private System.Windows.Forms.DataGridViewTextBoxColumn pnl;
    }
}