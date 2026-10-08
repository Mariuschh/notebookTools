using NotebookTools.Help;

namespace NotebookTools.Tests.HelpTests;

[TestClass]
public class HelpCommandTests
{
    [TestMethod]
    public void GetText_EnthaeltVerwendungUndBefehle()
    {
        string text = HelpCommand.GetText();

        StringAssert.Contains(text, "notebookTools <Datei> <Befehl> [Argumente]");
        StringAssert.Contains(text, "content");
        StringAssert.Contains(text, "show <Zelle>");
        StringAssert.Contains(text, "edit <Zelle>");
        StringAssert.Contains(text, "add");
        StringAssert.Contains(text, "delete <Zelle>");
        StringAssert.Contains(text, "move <Zelle> <Ziel>");
        StringAssert.Contains(text, "help");
    }

    [TestMethod]
    public void GetText_EnthaeltBeispieleUndWichtigeHinweise()
    {
        string text = HelpCommand.GetText();

        StringAssert.Contains(text, "notebookTools beispiel content");
        StringAssert.Contains(text, "notebookTools beispiel edit 2");
        StringAssert.Contains(text, "notebookTools beispiel move 2 4");
        StringAssert.Contains(text, "notebookTools zusammen merge erstes 1,3 zweites 2");
        StringAssert.Contains(text, "Die Zieldatei darf noch nicht existieren.");
        StringAssert.Contains(text, "<Dateiname>_bearbeitet.ipynb");
        StringAssert.Contains(text, "'abbrechen': Änderungen verwerfen.");
        StringAssert.Contains(text, "Vorhandene Dateien werden nicht überschrieben.");
        StringAssert.Contains(text, "Zellnummern und Zielpositionen beginnen bei 1.");
    }

    [TestMethod]
    public void GetHelpText_GibtGleichenTextWieGetTextZurueck()
    {
        string erwartet = HelpCommand.GetText();
        string ergebnis = HelpCommand.GetHelpText();
        Assert.AreEqual(erwartet, ergebnis);
    }
}
