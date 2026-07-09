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
            string json = File.ReadAllText( @"D:\Berufschule\2. BfK-S Projekt\notebookTools\tests\NotebookTools.Tests\TestData\example.ipynb");

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
                )       .Replace("\n", " ");

                if (typ == "code")          // Das die | gleichen abstand haben und gleich anfangen 
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