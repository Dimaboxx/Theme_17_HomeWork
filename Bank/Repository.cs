using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNameSpase
{
    public class Repository: INotifyPropertyChanged
    {
        private ObservableCollection<BaseBankClient> clients;
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string>  LogMessage;
        //public LogCenter Journal;

        public ObservableCollection<BaseBankClient> ALLlients { 
            get 
            { return clients ; } 
        }

        public ObservableCollection<BaseBankClient> VIPClients
        {
            get 
            {
                return new ObservableCollection<BaseBankClient>(clients.Where(c => c.ClientType == ClientType.VIP)); }
        }
        public ObservableCollection<BaseBankClient> StandartClients
        {
            get
            {
                return new ObservableCollection<BaseBankClient>(clients.Where(c => c.ClientType == ClientType.Standart));
            }
        }
        public ObservableCollection<BaseBankClient> OrganisationClients
        {
            get
            {
                return new ObservableCollection<BaseBankClient>(clients.Where(c => c.ClientType == ClientType.Organisation));
            }
        }

        public Repository()
        {
            //Journal = new LogCenter();
            //log += Journal.AddMessage();
            clients = new ObservableCollection<BaseBankClient>();

        }

        public void AddClient(BaseBankClient client)
        {
            LogMessage?.Invoke ($"добавлен новый клиент {client.ClientFullName}");
            clients.Add(client);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VIPClients)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StandartClients)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrganisationClients)));
        }

        public void Generate (int N)
        {
                Random random = new Random();
                BaseBankClient cl;
                for (int i = 0; i < N; i++)
                {
                    switch (random.Next(0, 3))
                    {
                        case 0: cl = new BankClient() { FirstName = $"FN_{i}", LastName = $"LN_{i}", ClientType = ClientType.Standart, HaveGoodHistory = random.Next(0, 2) == 1 };
                                break;
                        case 1: cl = new BankClient() { FirstName = $"FN_{i}", LastName = $"LN_{i}", ClientType = ClientType.VIP, HaveGoodHistory = random.Next(0, 2) == 1 };
                                break;
                        default: cl = new Organisation() { OrganisationName = $"Orgs_{i}", ClientType = ClientType.Organisation, HaveGoodHistory = random.Next(0, 2) == 1 };
                        break;
                    }
                cl.LogMessage += LogMessage;
                cl.GenerateAccaunts();
                AddClient(cl);
                }
        }

        public List<Accaunt> AllAccaunts
        {
            get
            {
                List<Accaunt> accaunts = new List<Accaunt>();
                foreach (var cl in clients)
                {
                    accaunts.AddRange(cl.Accaunts);
                };
                return accaunts;
            }
        }

    }
}
