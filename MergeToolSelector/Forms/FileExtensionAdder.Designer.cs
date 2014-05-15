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
            this._diffArgLabel = new System.Windows.Forms.Label();
            this._diffArgTextbox = new System.Windows.Forms.TextBox();
            this._diffArgHelpLabel1 = new System.Windows.Forms.Label();
            this._mergeArgHelpLabel1 = new System.Windows.Forms.Label();
            this._mergeArgLabel = new System.Windows.Forms.Label();
            this._mergeArgHelpLabel2 = new System.Windows.Forms.Label();
            this._mergeArgTextbox = new System.Windows.Forms.TextBox();
            this._diffArgHelpLabel2 = new System.Windows.Forms.Label();
            this._helpLabel = new System.Windows.Forms.Label();
            this._extLabel = new System.Windows.Forms.Label();
            this._extTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _commandLabel
            // 
            this._commandLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this._commandLabel.AutoSize = true;
            this._commandLabel.Location = new System.Drawing.Point(13, 17);
            this._commandLabel.Name = "_commandLabel";
            this._commandLabel.Size = new System.Drawing.Size(54, 13);
            this._commandLabel.TabIndex = 0;
            this._commandLabel.Text = "Command";
            // 
            // _commandTextbox
            // 
            this._commandTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._commandTextbox.Location = new System.Drawing.Point(73, 13);
            this._commandTextbox.Name = "_commandTextbox";
            this._commandTextbox.Size = new System.Drawing.Size(378, 20);
            this._commandTextbox.TabIndex = 1;
            // 
            // _commandButton
            // 
            this._commandButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._commandButton.Location = new System.Drawing.Point(457, 13);
            this._commandButton.Name = "_commandButton";
            this._commandButton.Size = new System.Drawing.Size(75, 20);
            this._commandButton.TabIndex = 2;
            this._commandButton.Text = "Set";
            this._commandButton.UseVisualStyleBackColor = true;
            this._commandButton.Click += new System.EventHandler(this._commandButton_Click);
            // 
            // _diffArgLabel
            // 
            this._diffArgLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this._diffArgLabel.AutoSize = true;
            this._diffArgLabel.Location = new System.Drawing.Point(12, 137);
            this._diffArgLabel.Name = "_diffArgLabel";
            this._diffArgLabel.Size = new System.Drawing.Size(76, 13);
            this._diffArgLabel.TabIndex = 12;
            this._diffArgLabel.Text = "Diff Arguments";
            // 
            // _diffArgTextbox
            // 
            this._diffArgTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._diffArgTextbox.Location = new System.Drawing.Point(94, 134);
            this._diffArgTextbox.Name = "_diffArgTextbox";
            this._diffArgTextbox.Size = new System.Drawing.Size(438, 20);
            this._diffArgTextbox.TabIndex = 4;
            // 
            // _diffArgHelpLabel1
            // 
            this._diffArgHelpLabel1.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this._diffArgHelpLabel1.AutoSize = true;
            this._diffArgHelpLabel1.Location = new System.Drawing.Point(91, 100);
            this._diffArgHelpLabel1.Name = "_diffArgHelpLabel1";
            this._diffArgHelpLabel1.Size = new System.Drawing.Size(104, 13);
            this._diffArgHelpLabel1.TabIndex = 5;
            this._diffArgHelpLabel1.Text = "$1 = Left, $2 = Right";
            // 
            // _mergeArgHelpLabel1
            // 
            this._mergeArgHelpLabel1.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this._mergeArgHelpLabel1.AutoSize = true;
            this._mergeArgHelpLabel1.Location = new System.Drawing.Point(91, 181);
            this._mergeArgHelpLabel1.Name = "_mergeArgHelpLabel1";
            this._mergeArgHelpLabel1.Size = new System.Drawing.Size(218, 13);
            this._mergeArgHelpLabel1.TabIndex = 6;
            this._mergeArgHelpLabel1.Text = "$1 = Left, $2 = Right, $3 = Center, $4 Output";
            // 
            // _mergeArgLabel
            // 
            this._mergeArgLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this._mergeArgLabel.AutoSize = true;
            this._mergeArgLabel.Location = new System.Drawing.Point(13, 216);
            this._mergeArgLabel.Name = "_mergeArgLabel";
            this._mergeArgLabel.Size = new System.Drawing.Size(90, 13);
            this._mergeArgLabel.TabIndex = 7;
            this._mergeArgLabel.Text = "Marge Arguments";
            // 
            // _mergeArgHelpLabel2
            // 
            this._mergeArgHelpLabel2.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this._mergeArgHelpLabel2.AutoSize = true;
            this._mergeArgHelpLabel2.Location = new System.Drawing.Point(91, 197);
            this._mergeArgHelpLabel2.Name = "_mergeArgHelpLabel2";
            this._mergeArgHelpLabel2.Size = new System.Drawing.Size(361, 13);
            this._mergeArgHelpLabel2.TabIndex = 8;
            this._mergeArgHelpLabel2.Text = "Optional: $5 Left Label, $6 Right Label, $7 Center Label, $8 = Output Label";
            // 
            // _mergeArgTextbox
            // 
            this._mergeArgTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._mergeArgTextbox.Location = new System.Drawing.Point(109, 213);
            this._mergeArgTextbox.Name = "_mergeArgTextbox";
            this._mergeArgTextbox.Size = new System.Drawing.Size(423, 20);
            this._mergeArgTextbox.TabIndex = 5;
            // 
            // _diffArgHelpLabel2
            // 
            this._diffArgHelpLabel2.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this._diffArgHelpLabel2.AutoSize = true;
            this._diffArgHelpLabel2.Location = new System.Drawing.Point(91, 118);
            this._diffArgHelpLabel2.Name = "_diffArgHelpLabel2";
            this._diffArgHelpLabel2.Size = new System.Drawing.Size(198, 13);
            this._diffArgHelpLabel2.TabIndex = 10;
            this._diffArgHelpLabel2.Text = "Optional: $3 = Left Label, $4 Right Label";
            // 
            // _helpLabel
            // 
            this._helpLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this._helpLabel.AutoSize = true;
            this._helpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._helpLabel.ForeColor = System.Drawing.Color.Red;
            this._helpLabel.Location = new System.Drawing.Point(70, 75);
            this._helpLabel.Name = "_helpLabel";
            this._helpLabel.Size = new System.Drawing.Size(445, 15);
            this._helpLabel.TabIndex = 11;
            this._helpLabel.Text = "NOTE: Quotes must be added by you, this allows for arguments such as: /sn=\"$1\"";
            // 
            // _extLabel
            // 
            this._extLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this._extLabel.AutoSize = true;
            this._extLabel.Location = new System.Drawing.Point(12, 42);
            this._extLabel.Name = "_extLabel";
            this._extLabel.Size = new System.Drawing.Size(171, 13);
            this._extLabel.TabIndex = 12;
            this._extLabel.Text = "Extensions (ex: .xml .config .csproj)";
            // 
            // _extTextbox
            // 
            this._extTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._extTextbox.Location = new System.Drawing.Point(189, 39);
            this._extTextbox.Name = "_extTextbox";
            this._extTextbox.Size = new System.Drawing.Size(263, 20);
            this._extTextbox.TabIndex = 3;
            // 
            // FileExtensionAdder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 261);
            this.Controls.Add(this._extTextbox);
            this.Controls.Add(this._extLabel);
            this.Controls.Add(this._helpLabel);
            this.Controls.Add(this._diffArgHelpLabel2);
            this.Controls.Add(this._mergeArgTextbox);
            this.Controls.Add(this._mergeArgHelpLabel2);
            this.Controls.Add(this._mergeArgLabel);
            this.Controls.Add(this._mergeArgHelpLabel1);
            this.Controls.Add(this._diffArgHelpLabel1);
            this.Controls.Add(this._diffArgTextbox);
            this.Controls.Add(this._diffArgLabel);
            this.Controls.Add(this._commandButton);
            this.Controls.Add(this._commandTextbox);
            this.Controls.Add(this._commandLabel);
            this.MinimumSize = new System.Drawing.Size(560, 300);
            this.Name = "FileExtensionAdder";
            this.Text = "Adder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _commandLabel;
        private System.Windows.Forms.TextBox _commandTextbox;
        private System.Windows.Forms.Button _commandButton;
        private System.Windows.Forms.Label _diffArgLabel;
        private System.Windows.Forms.TextBox _diffArgTextbox;
        private System.Windows.Forms.Label _diffArgHelpLabel1;
        private System.Windows.Forms.Label _mergeArgHelpLabel1;
        private System.Windows.Forms.Label _mergeArgLabel;
        private System.Windows.Forms.Label _mergeArgHelpLabel2;
        private System.Windows.Forms.TextBox _mergeArgTextbox;
        private System.Windows.Forms.Label _diffArgHelpLabel2;
        private System.Windows.Forms.Label _helpLabel;
        private System.Windows.Forms.Label _extLabel;
        private System.Windows.Forms.TextBox _extTextbox;
    }
}