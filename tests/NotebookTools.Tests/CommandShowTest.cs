using NotebookTools.Features;
using NotebookTools.Help;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NotebookTools.Tests
{
    [TestClass]
    public class CommandShowTest
    {
        //Arrange
        public string filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "example.ipynb");

        [TestMethod]
        public void TestCellNumberValid()
        {
            //Act
            string result = CommandShow.ErmittleSourceText("1", filePath);
            //Assert
            StringAssert.Contains(result, "# Task 1");
            StringAssert.Contains(result, "This is a markdown cell.");
        }

        [TestMethod]
        public void TestCellNumberNotExisted()
        {
            string result = CommandShow.ErmittleSourceText("99", filePath);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void TestCellTextInvalidFormat()
        {
            Assert.ThrowsExactly<FormatException>(() => CommandShow.ErmittleSourceText("a", filePath));
        }
    }
}
