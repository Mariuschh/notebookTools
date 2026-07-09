using System.Text.Json.Serialization;

namespace NotebookTools.Notebook;

public class Notebook
{
    [JsonPropertyName("cells")]
    public List<NotebookCell> Cells { get; set; } = new();
    [JsonPropertyName("metadata")]
    public Dictionary<string, object> Metadata { get; set; } = new();
    [JsonPropertyName("nbformat")]
    public int Format { get; set; }
    [JsonPropertyName("nbformat_minor")]
    public int MinorFormat { get; set; }
}