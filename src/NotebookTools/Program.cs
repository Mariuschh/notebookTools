using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace NotebookTools
{
    class Program
    {
        static void Main(string[] args)
        {
                                                              
            string ordner = Directory.GetCurrentDirectory(); // Ordner von dem aus das Programm gestartet wurde

            Console.WriteLine($"Aktueller Ordner: {ordner}");

          
            Console.Write("Bitte Namen der .ipynb-Datei eingeben: ");// Benutzer gibt Dateiname ein 
            string dateiname = Console.ReadLine();

            
            string pfad = Path.Combine(ordner, dateiname); // Zusammensetzen des Pfades 

           
            if (!File.Exists(pfad)) // Prüfen ob Datei überhaupt existiert 
            {
                Console.WriteLine($"Fehler: Datei nicht gefunden unter '{pfad}'.");
                return;
            }

            string json = File.ReadAllText(pfad);

            using JsonDocument doc = JsonDocument.Parse(json);

            JsonElement cells = doc.RootElement.GetProperty("cells");

            int nummer = 1;

            foreach (JsonElement cell in cells.EnumerateArray())
            {
                string typ = cell.GetProperty("cell_type").GetString();

                string inhalt = string.Concat(
                    cell.GetProperty("source")
                        .EnumerateArray()
                        .Select(x => x.GetString())
                ).Replace("\n", " ");

                if (typ == "code")
                {
                    Console.WriteLine($"{nummer} | code     | {inhalt}");
                }
                else
                {
                    Console.WriteLine($"{nummer} | markdown | {inhalt}");
                }

                nummer++;
            }
        }
    }
}