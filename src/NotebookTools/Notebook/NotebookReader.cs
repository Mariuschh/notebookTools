using System.Text.Json;

namespace NotebookTools.Notebook;

public class NotebookReader
{
    public static string FilePath = @"C:\Users\Ziegler\OneDrive - GTÜ mbH\GTÜ\Berufsschule\BfK-S\NotebookTools\notebookTools\tests\NotebookTools.Tests\TestData\example.ipynb";
    public static List<Notebook> LoadCells(string filePath)
    {
        FilePath = filePath;
        string jsonString = File.ReadAllText(filePath);
            
        Notebook notebook = JsonSerializer.Deserialize<Notebook>(jsonString)!;

        return new List<Notebook>();
    }
}