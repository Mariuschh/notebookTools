using NotebookTools.Features;
using System.Text.Json;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CellEditorTests : ConsoleCommandTestBase
{
    // Diese Fälle brechen vor dem Öffnen der Windows-App ab.
    [TestMethod]
    public void OpenCell_UngueltigeZellnummerOeffnetKeinenEditor()
    {
        foreach (int nummer in new int[] { 0, -1, 4 })
        {
            Assert.IsNull(CellEditor.OpenCell(originalPfad, nummer));
        }
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
        OriginalPruefen();
    }

    [TestMethod]
    public void OpenCell_UngueltigesJsonWirftFehler()
    {
        string ungueltigerPfad = Path.Combine(testOrdner, "ungueltig.ipynb");
        File.WriteAllText(ungueltigerPfad, "kein JSON");
        Assert.Throws<JsonException>(() =>
        {
            CellEditor.OpenCell(ungueltigerPfad, 1);
        });
    }

    [TestMethod]
    public void OpenCell_UngueltigeZelleWirftFehler()
    {
        string ungueltigerPfad = Path.Combine(testOrdner, "ungueltig.ipynb");
        File.WriteAllText(ungueltigerPfad, "{\"cells\":[null]}");
        Assert.ThrowsExactly<JsonException>(() =>
        {
            CellEditor.OpenCell(ungueltigerPfad, 1);
        });
    }
}
