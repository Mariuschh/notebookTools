using System.Text.Json;
using System.Text.Json.Nodes;
using NotebookTools.Help;
using NotebookTools.Notebook;

namespace NotebookTools.Features
{
    public class CommandMerge
    {
        public static int Merge(string[] args)
        {
            if (args.Length < 6 || args.Length % 2 != 0)
            {
                Console.Error.WriteLine("Verwendung: notebookTools <Zieldatei> merge <Notebook1> <Zellen> <Notebook2> <Zellen> ...");
                Console.Error.WriteLine("Beispiel: notebookTools zusammen merge erstes 1,3 zweites 2");
                return 1;
            }

            try
            {
                string outputPath = Path.GetFullPath(PathHelper.NormalizeInput(args[0]));
                if (File.Exists(outputPath))
                {
                    Console.Error.WriteLine("Die Zieldatei existiert bereits. Bitte wähle einen anderen Namen.");
                    return 1;
                }
                List<string> filePaths = new List<string>();
                List<List<int>> cellNumbers = new List<List<int>>();

                // Ab Position 2 folgen immer ein Notebook und seine Zellnummern.
                for (int argumentNumber = 2; argumentNumber < args.Length; argumentNumber += 2)
                {
                    List<int> selectedNumbers = new List<int>();
                    string[] numberTexts = args[argumentNumber + 1].Split(',');
                    foreach (string numberText in numberTexts)
                    {
                        int cellNumber;
                        if (!int.TryParse(numberText, out cellNumber) || cellNumber < 1)
                        {
                            Console.Error.WriteLine("Bitte gib Zellnummern ab 1 an, getrennt durch Kommas.");
                            return 1;
                        }
                        selectedNumbers.Add(cellNumber);
                    }

                    string filePath;
                    int searchExitCode = RunFileSearch.Search(new string[] { args[argumentNumber] }, out filePath);
                    if (searchExitCode != 0)
                    {
                        return searchExitCode;
                    }
                    filePaths.Add(filePath);
                    cellNumbers.Add(selectedNumbers);
                }

                JsonObject mergedNotebook = NotebookMerger.Merge(filePaths, cellNumbers);
                NotebookWriter.SaveNotebook(outputPath, mergedNotebook);
                Console.WriteLine("Notebook gespeichert: " + outputPath);
                return 0;
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
                return 1;
            }
            catch (IOException exception)
            {
                Console.Error.WriteLine(exception.Message);
                return 1;
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.Error.WriteLine(exception.Message);
                return 1;
            }
            catch (JsonException)
            {
                Console.Error.WriteLine("Eine Notebook-Datei enthält ungültiges JSON.");
                return 1;
            }
            catch (InvalidOperationException)
            {
                Console.Error.WriteLine("Eine Notebook-Datei enthält ungültige Daten.");
                return 1;
            }
        }
    }
}
