using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
    internal class BusinessBank : BankDepart
    {
        public override string Department { get; set; } = "Business";
    }
    internal class VIPBank : BankDepart
    {
        public override string Department { get; set; } = "VIP";
    }

}
