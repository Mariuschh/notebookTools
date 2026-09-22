using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string errorOutOfRange = "Die eingegebene Zahl war außerhalb des möglichen Bereichs.";



            while (sourceCounter < counter)
            {
                sourcePosition = notebookText.IndexOf("\"source\"", suchPosition);
                if (sourcePosition == -1)
                {
                    return "";
                }
                sourceCounter++;
                suchPosition = sourcePosition + "\"source\"".Length;
            }


            for (int i = sourcePosition; i < notebookText.Length; i++)                          //finde den text in source
                    {
                        char aktuellesZeichen = notebookText[i];

                        if (aktuellesZeichen == ']')
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

                        if (aktuellesZeichen == '[')
                        {
                            ersteKlammer = true;
                            klammerCounter++;
                        }
                        
                    }
            //Console.WriteLine (textEinerSourceInstanz);
            return textEinerSourceInstanz;
        }
        
        //New Class "Edit"
        // public static string ZelleBearbeiten (int counter, string notebook)
        // {
        //     int zellNummer = 0;
        //     int zellIndex = 0;
        //     if (zellNummer > 0)
        //     {
        //         zellIndex = zellNummer - 1;
        //     }
        //
        //     var cell = text.Cells[zellIndex];
        //     
        //     return "";
        // }
    }
}

