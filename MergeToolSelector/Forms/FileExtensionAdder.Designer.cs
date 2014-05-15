namespace MergeToolSelector.Forms
{
    partial class FileExtensionAdder
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
            this._commandLabel = new System.Windows.Forms.Label();
            this._commandTextbox = new System.Windows.Forms.TextBox();
            this._commandButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _commandLabel
            // 
            this._commandLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this._commandLabel.AutoSize = true;
            this._commandLabel.Location = new System.Drawing.Point(13, 13);
            this._commandLabel.Name = "_commandLabel";
            this._commandLabel.Size = new System.Drawing.Size(54, 13);
            this._commandLabel.TabIndex = 0;
            this._commandLabel.Text = "Command";
            // 
            // _commandTextbox
            // 
            this._commandTextbox.Location = new System.Drawing.Point(74, 13);
            this._commandTextbox.Name = "_commandTextbox";
            this._commandTextbox.Size = new System.Drawing.Size(239, 20);
            this._commandTextbox.TabIndex = 1;
            // 
            // _commandButton
            // 
            this._commandButton.Location = new System.Drawing.Point(319, 12);
            this._commandButton.Name = "_commandButton";
            this._commandButton.Size = new System.Drawing.Size(75, 23);
            this._commandButton.TabIndex = 2;
            this._commandButton.Text = "Set";
            this._commandButton.UseVisualStyleBackColor = true;
            this._commandButton.Click += new System.EventHandler(this._commandButton_Click);
            // 
            // Adder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 259);
            this.Controls.Add(this._commandButton);
            this.Controls.Add(this._commandTextbox);
            this.Controls.Add(this._commandLabel);
            this.Name = "FileExtensionAdder";
            this.Text = "Adder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _commandLabel;
        private System.Windows.Forms.TextBox _commandTextbox;
        private System.Windows.Forms.Button _commandButton;
    }
}