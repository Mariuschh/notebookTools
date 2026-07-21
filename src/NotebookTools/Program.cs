using System.Text.Json;
using NotebookTools.Notebook;

namespace NotebookTools
{
    public class Program
    {
        static void Main(string[] args)
        {
        string filePath = @"C:\Users\Ziegler\OneDrive - GTÜ mbH\GTÜ\Berufsschule\BfK-S\NotebookTools\notebookTools\tests\NotebookTools.Tests\TestData\example.ipynb";
        
            List<NotebookCell> cells = NotebookReader.LoadNotebook(filePath).Cells;
            
            Notebook.Notebook notebook = NotebookReader.LoadNotebook(filePath);

        }
    }
}
