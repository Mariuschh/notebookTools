using NotebookTools.Features;
using NotebookTools.Notebook;
using System.IO.Compression;
using System.Xml.Linq;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CommandExportTests : ConsoleCommandTestBase
{
    [TestMethod]
    public void Export_TxtEnthaeltAlleZellenInReihenfolge()
    {
        CommandExport.Export(new string[] { originalPfad, "export", "txt" }, originalPfad);
        string text = File.ReadAllText(Path.Combine(testOrdner, "original.txt"));
        string erwartet = "Hallo Welt\nNoch eine Zeile" + Environment.NewLine + Environment.NewLine
            + "Hallo Hallo" + Environment.NewLine + Environment.NewLine
            + "Ende" + Environment.NewLine + Environment.NewLine;
        Assert.AreEqual(erwartet, text);
        OriginalPruefen();
    }

    [TestMethod]
    public void Export_HtmlBehaeltSonderzeichenAlsText()
    {
        Notebooks notebook = NotebookLaden();
        notebook.Cells[0].Source.Clear();
        notebook.Cells[0].Source.Add("<script> & Grüße");
        string ziel = Path.Combine(testOrdner, "ausgabe.html");
        NotebookExporter.ExportNotebook(notebook, ziel, "html");
        string html = File.ReadAllText(ziel);
        StringAssert.Contains(html, "&lt;script&gt; &amp;");
        StringAssert.Contains(System.Net.WebUtility.HtmlDecode(html), "Grüße");
        StringAssert.Contains(html, "Hallo Hallo");
        Assert.DoesNotContain("<script>", html);
    }

    [TestMethod]
    public void Export_DocxEnthaeltZelltexteUndPaketdateien()
    {
        CommandExport.Export(new string[] { originalPfad, "export", "docx" }, originalPfad);
        using ZipArchive datei = ZipFile.OpenRead(Path.Combine(testOrdner, "original.docx"));
        Assert.IsNotNull(datei.GetEntry("[Content_Types].xml"));
        Assert.IsNotNull(datei.GetEntry("_rels/.rels"));
        using Stream dokument = datei.GetEntry("word/document.xml")!.Open();
        XDocument xml = XDocument.Load(dokument);
        XNamespace word = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        List<string> texte = new List<string>();
        foreach (XElement text in xml.Descendants(word + "t"))
        {
            texte.Add(text.Value);
        }
        CollectionAssert.AreEqual(new List<string> { "Hallo Welt", "Noch eine Zeile", "Hallo Hallo", "Ende" }, texte);
        OriginalPruefen();
    }

    [TestMethod]
    public void Export_VorhandeneZieldateiBleibtErhalten()
    {
        string ziel = Path.Combine(testOrdner, "original.txt");
        File.WriteAllText(ziel, "Vorhandener Inhalt");
        CommandExport.Export(new string[] { originalPfad, "export", "txt" }, originalPfad);
        Assert.AreEqual("Vorhandener Inhalt", File.ReadAllText(ziel));
        StringAssert.Contains(ausgabe.ToString(), "existiert bereits");
        OriginalPruefen();
    }
}
