using NotebookTools.Features;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotebookTools.Notebook;
using System.Text.Json;

namespace NotebookTools.Tests
{
    [TestClass]
    public class CommandShowTest
    {
        //Arrange
        private string dateiPfad = Path.Combine(AppContext.BaseDirectory, "TestData", "example.ipynb");

        [TestMethod]
        public void Show_GueltigeZellnummerZeigtInhalt()
        {
            //Act
            string ergebnis = CommandShow.ErmittleSourceText("1", dateiPfad);
            //Assert
            StringAssert.Contains(ergebnis, "# Task 1");
            StringAssert.Contains(ergebnis, "This is a markdown cell.");
        }

        [TestMethod]
        public void Show_ZellnummerZuGrossGibtFehlermeldungZurueck()
        {
            string ergebnis = CommandShow.ErmittleSourceText("99", dateiPfad);
            StringAssert.Contains(ergebnis, "außerhalb");
        }

        [TestMethod]
        public void Show_GibtTextMitKlammernUndZeilenumbruechenVollstaendigAus()
        {
            string tempPfad = Path.GetTempFileName();
            try
            {
                string inhalt = "Werte[0] = \"Hallo\";\n" + new string('a', 100);
                Notebooks notebook = new Notebooks();
                NotebookCell zelle = new NotebookCell();
                zelle.Source.Add(inhalt);
                notebook.Cells.Add(zelle);
                File.WriteAllText(tempPfad, JsonSerializer.Serialize(notebook));

                string ergebnis = CommandShow.ErmittleSourceText("1", tempPfad);
                Assert.AreEqual(inhalt, ergebnis);
            }
            finally
            {
                File.Delete(tempPfad);
            }
        }

        [TestMethod]
        public void Show_UngueltigeNummerWirftFehler()
        {
            Assert.ThrowsExactly<FormatException>(() =>
            {
                CommandShow.ErmittleSourceText("a", dateiPfad);
            });
        }
    }
}
