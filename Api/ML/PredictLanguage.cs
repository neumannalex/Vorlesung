using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.ML;
using Vorlesung.Shared.ML.Language;

namespace Vorlesung.Api.ML
{
    public static class PredictLanguage
    {
        [FunctionName("PredictLanguage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "predict")] HttpRequest req,
            ExecutionContext context,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function PredictLanguage processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<ModelInput>(requestBody);

                var logtext = data.Text.Length > 30 ? data.Text.Substring(0, 30) + "..." : data.Text;

                log.LogDebug($"Text to predict: {logtext}");

                string modelPath = Path.Combine(context.FunctionDirectory, "..", "ML", "MLModel.zip");

                LanguageModel model = new LanguageModel(modelPath);

                var prediction = model.Predict(data);
                var result = model.GetScoresFromPrediction(prediction);

                var json = JsonConvert.SerializeObject(result);
                log.LogDebug($"Prediction: {json}");

                return new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                log.LogError(ex, "Error in PredictLanguage");

                return new BadRequestObjectResult(new { 
                    Success = false,
                    Error = ex.Message
                });
            }
            
        }
    }
}
