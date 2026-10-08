using NotebookTools.Notebook;

namespace NotebookTools.Features
{
    public class CommandType
    {
        public static void Type(string[] args, string filePath)
        {
            int zellNummer;
            if (args.Length != 4 || !int.TryParse(args[2], out zellNummer))
            {
                Console.WriteLine("Verwendung: notebookTools <Datei> type <Zellnummer> <code/markdown>");
                return;
            }

            string neuerTyp = args[3].ToLower();
            if (neuerTyp != "code" && neuerTyp != "markdown")
            {
                Console.WriteLine("Bitte gib als Zelltyp 'code' oder 'markdown' an.");
                return;
            }

            Notebooks notebook = NotebookReader.LoadNotebook(filePath);
            if (zellNummer < 1 || zellNummer > notebook.Cells.Count)
            {
                Console.WriteLine("Die Zellnummer liegt außerhalb des möglichen Bereichs.");
                return;
            }

            NotebookCell cell = notebook.Cells[zellNummer - 1];
            if (cell.Type == neuerTyp)
            {
                Console.WriteLine("Die Zelle hat bereits den Typ " + neuerTyp + ".");
                return;
            }

            cell.Type = neuerTyp;
            // Gespeicherte Code-Ausgaben passen nach dem Typwechsel nicht mehr.
            cell.Outputs.Clear();
            cell.ExecutionCount = null;
            CommandContent.Content(filePath, notebook);
            CommandSave.Save(notebook, filePath);
        }
    }
}
