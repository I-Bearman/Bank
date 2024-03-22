using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class BusinessClient : Client
    {
            public override byte CreditRate { get; set; } = 9;
            public override byte DepositRate { get; set; } = 6;
    }
}
