using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vorlesung.ML.Collect
{
    public class DataCollector
    {
        public String TrainingDataPath { get; set; }
        public string TrainingDataFullPath
        {
            get
            {
                return Path.GetFullPath(TrainingDataPath);
            }
        }
        public string TrainingDataFilename { get; set; } = "data-training.tsv";
        public string TrainingDataFilePath
        {
            get
            {
                return Path.Combine(TrainingDataPath, TrainingDataFilename);
            }
        }
        public string TrainingDataFullFilePath
        {
            get
            {
                return Path.GetFullPath(TrainingDataFilePath);
            }
        }

        public async Task CollectData(List<DocumentRecord> records)
        {
            Console.WriteLine($"Start scraping {records.Count} documents.");

            if (!Directory.Exists(TrainingDataFullPath))
            {
                Console.WriteLine($"Creating output directory at {TrainingDataFullPath}.");
                Directory.CreateDirectory(TrainingDataFullPath);
            }
                
            if (File.Exists(TrainingDataFullFilePath))
            {
                Console.WriteLine($"Deleting existing file {TrainingDataFullFilePath}.");
                File.Delete(TrainingDataFullFilePath);
            }
                
            int count = 0;

            foreach (var record in records)
            {
                Console.WriteLine($"\tDocument {++count} of {records.Count}.");

                Console.Write($"\t\tGetting {record.Url} in {record.Language} as {record.Name}-{record.Language} ... ");
                //var parsedRecord = await ParseParagraphTextFromWiki(record);
                var parsedRecord = await record.Parse(record);
                Console.WriteLine("finished");

                // write single file for language

                var path = Path.Combine(TrainingDataFullPath, $"{record.Name}-{record.Language}.txt");                
                if (File.Exists(path))
                {
                    Console.WriteLine($"\t\tDeleting existing file {path}.");
                    File.Delete(path);
                }

                Console.WriteLine($"\t\tWriting text to {path} ... ");
                File.WriteAllText(path, parsedRecord.Text);

                // append paragraphs to total content file
                Console.WriteLine($"\t\tAppending paragraphs to {TrainingDataFullFilePath}");
                var lines = parsedRecord.Paragraphs.Select(x => $"{record.Language}\t{x}").ToList();
                File.AppendAllLines(TrainingDataFullFilePath, lines);
            }

            Console.WriteLine($"Finished scraping {records.Count} documents.");
        }

        public async Task<DocumentRecord> ParseParagraphTextFromWiki(DocumentRecord record)
        {
            // get div id="bodyContent" | id="mw-content-text"
            // get all <p>
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(record.Url);

            var contentNode = doc.GetElementbyId("mw-content-text");
            if (contentNode != null)
            {
                foreach (var pNode in contentNode.Descendants("p"))
                {
                    if (pNode.NodeType == HtmlNodeType.Element)
                    {
                        var text = pNode.InnerText;

                        text = HtmlEntity.DeEntitize(text); // Html Entities (&#160;) entfernen
                        text = Regex.Replace(text, "\\[[0-9]*?\\]", ""); // Fußnoten entfernen
                        text = text.Replace("|", ""); // Umbrüche im Paragraphen entfernen
                        text = text.Replace("\r\n", ""); // Umbrüche im Paragraphen entfernen
                        text = text.Replace("\n", ""); // Umbrüche im Paragraphen entfernen
                        text = text.Replace("\t", ""); // Tabs im Paragraphen entfernen
                        text = text.Trim();

                        if (!string.IsNullOrEmpty(text))
                            record.Paragraphs.Add(text);
                    }
                }
            }

            return record;
        }

        public async Task<DocumentRecord> ParseFromLiterotica(DocumentRecord record)
        {
            // get div class="b-story-body-x x-r15"
            // get all <p>
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(record.Url);

            var contentNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'b-story-body-x')]");
            if (contentNode != null)
            {
                foreach (var pNode in contentNode.Descendants("p"))
                {
                    if (pNode.NodeType == HtmlNodeType.Element)
                    {
                        var text = pNode.InnerText;

                        text = text.Trim();
                        text = text.Replace("\t", ""); // Tabs im Paragraphen entfernen
                        text = text.Replace("* * *", "");
                        text = HtmlEntity.DeEntitize(text); // Html Entities (&#160;) entfernen                        
                        text = Regex.Replace(text, @"^-\s*", "", RegexOptions.Multiline); // Spiegelstriche am Anfang einer Zeile entfernen

                        // split long text
                        var parts = text.Split("\n");
                        foreach (var p in parts)
                        {
                            var ptext = p.Replace("\r\n", ""); // Umbrüche im Paragraphen entfernen
                            ptext = ptext.Replace("\n", ""); // Umbrüche im Paragraphen entfernen
                            ptext = ptext.Trim();

                            if (!string.IsNullOrEmpty(ptext))
                                record.Paragraphs.Add(ptext);
                        }
                    }
                }
            }

            return record;
        }
    }
}
