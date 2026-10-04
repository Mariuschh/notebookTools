using NotebookTools.Notebook;
namespace NotebookTools.Features
{
    public class CommandAddCell // Fügt dem übergebenen Notebook eine neue Zelle hinzu.
    {
        public static Notebooks AddCell(Notebooks nb)
        {
            Console.WriteLine("Zelltyp eingeben (code/markdown):"); // Zelltyp vom Benutzer abfragen.
            string? type = Console.ReadLine();

            if (type == null)
            {
                return nb;
            }

            while (type != "code" && type != "markdown") //   ob der eingegebene Zelltyp gültig ist.
            {
                Console.WriteLine("Ungültiger Zelltyp. Bitte 'code' oder 'markdown' eingeben:"); // Bei einer ungültigen Eingabe wird erneut nach dem Zelltyp gefragt.
                type = Console.ReadLine();
                if (type == null)
                {
                    return nb;
                }
            }
            
            string? text;
            try
            {
                text = CellEditor.OpenNewCell(nb, type);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Console.WriteLine("Die Datei konnte nicht geöffnet werden. Bitte richte eine Standard-App für .ipynb ein.");
                return nb;
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
                return nb;
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.WriteLine(exception.Message);
                return nb;
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine("Die Notebook-Datei enthält ungültiges JSON.");
                return nb;
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Die Notebook-Datei enthält ungültige Daten.");
                return nb;
            }
            if (text == null)
            {
                return nb;
            }

            NotebookCell cell = new() // Neue Zelle erstellen und den gewählten Zelltyp festlegen.
            {Type = type};
       
            cell.Source.Add(text);  // Den Inhalt aus dem Editor zur Zelle hinzufügen.

            nb.Cells.Add(cell);// Die neue Zelle zum Notebook hinzufügen.

            return nb;  // Das neue  Notebook zurückgeben.
        }
    }
}

