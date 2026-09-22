using System.Data.OleDb;
using NotebookTools.Help;

namespace NotebookTools.Features;

public class RunFileSearch
{
    public static int Run(string[] args)
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine(
                "Verwendung: notebookTools <Dateiname>");
            return 1;
        }

        string userInput = args[0];

        try
        {
            IReadOnlyList<string> results =
                FileHelper.FindByFileName(userInput);

            return HandleResults(userInput, results);
        }
        catch (OleDbException exception)
        {
            Console.Error.WriteLine(
                "Windows Search konnte nicht verwendet werden.");

            Console.Error.WriteLine(exception.Message);
            return 2;
        }
        catch (ArgumentException exception)
        {
            Console.Error.WriteLine(exception.Message);
            return 1;
        }
    }

    private static int HandleResults(
        string userInput,
        IReadOnlyList<string> results)
    {
        if (results.Count == 0)
        {
            string fileName =
                PathHelper.NormalizeInput(userInput);

            Console.Error.WriteLine(
                $"Notebook „{fileName}“ wurde nicht gefunden.");

            Console.Error.WriteLine(
                "Navigiere zum Verzeichnis des Notebooks " +
                "oder übergib den vollständigen Pfad.");

            return 1;
        }

        if (results.Count == 1)
        {
            Console.WriteLine(
                $"Notebook gefunden: {results[0]}");

            // Später wird hier das Notebook verarbeitet.
            return 0;
        }

        Console.WriteLine(
            "Mehrere passende Notebooks gefunden:");

        for (int i = 0; i < results.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {results[i]}");
        }

        return 1;
    }
}