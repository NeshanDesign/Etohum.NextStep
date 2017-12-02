using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Etohum.NextStep.Data.Model
{
    public class SubscriptionHistory:EntityBase<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public SubscriptionAction Action { get; set; } = SubscriptionAction.None;

        public Subscriber Subscriber { get; set; }

        [ForeignKey("Subscriber")]
        public int SubscriberId { get; set; }
    }

    public enum SubscriptionAction
    {
        None = 0,
        Subscribe = 1,
        UnSubscribe = 2
    }
}
