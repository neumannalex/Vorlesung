using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vorlesung.ML.Collect;
using Vorlesung.ML.Build;
using Vorlesung.Shared.ML.Language;

namespace Vorlesung.ML
{
    class Program
    {
        private static string TrainingDataPath = @"./Data";
        private static string TrainingDataFilename = @"training-data.tsv";
        private static string ModelPath = @"../../../../Api/ML";
        private static string ModelFileName = @"MLModel.zip";
        
        private static DataCollector Collector;
        private static LanguageModel Model;

        static async Task Main(string[] args)
        {
            Collector = new DataCollector
            {
                TrainingDataPath = TrainingDataPath,
                TrainingDataFilename = TrainingDataFilename
            };

            await CollectData();

            Process.Start("explorer.exe", Collector.TrainingDataFullPath);

            //var modelFilePath = Path.Combine(Collector.TrainingDataFullPath, ModelFileName);

            var modelFilePath = Path.Combine(Path.GetFullPath(ModelPath), ModelFileName);

            TrainModel(Collector.TrainingDataFullFilePath, modelFilePath);

            Model = new LanguageModel(modelFilePath);

            TestPrediction("Hallo, mein Name ist Alexander.");
            TestPrediction("Hello, my name is Alexander.");
            TestPrediction("Bonjour, je m'appelle Alexander.");
            TestPrediction("Hola mi nombre es Alexander.");
        }

        private static async Task CollectData()
        {
            List<DocumentRecord> records = new List<DocumentRecord>
            {
                new DocumentRecord { Language = DocumentLanguage.DE, Name = "Berlin", Url = "https://de.wikipedia.org/wiki/Berlin", Parse = Collector.ParseParagraphTextFromWiki },
                new DocumentRecord { Language = DocumentLanguage.EN, Name = "Berlin", Url = "https://en.wikipedia.org/wiki/Berlin", Parse = Collector.ParseParagraphTextFromWiki },
                new DocumentRecord { Language = DocumentLanguage.ES, Name = "Berlin", Url = "https://es.wikipedia.org/wiki/Berl%C3%ADn", Parse = Collector.ParseParagraphTextFromWiki },
                new DocumentRecord { Language = DocumentLanguage.FR, Name = "Berlin", Url = "https://fr.wikipedia.org/wiki/Berlin", Parse = Collector.ParseParagraphTextFromWiki },

                new DocumentRecord { Language = DocumentLanguage.EN, Name = "Story1", Url = "https://www.literotica.com/s/the-percentage-game", Parse = Collector.ParseFromLiterotica },
                new DocumentRecord { Language = DocumentLanguage.DE, Name = "Story1", Url = "https://german.literotica.com/s/der-mai", Parse = Collector.ParseFromLiterotica },
                new DocumentRecord { Language = DocumentLanguage.ES, Name = "Story1", Url = "https://spanish.literotica.com/s/el-viaje", Parse = Collector.ParseFromLiterotica },
                new DocumentRecord { Language = DocumentLanguage.FR, Name = "Story1", Url = "https://french.literotica.com/s/soiree-enneigee", Parse = Collector.ParseFromLiterotica },
            };

            await Collector.CollectData(records);
        }

        private static void TrainModel(string trainingDataFilePath, string modelFilePath)
        {
            ModelBuilder.CreateModel(trainingDataFilePath, modelFilePath);
        }
    
        private static void TestPrediction(string text)
        {
            Console.Write($"I guess the text '{text}' is written in ... ");
            var prediction = Model.Predict(new ModelInput
            {
                Text = text
            });

            var scores = Model.GetScoresFromPrediction(prediction);
            var scoreString = string.Join(", ", scores.Select(x => $"{x.Language}: {Math.Round(x.Score * 100, 0)}%"));
            
            var predictedScore = Model.GetPredictedScore(prediction);
            var predictedScoreString = double.IsNaN(predictedScore) ? "-" : $"{Math.Round(predictedScore * 100, 0)}%";

            Console.WriteLine($"{prediction.Prediction} ({predictedScoreString}) | Scores: [{scoreString}]");
        }
    }
}
