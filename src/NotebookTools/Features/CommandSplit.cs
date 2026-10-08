using NotebookTools.Notebook;
using System.Text.Json;

namespace NotebookTools.Features
{
    public class CommandSplit
    {
        public static void Split(string[] args, string filePath)
        {
            int zellNummer;
            int zeilenNummer;
            if (args.Length != 4 || !int.TryParse(args[2], out zellNummer) || !int.TryParse(args[3], out zeilenNummer))
            {
                Console.WriteLine("Verwendung: notebookTools <Datei> split <Zellnummer> <NachZeile>");
                return;
            }

            Notebooks notebook = NotebookReader.LoadNotebook(filePath);
            if (zellNummer < 1 || zellNummer > notebook.Cells.Count)
            {
                Console.WriteLine("Die Zellnummer liegt außerhalb des möglichen Bereichs.");
                return;
            }

            NotebookCell cell = notebook.Cells[zellNummer - 1];
            string content = string.Join("", cell.Source);
            int position = 0;
            int zeile = 0;
            // Nach dem Zeilenumbruch trennen und den ursprünglichen Text erhalten.
            for (int index = 0; index < content.Length; index++)
            {
                if (content[index] == '\r' || content[index] == '\n')
                {
                    if (content[index] == '\r' && index + 1 < content.Length && content[index + 1] == '\n')
                    {
                        index++;
                    }
                    zeile++;
                    if (zeile == zeilenNummer)
                    {
                        position = index + 1;
                        break;
                    }
                }
            }
            if (position == 0 || position == content.Length)
            {
                Console.WriteLine("Bitte wähle eine Zeile, nach der noch Text für die zweite Zelle folgt.");
                return;
            }

            NotebookCell neueZelle = new NotebookCell();
            neueZelle.Type = cell.Type;
            neueZelle.Metadata = new Dictionary<string, object>(cell.Metadata);
            neueZelle.AdditionalData = new Dictionary<string, JsonElement>(cell.AdditionalData);
            neueZelle.AdditionalData.Remove("id");
            neueZelle.Source.Add(content.Substring(position));
            cell.Source.Clear();
            cell.Source.Add(content.Substring(0, position));
            cell.Outputs.Clear();
            cell.ExecutionCount = null;
            notebook.Cells.Insert(zellNummer, neueZelle);
            CommandContent.Content(filePath, notebook);
            CommandSave.Save(notebook, filePath);
        }
    }
}
