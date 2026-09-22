using NotebookTools.Notebook;

namespace NotebookTools.Features
{
    public class CommandAddCell
    // was macht eine zelle und was brauche ich um eine zelle zu erstellen 
    // typ mark code was steht drin source überschriften struktur anschauen notebook user asuwählen lassen inhalt titel
    // die neue id darf nicht bereits existieren!
    {
        public static Notebooks AddCell(Notebooks nb)
        {
            Console.WriteLine("Zelltyp eingeben (code/markdown):");
            string? type = Console.ReadLine();

            Console.WriteLine("Inhalt eingeben:");
            string? text = Console.ReadLine();

            NotebookCell cell = new NotebookCell();

            cell.Type = type ?? "code";
            cell.Source.Add(text ?? "");

            nb.Cells.Add(cell);

            return nb;
        }
    }
}