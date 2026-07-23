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
