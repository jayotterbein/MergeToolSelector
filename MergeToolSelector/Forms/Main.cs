using System;
using System.Windows.Forms;
using MergeToolSelector.Utility.Settings;

namespace MergeToolSelector.Forms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void _addToolButton_Click(object sender, EventArgs e)
        {
            using (var extAdder = new FileExtensionAdder())
            {
                var dialogRes = extAdder.ShowDialog(this);
                if (dialogRes != DialogResult.OK)
                {
                    var fileExt = extAdder.GetFileExtension();
                    var fileProvider = new FileProvider();
                    var persister = new FileExtensionPersister(fileProvider);
                    persister.SaveFileExtensions(fileExt);
                }
            }
        }
    }
}
