using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookTools.Features
{
    public class CommandShow
    {
        public static string ReadText(string path)
        {
            int counter = 3;
            string json = File.ReadAllText(path);

            string sourceText = ErmittleSourceText(counter, json);
            return sourceText;

        }

        public static string ErmittleSourceText(int counter, string text)
        {
            string blockSourceText = "";
            int sourceCounter = 0;
            int sourcePosition = -1;
            int klammerCounter = 0;
            int anfang = 0;
            int ende = 0;
            char buchstabe = ' ';
            bool neueZeile = true;
            int suchPosition = 0;

            while (sourceCounter < counter)
            {
                sourcePosition = text.IndexOf("\"source\"", suchPosition);

                if (sourcePosition == -1)
                {
                    return "";
                }

                sourceCounter++;
                suchPosition = sourcePosition + "\"source\"".Length;
            }


            if (sourcePosition != -1)
            {
                anfang = text.IndexOf("[", sourcePosition) + 1;
                if (anfang == 0)
                {
                    return "";
                }
                klammerCounter++;
            }



            for (int i = anfang; i < text.Length; i++)
            {
                buchstabe = text[i];

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



                if (neueZeile == true)
                {
                    if (buchstabe == '"')
                    {
                        neueZeile = false;
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (buchstabe == '\\')
                {
                    char buchstabePlus1 = text[i + 1];

                    if (buchstabePlus1 == 'n')
                    {
                        i = i + 3;
                        blockSourceText += Environment.NewLine;
                        neueZeile = true;
                        continue;
                    }
                }


                blockSourceText += buchstabe;

            }
            return blockSourceText;
        }

    }
}

