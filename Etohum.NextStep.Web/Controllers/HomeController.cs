using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Etohum.NextStep.Common.Dto;
using Etohum.NextStep.MQ;
using Etohum.NextStep.Web.ViewModel;
using Etohum.NextStep.Web.Mapping;

namespace Etohum.NextStep.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly IQueueProvider queueProvider;
        public HomeController(IQueueProvider queueProvider)
        {
            this.queueProvider = queueProvider;
        }
        [Route("")]
        [Route("subscribe")]
        [Route("Home")]
        [Route("Home/Index")]
        public ActionResult Index()
        {
            return View(new SubscribeViewModel());
        }

        [HttpPost]
        [Route("")]
        [Route("subscribe")]
        [Route("Home")]
        [Route("Home/Index")]
        public ActionResult Index(SubscribeViewModel model)
        {
            var subscriber = model.Get<SubscribeViewModel, SubscriberInfo>();
            var isOk = queueProvider.Suscribe(subscriber);
            model.SubscriptionIsOk = isOk;
            model.UnSubscriptionIsOk = true;
            if (isOk)
            {
                ViewBag.SubscribeMsg =
                    "your subscription will proccess soon & you will get informed throgh given email address";
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult UnSubscribe(SubscribeViewModel model)
        {
            var unsubscriber = new UnSubscriberInfo() { EmailAddress = model.UnSubscribeEmailAddress };
            var isOk = queueProvider.Unsubscribe(unsubscriber);
            model.SubscriptionIsOk = true;
            model.UnSubscriptionIsOk = isOk;
            if (isOk)
            {
                ViewBag.UnsubscribeMsg =
                    "for unsubscribing, a confrimation request will be sent to given address";
            }
            return View("Index", model);
        }
    }
}