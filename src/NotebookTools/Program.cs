using System.Text.Json;
using NotebookTools.Notebook;

namespace NotebookTools
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<Notebook.Notebook> cells = NotebookReader.LoadCells(NotebookReader.FilePath);

            foreach (Notebook.Notebook cell in cells)
            {
                Console.WriteLine(cell.Type);
                Console.WriteLine(cell.Source);
            }
        }
    }
}
