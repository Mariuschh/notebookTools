using NotebookTools.Features;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void Show_ZellnummerZuGrossGibtLeerenTextZurueck()
        {
            string ergebnis = CommandShow.ErmittleSourceText("99", dateiPfad);
            Assert.AreEqual("", ergebnis);
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
