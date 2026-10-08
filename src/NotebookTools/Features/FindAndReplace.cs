using System;
using NotebookTools.Notebook;

namespace NotebookTools.Features
{
    public class FindAndReplace
    {
        public static void FindAndReplaces(Notebooks notebook, string originalPath)
        {
            Console.Write("Suchtext eingeben: ");
            string searchText = Console.ReadLine() ?? "";
            if (string.IsNullOrEmpty(searchText))
            {
                Console.WriteLine("Kein Suchtext angegeben.");
                return;
            }

            Console.Write("Ersetzen durch: ");
            string? replaceText = Console.ReadLine();
            if (replaceText == null)
            {
                Console.WriteLine("Ersetzen abgebrochen.");
                return;
            }

            int changedCells = 0;
            foreach (NotebookCell cell in notebook.Cells)
            {
                if (cell.Source == null)
                {
                    continue;
                }

                // Textstücke zusammenfügen, damit auch Treffer zwischen zwei Stücken gefunden werden.
                string cellText = string.Join("", cell.Source);
                string newText = cellText.Replace(searchText, replaceText);
                if (newText != cellText)
                {
                    cell.Source.Clear();
                    cell.Source.Add(newText);
                    changedCells++;

                    // Alte Ergebnisse passen nicht mehr zum geänderten Code.
                    if (cell.Type == "code")
                    {
                        cell.Outputs.Clear();
                        cell.ExecutionCount = null;
                    }
                }
            }

            if (changedCells > 0)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{changedCells} Zelle(n) wurden geändert.");
                }
                finally
                {
                    Console.ForegroundColor = previousColor;
                }
                CommandSave.Save(notebook, originalPath);
            }
            else
            {
                Console.WriteLine("Es wurde nichts gespeichert, da kein Suchtext gefunden wurde oder der Ersatz unverändert ist.");
            }
        }
    }
}
