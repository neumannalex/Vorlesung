// This file was auto-generated by ML.NET Model Builder. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Vorlesung.Shared.ML.Language
{
    public class LanguageModel
    {
        public string ModelPath { get; set; } = "MLModel.zip";
        public PredictionEngine<ModelInput, ModelOutput> PredictionEngine { get; private set; }

        public LanguageModel(string modelPath)
        {
            ModelPath = modelPath;
            PredictionEngine = CreatePredictionEngine();
        }

        public ModelOutput Predict(ModelInput input)
        {
            ModelOutput result = PredictionEngine.Predict(input);
            return result;
        }

        public List<LanguagePrediction> GetScoresFromPrediction(ModelOutput prediction)
        {
            var labels = Labels;

            return labels.Select(x => new LanguagePrediction
                {
                    Language = x,
                    Score = (double)prediction.Score[Array.IndexOf(labels, x)]
                })
                .OrderByDescending(x => x.Score)
                .ToList();
        }

        public double GetPredictedScore(ModelOutput prediction)
        {
            var labels = Labels;
            var index = Array.IndexOf(labels, prediction.Prediction);

            if (index >= 0 && index < labels.Length)
                return prediction.Score[index];
            else
                return double.NaN;
        }

        public string[] Labels
        {
            get
            {
                if (PredictionEngine == null)
                    return new string[0];

                var labelBuffer = new VBuffer<ReadOnlyMemory<char>>();
                PredictionEngine.OutputSchema["Score"].Annotations.GetValue("SlotNames", ref labelBuffer);
                return labelBuffer.DenseValues().Select(x => x.ToString()).ToArray();
            }
        }


        private PredictionEngine<ModelInput, ModelOutput> CreatePredictionEngine()
        {
            MLContext mlContext = new MLContext();

            ITransformer mlModel = mlContext.Model.Load(ModelPath, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            return predEngine;
        }
    }
}