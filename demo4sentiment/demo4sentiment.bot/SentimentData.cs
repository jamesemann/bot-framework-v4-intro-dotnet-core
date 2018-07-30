using Microsoft.ML.Runtime.Api;

namespace demo4sentiment.bot
{
    public class SentimentData
    {
        [Column(ordinal: "1", name: "Label")]
        public float Sentiment;
        [Column(ordinal: "0")]
        public string SentimentText;
    }

    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Sentiment;
    }
}