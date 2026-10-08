using NotebookTools.Notebook;

namespace NotebookTools.Tests.FeaturesTests;

// Gemeinsame Vorbereitung für die Konsolentests. Die Testklassen erben diese Methoden.
// Konsolenbefehle verändern globale Ein- und Ausgaben und dürfen nicht parallel laufen.
[DoNotParallelize]
public class ConsoleCommandTestBase
{
    protected string testOrdner = "";
    protected string originalPfad = "";
    protected string originalText = "";
    protected StringWriter ausgabe = new StringWriter();
    private TextReader alteEingabe = Console.In;
    private TextWriter alteAusgabe = Console.Out;
    private TextWriter alteFehlerAusgabe = Console.Error;
    private ConsoleColor alteFarbe;

    [TestInitialize]
    public void Vorbereiten()
    {
        // Die echte Konsole merken und durch Test-Eingaben und eine gesammelte Ausgabe ersetzen.
        alteEingabe = Console.In;
        alteAusgabe = Console.Out;
        alteFehlerAusgabe = Console.Error;
        alteFarbe = Console.ForegroundColor;
        ausgabe = new StringWriter();
        Console.SetOut(ausgabe);
        Console.SetError(ausgabe);
        Console.SetIn(new StringReader(""));

        testOrdner = Path.Combine(Path.GetTempPath(), "NotebookToolsTests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(testOrdner);
        originalPfad = Path.Combine(testOrdner, "original.ipynb");
        originalText = """
            {"cells":[
              {"cell_type":"markdown","source":["Hallo Welt\n","Noch eine Zeile"],"metadata":{}},
              {"cell_type":"code","source":["Hallo Hallo"],"metadata":{"tag":"behalten"},"execution_count":2,"outputs":[{"output_type":"stream","name":"stdout","text":["Hallo"]}]},
              {"cell_type":"markdown","source":["Ende"],"metadata":{}}
            ],"metadata":{"language_info":{"name":"python"}},"nbformat":4,"nbformat_minor":5}
            """;
        File.WriteAllText(originalPfad, originalText);
    }

    [TestCleanup]
    public void Aufraeumen()
    {
        // Nach jedem Test die Konsole zurücksetzen und nur den eigenen Testordner löschen.
        Console.SetIn(alteEingabe);
        Console.SetOut(alteAusgabe);
        Console.SetError(alteFehlerAusgabe);
        Console.ForegroundColor = alteFarbe;
        ausgabe.Dispose();
        Directory.Delete(testOrdner, true);
    }

    protected Notebooks NotebookLaden()
    {
        return NotebookReader.LoadNotebook(originalPfad);
    }

    protected void EingabeSetzen(string text)
    {
        Console.SetIn(new StringReader(text));
    }

    protected void AusgabeLeeren()
    {
        ausgabe.Dispose();
        ausgabe = new StringWriter();
        Console.SetOut(ausgabe);
        Console.SetError(ausgabe);
    }

    protected void OriginalPruefen()
    {
        Assert.AreEqual(originalText, File.ReadAllText(originalPfad));
    }
}
