using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Etohum.NextStep.Web.ViewModel
{
    public class BaseViewModel
    {
        public bool IsValid { get; set; } = true;
        public bool SubscriptionIsOk { get; set; } = true;
        public bool UnSubscriptionIsOk { get; set; } = true;
        public string Message { get; set; }
        public ActionStatus Status { get; set; } = ActionStatus.NotSpecified;
    }

    

    public enum ActionStatus {
        NotSpecified =0,
        Continue = 100,
        Ok = 200,
        ClientError = 400,
        Unauthorized = 403,
        NotFound = 404,
        ServerError = 500
    }
}