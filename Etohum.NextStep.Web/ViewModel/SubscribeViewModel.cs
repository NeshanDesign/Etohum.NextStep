using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Etohum.NextStep.Web.ViewModel
{
    [Bind(Exclude = "UserId")]
    public class SubscribeViewModel: BaseViewModel
    {
        public long UserId { get; set; }

        [MaxLength(30, ErrorMessage = "Too long first name")]
        [DisplayName("First Name:")]
        public string FirstName { get; set; }

      
        [MaxLength(30,ErrorMessage = "Too long last name")]
        [DisplayName("Last Name:")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [DisplayName("Email Address:")]
        public string EmailAddress { get; set; }


        [EmailAddress(ErrorMessage = "Invalid email address")]
        [DisplayName("Email Address:")]
        public string UnSubscribeEmailAddress { get; set; }
    }
}