using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotebookTools.Features
{
    public class CommandShow
    {
        public static string ErmittleSourceText(string sCounter, string filePath)
        {
            int sourceCounter = 0;
            int klammerCounter = 0;
            int counter = Convert.ToInt32(sCounter);
            int sourcePosition = -1;
            int suchPosition = 0;
            string notebookText = File.ReadAllText (filePath);
            string textEinerSourceInstanz = "";
            bool ersteKlammer = false;
            bool innerhalbText = false;
            bool escapeZeichen = false;
            string errorOutOfRange = "Die eingegebene Zahl war außerhalb des möglichen Bereichs.";

            if (counter <= 0)
            {
                return errorOutOfRange;
            }


            while (sourceCounter < counter)
            {
                sourcePosition = notebookText.IndexOf("\"source\"", suchPosition);
                if (sourcePosition == -1)
                {
                    return errorOutOfRange;
                }
                sourceCounter++;
                suchPosition = sourcePosition + "\"source\"".Length;
            }


            for (int i = sourcePosition; i < notebookText.Length; i++)   //finde den text in source
            {
                char aktuellesZeichen = notebookText[i];

                // Klammern innerhalb eines JSON-Texts gehören zum Zellinhalt.
                if (ersteKlammer && aktuellesZeichen == '"' && !escapeZeichen)
                {
                    innerhalbText = !innerhalbText;
                }

                if (aktuellesZeichen == ']' && !innerhalbText)
                {
                    klammerCounter--;
                    if (klammerCounter == 0)
                    {
                                break;
                    }
                }

                if (ersteKlammer == true)
                {
                    textEinerSourceInstanz += aktuellesZeichen;
                }

                if (aktuellesZeichen == '[' && !innerhalbText)
                {
                    ersteKlammer = true;
                    klammerCounter++;
                }

                if (aktuellesZeichen == '\\' && !escapeZeichen)
                {
                    escapeZeichen = true;
                }
                else
                {
                    escapeZeichen = false;
                }
                        
            }
            // JSON-Anführungszeichen entfernen und zum Beispiel \n als Zeilenumbruch anzeigen.
            List<string>? sourceZeilen = JsonSerializer.Deserialize<List<string>>("[" + textEinerSourceInstanz + "]");
            if (sourceZeilen == null)
            {
                return "";
            }
            return string.Join("", sourceZeilen);
        }
    }
}

