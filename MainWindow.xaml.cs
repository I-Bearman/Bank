﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CoreLibrary;
using CollectionsLibrary;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Core core = new Core();
        readonly string pathToBase = "ClientBase.json";
        private List<string> logList = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            core.CreateBank();
            core.LoadBase(pathToBase);
            ClientTypeLB.ItemsSource = Enum.GetNames(typeof(ClientType)).ToList();
            ClientTypeLB.SelectedIndex = 0; //default
            DepositTypeCB.ItemsSource = Enum.GetNames(typeof(DepositType)).Where(n => n != "None").ToList();
            ClientListReload();

            core.Transaction += TransactionEvent;
            TransactionList.ItemsSource = logList;
        }
        private void TransactionEvent(string message)
        {
            logList.Add(message);
            TransactionList.Items.Refresh();
        }
        private void ClientListReload()
        {
            List<Client> clients = core.bank[ClientTypeLB.SelectedIndex].Clients;
            ClientList.ItemsSource = clients;
            RecipientCB.ItemsSource = clients;
            ClientTypeLB.Items.Refresh();
        }
        private void ClientTypeLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientListReload();
        }

        private void CreateNewClientButton_Click(object sender, RoutedEventArgs e)
        {
            string clientName = ClientNameInput.Text;
            if (clientName == "")
                MessageBox.Show("Введите Имя клиента", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (!uint.TryParse(MoneySumInput.Text, out uint clientMoney))
                MessageBox.Show("Введите в качестве Накопления клиента целочисленное значение", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                bool creditStoreISGood = (bool)CreditStoryStatusInput.IsChecked;
                switch (ClientTypeLB.SelectedIndex)
                {
                    case 0:
                        core.CreateIndividClient(clientName, clientMoney, creditStoreISGood);
                        break;
                    case 1:
                        core.CreateBusinessClient(clientName, clientMoney, creditStoreISGood);
                        break;
                    case 2:
                        core.CreateVIPClient(clientName, clientMoney, creditStoreISGood);
                        break;
                    default:
                        break;
                }

                ClientNameInput.Text = string.Empty;
                MoneySumInput.Text = string.Empty;
                CreditStoryStatusInput.IsChecked = false;

                ClientList.Items.Refresh();
                RecipientCB.Items.Refresh();
            }
        }

        private void ClientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientInfo.ItemsSource = ClientList.SelectedItems;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            core.SaveBase(pathToBase, core.bank);
        }

        private void GiveCreditButton_Click(object sender, RoutedEventArgs e)
        {
            Client senderC = ClientList.SelectedItem as Client;

            //Содержит 2 проверки:
            //- выбран ли клиент из списка
            //- верное ли число введено в поле Суммы
            if (senderC == null)
                MessageBox.Show("Не Выбран клиент из списка", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                uint resultSum;
                try
                {
                    resultSum = CreditSumTB.Text.TryParse();
                }
                catch (UncorrectSumWriteException ex)
                {
                    MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                core.GiveCredit(senderC, resultSum);
                ClientInfo.Items.Refresh();
                MessageBox.Show("Кредит выдан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }


        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            Client senderC = ClientList.SelectedItem as Client;
            Client recipient = RecipientCB.SelectedItem as Client;

            if (senderC == recipient)
                MessageBox.Show("Клиент не может сделать перевод самому себе", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                //Содержит 4 проверки последовательно:
                //- выбран ли клиент из списка
                //- выбран ли получатель из комбобокса
                //- верное ли число введено в поле Суммы
                //- у клиента достаточно денег для перевода
                if (senderC == null)
                    MessageBox.Show("Не Выбран клиент из списка", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (recipient == null)
                    MessageBox.Show("Не Выбран получатель трансфера", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    uint resultSum;
                    try
                    {
                        resultSum = TransSumTB.Text.TryParse();
                        senderC.CheckEnoughSum(resultSum);
                    }
                    catch (UncorrectSumWriteException ex)
                    {
                        MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    catch (DontEnoughSumException ex)
                    {
                        MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    core.Transfer(senderC, recipient, resultSum);
                    ClientInfo.Items.Refresh();
                    MessageBox.Show("Трансфер произведён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }

        private void GetDepositButton_Click(object sender, RoutedEventArgs e)
        {
            Client client = ClientList.SelectedItem as Client;

            //Содержит 4 проверки последовательно:
            //- выбран ли клиент из списка
            //- выбран ли тип депозита из комбобокса
            //- верное ли число введено в поле Суммы
            //- у клиента достаточно денег для перевода
            if (client == null)
                MessageBox.Show("Не выбран клиент из списка", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (DepositTypeCB.SelectedIndex == -1)
                MessageBox.Show("Выберите тип депозита", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                uint depositSum;
                try
                {
                    depositSum = DepositSumTB.Text.TryParse();
                    client.CheckEnoughSum(depositSum);
                }
                catch (UncorrectSumWriteException ex)
                {
                    MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                catch (DontEnoughSumException ex)
                {
                    MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                DepositType depositType = (DepositType)Enum.Parse(typeof(DepositType), DepositTypeCB.SelectedItem.ToString());
                if (depositType == DepositType.Simple)
                    core.MakeSimpleDeposit(client, depositSum);
                else
                    core.MakeCapitalisedDeposit(client, depositSum);
                ClientInfo.Items.Refresh();
                MessageBox.Show("Депозит принят", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void CloseDepositButton_Click(object sender, RoutedEventArgs e)
        {
            Client client = ClientList.SelectedItem as Client;
            if (client == null)
                MessageBox.Show("Не выбран клиент из списка", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (!client.IsDeposit)
                MessageBox.Show("У клиента нет депозита", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                core.CloseDeposit(client);
                ClientInfo.Items.Refresh();
                MessageBox.Show("Депозит закрыт", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CloseCreditButton_Click(object sender, RoutedEventArgs e)
        {
            Client client = ClientList.SelectedItem as Client;
            if (client == null)
                MessageBox.Show("Не выбран клиент из списка", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (!client.IsCredit)
                MessageBox.Show("У клиента нет кредита", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                try
                {
                    client.CheckEnoughSum(client.CreditSum);
                }
                catch (DontEnoughSumException ex)
                {
                    MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            core.CloseCredit(client);
            ClientInfo.Items.Refresh();
            MessageBox.Show("Кредит закрыт", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
