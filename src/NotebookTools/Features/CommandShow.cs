using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookTools.Features
{
    public class CommandShow
    {
        public static string ErmittleSourceText(int counter, string text)
        {
            int sourceCounter = 0;
            int sourcePosition = -1;
            int suchPosition = 0;


            
            int klammerCounter = 0;             //neue vars
            string textEinerSourceInstanz = "";
            bool ersteKlammer = false;



            while (sourceCounter < counter)
            {
                sourcePosition = text.IndexOf("\"source\"", suchPosition);

                if (sourcePosition == -1)
                {
                    return "";
                }



                
                    for (int i = sourcePosition; i < text.Length; i++)                          //finde den text in source
                    {
                        char aktuellesZeichen = text[i];

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
               






                sourceCounter++;
                suchPosition = sourcePosition + "\"source\"".Length;
            }




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

