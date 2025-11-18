namespace loan
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.LblLoan = new System.Windows.Forms.Label();
            this.LblRate = new System.Windows.Forms.Label();
            this.LblYear = new System.Windows.Forms.Label();
            this.LblPay = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(27, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "貸款本息定額每月攤還試算表";
            // 
            // LblLoan
            // 
            this.LblLoan.AutoSize = true;
            this.LblLoan.Font = new System.Drawing.Font("Microsoft JhengHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LblLoan.Location = new System.Drawing.Point(27, 59);
            this.LblLoan.Name = "LblLoan";
            this.LblLoan.Size = new System.Drawing.Size(100, 19);
            this.LblLoan.TabIndex = 1;
            this.LblLoan.Text = "1. 貸款金額：";
            // 
            // LblRate
            // 
            this.LblRate.AutoSize = true;
            this.LblRate.Font = new System.Drawing.Font("Microsoft JhengHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LblRate.Location = new System.Drawing.Point(27, 92);
            this.LblRate.Name = "LblRate";
            this.LblRate.Size = new System.Drawing.Size(85, 19);
            this.LblRate.TabIndex = 2;
            this.LblRate.Text = "2. 年利率：";
            // 
            // LblYear
            // 
            this.LblYear.AutoSize = true;
            this.LblYear.Font = new System.Drawing.Font("Microsoft JhengHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LblYear.Location = new System.Drawing.Point(27, 126);
            this.LblYear.Name = "LblYear";
            this.LblYear.Size = new System.Drawing.Size(70, 19);
            this.LblYear.TabIndex = 3;
            this.LblYear.Text = "3. 年數：";
            // 
            // LblPay
            // 
            this.LblPay.AutoSize = true;
            this.LblPay.Font = new System.Drawing.Font("Microsoft JhengHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LblPay.Location = new System.Drawing.Point(27, 158);
            this.LblPay.Name = "LblPay";
            this.LblPay.Size = new System.Drawing.Size(130, 19);
            this.LblPay.TabIndex = 4;
            this.LblPay.Text = "4. 每月攤還本息：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 190);
            this.Controls.Add(this.LblPay);
            this.Controls.Add(this.LblYear);
            this.Controls.Add(this.LblRate);
            this.Controls.Add(this.LblLoan);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label LblLoan;
        private Label LblRate;
        private Label LblYear;
        private Label LblPay;
    }
}