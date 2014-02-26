using System.Windows.Forms;

namespace MergeToolSelector.Utility
{
    public class MessageDisplayer : IMessageDisplayer
    {
        public void Display(string message)
        {
            MessageBox.Show(message);
        }
    }
}