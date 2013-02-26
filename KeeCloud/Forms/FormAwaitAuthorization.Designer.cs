namespace KeeCloud.Forms
{
    partial class FormAwaitAuthorization
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelInstrucctions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelInstrucctions
            // 
            this.labelInstrucctions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInstrucctions.Location = new System.Drawing.Point(15, 69);
            this.labelInstrucctions.Name = "labelInstrucctions";
            this.labelInstrucctions.Size = new System.Drawing.Size(451, 121);
            this.labelInstrucctions.TabIndex = 0;
            this.labelInstrucctions.Text = "A browser window should have opened. You must authenticate with the service provi" +
    "der. Once you are done, click next. If you click next before you have authentica" +
    "ted then this process will fail.";
            // 
            // FormAwaitAuthorization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelInstrucctions);
            this.Name = "FormAwaitAuthorization";
            this.Size = new System.Drawing.Size(484, 248);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelInstrucctions;
    }
}
