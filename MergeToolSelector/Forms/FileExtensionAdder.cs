using System;
using System.IO;
using System.Windows.Forms;
using MergeToolSelector.Utility.FileExtensions;

namespace MergeToolSelector.Forms
{
    public partial class FileExtensionAdder : Form
    {
        public FileExtensionAdder()
        {
            InitializeComponent();
        }

        public void PopulateFrom(FileExtension fileExtension)
        {
            _commandTextbox.Text = fileExtension.Command;
            _extTextbox.Text = string.Join(" ", fileExtension.FileExts);
            _diffArgTextbox.Text = fileExtension.DiffArguments;
            _mergeArgTextbox.Text = fileExtension.MergeArguments;
        }

        public FileExtension GetFileExtension()
        {
            return new FileExtension
            {
                Command = _commandTextbox.Text,
                DiffArguments = _diffArgTextbox.Text,
                MergeArguments = _mergeArgTextbox.Text,
                FileExts = _extTextbox.Text.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries),
            };
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
