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


    }
}
