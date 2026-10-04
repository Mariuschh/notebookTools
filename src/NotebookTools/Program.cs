using System.ComponentModel.Design;
using System.Text.Json;
using NotebookTools.Features;
using NotebookTools.Help;
using NotebookTools.Notebook;


namespace NotebookTools
{
    class Program
    {
        static void Main(string[] args)
        {

            // Prüft, ob beim Programmstart ein Dateipfad übergeben wurde.
            if (args.Length == 0)
            {
                Console.WriteLine("Bitte gib eine Notebook-Datei an.");
                return;
            }

            string filePath;
            int searchExitCode = RunFileSearch.Run(args, out filePath);
            if (searchExitCode != 0)
            {
                Environment.ExitCode = searchExitCode;
                return;
            }

            if (args.Length == 1)
            {
                return;
            }

            // Liest die Notebook-Datei ein und speichert sie als Notebook-Objekt.
            Notebook.Notebook notebook = NotebookReader.LoadNotebook(filePath);




            // Methoden aufrufe
            string errorEingabe = "Problem bei der Eingabe.";
            if (args.Length > 1)   
            {
                switch (args[1].ToLower())
                {
                    case "content":
                        CommandContent.Content(filePath, notebook);
                        break;

                    case "show":
                        if(args.Length > 2)
                        {
                            string notebookText = File.ReadAllText(filePath);
                            string choosenSource = CommandShow.ErmittleSourceText(args[2], notebookText);
                            Console.WriteLine(choosenSource);
                            break;
                        }
                        else 
                        {
                            Console.WriteLine(errorEingabe);
                            break;
                        }

                    case "help":
                        string helpText = HelpCommand.GetHelpText();
                        Console.WriteLine(helpText);
                        break;
                        
                }
            }
            else
            {
                Console.WriteLine(errorEingabe);
            }
        }

    }
}
