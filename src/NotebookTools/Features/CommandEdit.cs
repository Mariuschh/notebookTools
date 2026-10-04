using NotebookTools.Notebook;
using System.ComponentModel;
using System.Text.Json;

namespace NotebookTools.Features
{
    public class CommandEdit
    {
        public static void Edit(string[] args, string filePath)
        {
            int zellNummer;
            if (args.Length != 3 || !int.TryParse(args[2], out zellNummer))
            {
                Console.WriteLine("Verwendung: notebookTools <Datei> edit <Zellnummer>");
                return;
            }

            Notebooks notebook = NotebookReader.LoadNotebook(filePath);
            if (zellNummer < 1 || zellNummer > notebook.Cells.Count)
            {
                Console.WriteLine("Die Zellnummer liegt außerhalb des möglichen Bereichs.");
                return;
            }

            string? neuerInhalt;
            try
            {
                neuerInhalt = CellEditor.OpenCell(filePath, zellNummer);
            }
            catch (Win32Exception)
            {
                Console.WriteLine("Die Datei konnte nicht geöffnet werden. Bitte richte eine Standard-App für .ipynb ein.");
                return;
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }
            catch (JsonException)
            {
                Console.WriteLine("Die Notebook-Datei enthält ungültiges JSON.");
                return;
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Die Notebook-Datei enthält ungültige Daten.");
                return;
            }
            if (neuerInhalt == null)
            {
                return;
            }

            ZelleBearbeiten(zellNummer, notebook, neuerInhalt);
            CommandContent.Content(filePath, notebook);
            CommandSave.Save(notebook, filePath);
        }

        public static Notebooks ZelleBearbeiten(int zellNummer, Notebooks notebook, string neuerInhalt)
        {
            int zellIndex = zellNummer - 1;
            NotebookCell cell = notebook.Cells[zellIndex];
            cell.Source.Clear();
            cell.Source.Add(neuerInhalt);

            // Alte Ausgaben passen nach einer Änderung nicht mehr zum Code.
            if (cell.Type == "code")
            {
                cell.Outputs.Clear();
                cell.ExecutionCount = null;
            }
            return notebook;
        }
    }
}
