namespace KeeCloud.Forms
{
    partial class FormSelectProvider
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
            this.labelInstructions = new System.Windows.Forms.Label();
            this.comboBoxProviders = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labelInstructions
            // 
            this.labelInstructions.Location = new System.Drawing.Point(23, 24);
            this.labelInstructions.Name = "labelInstructions";
            this.labelInstructions.Size = new System.Drawing.Size(439, 69);
            this.labelInstructions.TabIndex = 0;
            this.labelInstructions.Text = "This wizard allows those providers that require linking to the account to work.\r\n" +
    "\r\nPlease select a provider to integrate ";
            // 
            // comboBoxProviders
            // 
            this.comboBoxProviders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProviders.FormattingEnabled = true;
            this.comboBoxProviders.Location = new System.Drawing.Point(26, 97);
            this.comboBoxProviders.Name = "comboBoxProviders";
            this.comboBoxProviders.Size = new System.Drawing.Size(436, 21);
            this.comboBoxProviders.TabIndex = 1;
            this.comboBoxProviders.SelectedIndexChanged += new System.EventHandler(this.comboBoxProviders_SelectedIndexChanged);
            // 
            // FormSelectProvider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxProviders);
            this.Controls.Add(this.labelInstructions);
            this.Name = "FormSelectProvider";
            this.Size = new System.Drawing.Size(484, 248);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelInstructions;
        private System.Windows.Forms.ComboBox comboBoxProviders;

    }
}
