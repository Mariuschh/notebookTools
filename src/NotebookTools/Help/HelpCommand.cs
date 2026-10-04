using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookTools.Help
{
    public class HelpCommand
    {
        public static string GetText()
        {
            return """
               NotebookTools

               Commands:
                 help
                 content <file.ipynb>
                 show <file.ipynb> <cellNumber>

               Mit merge übernimmst du ausgewählte Zellen aus mehreren Notebooks in eine neue Datei:
                 notebookTools zusammen.ipynb merge erstes.ipynb 1,3 zweites.ipynb 2
               Gib nach jedem Notebook die gewünschten Zellnummern mit Kommas getrennt an.
               Die Nummerierung beginnt bei 1. Die Zellen werden in der angegebenen Reihenfolge übernommen.
               Die Zieldatei darf noch nicht existieren.
               """;
        }
        public static string GetHelpText() //Methode ertsellen help,aufruf der helptext 
        {
            string helpText = GetText();
            return helpText;
        }
    }
    //comnad help methode das uafgerufen wird dann dort error catching wenn help nicht richtig das befehl nixht richtig ist für jedes command 
    // user muss immer wissen was das problem geben zsm notebook ist leer 
    //Methode erstellen 1 
}
