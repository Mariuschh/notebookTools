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

            // Wandelt den eingegebenen relativen Pfad in einen vollständigen Pfad um.
            string filePath = Path.GetFullPath(args[0]);

            // Beendet das Programm, wenn die angegebene Datei nicht existiert.
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Die Datei wurde nicht gefunden: {filePath}");
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
                    case "show":
                        if(args.Length > 2)
                        {
                            string choosenSource = CommandShow.ErmittleSourceText(args[2], filePath);
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