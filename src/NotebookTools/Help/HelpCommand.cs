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
                 add [Textdatei]     Neue Zelle im Editor oder aus einer Datei hinzufügen
                 export <Format>     Zellinhalte als txt, html oder docx exportieren
                 delete <Zelle>      Zelle löschen
                 move <Zelle> <Ziel> Zelle an die Zielposition verschieben
                 type <Zelle> <Typ>  Zelltyp auf code oder markdown ändern
                 split <Zelle> <Zeile> Zelle nach der angegebenen Textzeile aufteilen
                 join <Auswahl>      Zellen gleichen Typs zusammenführen
                 findandreplace      Text in allen Zellen suchen und ersetzen
                 help                Diese Hilfe anzeigen
                 --help              Diese Hilfe anzeigen

               Beispiele:
               ----------------------------------------------------------------------
                 notebookTools beispiel content
                 notebookTools beispiel edit 2
                 notebookTools beispiel move 2 4
                 notebookTools beispiel type 2 markdown
                 notebookTools beispiel split 2 5
                 notebookTools beispiel join 1,3,5
                 notebookTools beispiel join code
                 notebookTools beispiel add "aufgabe.txt"
                 notebookTools beispiel export html
                 notebookTools beispiel findandreplace

                 Bei findandreplace gibst du Suchtext und Ersatz im Terminal ein.
                 Groß- und Kleinschreibung wird beachtet. Ein leerer Ersatz entfernt den Suchtext.

               Notebooks zusammenführen:
               ----------------------------------------------------------------------
                 notebookTools <Zieldatei> merge <Notebook1> <Zellen> <Notebook2> <Zellen> ...
                 notebookTools zusammen merge erstes 1,3 zweites 2

                 Zellnummern mit Kommas trennen. Die angegebene Reihenfolge bleibt erhalten.
                 Die Zieldatei darf noch nicht existieren.

               Exportieren:
               ----------------------------------------------------------------------
                 notebookTools <Datei> export <txt/html/docx> [Zieldatei]
                 notebookTools beispiel export docx "C:\Notebooks\ausgabe.docx"
                 Ohne Zieldatei wird neben dem Notebook mit dem gleichen Namen exportiert.
                 Exportiert werden Zellinhalte, keine gespeicherten Ausgaben oder Bilder.
                 Markdown bleibt als Text erhalten. Für DOCX muss Word nicht installiert sein.
                 Vorhandene Dateien werden nicht überschrieben.

               Zellen aufteilen und zusammenführen:
               ----------------------------------------------------------------------
                 split teilt nach einer Textzeile. Beide Zellen behalten den Zelltyp.
                 join nimmt Zellnummern mit Kommas oder alle Zellen von code oder markdown.
                 Die Notebook-Reihenfolge bleibt erhalten; die erste ausgewählte Zelle bleibt stehen.
                 Ihr Zelltyp und ihre Metadaten werden übernommen, alte Code-Ausgaben entfernt.
                 Anschließend speicherst du wie gewohnt in eine neue Notebook-Datei.

               Bearbeiten im Editor:
               ----------------------------------------------------------------------
                 edit und add öffnen eine temporäre Datei mit genau einer Zelle.
                 Bei add wählst du vorher den Zelltyp im Terminal (code oder markdown).
                 Im Editor speichern und schließen, danach im Terminal Enter drücken.
                 Es wird nur der Zellinhalt übernommen. Mit 'abbrechen' kannst du abbrechen.

               Änderungen speichern:
               ----------------------------------------------------------------------
                 Nach Änderungen wirst du nach der Zieldatei gefragt.
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
