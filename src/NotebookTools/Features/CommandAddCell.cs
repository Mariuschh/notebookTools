using NotebookTools.Notebook;
namespace NotebookTools.Features
{
    public class CommandAddCell // Fügt dem übergebenen Notebook eine neue Zelle hinzu.
    {
        public static Notebooks AddCell(Notebooks nb)
        {
            Console.WriteLine("Zelltyp eingeben (code/markdown):"); // Zelltyp vom Benutzer abfragen.
            string? type = Console.ReadLine();

            while (type != "code" && type != "markdown") //   ob der eingegebene Zelltyp gültig ist.
            {
                Console.WriteLine("Ungültiger Zelltyp. Bitte 'code' oder 'markdown' eingeben:"); // Bei einer ungültigen Eingabe wird erneut nach dem Zelltyp gefragt.
                type = Console.ReadLine();
            }
            
            Console.WriteLine("Inhalt eingeben:"); // Inhalt der neuen Zelle vom Benutzer abfragen.
            string? text = Console.ReadLine();

            NotebookCell cell = new() // Neue Zelle erstellen und den gewählten Zelltyp festlegen.
            {Type = type};
       
            cell.Source.Add(text ?? "");  // Den eingegebenen Inhalt zur Zelle hinzufügen.

            nb.Cells.Add(cell);// Die neue Zelle zum Notebook hinzufügen.

            return nb;  // Das neue  Notebook zurückgeben.
        }
    }
}

