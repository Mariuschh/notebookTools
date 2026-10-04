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
                   _   __      __       __                __  ______            __
                  / | / /___  / /____  / /_  ____  ____  / /_/_  __/___  ____  / /____
                 /  |/ / __ \/ __/ _ \/ __ \/ __ \/ __ \/ //_/ / / / __ \/ __ \/ / ___/
                / /|  / /_/ / /_/  __/ /_/ / /_/ / /_/ / ,<   / / / /_/ / /_/ / (__  )
               /_/ |_/\____/\__/\___/_.___/\____/\____/_/|_| /_/  \____/\____/_/____/
               \_\  \_\____\___\____\_____\_____\_____\_\ \_\ \_\   \____\_____\_\____\

               Verwendung:
               ----------------------------------------------------------------------
                 notebookTools <Datei> <Befehl> [Argumente]

               Befehle:
               ----------------------------------------------------------------------
                 content             Kurze Übersicht aller Zellen anzeigen
                 show <Zelle>        Inhalt einer Zelle anzeigen
                 edit <Zelle>        Zellinhalt im Editor bearbeiten
                 add                 Neue Zelle am Ende hinzufügen
                 delete <Zelle>      Zelle löschen
                 move <Zelle> <Ziel> Zelle an die Zielposition verschieben
                 help                Diese Hilfe anzeigen

               Beispiele:
               ----------------------------------------------------------------------
                 notebookTools beispiel content
                 notebookTools beispiel edit 2
                 notebookTools beispiel move 2 4

               Notebooks zusammenführen:
               ----------------------------------------------------------------------
                 notebookTools <Zieldatei> merge <Notebook1> <Zellen> <Notebook2> <Zellen> ...
                 notebookTools zusammen merge erstes 1,3 zweites 2

                 Zellnummern mit Kommas trennen. Die angegebene Reihenfolge bleibt erhalten.
                 Die Zieldatei darf noch nicht existieren.

               Bearbeiten im Editor:
               ----------------------------------------------------------------------
                 edit und add öffnen eine temporäre Datei mit genau einer Zelle.
                 Bei add wählst du vorher den Zelltyp im Terminal (code oder markdown).
                 Im Editor speichern und schließen, danach im Terminal Enter drücken.
                 Es wird nur der Zellinhalt übernommen. Mit 'abbrechen' kannst du abbrechen.

               Änderungen speichern:
               ----------------------------------------------------------------------
                 Nach edit, add, delete und move wirst du nach der Zieldatei gefragt.
                 Enter: als <Dateiname>_bearbeitet.ipynb neben dem Original speichern.
                 Eigener Dateipfad: an einem anderen Ort oder unter anderem Namen speichern.
                 Die Endung .ipynb wird auch beim Speichern automatisch ergänzt.
                 'abbrechen': Änderungen verwerfen.
                 Vorhandene Dateien werden nicht überschrieben. Das Original bleibt erhalten.

               Hinweise:
               ----------------------------------------------------------------------
                 Zellnummern und Zielpositionen beginnen bei 1.
                 Gib nur den Dateinamen an. Eine fehlende .ipynb-Endung wird automatisch ergänzt.
                 Bereits vorhandene .ipynb-Endungen bleiben unverändert.
                 Pfade mit Leerzeichen in Anführungszeichen setzen.
                 Ohne Befehl wird die Datei gesucht: notebookTools beispiel
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
