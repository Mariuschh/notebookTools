using NotebookTools.Help;

namespace NotebookTools.Tests.HelpTests;

[TestClass]
public class HelpCommandTests
{
    [TestMethod]
    public void GetText_GibtHilfetextZurueck()
    {
        string text = HelpCommand.GetText();

        Assert.IsFalse(string.IsNullOrWhiteSpace(text));
    }

    [TestMethod]
    public void GetText_EnthaeltVerwendung()
    {
        string text = HelpCommand.GetText();

        Assert.Contains("notebookTools <Datei> <Befehl> [Argumente]", text);
    }

    [TestMethod]
    public void GetText_EnthaeltBefehle()
    {
        string text = HelpCommand.GetText();

        Assert.Contains("content", text);
        Assert.Contains("show <Zelle>", text);
        Assert.Contains("edit <Zelle>", text);
        Assert.Contains("add", text);
        Assert.Contains("delete <Zelle>", text);
        Assert.Contains("move <Zelle> <Ziel>", text);
        Assert.Contains("help", text);
    }

    [TestMethod]
    public void GetText_EnthaeltBeispiele()
    {
        string text = HelpCommand.GetText();

        Assert.Contains("notebookTools beispiel content", text);
        Assert.Contains("notebookTools beispiel edit 2", text);
        Assert.Contains("notebookTools beispiel move 2 4", text);
    }

    [TestMethod]
    public void GetText_ErklaertZusammenfuehren()
    {
        string text = HelpCommand.GetText();

        Assert.Contains("notebookTools zusammen merge erstes 1,3 zweites 2", text);
        Assert.Contains("Die Zieldatei darf noch nicht existieren.", text);
    }

    [TestMethod]
    public void GetText_ErklaertSpeichernUndAbbrechen()
    {
        string text = HelpCommand.GetText();

        Assert.Contains("<Dateiname>_bearbeitet.ipynb", text);
        Assert.Contains("'abbrechen': Änderungen verwerfen.", text);
        Assert.Contains("Vorhandene Dateien werden nicht überschrieben.", text);
    }

    [TestMethod]
    public void GetText_EnthaeltHinweisZuZellnummern()
    {
        string text = HelpCommand.GetText();

        Assert.Contains("Zellnummern und Zielpositionen beginnen bei 1.", text);
    }

    [TestMethod]
    public void GetHelpText_GibtGleichenTextWieGetTextZurueck()
    {
        string erwartet = HelpCommand.GetText();

        string ergebnis = HelpCommand.GetHelpText();

        Assert.AreEqual(erwartet, ergebnis);
    }
}
