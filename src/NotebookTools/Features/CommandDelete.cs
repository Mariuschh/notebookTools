using NotebookTools.Notebook;

namespace NotebookTools.Features
{
    public class CommandDelete
    {
        public static void Delete(string[] args, string filePath)
        {
            int zellNummer;
            if (args.Length != 3 || !int.TryParse(args[2], out zellNummer))
            {
                Console.WriteLine("Verwendung: notebookTools <Datei> delete <Zellnummer>");
                return;
            }

            Notebooks notebook = NotebookReader.LoadNotebook(filePath);
            if (zellNummer < 1 || zellNummer > notebook.Cells.Count)
            {
                Console.WriteLine("Die Zellnummer liegt außerhalb des möglichen Bereichs.");
                return;
            }

            ZelleLoeschen(zellNummer, notebook);
            CommandContent.Content(filePath, notebook);
            Console.WriteLine("Das Notebook wurde nur im Arbeitsspeicher geändert. Die Datei wurde nicht gespeichert.");
        }

        public static Notebooks ZelleLoeschen(int zellNummer, Notebooks notebook)
        {
            int zellIndex = zellNummer - 1;
            notebook.Cells.RemoveAt(zellIndex);
            return notebook;
        }
    }
}
