using NotebookTools.Help;

namespace NotebookTools
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "help" || args[0] == "--help")
            {
                Console.WriteLine(HelpText.GetText());
                return;
            }
        }
    }
}
