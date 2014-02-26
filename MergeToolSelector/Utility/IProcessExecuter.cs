namespace MergeToolSelector.Utility
{
    public interface IProcessExecuter
    {
        void Start(string command, string arguments);
    }
}