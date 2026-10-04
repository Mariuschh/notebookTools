using System.Text.Json;
using System.Text.Json.Nodes;

namespace NotebookTools.Notebook
{
    public class NotebookWriter
    {
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
