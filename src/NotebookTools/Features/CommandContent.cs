using NotebookTools.Notebook;
using System.Text.Json;

namespace NotebookTools.Features
{
    public class CommandContent
    {
        public static void Content (string filePath, Notebook.Notebook notebook)
        {
            int nummer = 1;

            foreach (NotebookCell cell in notebook.Cells)    // Geht alle Zellen im Notebook durch
            {
                string type = cell.Type; // Typ der Zelle (code oder markdown)

                string content = cell.Source.ToString(); // Inhalt der Zelle

                if (type == "code") // Ausgabe je nach Zelltyp
                {
                    Console.WriteLine($"{nummer} | code     | {content}");
                }
                else
                {
                    Console.WriteLine($"{nummer} | markdown | {content}");
                }

                nummer++; // Nummer für die nächste Zelle erhöhen

            }

        }

    }
}
