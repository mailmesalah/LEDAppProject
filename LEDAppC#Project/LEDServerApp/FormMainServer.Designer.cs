namespace LEDServerApp
{
    partial class FormMainServer
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
            this.textHostIPAddress = new System.Windows.Forms.TextBox();
            this.textPortNo = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rightLED3 = new System.Windows.Forms.Button();
            this.rightLED2 = new System.Windows.Forms.Button();
            this.rightLED1 = new System.Windows.Forms.Button();
            this.rightLED0 = new System.Windows.Forms.Button();
            this.leftLED3 = new System.Windows.Forms.Button();
            this.leftLED2 = new System.Windows.Forms.Button();
            this.leftLED1 = new System.Windows.Forms.Button();
            this.leftLED0 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textHostIPAddress
            // 
            this.textHostIPAddress.Location = new System.Drawing.Point(100, 67);
            this.textHostIPAddress.Name = "textHostIPAddress";
            this.textHostIPAddress.Size = new System.Drawing.Size(100, 20);
            this.textHostIPAddress.TabIndex = 8;
            this.textHostIPAddress.Text = "127.0.0.1";
            // 
            // textPortNo
            // 
            this.textPortNo.Location = new System.Drawing.Point(100, 93);
            this.textPortNo.Name = "textPortNo";
            this.textPortNo.Size = new System.Drawing.Size(100, 20);
            this.textPortNo.TabIndex = 9;
            this.textPortNo.Text = "8080";
            // 
            // buttonConnect
            // 
            this.buttonConnect.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonConnect.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.buttonConnect.Location = new System.Drawing.Point(208, 65);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 10;
            this.buttonConnect.Text = "Run";
            this.buttonConnect.UseVisualStyleBackColor = false;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label1.Location = new System.Drawing.Point(32, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "IP Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label2.Location = new System.Drawing.Point(47, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Port No";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label4.Location = new System.Drawing.Point(205, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 16);
            this.label4.TabIndex = 33;
            this.label4.Text = "Remote";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label3.Location = new System.Drawing.Point(63, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 16);
            this.label3.TabIndex = 32;
            this.label3.Text = "Main";
            // 
            // rightLED3
            // 
            this.rightLED3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rightLED3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.rightLED3.Location = new System.Drawing.Point(197, 341);
            this.rightLED3.Name = "rightLED3";
            this.rightLED3.Size = new System.Drawing.Size(64, 53);
            this.rightLED3.TabIndex = 31;
            this.rightLED3.UseVisualStyleBackColor = false;
            this.rightLED3.Click += new System.EventHandler(this.rightLED3_Click);
            // 
            // rightLED2
            // 
            this.rightLED2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rightLED2.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.rightLED2.Location = new System.Drawing.Point(197, 282);
            this.rightLED2.Name = "rightLED2";
            this.rightLED2.Size = new System.Drawing.Size(64, 53);
            this.rightLED2.TabIndex = 30;
            this.rightLED2.UseVisualStyleBackColor = false;
            this.rightLED2.Click += new System.EventHandler(this.rightLED2_Click);
            // 
            // rightLED1
            // 
            this.rightLED1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rightLED1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.rightLED1.Location = new System.Drawing.Point(197, 223);
            this.rightLED1.Name = "rightLED1";
            this.rightLED1.Size = new System.Drawing.Size(64, 53);
            this.rightLED1.TabIndex = 29;
            this.rightLED1.UseVisualStyleBackColor = false;
            this.rightLED1.Click += new System.EventHandler(this.rightLED1_Click);
            // 
            // rightLED0
            // 
            this.rightLED0.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rightLED0.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.rightLED0.Location = new System.Drawing.Point(197, 164);
            this.rightLED0.Name = "rightLED0";
            this.rightLED0.Size = new System.Drawing.Size(64, 53);
            this.rightLED0.TabIndex = 28;
            this.rightLED0.UseVisualStyleBackColor = false;
            this.rightLED0.Click += new System.EventHandler(this.rightLED0_Click);
            // 
            // leftLED3
            // 
            this.leftLED3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.leftLED3.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.leftLED3.Location = new System.Drawing.Point(50, 341);
            this.leftLED3.Name = "leftLED3";
            this.leftLED3.Size = new System.Drawing.Size(64, 53);
            this.leftLED3.TabIndex = 27;
            this.leftLED3.UseVisualStyleBackColor = false;
            this.leftLED3.Click += new System.EventHandler(this.leftLED3_Click);
            // 
            // leftLED2
            // 
            this.leftLED2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.leftLED2.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.leftLED2.Location = new System.Drawing.Point(50, 282);
            this.leftLED2.Name = "leftLED2";
            this.leftLED2.Size = new System.Drawing.Size(64, 53);
            this.leftLED2.TabIndex = 26;
            this.leftLED2.UseVisualStyleBackColor = false;
            this.leftLED2.Click += new System.EventHandler(this.leftLED2_Click);
            // 
            // leftLED1
            // 
            this.leftLED1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.leftLED1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.leftLED1.Location = new System.Drawing.Point(50, 223);
            this.leftLED1.Name = "leftLED1";
            this.leftLED1.Size = new System.Drawing.Size(64, 53);
            this.leftLED1.TabIndex = 25;
            this.leftLED1.UseVisualStyleBackColor = false;
            this.leftLED1.Click += new System.EventHandler(this.leftLED1_Click);
            // 
            // leftLED0
            // 
            this.leftLED0.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.leftLED0.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.leftLED0.Location = new System.Drawing.Point(50, 164);
            this.leftLED0.Name = "leftLED0";
            this.leftLED0.Size = new System.Drawing.Size(64, 53);
            this.leftLED0.TabIndex = 24;
            this.leftLED0.UseVisualStyleBackColor = false;
            this.leftLED0.Click += new System.EventHandler(this.leftLED0_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label5.Location = new System.Drawing.Point(63, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(186, 16);
            this.label5.TabIndex = 34;
            this.label5.Text = "AutoHotBox LED Example";
            // 
            // FormMainServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(309, 416);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rightLED3);
            this.Controls.Add(this.rightLED2);
            this.Controls.Add(this.rightLED1);
            this.Controls.Add(this.rightLED0);
            this.Controls.Add(this.leftLED3);
            this.Controls.Add(this.leftLED2);
            this.Controls.Add(this.leftLED1);
            this.Controls.Add(this.leftLED0);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textPortNo);
            this.Controls.Add(this.textHostIPAddress);
            this.Name = "FormMainServer";
            this.Text = "LED Server App";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textHostIPAddress;
        private System.Windows.Forms.TextBox textPortNo;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button rightLED3;
        private System.Windows.Forms.Button rightLED2;
        private System.Windows.Forms.Button rightLED1;
        private System.Windows.Forms.Button rightLED0;
        private System.Windows.Forms.Button leftLED3;
        private System.Windows.Forms.Button leftLED2;
        private System.Windows.Forms.Button leftLED1;
        private System.Windows.Forms.Button leftLED0;
        private System.Windows.Forms.Label label5;

    }
}

