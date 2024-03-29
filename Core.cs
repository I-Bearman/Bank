using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Bank
{
    class Core
    {
        public List<BankDepart> bank;

        public void CreateBank()
        {
            bank = new List<BankDepart>
            {
                new IndividBank(),
                new BusinessBank(),
                new VIPBank()
            };
        }
        /// <summary>
        /// Сохранение базы клиентов
        /// </summary>
        /// <param name="pathToBase">путь к базе</param>
        /// <param name="bank">коллекция клиентов</param>
        public void SaveBase(string pathToBase, List<BankDepart> bank)
        {
            string json = JsonConvert.SerializeObject(bank);
            File.WriteAllText(pathToBase, json);
        }
        /// <summary>
        /// загрузка базы клиентов
        /// </summary>
        /// <param name="pathToBase">путь к базе</param>
        public void LoadBase(string pathToBase)
        {
            if (File.Exists(pathToBase))
            {
                string json = File.ReadAllText(pathToBase);
                bank = JsonConvert.DeserializeObject(json) as List<BankDepart>;
            }
        }
        /// <summary>
        /// Создание простого клиента
        /// </summary>
        /// <param name="name">полное имя клиента</param>
        /// <param name="money">накопления клиента</param>
        /// <param name="creditStoreISGood">статус хорошей кредитной истории</param>
        public void CreateIndividClient(string name, uint money, bool creditStoreISGood)
        {
            Client client = new IndividClient(name, money);
            if (creditStoreISGood)
                GoodCreditStorySetup(client);
            bank[(int)ClientType.Individual].Clients.Add(client);
        }
        /// <summary>
        /// Создание бизнес клиента
        /// </summary>
        /// <param name="name">полное имя клиента</param>
        /// <param name="money">накопления клиента</param>
        /// <param name="creditStoreISGood">статус хорошей кредитной истории</param>
        public void CreateBusinessClient(string name, uint money, bool creditStoreISGood)
        {
            Client client = new BusinessClient(name, money);
            if (creditStoreISGood)
                GoodCreditStorySetup(client);
            bank[(int)ClientType.Business].Clients.Add(client);
        }
        /// <summary>
        /// Создание VIP клиента
        /// </summary>
        /// <param name="name">полное имя клиента</param>
        /// <param name="money">накопления клиента</param>
        /// <param name="creditStoreISGood">статус хорошей кредитной истории</param>
        public void CreateVIPClient(string name, uint money, bool creditStoreISGood)
        {
            Client client = new VIPClient(name, money);
            if (creditStoreISGood)
                GoodCreditStorySetup(client);
            bank[(int)ClientType.VIP].Clients.Add(client);
        }
        /// <summary>
        /// Приведение параметров клиента к статусу хорошей кред. истории
        /// </summary>
        /// <param name="client">клиент, над которым совершается операция</param>
        /// <returns></returns>
        private Client GoodCreditStorySetup(Client client)
        {
            client.CreditStoryIsGood = true;
            client.CreditRate -= 3;
            client.DepositRate += 3;
            return client;
        }
        /// <summary>
        /// Метод проверки достаточности денег для совершения операции
        /// </summary>
        /// <param name="client">клиент, у которого проверяется количество</param>
        /// <param name="checkSum">сумма планируемой операции</param>
        /// <returns></returns>
        public bool CheckEnoughSum(Client client, uint checkSum)
        {
            bool result = client.Money >= checkSum;
            return result;
        }
        /// <summary>
        /// Метод выдачи кредита
        /// </summary>
        /// <param name="client">клиент, которому выдаётся кредит</param>
        /// <param name="creditSum">сумма кредита</param>
        public void GiveCredit(Client client, uint creditSum)
        {
            client.CreditSum += creditSum;
            client.Money += creditSum;
            client.IsCredit = true;
        }
        /// <summary>
        /// Метод закрытия кредита
        /// </summary>
        /// <param name="client">клиент</param>
        public void CloseCredit(Client client)
        {
            uint creditSum = client.CreditSum;
            client.CreditSum = 0;
            client.Money -= creditSum;
            client.IsCredit = false;
        }
        /// <summary>
        /// Метод перевода денег между клиентами
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="recipient">получатель</param>
        /// <param name="transSum">сумма перевода</param>
        public void Transfer(Client sender, Client recipient, uint transSum)
        {
            sender.Money -= transSum;
            recipient.Money += transSum;
        }
        /// <summary>
        /// Метод создания простого депозита
        /// </summary>
        /// <param name="client">клиент</param>
        /// <param name="depositSum">сумма депозита</param>
        public void MakeSimpleDeposit(Client client, uint depositSum)
        {
            client.Money -= depositSum;
            client.DepositType = DepositType.Simple;
            client.IsDeposit = true;
            client.DepositSum += depositSum;
            DepositInfo(client);
        }
        /// <summary>
        /// Метод создания депозита с капитализацией
        /// </summary>
        /// <param name="client">клиент</param>
        /// <param name="depositSum">сумма депозита</param>
        public void MakeCapitalisedDeposit(Client client, uint depositSum)
        {
            client.Money -= depositSum;
            client.DepositType = DepositType.Capitalisation;
            client.IsDeposit = true;
            client.DepositSum += depositSum;
            DepositInfo(client);
        }
        /// <summary>
        /// Метод закрытия депозита
        /// </summary>
        /// <param name="client">клиент</param>
        public void CloseDeposit(Client client)
        {
            uint depositSum = client.DepositSum;
            client.Money += depositSum;
            client.DepositType = DepositType.None;
            client.IsDeposit = false;
            client.DepositSum = 0;
            client.DepositInfo = string.Empty;
        }
        /// <summary>
        /// Расчёт ежемесячного дохода от депозита и формирование строки инфо
        /// </summary>
        /// <param name="client">клиент</param>
        /// <returns></returns>
        public void DepositInfo(Client client)
        {
            float depositSum = client.DepositSum;
            float depositRate = client.DepositRate;
            float[] months = new float[12];
            string infoString = "Ежемесячный расчёт депозита:\n";

            months[0] = depositSum + (depositSum * depositRate / 100 / 12);
            months[0] = (float)Math.Round(months[0], 2);
            infoString += $"1 месяц: {months[0]}\n";

            //Простой депозит
            if (client.DepositType == DepositType.Simple)
            {
                for (int i = 1; i < months.Length; i++)
                {
                    months[i] = months[i - 1] + (depositSum * depositRate / 100 / 12);
                    months[i] = (float)Math.Round(months[i], 2);
                    infoString += $"{i + 1} месяц: {months[i]}\n";
                }
            }
            //Депозит с капитализацией
            else
            {
                for (int i = 1; i < months.Length; i++)
                {
                    months[i] = months[i - 1] + (months[i - 1] * depositRate / 100 / 12);
                    months[i] = (float)Math.Round(months[i], 2);
                    infoString += $"{i + 1} месяц: {months[i]}\n";
                }
            }
            client.DepositInfo = infoString;
        }
    }
}
