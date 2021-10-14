using System;
using System.Linq;
using Microsoft.ML;  // v1.3.1 via NuGet
using Microsoft.ML.Trainers;

namespace ML.NET.LogisticRegression
{
    public class BinaryClassifier
    {
        public PredictionEngine<ContactInput, ContactOuput> TrainLR()
        {
            Console.WriteLine("\nBegin ML.NET predict churn");
            MLContext mlc = new MLContext(seed: 1);

            // 1. load data and create data pipeline
            Console.WriteLine("Loading normalized data into memory ");
            string trainDataPath = "C:\\Users\\leda\\Documents\\HSE\\churn\\contacts_fixed_float.csv";

            IDataView trainData = mlc.Data.LoadFromTextFile<ContactInput>
              (trainDataPath, ';', hasHeader: true);

            var preview = mlc.Data.CreateEnumerable<ContactInput>(trainData, false).FirstOrDefault(co => co.ChurnBool);

            var b = mlc.Transforms.Categorical.OneHotEncoding(new[]
              { new InputOutputColumnPair("DoNotEMail", "DoNotEMail") });
            var c = mlc.Transforms.Categorical.OneHotEncoding(new[]
              { new InputOutputColumnPair("sendsms", "sendsms") });
            var d = mlc.Transforms.Categorical.OneHotEncoding(new[]
              { new InputOutputColumnPair("has_last_week_transaction", "has_last_week_transaction") });
            var e = mlc.Transforms.Categorical.OneHotEncoding(new[]
              { new InputOutputColumnPair("has_last_month_transaction", "has_last_month_transaction") });
            var f = mlc.Transforms.Categorical.OneHotEncoding(new[]
              { new InputOutputColumnPair("has_last_halfyear_transaction", "has_last_halfyear_transaction") });

            var features = mlc.Transforms.Concatenate("Features", new[]
              { "balance", "credit", "debet", "discount_summ", "quantity", "summ", "average_cheque"});

            var dataPipe = features.Append(b).Append(c).Append(d).Append(e).Append(f);

            // 2. train model
            Console.WriteLine("Creating a logistic regression model");
            var options = new LbfgsLogisticRegressionBinaryTrainer.Options()
            {
                LabelColumnName = "churn_bool",
                FeatureColumnName = "Features",
                MaximumNumberOfIterations = 100,
                OptimizationTolerance = 1e-8f
            };

            var trainer = mlc.BinaryClassification.Trainers.LbfgsLogisticRegression(options);
            var trainPipe = dataPipe.Append(trainer);

            Console.WriteLine("Starting training");
            ITransformer model = trainPipe.Fit(trainData);
            Console.WriteLine("Training complete");

            // 3. evaluate model
            IDataView predictions = model.Transform(trainData);
            var metrics = mlc.BinaryClassification.EvaluateNonCalibrated(predictions, "churn_bool");
            Console.Write("Model accuracy on training data = ");
            Console.WriteLine(metrics.Accuracy.ToString("F4"));

            // 4. use model

            var seems_churn = new ContactInput
            {
                DoNotEMail = true,
                Sendsms = true,
                Balance = 0,
                Credit = (float)28776.9,
                Debet = (float)28776.9,
                Discount_summ = 0,
                Average_cheque = (float)1534.44,
                Quantity = 3,
                Summ = (float)4603.32,
                LastWeekTransaction = false,
                LastHalfYearTransaction = false,
                LastMonthTransaction = false
            };

            var seems_not_churn = new ContactInput
            {
                DoNotEMail = false,
                Sendsms = true,
                Balance = 100400,
                Credit = 2240,
                Debet = 56740,
                Discount_summ = 10400,
                Average_cheque = 50300,
                Quantity = 2030,
                Summ = 100000,
                LastWeekTransaction = true,
                LastHalfYearTransaction = true,
                LastMonthTransaction = true
            };

            var pe = mlc.Model.CreatePredictionEngine<ContactInput, ContactOuput>(model);

            return pe;
        }

        public bool PredictChurn(PredictionEngine<ContactInput, ContactOuput> pe, ContactInput contactData)
        {
            var y_churn = pe.Predict(contactData);
            return y_churn.Churn;
        }
    } // Program
}


