using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotebookTools.Features
{
    internal class CommandContent
    {
        public static void Content (string filePath)
        {
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
