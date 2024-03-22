using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class IndividClient : Client
    {
        public IndividClient(string name = "Default Name", uint money = 0) : base(name, money)
        {
        }

        public override byte CreditRate { get; set; } = 12;
            public override byte DepositRate { get; set; } = 3;
    }
}
