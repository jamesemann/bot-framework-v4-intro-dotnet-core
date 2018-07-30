// <Snippet1>
using demo4sentiment.train;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SentimentAnalysis
{
    class Program
    {
        static async Task Main(string[] args)
        {
            PrepareData();

            var model = await Train();
        }

        private static void PrepareData()
        {
            // this is designed to work on the IMDB dataset available @
            // http://ai.stanford.edu/~amaas/data/sentiment/
            // extract this file and change this path to point to the local folder for training

            var imdbRoot = @"C:\mlnet\imdb";

            var positiveReviewsDirectory = new DirectoryInfo(Path.Combine(imdbRoot, "train", "pos"));
            var negativeReviewsDirectory = new DirectoryInfo(Path.Combine(imdbRoot, "train", "neg"));

            var outFile = "imbd-sentiment.txt";
            using (var logWriter = new System.IO.StreamWriter(outFile, false))
            {
                foreach (var positiveReview in positiveReviewsDirectory.GetFiles())
                {
                    logWriter.WriteLine($"1{"\t"}{File.ReadAllText(positiveReview.FullName)}");
                }
                foreach (var negativeReview in negativeReviewsDirectory.GetFiles())
                {
                    logWriter.WriteLine($"0{"\t"}{File.ReadAllText(negativeReview.FullName)}");
                }
            }
        }

        public static async Task<PredictionModel<SentimentData, SentimentPrediction>> Train()
        {
            var pipeline = new LearningPipeline();
            pipeline.Add(new TextLoader(Path.Combine(Environment.CurrentDirectory, "imbd-sentiment.txt")).CreateFrom<SentimentData>());
            pipeline.Add(new TextFeaturizer("Features", "SentimentText"));
            pipeline.Add(new FastTreeBinaryClassifier() { NumLeaves = 5, NumTrees = 5, MinDocumentsInLeafs = 2 });
            PredictionModel<SentimentData, SentimentPrediction> model =
                pipeline.Train<SentimentData, SentimentPrediction>();
            await model.WriteAsync(Path.Combine(Environment.CurrentDirectory,  "Model.zip"));
            return model;
        }
    }
}