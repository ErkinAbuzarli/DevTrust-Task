namespace DevTrust_Task.Models
{
    public class Person
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public long? AddressId { get; set; }

        public virtual Address Address { get; set; }
    }
}
