# notebookTools

Ein C#-Kommandozeilentool zum Anzeigen und Bearbeiten von Jupyter Notebooks im `.ipynb`-Format. Entstanden als zweites BfK-S Projekt.

## Voraussetzungen und Start

- Windows x64 für die fertige EXE. Zum Bauen des Quellcodes wird das .NET 9 SDK benötigt.
- Für `edit` und `add`: ein Editor, der `.ipynb`-Dateien öffnen und speichern kann, beispielsweise Visual Studio Code mit der Jupyter-Erweiterung. Diesen in Windows als Standard-App für `.ipynb` auswählen.
- Für die Suche außerhalb des aktuellen Ordners: Windows Search und ein indizierter Speicherort

Für den Aufruf aus jedem Terminal die EXE in einen festen Ordner kopieren und diesen Ordner zur Benutzer-Umgebungsvariable `Path` hinzufügen: In Windows nach „Umgebungsvariablen“ suchen, die Benutzer-Variable `Path` bearbeiten und den Ordner als neuen Eintrag hinzufügen. Anschließend ein neues Terminal öffnen und `notebookTools --help` aufrufen.

Die EXE liegt in diesem Pfad:

```powershell
.\release\win-x64\NotebookTools.exe
```

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
| `add [Textdatei]` | Neue Zelle im Editor oder aus einer Textdatei einfügen | `notebookTools beispiel add` |
| `delete <Zelle>` | Zelle löschen | `notebookTools beispiel delete 2` |
| `move <Zelle> <Ziel>` | Zelle an die angegebene Position verschieben | `notebookTools beispiel move 2 4` |
| `split <Zelle> <Zeile>` | Zelle nach einer Textzeile aufteilen | `notebookTools beispiel split 2 5` |
| `join <Auswahl>` | Ausgewählte Zellen oder alle Zellen eines Typs zusammenführen | `notebookTools beispiel join markdown` |
| `type <Zelle> <Typ>` | Zelltyp auf code oder markdown ändern | `notebookTools beispiel type 2 markdown` |
| `export <Format> [Zieldatei]` | Zellinhalte als TXT, HTML oder DOCX exportieren | `notebookTools beispiel export html` |
| `findandreplace` | Text in allen Zellen suchen und ersetzen | `notebookTools beispiel findandreplace` |

`notebookTools help`, `notebookTools --help` oder `notebookTools` ohne Argumente zeigt die Hilfe an. Befehle können auch mit Großbuchstaben geschrieben werden.

### Bearbeiten und Einfügen

`edit` und `add` ohne Textdatei öffnen eine temporäre Notebook-Datei mit genau einer Zelle. Bei `add` wählst du vorher im Terminal den Zelltyp aus. Bearbeite den Zellinhalt im Editor, speichere die Datei und schließe sie. Drücke danach im Terminal Enter, damit der Inhalt übernommen wird. Mit `abbrechen` kannst du den Vorgang beenden.

Es wird nur der Inhalt übernommen. Die temporäre Datei dient nicht zum Löschen oder Verschieben von Zellen und nicht zum Ändern des Zelltyps.

Mit `notebookTools beispiel add "aufgabe.txt"` wird der Inhalt der Textdatei direkt als neue Zelle am Ende eingefügt. Den Zelltyp wählst du im Terminal. Anschließend folgt die gewohnte Speicherabfrage.

Mit `type` lässt sich der Typ einer vorhandenen Zelle ändern. Der Zellinhalt bleibt erhalten; alte Code-Ausgaben und der Ausführungszähler werden zurückgesetzt.

### Exportieren

```text
notebookTools beispiel export txt
notebookTools beispiel export html
notebookTools beispiel export docx "C:\Notebooks\ausgabe.docx"
```

Ohne Zieldatei wird neben dem Notebook mit dem gleichen Dateinamen und der gewählten Endung gespeichert. Eine fehlende Export-Endung wird bei einem eigenen Ziel ergänzt. Vorhandene Dateien werden nicht überschrieben.

Exportiert werden die Zellinhalte in ihrer Reihenfolge. Markdown bleibt als Text erhalten; gespeicherte Ausgaben und Bilder werden nicht exportiert. Für DOCX muss Word nicht installiert sein.

### Zellen aufteilen und zusammenführen

`notebookTools beispiel split 2 5` teilt Zelle 2 nach ihrer fünften Textzeile. Die zweite Zelle wird direkt dahinter eingefügt. Beide behalten den Zelltyp; nach der Trennstelle muss noch Text folgen.

`notebookTools beispiel join 1,3,5` verbindet die gewählten Zellen in ihrer Notebook-Reihenfolge. Sie dürfen an beliebigen Positionen stehen, müssen aber den gleichen Zelltyp haben. `join code` oder `join markdown` verbindet alle Zellen dieses Typs. Mindestens zwei Zellen sind nötig.

Die zusammengeführte Zelle steht an der Position der ersten ausgewählten Zelle und übernimmt deren Metadaten. Die übrigen ausgewählten Zellen werden entfernt. Falls nötig, wird ein Zeilenumbruch zwischen den Inhalten eingefügt. Alte Code-Ausgaben werden zurückgesetzt. Anschließend folgt die Speicherabfrage für eine neue Notebook-Datei.

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

Zur Verarbeitung der Notebook-Dateien wird `System.Text.Json` verwendet. Windows Search wird über `System.Data.OleDb` angesprochen. Bei der Umsetzung, Fehlerkorrektur, Erstellung von Tests und Überarbeitung der Dokumentation wurde ChatGPT unterstützend eingesetzt.

## EXE neu erstellen

```powershell
dotnet publish src/NotebookTools/NotebookTools.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=None -p:DebugSymbols=false -o release/win-x64
```
