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

               Zellen bearbeiten, löschen oder verschieben:
                 notebookTools beispiel.ipynb edit 2
                 notebookTools beispiel.ipynb delete 2
                 notebookTools beispiel.ipynb move 2 4
                 notebookTools beispiel.ipynb add
               Bei add wählst du den Zelltyp im Terminal und schreibst den Inhalt im Editor.
               Speichere und schließe die temporäre Datei und bestätige mit Enter. Die Zelle wird am Ende angefügt.
               Bei edit öffnet sich eine temporäre Notebook-Datei mit der ausgewählten Zelle.
               Speichere und schließe sie im Editor und bestätige danach mit Enter im Terminal.
               Nur der Zellinhalt wird übernommen. Die temporäre Datei muss genau eine Zelle enthalten.
               Bei move ist die letzte Zahl die neue Position der Zelle. Zellnummern beginnen bei 1.
               Die Änderungen werden angezeigt, aber noch nicht gespeichert und gehen beim Beenden verloren.
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
