# notebookTools

Ein C#-Kommandozeilentool zum Anzeigen und Bearbeiten von Jupyter Notebooks im `.ipynb`-Format. Entstanden als zweites BfK-S Projekt.

## Voraussetzungen und Start

- Windows und das .NET 9 SDK zum Bauen und Ausführen des Quellcodes
- Für `edit` und `add`: ein Editor, der `.ipynb`-Dateien öffnen und speichern kann, beispielsweise Visual Studio Code mit der Jupyter-Erweiterung. Diesen in Windows als Standard-App für `.ipynb` auswählen.
- Für die Suche außerhalb des aktuellen Ordners: Windows Search und ein indizierter Speicherort


Für den Aufruf als `notebookTools` den Ordner mit der gebauten Anwendung, zum Beispiel `src\NotebookTools\bin\Debug\net9.0`, zur Benutzer-Umgebungsvariable `Path` hinzufügen und anschließend ein neues Terminal öffnen. Die übrigen Dateien im Ausgabeordner werden ebenfalls benötigt. Eine fertige EXE für die Abgabe wird zum Projektabschluss bereitgestellt.

## Verwendung

```text
notebookTools <Datei> <Befehl> [Argumente]
```

Die Endung `.ipynb` wird automatisch ergänzt. Du kannst einen Dateinamen, einen relativen oder einen vollständigen Pfad angeben. Pfade mit Leerzeichen gehören in Anführungszeichen. Zellnummern und Zielpositionen beginnen bei **1**.

| Befehl | Funktion | Beispiel |
|---|---|---|
| `content` | Kurze Übersicht aller Zellen | `notebookTools beispiel content` |
| `show <Zelle>` | Vollständigen Inhalt einer Zelle anzeigen | `notebookTools beispiel show 2` |
| `edit <Zelle>` | Zellinhalt im Editor bearbeiten | `notebookTools beispiel edit 2` |
| `add` | Neue Code- oder Markdown-Zelle am Ende einfügen | `notebookTools beispiel add` |
| `delete <Zelle>` | Zelle löschen | `notebookTools beispiel delete 2` |
| `move <Zelle> <Ziel>` | Zelle an die angegebene Position verschieben | `notebookTools beispiel move 2 4` |
| `findandreplace` | Text in allen Zellen suchen und ersetzen | `notebookTools beispiel findandreplace` |

`notebookTools help` oder `notebookTools` ohne Argumente zeigt die Hilfe an. Befehle können auch mit Großbuchstaben geschrieben werden.

### Bearbeiten und Einfügen

`edit` und `add` öffnen eine temporäre Notebook-Datei mit genau einer Zelle. Bei `add` wählst du vorher im Terminal den Zelltyp aus. Bearbeite den Zellinhalt im Editor, speichere die Datei und schließe sie. Drücke danach im Terminal Enter, damit der Inhalt übernommen wird. Mit `abbrechen` kannst du den Vorgang beenden.

Es wird nur der Inhalt übernommen. Die temporäre Datei dient nicht zum Löschen oder Verschieben von Zellen und nicht zum Ändern des Zelltyps.

### Suchen und Ersetzen

Bei `findandreplace` gibst du Suchtext und Ersatz im Terminal ein. Groß- und Kleinschreibung wird beachtet. Ein leerer Ersatz entfernt den Suchtext. Gesucht wird im Zellinhalt, nicht in gespeicherten Ausgaben.

### Notebooks zusammenführen

```text
notebookTools zusammen merge erstes 1,3 zweites 2
```

Das Beispiel übernimmt die Zellen 1 und 3 aus `erstes.ipynb` und anschließend Zelle 2 aus `zweites.ipynb` in `zusammen.ipynb`. Es müssen mindestens zwei Quellnotebooks angegeben werden. Weitere Paare aus Notebook und Zellnummern sind möglich. Die Zieldatei darf noch nicht existieren.

### Änderungen speichern

Nach einer Änderung fragt das Programm nach einer neuen Zieldatei. Enter speichert als `<Dateiname>_bearbeitet.ipynb` neben dem Original. Du kannst stattdessen einen eigenen Dateinamen oder Pfad eingeben; relative Pfade beziehen sich auf den aktuellen Terminalordner. Der Zielordner muss bereits existieren. Eine fehlende `.ipynb`-Endung wird ergänzt.

Mit `abbrechen` verwirfst du die Änderungen. Das Original und vorhandene Zieldateien werden nicht überschrieben. Es gibt keinen eigenen `save`-Befehl. Für weitere Bearbeitungen gibst du die neu gespeicherte Datei an.

### Dateisuche

Zuerst wird der angegebene Pfad beziehungsweise der aktuelle Ordner geprüft. Wird dort nichts gefunden, sucht das Programm nach dem Dateinamen im Windows-Suchindex. Bei mehreren Treffern gibst du den gewünschten vollständigen Pfad an. Wird nichts gefunden oder ist Windows Search nicht verfügbar, wechsle in den Notebook-Ordner oder verwende den vollständigen Pfad. Eine rekursive Suche findet nicht statt.

## Tests

Die automatisierten Tests verwenden MSTest.



Sie prüfen die Befehle, die Notebook-Verarbeitung und Hilfsfunktionen. Das Öffnen und Bearbeiten im externen Editor wird zusätzlich manuell geprüft.

## Projektunterlagen

- [JSON-Analyse](docs/Analyse.docx)
- [Retrospektive](docs/Retrospektive.docx)
- [Präsentation](docs/notebookTools.pptx)
- [Backlog und Aufwände](docs/Backlog.docx)

Die Aufgabenverwaltung erfolgt im SCRUM Board in GitHub Projects. Der Backlog im Repository hält den dokumentierten Abschlussstand fest.

## Projektstruktur

- `src/NotebookTools`: Hauptprogramm und Klassen für Befehle, Notebook-Verarbeitung, Hilfe und Dateisuche
- `tests/NotebookTools.Tests`: Unit Tests
- `docs`: Analyse, Retrospektive, Präsentation und Backlog

## Verwendete Hilfsmittel

Zur Verarbeitung der Notebook-Dateien wird `System.Text.Json` verwendet. Windows Search wird über `System.Data.OleDb` angesprochen. Bei der Umsetzung, Fehlerkorrektur, Erstellung von Tests und Überarbeitung der Dokumentation wurde ChatGPT/Codex unterstützend eingesetzt.
