using NotebookTools.Help;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.ComponentModel.Design;


namespace NotebookTools
{
    public class Program
    {
        static void Main(string[] args)
        {
            string text = "";
         
            text = ReadText("D:\\Berufsschule\\BFK-S\\Neuer Ordner\\arbeitsblatt-sequenz.ipynb");

            Console.WriteLine(text);
        }
        
    
    public static string ReadText (string path)
        {
            int counter = 1;
            string json = File.ReadAllText(path);
                 
                    string sourceText = ErmittleSourceText(counter, json);
                    return sourceText;
            
        }

        public static string ErmittleSourceText(int counter, string text)
        {
            string blockSourceText = "";
            int sourceCounter = 0;
            int sourcePosition = 0;
            int klammerCounter = 0;
            int anfang = 0;
            int ende = 0;
            char buchstabe = ' ';



                sourceCounter++;
                if(sourceCounter == counter)
                {
                     sourcePosition = text.IndexOf("\"source\"");
                }


                
            if (sourcePosition != -1)
            {
                anfang = text.IndexOf("[", sourcePosition) + 1;
                if(anfang == 0)
                {
                    return "";
                }
                klammerCounter++;

                for (int i = anfang; i < text.Length; i++)
                    {
                        buchstabe = text[i];
                    if (buchstabe == '\\')
                    {
                        char buchstabePlus1 = text[i + 1];
                        
                        if (buchstabePlus1 == 'n')
                        {
                            //buchstabe = ' ';
                            i = i + 4;
                            blockSourceText += Environment.NewLine;
                            continue;
                        }
                    }
                        if (buchstabe == ']')
                        {
                            klammerCounter--;
                        }

                        else if (buchstabe == '[')
                        {
                            klammerCounter++;
                        }

                        if (klammerCounter == 0)
                        {
                            ende = text[i];
                            break;
                        }

                    blockSourceText += buchstabe;
                }
            }
            return blockSourceText;
        }

}
}
