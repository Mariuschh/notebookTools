using NotebookTools.Help;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;


namespace NotebookTools
{
    public class Program
    {
        static void Main(string[] args)
        {
            string text = "";
            if (args.Length == 0 || args[0] == "help" || args[0] == "--help")
            {
                Console.WriteLine(HelpText.GetText());
                return;
            }

            text = ReadText("Jsondatei.txt");
            Console.WriteLine(text);
        }
        
    
    public static string ReadText (string path)
        {
            int counter = 0;
            string json = File.ReadAllText (path);
            string[] jsongesplittet = json.Split(';');

            for( int i = 0; i < jsongesplittet.Length; i++)
            {
                if (jsongesplittet[i] == "source")
                {
                    counter++;
                                       
                    string sourceText = ErmittleSourceText(counter, json);
                  Console.WriteLine (sourceText);
                    return sourceText;
                }
            }
            return "";
        }

        public static string ErmittleSourceText(int counter, string text)
        {
            string blockSourceText = "";
            int sourceCounter = 0;
            int klammerCounter = 0;
            int anfang = 0;
            int ende = 0;

            if (text == "source")
            {
                if (text == "[")
                {
                    anfang = text.IndexOf("[");
                    klammerCounter++;
                }
                else if (text == "]")
                {
                    ende = text.IndexOf("]");
                    klammerCounter--;
                }
                blockSourceText = text.Substring(anfang, ende - anfang);
            }

            return blockSourceText;
        }

}
}
