using System.Text.Json;
using NotebookTools.Help;
using NotebookTools.Notebook;

namespace NotebookTools.Features
{
    public class CommandSave
    {
        public static void Save(Notebooks notebook, string originalPath)
        {
            string originalDirectory = Path.GetDirectoryName(Path.GetFullPath(originalPath)) ?? Environment.CurrentDirectory;
            string defaultPath = Path.Combine(originalDirectory,
                Path.GetFileNameWithoutExtension(originalPath) + "_bearbeitet.ipynb");

            Console.WriteLine("In welcher neuen Datei sollen die Änderungen gespeichert werden?");
            Console.WriteLine("Drücke Enter für " + defaultPath + " oder gib einen eigenen Dateipfad ein.");
            Console.WriteLine("Gib 'abbrechen' ein, um die Änderungen zu verwerfen.");
            string? userInput = Console.ReadLine();
            if (userInput == null || userInput.Trim().ToLower() == "abbrechen")
            {
                Console.WriteLine("Die Änderungen wurden nicht gespeichert.");
                return;
            }

            try
            {
                string outputPath = defaultPath;
                if (userInput.Trim() != "")
                {
                    outputPath = Path.GetFullPath(PathHelper.NormalizeInput(userInput.Trim()));
                }
                if (File.Exists(outputPath))
                {
                    Console.WriteLine("Die Zieldatei existiert bereits. Es wurde nichts überschrieben.");
                    return;
                }

                NotebookWriter.SaveNotebook(outputPath, notebook);
                Console.WriteLine("Notebook gespeichert: " + outputPath);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (JsonException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (NotSupportedException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
