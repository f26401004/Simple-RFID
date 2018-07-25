namespace RFID_ReaderGUI
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_start = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.label_title = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(50, 43);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(180, 23);
            this.button_start.TabIndex = 0;
            this.button_start.Text = "開始讀取";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(50, 72);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(180, 23);
            this.button_close.TabIndex = 1;
            this.button_close.Text = "關閉讀取";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Location = new System.Drawing.Point(85, 18);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(102, 12);
            this.label_title.TabIndex = 2;
            this.label_title.Text = "Simple RFID Reader";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 109);
            this.Controls.Add(this.label_title);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_start);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(295, 148);
            this.MinimumSize = new System.Drawing.Size(295, 148);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Label label_title;
    }
}

