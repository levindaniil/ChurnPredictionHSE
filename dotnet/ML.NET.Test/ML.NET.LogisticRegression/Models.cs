using Microsoft.ML.Data;

namespace ML.NET.LogisticRegression
{
    public class ContactInput
    {
        [ColumnName("ContactId"), LoadColumn(0)]
        public string ContactId { get; set; }       
       

        [ColumnName("GenderCode"), LoadColumn(1)]
        public string GenderCode { get; set; }


        [ColumnName("DoNotEMail"), LoadColumn(2)]
        public bool DoNotEMail { get; set; }


        [ColumnName("registration_date"), LoadColumn(3)]
        public string Registration_date { get; set; }


        [ColumnName("sendsms"), LoadColumn(4)]
        public bool Sendsms { get; set; }


        [ColumnName("source"), LoadColumn(5)]
        public string Source { get; set; }


        [ColumnName("allownotification"), LoadColumn(6)]
        public string Allownotification { get; set; }


        [ColumnName("balance"), LoadColumn(7)]
        public float Balance { get; set; }


        [ColumnName("credit"), LoadColumn(8)]
        public float Credit { get; set; }


        [ColumnName("debet"), LoadColumn(9)]
        public float Debet { get; set; }


        [ColumnName("discount_summ"), LoadColumn(10)]
        public float Discount_summ { get; set; }


        [ColumnName("quantity"), LoadColumn(11)]
        public float Quantity { get; set; }


        [ColumnName("summ"), LoadColumn(12)]
        public float Summ { get; set; }


        [ColumnName("average_cheque"), LoadColumn(13)]
        public float Average_cheque { get; set; }


        [ColumnName("last_transaction_date"), LoadColumn(14)]
        public string Last_transaction_date { get; set; }


        [ColumnName("churn_bool"), LoadColumn(15)]
        public bool ChurnBool { get; set; }

        [ColumnName("has_last_week_transaction"), LoadColumn(16)]
        public bool LastWeekTransaction { get; set; }

        [ColumnName("has_last_month_transaction"), LoadColumn(17)]
        public bool LastMonthTransaction { get; set; }

        [ColumnName("has_last_halfyear_transaction"), LoadColumn(18)]
        public bool LastHalfYearTransaction { get; set; }
    }

    public class ContactOuput
    {
        [ColumnName("churn_bool")]
        public bool Churn { get; set; }
    }
}
