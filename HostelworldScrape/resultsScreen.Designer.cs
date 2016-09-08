namespace HostelworldScrape
{
    partial class resultsScreen
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.labelLocation = new System.Windows.Forms.Label();
            this.labelOutputLoc = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.labelOutputLoc);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.labelLocation);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(362, 152);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Location:";
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(99, 20);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(35, 13);
            this.labelLocation.TabIndex = 1;
            this.labelLocation.Text = "label2";
            // 
            // labelOutputLoc
            // 
            this.labelOutputLoc.AutoSize = true;
            this.labelOutputLoc.Location = new System.Drawing.Point(99, 40);
            this.labelOutputLoc.Name = "labelOutputLoc";
            this.labelOutputLoc.Size = new System.Drawing.Size(35, 13);
            this.labelOutputLoc.TabIndex = 7;
            this.labelOutputLoc.Text = "label5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Output Location:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.label);
            this.panel2.Location = new System.Drawing.Point(17, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(327, 68);
            this.panel2.TabIndex = 8;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(3, 43);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(312, 13);
            this.label.TabIndex = 0;
            this.label.Text = "Process is now complete. Pressing OK will restart the application.";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(240, 14);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // resultsScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 170);
            this.Controls.Add(this.panel1);
            this.Name = "resultsScreen";
            this.Text = "Summary";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label labelOutputLoc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.Label label1;
    }
}