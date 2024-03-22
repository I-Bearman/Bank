using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Core
    {
        public List<BankDepart> bank;

        public List<BankDepart> CreateBank()
        {
            bank = new List<BankDepart>
            {
                new IndividBank(),
                new BusinessBank(),
                new VIPBank()
            };
            return bank;
        }
        bool CreateIndividClient(string name, uint money)
        {
            Client client = new IndividClient(name,money);
            return true;
        }
    }
}
