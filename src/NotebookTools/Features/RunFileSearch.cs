using System.Data.OleDb;
using System.Runtime.InteropServices;
using NotebookTools.Help;

namespace NotebookTools.Features;

public class RunFileSearch
{
    public static int Search(string[] args)
    {
        return Search(args, out _);
    }

    public static int Search(string[] args, out string filePath)
    {
        filePath = "";

        if (args.Length == 0)
        {
            Console.Error.WriteLine(
                "Verwendung: notebookTools <Dateiname>");
            return 1;
        }

        string userInput = args[0];

        try
        {
            string? localPath = PathHelper.GetLocalPath(userInput);

            if (localPath != null)
            {
                filePath = localPath;
                Console.WriteLine($"Notebook gefunden: {filePath}");
                return 0;
            }

            string fileName = Path.GetFileName(
                PathHelper.NormalizeInput(userInput));

            IReadOnlyList<string> results =
                FileHelper.FindByFileName(fileName);

            if (results.Count == 1)
            {
                filePath = results[0];
            }

            return HandleResults(results);
        }
        catch (OleDbException)
        {
            return ShowSearchError();
        }
        catch (InvalidOperationException)
        {
            return ShowSearchError();
        }
        catch (NotSupportedException)
        {
            return ShowSearchError();
        }
        catch (COMException)
        {
            return ShowSearchError();
        }
        catch (ArgumentException exception)
        {
            Console.Error.WriteLine(exception.Message);
            return 1;
        }
    }

    private static int ShowSearchError()
    {
        Console.Error.WriteLine(
            "Windows Search ist nicht verfügbar. Navigiere zum Verzeichnis des Notebooks oder übergib den vollständigen Pfad.");
        return 2;
    }

    private static int HandleResults(
        IReadOnlyList<string> results)
    {
        if (results.Count == 0)
        {
            Console.Error.WriteLine(
                "Notebook nicht gefunden. Navigiere zum Verzeichnis des Notebooks oder übergib den vollständigen Pfad.");

            return 1;
        }

        if (results.Count == 1)
        {
            Console.WriteLine(
                $"Notebook gefunden: {results[0]}");

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
