namespace ReBornWarRock_PServer
{
    partial class Form11
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
            this.components = new System.ComponentModel.Container();
            this.searchableRichTextBox1 = new SearchableControls.SearchableRichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // searchableRichTextBox1
            // 
            this.searchableRichTextBox1.Location = new System.Drawing.Point(12, 25);
            this.searchableRichTextBox1.Name = "searchableRichTextBox1";
            this.searchableRichTextBox1.Size = new System.Drawing.Size(1114, 700);
            this.searchableRichTextBox1.TabIndex = 0;
            this.searchableRichTextBox1.Text = "";
            this.searchableRichTextBox1.TextChanged += new System.EventHandler(this.searchableRichTextBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(267, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "CTRL + F To Find. TIP = This editor save the changes!";
            // 
            // Form11
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 737);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchableRichTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form11";
            this.Text = "Item Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form11_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public SearchableControls.SearchableRichTextBox searchableRichTextBox1;
        private System.Windows.Forms.Label label1;
    }
}