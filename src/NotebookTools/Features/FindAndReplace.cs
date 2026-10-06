using System;
using NotebookTools.Notebook;

namespace NotebookTools.Features
{
    public class FindAndReplace
    {
        public static void FindAndReplaces(Notebooks notebook, string originalPath)
        {
             
            Console.Write("Suchtext eingeben: "); string searchText = Console.ReadLine() ?? "";  // Suchtext vom Benutzer abfragen

             
            Console.Write("Ersetzen durch: "); string replaceText = Console.ReadLine() ?? ""; //  durch den der Suchtext ersetzt werden soll
            Console.ForegroundColor = ConsoleColor.Green;

            if (string.IsNullOrEmpty(searchText))
            {
                Console.WriteLine("Kein Suchtext angegeben."); // Abbrechen, wenn kein Suchtext eingegeben wurde
                return;
            }

            
            int replacements = 0;  // Anzahl der geänderten Zeilen

            
            foreach (var cell in notebook.Cells)  // Alle Zellen des Notebooks durchsuchen
            {
                 
                if (cell.Source == null) // Zellen ohne Inhalt überspringen
                    continue;

                 
                for (int i = 0; i < cell.Source.Count; i++) // Alle Zeilen der aktuellen Zelle durchsuchen
                {
                    string line = cell.Source[i] ?? "";

                     
                    if (line.Contains(searchText)) // Prüfen, ob der Suchtext in der Zeile vorkommt
                    {
                         
                        cell.Source[i] = line.Replace(searchText, replaceText); // Suchtext durch den neuen Text ersetzen

                        
                        replacements++;  // Zähler erhöhen
                    }
                }
            }

             
            Console.WriteLine($"{replacements} Zeile(n) wurden geändert."); // Anzahl der geänderten Zeilen ausgeben

            
            if (replacements > 0)  // Notebook speichern, wenn Änderungen vorgenommen wurden
            {
                CommandSave.Save(notebook, originalPath);
            }
            else
            {
                Console.WriteLine("Es wurde nichts gespeichert, da kein Suchtext gefunden wurde.");
            }
        }
    }
}