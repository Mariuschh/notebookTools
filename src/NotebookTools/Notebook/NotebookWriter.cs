using System.Text.Json;
using System.Text.Json.Nodes;

namespace NotebookTools.Notebook
{
    public class NotebookWriter
    {
        public static void SaveNotebook(string filePath, Notebooks notebook)
        {
            JsonObject? notebookData = JsonSerializer.SerializeToNode(notebook) as JsonObject;
            if (notebookData == null) throw new JsonException("Ungültiges Notebook.");
            JsonArray? cells = notebookData["cells"] as JsonArray;
            if (cells == null) throw new JsonException("Ungültige Zellliste.");

            foreach (JsonNode? cell in cells)
            {
                JsonObject? cellData = cell as JsonObject;
                if (cellData == null) throw new JsonException("Ungültige Zelle.");
                if (cellData["id"] == null)
                {
                    cellData["id"] = Guid.NewGuid().ToString("N");
                }
                // Nur Code-Zellen haben Ausgaben und einen Ausführungszähler.
                JsonNode? cellType = cellData["cell_type"];
                if (cellType == null || cellType.GetValue<string>() != "code")
                {
                    cellData.Remove("outputs");
                    cellData.Remove("execution_count");
                }
            }
            if (notebook.MinorFormat < 5)
            {
                notebookData["nbformat_minor"] = 5;
            }
            SaveNotebook(filePath, notebookData);
        }

        public static void SaveNotebook(string filePath, JsonObject notebook)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            string notebookText = notebook.ToJsonString(options);

            // CreateNew verhindert, dass vorhandene Dateien überschrieben werden.
            using (FileStream file = new FileStream(filePath, FileMode.CreateNew))
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.Write(notebookText);
            }
        }
    }
}
