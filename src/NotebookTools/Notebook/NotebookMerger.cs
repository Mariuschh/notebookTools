using System.Text.Json.Nodes;

namespace NotebookTools.Notebook
{
    public class NotebookMerger
    {
        public static JsonObject Merge(List<string> filePaths, List<List<int>> cellNumbers)
        {
            JsonObject mergedNotebook = new JsonObject();
            JsonArray mergedCells = new JsonArray();
            int mergedMinorFormat = 5;

            for (int notebookNumber = 0; notebookNumber < filePaths.Count; notebookNumber++)
            {
                string notebookText = File.ReadAllText(filePaths[notebookNumber]);
                JsonObject? notebook = JsonNode.Parse(notebookText) as JsonObject;
                if (notebook == null)
                {
                    throw new ArgumentException("Ungültiges Notebook: " + filePaths[notebookNumber]);
                }
                JsonArray? cells = notebook["cells"] as JsonArray;
                if (cells == null)
                {
                    throw new ArgumentException("Das Notebook enthält keine gültige Zellliste.");
                }
                JsonNode? format = notebook["nbformat"];
                if (format == null || format.GetValue<int>() != 4)
                {
                    throw new ArgumentException("Es werden nur Notebooks im Format 4 unterstützt.");
                }

                if (notebookNumber == 0)
                {
                    // Metadaten und weitere Notebook-Angaben aus der ersten Datei übernehmen.
                    mergedNotebook = (JsonObject)notebook.DeepClone();
                    mergedNotebook["cells"] = mergedCells;
                }
                JsonNode? minorFormat = notebook["nbformat_minor"];
                if (minorFormat != null)
                {
                    int notebookMinorFormat = minorFormat.GetValue<int>();
                    if (notebookMinorFormat > mergedMinorFormat)
                    {
                        mergedMinorFormat = notebookMinorFormat;
                    }
                }

                foreach (int cellNumber in cellNumbers[notebookNumber])
                {
                    if (cellNumber < 1 || cellNumber > cells.Count)
                    {
                        throw new ArgumentException("Ungültige Zellnummer " + cellNumber + " in " + filePaths[notebookNumber]);
                    }
                    // Zellnummern beginnen bei 1, die Liste beginnt bei 0.
                    JsonObject? selectedCell = cells[cellNumber - 1] as JsonObject;
                    if (selectedCell == null)
                    {
                        throw new ArgumentException("Ungültige Zelle in " + filePaths[notebookNumber]);
                    }
                    // Die gesamte Zelle kopieren, einschließlich Ausgaben und eingebetteter Bilder.
                    JsonObject copiedCell = (JsonObject)selectedCell.DeepClone();
                    // Jede Kopie bekommt eine eigene ID, auch bei mehrfach ausgewählten Zellen.
                    copiedCell["id"] = Guid.NewGuid().ToString("N");
                    mergedCells.Add(copiedCell);
                }
            }
            // Ab Format 4.5 werden Zell-IDs unterstützt.
            mergedNotebook["nbformat_minor"] = mergedMinorFormat;
            return mergedNotebook;
        }
    }
}
