using System.Text.Json.Serialization;

namespace NotebookTools.Notebook;

/// Stellt ein vollständiges Jupyter Notebook dar.
/// Das Objekt enthält die einzelnen Notebook-Zellen sowie allgemeine
/// Informationen zum Notebook-Format.
public class Notebook
{
    /// Enthält alle Zellen des Notebooks in ihrer ursprünglichen Reihenfolge.
    /// Über diese Liste können Code und Markdown Zellen verarbeitet werden.
    [JsonPropertyName("cells")]
    public List<NotebookCell> Cells { get; set; } = new();
    
    /// Enthält allgemeine Metadaten des Notebooks,
    /// beispielsweise Informationen zum verwendeten Kernel.
    [JsonPropertyName("metadata")]
    public Dictionary<string, object> Metadata { get; set; } = new();
    
    /// Hauptversion des verwendeten Jupyter-Notebook-Formats.
    [JsonPropertyName("nbformat")]
    public int Format { get; set; }
    
    /// Unterversion des verwendeten Jupyter-Notebook-Formats.
    [JsonPropertyName("nbformat_minor")]
    public int MinorFormat { get; set; }
}