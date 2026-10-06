using System.Text.Json;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.NotebookTests;

[TestClass]
public class NotebookCellTests
{
    [TestMethod]
    public void Constructor_InitializesEmptyCollectionsAndDefaultValues()
    {
        var cell = new NotebookCell();

        Assert.AreEqual(string.Empty, cell.Type);
        Assert.IsNull(cell.ExecutionCount);
        Assert.IsNotNull(cell.Source);
        Assert.HasCount(0, cell.Source);
        Assert.IsNotNull(cell.Outputs);
        Assert.HasCount(0, cell.Outputs);
        Assert.IsNotNull(cell.Metadata);
        Assert.HasCount(0, cell.Metadata);
        Assert.IsNotNull(cell.AdditionalData);
        Assert.HasCount(0, cell.AdditionalData);
    }

    [TestMethod]
    public void Constructor_CreatesIndependentCollectionsForEachInstance()
    {
        var first = new NotebookCell();
        var second = new NotebookCell();

        first.Source.Add("print(1)");
        first.Outputs.Add(new { output_type = "stream", text = new[] { "1\n" } });
        first.Metadata.Add("collapsed", true);
        first.AdditionalData.Add("id", JsonSerializer.SerializeToElement("cell-1"));

        Assert.HasCount(0, second.Source);
        Assert.HasCount(0, second.Outputs);
        Assert.HasCount(0, second.Metadata);
        Assert.HasCount(0, second.AdditionalData);
    }

    [TestMethod]
    public void Serialize_WritesCellPropertyNamesAndContents()
    {
        var cell = new NotebookCell
        {
            Type = "code",
            Source = new List<string> { "value = 1\n", "print(value)" },
            Outputs = new List<object>
            {
                new { output_type = "stream", name = "stdout", text = new[] { "1\n" } },
                new { output_type = "stream", name = "stderr", text = new[] { "Warning\n" } }
            },
            Metadata = new Dictionary<string, object> { ["collapsed"] = true },
            ExecutionCount = 3
        };

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(cell));
        var root = document.RootElement;

