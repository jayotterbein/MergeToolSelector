using System;
using System.IO;
using System.Windows.Forms;

namespace MergeToolSelector.Forms
{
    public partial class FileExtensionAdder : Form
    {
        public FileExtensionAdder()
        {
            InitializeComponent();
        }

        private void _commandButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.CheckFileExists = true;
                dialog.Multiselect = false;
                dialog.AutoUpgradeEnabled = true;
                dialog.FilterIndex = 0;
                dialog.Filter = "Executable Files (exe, com, bat)|*.exe;*.com;*.bat|All Files (*.*);*.*|";
                dialog.ShowReadOnly = false;
                dialog.Title = "Select the merge/diff tool";

                if (!string.IsNullOrWhiteSpace(_commandTextbox.Text))
                {
                    var path = Path.GetDirectoryName(_commandTextbox.Text);
                    if (!string.IsNullOrEmpty(path))
                    {
                        dialog.InitialDirectory = path;
                    }
                }

                var dialogResult = dialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    _commandTextbox.Text = dialog.FileName;
                }
            }
        }
    }
}
