using NotebookTools.Notebook;

namespace NotebookTools.Features
{
    public class CommandMove
    {
        public static void Move(string[] args, string filePath)
        {
            int zellNummer;
            int zielNummer;
            if (args.Length != 4 || !int.TryParse(args[2], out zellNummer) || !int.TryParse(args[3], out zielNummer))
            {
                Console.WriteLine("Verwendung: notebookTools <Datei> move <Zellnummer> <Zielposition>");
                return;
            }

            Notebooks notebook = NotebookReader.LoadNotebook(filePath);
            if (zellNummer < 1 || zellNummer > notebook.Cells.Count)
            {
                Console.WriteLine("Die Zellnummer liegt außerhalb des möglichen Bereichs.");
                return;
            }
            if (zielNummer < 1 || zielNummer > notebook.Cells.Count)
            {
                Console.WriteLine("Die Zielposition liegt außerhalb des möglichen Bereichs.");
                return;
            }

            ZelleVerschieben(zellNummer, zielNummer, notebook);
            CommandContent.Content(filePath, notebook);
            Console.WriteLine("Das Notebook wurde nur im Arbeitsspeicher geändert. Die Datei wurde nicht gespeichert.");
        }

        public static Notebooks ZelleVerschieben(int zellNummer, int zielNummer, Notebooks notebook)
        {
            int zellIndex = zellNummer - 1;
            int zielIndex = zielNummer - 1;
            NotebookCell cell = notebook.Cells[zellIndex];

            notebook.Cells.RemoveAt(zellIndex);
            notebook.Cells.Insert(zielIndex, cell);
            return notebook;
        }
    }
}