        Assert.AreEqual(5, root.EnumerateObject().Count());
        Assert.AreEqual("code", root.GetProperty("cell_type").GetString());
        Assert.AreEqual(3, root.GetProperty("execution_count").GetInt32());
        var source = root.GetProperty("source");
        Assert.AreEqual(2, source.GetArrayLength());
        Assert.AreEqual("value = 1\n", source[0].GetString());
        Assert.AreEqual("print(value)", source[1].GetString());
        var outputs = root.GetProperty("outputs");
        Assert.AreEqual(2, outputs.GetArrayLength());
        Assert.AreEqual("stream", outputs[0].GetProperty("output_type").GetString());
        Assert.AreEqual("stdout", outputs[0].GetProperty("name").GetString());
        Assert.AreEqual("1\n", outputs[0].GetProperty("text")[0].GetString());
        Assert.AreEqual("stderr", outputs[1].GetProperty("name").GetString());
        Assert.AreEqual("Warning\n", outputs[1].GetProperty("text")[0].GetString());
        Assert.IsTrue(root.GetProperty("metadata").GetProperty("collapsed").GetBoolean());
    }

    [TestMethod]
    public void Deserialize_ReadsCellPropertiesAndContents()
    {
        const string json = """
            {
              "cell_type": "code",
              "source": ["value = 1\n", "print(value)"],
              "outputs": [
                { "output_type": "stream", "name": "stdout", "text": ["1\n"] },
                { "output_type": "stream", "name": "stderr", "text": ["Warning\n"] }
              ],
              "metadata": { "tags": ["example", "test"] },
              "execution_count": 3
            }
            """;

        var cell = JsonSerializer.Deserialize<NotebookCell>(json);

        Assert.IsNotNull(cell);
        Assert.AreEqual("code", cell.Type);
        Assert.AreEqual((int?)3, cell.ExecutionCount);
        CollectionAssert.AreEqual(new[] { "value = 1\n", "print(value)" }, cell.Source);
        Assert.HasCount(2, cell.Outputs);
        var stdout = (JsonElement)cell.Outputs[0];
        Assert.AreEqual("stream", stdout.GetProperty("output_type").GetString());
        Assert.AreEqual("stdout", stdout.GetProperty("name").GetString());
        Assert.AreEqual("1\n", stdout.GetProperty("text")[0].GetString());
        var stderr = (JsonElement)cell.Outputs[1];
        Assert.AreEqual("stderr", stderr.GetProperty("name").GetString());
        Assert.AreEqual("Warning\n", stderr.GetProperty("text")[0].GetString());
        Assert.HasCount(1, cell.Metadata);
        var tags = (JsonElement)cell.Metadata["tags"];
        Assert.AreEqual(2, tags.GetArrayLength());
        Assert.AreEqual("example", tags[0].GetString());
        Assert.AreEqual("test", tags[1].GetString());
        Assert.HasCount(0, cell.AdditionalData);
    }

    [TestMethod]
    public void Deserialize_MissingProperties_PreservesDefaults()
    {
        var cell = JsonSerializer.Deserialize<NotebookCell>("{}");

        Assert.IsNotNull(cell);
        Assert.AreEqual(string.Empty, cell.Type);
        Assert.IsNull(cell.ExecutionCount);
        Assert.IsNotNull(cell.Source);
        Assert.HasCount(0, cell.Source);
        Assert.IsNotNull(cell.Outputs);
        Assert.HasCount(0, cell.Outputs);
        Assert.IsNotNull(cell.Metadata);
        Assert.HasCount(0, cell.Metadata);
        Assert.IsNotNull(cell.AdditionalData);
        Assert.HasCount(0, cell.AdditionalData);
    }

    [TestMethod]
    [DataRow("code")]
    [DataRow("markdown")]
    public void JsonRoundTrip_NullExecutionCount_RemainsNull(string cellType)
    {
        var original = new NotebookCell { Type = cellType, ExecutionCount = null };

        var json = JsonSerializer.Serialize(original);
        using var document = JsonDocument.Parse(json);
        var restored = JsonSerializer.Deserialize<NotebookCell>(json);

        Assert.AreEqual(JsonValueKind.Null, document.RootElement.GetProperty("execution_count").ValueKind);
        Assert.IsNotNull(restored);
        Assert.AreEqual(cellType, restored.Type);
        Assert.IsNull(restored.ExecutionCount);
    }

    [TestMethod]
    public void JsonRoundTrip_PreservesCellIdAndAttachmentsAtRootLevel()
    {
        const string json = """
            {
              "cell_type": "markdown",
              "source": ["![Image](attachment:image.png)"],
              "metadata": {},
              "id": "cell-1",
              "attachments": { "image.png": { "image/png": "aW1hZ2U=" } },
              "custom": null
            }
            """;

        var cell = JsonSerializer.Deserialize<NotebookCell>(json);

        Assert.IsNotNull(cell);
        Assert.HasCount(3, cell.AdditionalData);
        Assert.AreEqual("cell-1", cell.AdditionalData["id"].GetString());
        Assert.AreEqual("aW1hZ2U=", cell.AdditionalData["attachments"]
            .GetProperty("image.png").GetProperty("image/png").GetString());
        Assert.AreEqual(JsonValueKind.Null, cell.AdditionalData["custom"].ValueKind);

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(cell));
        var root = document.RootElement;

        Assert.AreEqual(8, root.EnumerateObject().Count());
        Assert.IsFalse(root.TryGetProperty("AdditionalData", out _));
        Assert.AreEqual("cell-1", root.GetProperty("id").GetString());
        Assert.AreEqual("aW1hZ2U=", root.GetProperty("attachments")
            .GetProperty("image.png").GetProperty("image/png").GetString());
        Assert.AreEqual(JsonValueKind.Null, root.GetProperty("custom").ValueKind);
    }
}
