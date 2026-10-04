using System.Text.Json.Serialization;

namespace NotebookTools.Notebook;

/// Stellt eine einzelne Zelle innerhalb eines Jupyter Notebooks dar.
public class NotebookCell
{
    /// Gibt den Typ der Zelle an.
    [JsonPropertyName("cell_type")]
    public string Type { get; set; } = "";
    
    /// Enthält den eigentlichen Inhalt der Zelle.
    /// Der Inhalt wird im Notebook-Format als Liste einzelner Textabschnitte
    /// oder Zeilen gespeichert.
    /// Der vollständige Zellinhalt kann beispielsweise mit
    /// string.Join("", cell.Source) zusammengesetzt werden.
    [JsonPropertyName("source")]
    public List<string> Source { get; set; } = new();
    
    /// Enthält die Ausgaben einer Code-Zelle, beispielsweise Textausgaben,
    /// Fehlermeldungen oder berechnete Ergebnisse.
    /// Bei Markdown Zellen ist diese Liste normalerweise leer.
    [JsonPropertyName("outputs")]
    public List<object> Outputs { get; set; } = new();
    
    /// Enthält zusätzliche Informationen zur Zelle.
    /// Welche Werte enthalten sind, kann je nach Notebook und Zelltyp variieren.
    [JsonPropertyName("metadata")]
    public Dictionary<string, object> Metadata { get; set; } = new();
    
    /// Gibt an, an welcher Stelle eine Code-Zelle ausgeführt wurde.
    /// Der Wert kann null sein, wenn die Zelle noch nicht ausgeführt wurde
    /// oder wenn es sich nicht um eine Code-Zelle handelt.
    [JsonPropertyName("execution_count")]
    public int? ExecutionCount { get; set; }

    // Zum Beispiel Zell-IDs und eingebettete Bilder behalten.
    [JsonExtensionData]
    public Dictionary<string, System.Text.Json.JsonElement> AdditionalData { get; set; } = new();
    
}
