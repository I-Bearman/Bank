using System.Collections.Generic;

namespace CollectionsLibrary
{
    public abstract class BankDepart
    {
        public List<Client> Clients { get; set; }
        public abstract string Department { get; set; }
    }

    public class IndividBank : BankDepart
    {
        public override string Department { get; set; } = "Individual";
        public IndividBank()
        {
            Clients = new List<Client>();
        }
    }
    public class BusinessBank : BankDepart
    {
        public override string Department { get; set; } = "Business";
        public BusinessBank()
        {
            Clients = new List<Client>();
        }
    }
    public class VIPBank : BankDepart
    {
        public override string Department { get; set; } = "VIP";
        public VIPBank()
        {
            Clients = new List<Client>();
        }
    }
}
