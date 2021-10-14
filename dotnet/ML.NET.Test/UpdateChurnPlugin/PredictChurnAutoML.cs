using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using ML_NET_TestML.Model;

namespace UpdateChurnAutoML
{
    public class PredictChurnAutoML : PluginBase
    {
        public PredictChurnAutoML() : base(typeof(PredictChurnAutoML))
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

            var contact = (Entity) context.InputParameters["Target"];
            var input = this.MapContact(contact);
            var isChurn = this.PredictChurn(input);

            contact["is_churn"] = isChurn;
            service.Update(contact);
        }

        private ModelInput MapContact(Entity contact)
        {
            return new ModelInput
            {
                Allownotification = float.Parse(contact["allownotification"].ToString()),
                Average_cheque = contact["average_cheque"].ToString(),
                Balance = contact["balance"].ToString(),
                Credit = contact["credit"].ToString(),
                Debet = contact["debet"].ToString(),
                Discount_summ = float.Parse(contact["discount_summ"].ToString()),
                DoNotEMail = float.Parse(contact["donotemail"].ToString()),
                Last_transaction_date = contact["last_transaction_date"].ToString(),
                Registration_date = contact["registration_date"].ToString(),
                Sendsms = float.Parse(contact["sendsms"].ToString()),
                Summ = contact["summ"].ToString(),
                Source = float.Parse(contact["source"].ToString()),
                Quantity = float.Parse(contact["quantity"].ToString()),
                Has_last_week_transaction = float.Parse(contact["has_last_week_transaction"].ToString()),
                Has_last_month_transaction = float.Parse(contact["has_last_month_transaction"].ToString()),
                Has_last_halfyear_transaction = float.Parse(contact["has_last_halfyear_transaction"].ToString()),
                GenderCode = contact["gendercode"].ToString()
            };
        }

        private bool PredictChurn(ModelInput input)
        {
            return new ML_NET_TestML.ConsoleApp.Program().PredictChurn(input);
        }
    }
}
