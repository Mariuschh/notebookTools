using NotebookTools.Notebook;
using System.Text.Json;

namespace NotebookTools.Features
{
    public class CommandExport
    {
        public static void Export(string[] args, string filePath)
        {
            if (args.Length != 3 && args.Length != 4)
            {
                Console.WriteLine("Verwendung: notebookTools <Datei> export <txt/html/docx> [Zieldatei]");
                return;
            }

            string format = args[2].ToLower();
            if (format != "txt" && format != "html" && format != "docx")
            {
                Console.WriteLine("Bitte wähle txt, html oder docx als Exportformat.");
                return;
            }

            try
            {
                string outputPath = Path.ChangeExtension(filePath, format);
                if (args.Length == 4)
                {
                    outputPath = Path.GetFullPath(args[3]);
                    if (!outputPath.EndsWith("." + format, StringComparison.OrdinalIgnoreCase))
                    {
                        outputPath += "." + format;
                    }
                }
                if (File.Exists(outputPath))
                {
                    Console.WriteLine("Die Zieldatei existiert bereits. Es wurde nichts überschrieben.");
                    return;
                }

                Notebooks notebook = NotebookReader.LoadNotebook(filePath);
                NotebookExporter.ExportNotebook(notebook, outputPath, format);
                Console.WriteLine("Notebook exportiert: " + outputPath);
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (NotSupportedException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (JsonException)
            {
                Console.WriteLine("Die Notebook-Datei enthält ungültiges JSON.");
            }
        }
    }
}
