namespace MergeToolSelector.Forms
{
    partial class Main
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
            this._addToolButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _addToolButton
            // 
            this._addToolButton.Location = new System.Drawing.Point(13, 40);
            this._addToolButton.Name = "_addToolButton";
            this._addToolButton.Size = new System.Drawing.Size(75, 23);
            this._addToolButton.TabIndex = 0;
            this._addToolButton.Text = "Add New";
            this._addToolButton.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 300);
            this.Controls.Add(this._addToolButton);
            this.Name = "Main";
            this.Text = "Main";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _addToolButton;
    }
}