using System.Windows.Forms;

namespace MergeToolSelector.Utility
{
    public class FormDisplayer : IFormDisplayer
    {
        public void Display(Form form)
        {
            Application.Run(form);
        }
    }
}