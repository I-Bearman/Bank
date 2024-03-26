using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class BusinessClient : Client
    {
        public BusinessClient(string name, uint money) : base(name, money)
        {
        }
        public override ClientType Type { get; set; } = ClientType.Business;
        public override byte CreditRate { get; set; } = 9;
        public override byte DepositRate { get; set; } = 6;
    }
}
