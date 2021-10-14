using System;
using Microsoft.Xrm.Sdk;
using ML.NET.LogisticRegression;

namespace UpdateChurnAutoML
{
    public class PredictChurnMLNet : PluginBase
    {
        public PredictChurnMLNet(Type childClassName) : base(childClassName)
        {
            base.RegisterEvent(40, "Update", "contact", ExecutePostContactUpdate);
        }

        private void ExecutePostContactUpdate(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }

            IPluginExecutionContext context = localContext.PluginExecutionContext;
            IOrganizationService service = localContext.OrganizationService;
            ITracingService trace = localContext.TracingService;

            if (context == null) { throw new InvalidPluginExecutionException("Failed to Receive the plugin execution context."); }
            if (service == null) { throw new InvalidPluginExecutionException("Failed to Receive the organization service."); }
            if (trace == null) { throw new InvalidPluginExecutionException("Failed to Receive the tracing service."); }

            var contact = (Entity)context.InputParameters["Target"];
            var input = this.MapContact(contact);
            var isChurn = this.PredictChurn(input);

            contact["is_churn"] = isChurn;
            service.Update(contact);
        }

        private ContactInput MapContact(Entity contact)
        {
            return new ContactInput
            {
                Allownotification = contact["allownotification"].ToString(),
                Average_cheque = float.Parse(contact["average_cheque"].ToString()),
                Balance = float.Parse(contact["balance"].ToString()),
                Credit = float.Parse(contact["credit"].ToString()),
                Debet = float.Parse(contact["debet"].ToString()),
                Discount_summ = float.Parse(contact["discount_summ"].ToString()),
                DoNotEMail = bool.Parse(contact["donotemail"].ToString()),
                Last_transaction_date = contact["last_transaction_date"].ToString(),
                Registration_date = contact["registration_date"].ToString(),
                Sendsms = bool.Parse(contact["sendsms"].ToString()),
                Summ = float.Parse(contact["summ"].ToString()),
                Source = contact["source"].ToString(),
                Quantity = float.Parse(contact["quantity"].ToString()),
                LastWeekTransaction = bool.Parse(contact["has_last_week_transaction"].ToString()),
                LastMonthTransaction = bool.Parse(contact["has_last_month_transaction"].ToString()),
                LastHalfYearTransaction= bool.Parse(contact["has_last_halfyear_transaction"].ToString()),
                GenderCode = contact["gendercode"].ToString()
            };
        }

        private bool PredictChurn(ContactInput input)
        {
            var classifier = new ML.NET.LogisticRegression.BinaryClassifier();
            var engine = classifier.TrainLR();
            return classifier.PredictChurn(engine, input);
        }

    }
}
