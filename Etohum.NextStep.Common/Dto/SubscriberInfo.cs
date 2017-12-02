namespace Etohum.NextStep.Common.Dto
{
    
    public class SubscriberInfo:UnSubscriberInfo // need base class for generic constraint
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
