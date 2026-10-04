using NotebookTools.Notebook;
using System.Text.Json;

namespace NotebookTools.Features
{
    public class CommandContent
    {
        public static void Content (string filePath, Notebook.Notebooks notebook)
        {
            int nummer = 1;
            int maximaleZeichen = 80;

            foreach (NotebookCell cell in notebook.Cells)    // Geht alle Zellen im Notebook durch
            {
                string type = cell.Type; // Typ der Zelle (code oder markdown)

                string content = string.Join("", cell.Source); // Inhalt der Zelle
                content = content.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");
                content = content.Trim();

                if (content.Length > maximaleZeichen)
                {
                    content = content.Substring(0, maximaleZeichen) + "...";
                }

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
