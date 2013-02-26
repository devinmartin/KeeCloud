namespace KeeCloud.Forms
{
    partial class FormAuthorizationSuccess
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
            this.labelUsernameHeader = new System.Windows.Forms.Label();
            this.labelPasswordHeader = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.buttonShow = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelUsernameHeader
            // 
            this.labelUsernameHeader.AutoSize = true;
            this.labelUsernameHeader.Location = new System.Drawing.Point(19, 48);
            this.labelUsernameHeader.Name = "labelUsernameHeader";
            this.labelUsernameHeader.Size = new System.Drawing.Size(58, 13);
            this.labelUsernameHeader.TabIndex = 0;
            this.labelUsernameHeader.Text = "Username:";
            // 
            // labelPasswordHeader
            // 
            this.labelPasswordHeader.AutoSize = true;
            this.labelPasswordHeader.Location = new System.Drawing.Point(19, 88);
            this.labelPasswordHeader.Name = "labelPasswordHeader";
            this.labelPasswordHeader.Size = new System.Drawing.Size(56, 13);
            this.labelPasswordHeader.TabIndex = 1;
            this.labelPasswordHeader.Text = "Password:";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(83, 48);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(39, 13);
            this.labelUsername.TabIndex = 2;
            this.labelUsername.Text = "uname";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(83, 88);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(23, 13);
            this.labelPassword.TabIndex = 3;
            this.labelPassword.Text = "****";
            // 
            // buttonShow
            // 
            this.buttonShow.Location = new System.Drawing.Point(22, 132);
            this.buttonShow.Name = "buttonShow";
            this.buttonShow.Size = new System.Drawing.Size(100, 23);
            this.buttonShow.TabIndex = 4;
            this.buttonShow.Text = "Show Password";
            this.buttonShow.UseVisualStyleBackColor = true;
            this.buttonShow.Click += new System.EventHandler(this.buttonShow_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(142, 131);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(86, 23);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save as Entry";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // FormAuthorizationSuccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonShow);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.labelPasswordHeader);
            this.Controls.Add(this.labelUsernameHeader);
            this.Name = "FormAuthorizationSuccess";
            this.Size = new System.Drawing.Size(484, 248);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUsernameHeader;
        private System.Windows.Forms.Label labelPasswordHeader;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Button buttonShow;
        private System.Windows.Forms.Button buttonSave;
    }
}
