using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vorlesung.ML.Collect
{
    public delegate Task<DocumentRecord> ParseDelegate(DocumentRecord record);

    public class DocumentRecord
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public ParseDelegate Parse { get; set; }
        public List<string> Paragraphs { get; set; } = new List<string>();
        public string Text
        {
            get
            {
                return string.Join("\n", Paragraphs);
            }
        }
    }

    public class DocumentLanguage
    {
        public const string DE = "de";
        public const string EN = "en";
        public const string ES = "es";
        public const string FR = "fr";
    }
}
