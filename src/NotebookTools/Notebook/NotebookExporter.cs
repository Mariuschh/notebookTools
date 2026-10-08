using System.IO.Compression;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace NotebookTools.Notebook
{
    public class NotebookExporter
    {
        public static void ExportNotebook(Notebooks notebook, string filePath, string format)
        {
            // CreateNew schützt vorhandene Dateien auch beim Export.
            using FileStream file = new FileStream(filePath, FileMode.CreateNew);
            if (format == "docx")
            {
                ExportWord(notebook, file);
                return;
            }

            using StreamWriter writer = new StreamWriter(file, new UTF8Encoding(false));
            if (format == "html")
            {
                writer.WriteLine("<!DOCTYPE html><html lang=\"de\"><head><meta charset=\"utf-8\"><title>Notebook</title>");
                writer.WriteLine("<style>body{font-family:Arial;max-width:900px;margin:30px auto;padding:20px}pre{white-space:pre-wrap}section{margin-bottom:24px}section.code{background:#f2f2f2;padding:16px}</style></head><body>");
            }

            foreach (NotebookCell cell in notebook.Cells)
            {
                string content = string.Join("", cell.Source);
                if (format == "html")
                {
                    // Zelltext als Text ausgeben, damit HTML-Zeichen keinen Code ausführen.
                    string cellType = WebUtility.HtmlEncode(cell.Type);
                    writer.WriteLine("<section class=\"" + cellType + "\"><pre>" + WebUtility.HtmlEncode(content) + "</pre></section>");
                }
                else
                {
                    writer.WriteLine(content);
                    writer.WriteLine();
                }
            }
            if (format == "html")
            {
                writer.WriteLine("</body></html>");
            }
        }

        private static void ExportWord(Notebooks notebook, Stream file)
        {
            // DOCX ist ein ZIP-Archiv. Diese drei XML-Dateien bilden ein einfaches Word-Dokument.
            // Aufbau: https://learn.microsoft.com/en-us/office/open-xml/general/how-to-create-a-package
            using ZipArchive archive = new ZipArchive(file, ZipArchiveMode.Create);
            WriteEntry(archive, "[Content_Types].xml", """
                <?xml version="1.0" encoding="utf-8"?>
                <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
                  <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
                  <Default Extension="xml" ContentType="application/xml"/>
                  <Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/>
                </Types>
                """);
            WriteEntry(archive, "_rels/.rels", """
                <?xml version="1.0" encoding="utf-8"?>
                <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
                  <Relationship Id="document" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/>
                </Relationships>
                """);

            XNamespace word = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
            XElement body = new XElement(word + "body");
            foreach (NotebookCell cell in notebook.Cells)
            {
                string content = string.Join("", cell.Source);
                string[] lines = content.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
                foreach (string line in lines)
                {
                    XElement text = new XElement(word + "t", line);
                    text.SetAttributeValue(XNamespace.Xml + "space", "preserve");
                    XElement run = new XElement(word + "r");
                    if (cell.Type == "code")
                    {
                        XElement font = new XElement(word + "rFonts");
                        font.SetAttributeValue(word + "ascii", "Consolas");
                        font.SetAttributeValue(word + "hAnsi", "Consolas");
                        run.Add(new XElement(word + "rPr", font));
                    }
                    run.Add(text);
                    body.Add(new XElement(word + "p", run));
                }
                body.Add(new XElement(word + "p"));
            }
            XElement document = new XElement(word + "document", body);
            WriteEntry(archive, "word/document.xml", document.ToString());
        }

        private static void WriteEntry(ZipArchive archive, string name, string content)
        {
            ZipArchiveEntry entry = archive.CreateEntry(name);
            using StreamWriter writer = new StreamWriter(entry.Open(), new UTF8Encoding(false));
            writer.Write(content);
        }
    }
}
