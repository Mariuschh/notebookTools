namespace NotebookTools.Help;

public class HelpText
{
    public static string GetText()
    {
        return """
               NotebookTools

               Commands:
                 help
                 content <file.ipynb>
                 show <file.ipynb> <cellNumber>
               """;
    }
}