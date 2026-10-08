using NotebookTools.Notebook;
using System.Text.Json;

namespace NotebookTools.Features
{
    public class CommandJoin
    {
        public static void Join(string[] args, string filePath)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Verwendung: notebookTools <Datei> join <Zellen/code/markdown>");
                return;
            }

            Notebooks notebook = NotebookReader.LoadNotebook(filePath);
            List<int> zellNummern = new List<int>();
            string auswahl = args[2].ToLower();
            if (auswahl == "code" || auswahl == "markdown")
            {
                for (int index = 0; index < notebook.Cells.Count; index++)
                {
                    if (notebook.Cells[index].Type == auswahl)
                    {
                        zellNummern.Add(index + 1);
                    }
                }
            }
            else
            {
                foreach (string nummer in auswahl.Split(','))
                {
                    int zellNummer;
                    if (!int.TryParse(nummer, out zellNummer) || zellNummer < 1 || zellNummer > notebook.Cells.Count
                        || zellNummern.Contains(zellNummer))
                    {
                        Console.WriteLine("Bitte gib gültige Zellnummern ohne Wiederholungen an, getrennt durch Kommas.");
                        return;
                    }
                    zellNummern.Add(zellNummer);
                }
                zellNummern.Sort();
            }
            if (zellNummern.Count < 2)
            {
                Console.WriteLine("Zum Zusammenführen werden mindestens zwei Zellen benötigt.");
                return;
            }

            NotebookCell ersteZelle = notebook.Cells[zellNummern[0] - 1];
            string content = "";
            Dictionary<string, JsonElement> attachments = new Dictionary<string, JsonElement>();
            foreach (int nummer in zellNummern)
            {
                NotebookCell cell = notebook.Cells[nummer - 1];
                if (cell.Type != ersteZelle.Type)
                {
                    Console.WriteLine("Es können nur Zellen mit dem gleichen Typ zusammengeführt werden.");
                    return;
                }
                if (content != "" && !content.EndsWith("\n") && !content.EndsWith("\r"))
                {
                    content += Environment.NewLine;
                }
                content += string.Join("", cell.Source);

                // Eingebettete Bilder aus allen ausgewählten Zellen behalten.
                if (cell.AdditionalData.ContainsKey("attachments"))
                {
                    foreach (JsonProperty bild in cell.AdditionalData["attachments"].EnumerateObject())
                    {
                        if (attachments.ContainsKey(bild.Name)
                            && attachments[bild.Name].GetRawText() != bild.Value.GetRawText())
                        {
                            Console.WriteLine("Die Zellen enthalten unterschiedliche Bilder mit dem gleichen Namen. Es wurde nichts geändert.");
                            return;
                        }
                        attachments[bild.Name] = bild.Value;
                    }
                }
            }

            ersteZelle.Source.Clear();
            ersteZelle.Source.Add(content);
            ersteZelle.Outputs.Clear();
            ersteZelle.ExecutionCount = null;
            if (attachments.Count > 0)
            {
                ersteZelle.AdditionalData["attachments"] = JsonSerializer.SerializeToElement(attachments);
            }
            // Von hinten entfernen, damit die Zellnummern beim Löschen nicht verrutschen.
            for (int index = zellNummern.Count - 1; index > 0; index--)
            {
                notebook.Cells.RemoveAt(zellNummern[index] - 1);
            }
            CommandContent.Content(filePath, notebook);
            CommandSave.Save(notebook, filePath);
        }
    }
}
