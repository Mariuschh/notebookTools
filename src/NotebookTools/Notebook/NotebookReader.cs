using System.Text.Json;

namespace NotebookTools.Notebook;

/// Liest eine Jupyter-Notebook-Datei ein und wandelt den JSON-Inhalt
/// in ein Notebook-Objekt um.
public class NotebookReader
{
    /// Liest eine .ipynb-Datei vom angegebenen Dateipfad ein.
    /// Gibt ein Notebook-Objekt mit allen enthaltenen Zellen und Metadaten zurück.
    public static Notebook LoadNotebook(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException(
                "Es wurde kein Dateipfad angegeben.",
                nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "Die Notebook-Datei wurde nicht gefunden.",
                filePath);
        }

        string jsonString = File.ReadAllText(filePath);

        Notebook? notebook =
            JsonSerializer.Deserialize<Notebook>(jsonString);

        if (notebook is null)
        {
            throw new InvalidDataException(
                "Die Notebook-Datei konnte nicht eingelesen werden.");
        }

        return notebook;
    }
}