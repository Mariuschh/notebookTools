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

            // Ohne Argumente oder bei help direkt die Hilfe anzeigen.
            if (args.Length == 0 || args[0].ToLower() == "help" || args[0].ToLower() == "--help")
            {
                Console.WriteLine(HelpCommand.GetHelpText());
                return;
            }
            
            string filePath = args[0];
            if (args.Length == 1 || args[1].ToLower() != "merge")
            {
                int searchExitCode = RunFileSearch.Search(args, out filePath);
                if (searchExitCode != 0)
                {
                    return;
                }
            }

            if (args.Length == 1)
            {
                return;
            }

            // Methoden aufrufe
            string errorEingabe = "Problem bei der Eingabe.";
            if (args.Length > 1)
            {
                switch (args[1].ToLower())
                {
                    case "content":
                        Notebook.Notebooks notebook = NotebookReader.LoadNotebook(filePath);
                        CommandContent.Content(filePath, notebook);
                        break;

                    case "merge":
                        CommandMerge.Merge(args);
                        break;

                    case "edit":
                        CommandEdit.Edit(args, filePath);
                        break;

                    case "delete":
                        CommandDelete.Delete(args, filePath);
                        break;

                    case "move":
                        CommandMove.Move(args, filePath);
                        break;

                    case "type":
                        CommandType.Type(args, filePath);
                        break;

                    case "split":
                        CommandSplit.Split(args, filePath);
                        break;

                    case "join":
                        CommandJoin.Join(args, filePath);
                        break;

                    case "export":
                        CommandExport.Export(args, filePath);
                        break;

                    case "add":
                        if (args.Length != 2 && args.Length != 3)
                        {
                            Console.WriteLine("Verwendung: notebookTools <Datei> add [Textdatei]");
                            break;
                        }
                        Notebooks addNotebook = NotebookReader.LoadNotebook(filePath);
                        int bisherigeZellAnzahl = addNotebook.Cells.Count;
                        string? textFile = null;
                        if (args.Length == 3)
                        {
                            textFile = args[2];
                        }
                        CommandAddCell.AddCell(addNotebook, textFile);
                        if (addNotebook.Cells.Count == bisherigeZellAnzahl)
                        {
                            break;
                        }
                        CommandContent.Content(filePath, addNotebook);
                        CommandSave.Save(addNotebook, filePath);
                        break;

                    case "show":
                        if (args.Length != 3)
                        {
                            Console.WriteLine(
                                "Verwendung: NotebookTools <Datei> show <Zellennummer>");
                            break;
                        }

                        if (!int.TryParse(args[2], out int cellNumber))
                        {
                            Console.WriteLine("Bitte gib eine Zahl nach 'show' ein.");
                            break;
                        }

                        if (cellNumber <= 0)
                        {
                            Console.WriteLine("Bitte gib eine Zahl größer als 0 ein.");
                            break;
                        }

                        string choosenSource =
                            CommandShow.ErmittleSourceText(args[2], filePath);

                        Console.WriteLine(choosenSource);
                        break;

                    case "help":
                    case "--help":
                        string helpText = HelpCommand.GetHelpText();
                        Console.WriteLine(helpText);
                        break;

                    case "findandreplace":
                        Notebooks findNotebook = NotebookReader.LoadNotebook(filePath);
                        FindAndReplace.FindAndReplaces(findNotebook, filePath);
                        break;

                    default:
                        Console.WriteLine("Unbekannter Befehl: " + args[1] + ". Mit 'notebookTools help' siehst du die verfügbaren Befehle.");
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
