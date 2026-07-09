using System.Text.Json.Serialization;

namespace NotebookTools.Notebook;

public class NotebookCell
{
    [JsonPropertyName("cell_type")]
    public string Type { get; set; } = "";
    
    [JsonPropertyName("source")]
    public List<string> Source { get; set; } = new();
    
    [JsonPropertyName("outputs")]
    public List<object> Outputs { get; set; } = new();
    
    [JsonPropertyName("metadata")]
    public Dictionary<string, object> Metadata { get; set; } = new();
    
    [JsonPropertyName("execution_count")]
    public int? ExecutionCount { get; set; }
    
}