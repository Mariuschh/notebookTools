using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using NotebookTools.Notebook;

namespace NotebookTools.Features
{
    public class CellEditor
    {
        public static string? OpenCell(string filePath, int zellNummer)
        {
            JsonObject? notebook = JsonNode.Parse(File.ReadAllText(filePath)) as JsonObject;
            if (notebook == null) throw new JsonException();
            JsonArray? cells = notebook["cells"] as JsonArray;
            if (cells == null || zellNummer < 1 || zellNummer > cells.Count)
            {
                Console.WriteLine("Die Zellnummer liegt außerhalb des möglichen Bereichs.");
                return null;
            }
            JsonObject? selectedCell = cells[zellNummer - 1] as JsonObject;
            if (selectedCell == null) throw new JsonException();
            JsonArray selectedCells = new JsonArray();
            selectedCells.Add(selectedCell.DeepClone());
            notebook["cells"] = selectedCells;

            return OpenNotebook(notebook);
        }

        public static string? OpenNewCell(Notebooks notebook, string type)
        {
            JsonObject newCell = new JsonObject();
            newCell["cell_type"] = type;
            newCell["id"] = Guid.NewGuid().ToString("N");
            newCell["metadata"] = new JsonObject();
            newCell["source"] = new JsonArray();
            if (type == "code")
            {
                newCell["outputs"] = new JsonArray();
                newCell["execution_count"] = null;
            }

            JsonArray cells = new JsonArray();
            cells.Add(newCell);
            JsonObject tempNotebook = new JsonObject();
            tempNotebook["cells"] = cells;
            tempNotebook["metadata"] = JsonSerializer.SerializeToNode(notebook.Metadata);
            tempNotebook["nbformat"] = 4;
            tempNotebook["nbformat_minor"] = 5;
            return OpenNotebook(tempNotebook);
        }

        private static string? OpenNotebook(JsonObject notebook)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "NotebookTools-" + Guid.NewGuid().ToString("N") + ".ipynb");
            try
            {
                NotebookWriter.SaveNotebook(tempPath, notebook);

                ProcessStartInfo editor = new ProcessStartInfo();
                editor.FileName = tempPath;
                editor.UseShellExecute = true;
                Process.Start(editor);

                Console.WriteLine("Bearbeite nur den Inhalt der Zelle und speichere die Datei im Editor.");
                Console.WriteLine("Schließe danach die Datei im Editor und drücke hier Enter. Mit 'abbrechen' kannst du abbrechen.");
                string? confirmation = Console.ReadLine();
                if (confirmation == null || confirmation.Trim() != "")
                {
                    Console.WriteLine("Bearbeitung abgebrochen.");
                    return null;
                }

                JsonObject? editedNotebook = JsonNode.Parse(File.ReadAllText(tempPath)) as JsonObject;
                JsonArray? editedCells = editedNotebook?["cells"] as JsonArray;
                if (editedCells == null || editedCells.Count != 1)
                {
                    Console.WriteLine("Die temporäre Datei muss genau eine Zelle enthalten.");
                    return null;
                }
                JsonObject? editedCell = editedCells[0] as JsonObject;
                if (editedCell == null)
                {
                    Console.WriteLine("Die temporäre Datei enthält keine gültige Zelle.");
                    return null;
                }

                // Nur den Inhalt übernehmen, nicht Zelltyp oder andere Angaben.
                JsonNode? source = editedCell["source"];
                if (source is JsonArray)
                {
                    string content = "";
                    foreach (JsonNode? line in (JsonArray)source)
                    {
                        if (line == null) throw new JsonException();
                        content += line.GetValue<string>();
                    }
                    return content;
                }
                if (source is JsonValue)
                {
                    return source.GetValue<string>();
                }
                Console.WriteLine("Die bearbeitete Zelle enthält keinen gültigen Inhalt.");
                return null;
            }
            finally
            {
                try
                {
                    File.Delete(tempPath);
                }
                catch (IOException)
                {
                    Console.WriteLine("Die temporäre Datei konnte nicht gelöscht werden: " + tempPath);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Die temporäre Datei konnte nicht gelöscht werden: " + tempPath);
                }
            }
        }
    }
}
