using Microsoft.CodeCoverage.Core.Reports.Coverage;
using NotebookTools.Features;
using NotebookTools.Notebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookTools.Tests
{
    [TestClass]
    public class CommandSaveTest
    {
        public static string filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "example.ipynb");
        Notebooks notebook = NotebookReader.LoadNotebook(filePath);

        [TestMethod][DoNotParallelize]
        public void TestSaveCanceled()
        {
            //Arrange
            Console.SetIn(new StringReader("abbrechen"));

            StringWriter output = new StringWriter();
            Console.SetOut(output);

            //Act
            CommandSave.Save(notebook, filePath);

            //Assert
            StringAssert.Contains(output.ToString(), "Die Änderungen wurden nicht gespeichert.");
        }

    }
}
