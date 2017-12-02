using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etohum.NextStep.Data.Model
{
    public class Subscriber:EntityBase<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Index(IsUnique = true)]
        [EmailAddress]
        [MaxLength(120)]
        [Required]
        [DefaultValue("info@sample.com")]
        public string EmailAddress { get; set; } = "info@sample.com";

        [MaxLength(50)]
        [DefaultValue("No FirstName")]
        public string FirstName { get; set; } = "No FirstName";

        [MaxLength(50)]
        [DefaultValue("No LastName")]
        public string LastName { get; set; } = "No LastName";

        [Index(IsUnique = false)]
        [DefaultValue(false)]
        public bool IsRemoved { get; set; } = false;

        public ICollection<SubscriptionHistory> SubscriptionHistories { get; set; }
    }
}
