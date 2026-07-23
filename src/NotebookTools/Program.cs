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

            //Methode aufrufen als Test 
            HelpCommand.GetHelpText();
        }
    }
}