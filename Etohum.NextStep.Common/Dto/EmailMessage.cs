namespace Etohum.NextStep.Common.Dto
{
    public class EmailMessage
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }


        public override string ToString()
        {
            return $"\n\nSending Email to <<{EmailAddress}>>\nSubject:[{Subject}]\nBody:\t\"{Body}\"\n\n";
        }
    }
}