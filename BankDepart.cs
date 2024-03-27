using System.Collections.Generic;

namespace Bank
{
    public abstract class BankDepart
    {
        public List<Client> Clients { get; set; }
        public abstract string Department { get; set; }
    }

    internal class IndividBank : BankDepart
    {
        public override string Department { get; set; } = "Individual";
        public IndividBank()
        {
            Clients = new List<Client>();
        }
    }
    internal class BusinessBank : BankDepart
    {
        public override string Department { get; set; } = "Business";
        public BusinessBank()
        {
            Clients = new List<Client>();
        }
    }
    internal class VIPBank : BankDepart
    {
        public override string Department { get; set; } = "VIP";
        public VIPBank()
        {
            Clients = new List<Client>();
        }
    }
}
