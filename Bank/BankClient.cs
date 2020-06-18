using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankNameSpase
{
    /// <summary>
    /// уневерсальный клент банка
    /// </summary>
    public class BaseBankClient : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Accaunt> Accaunts { get; }
        public static int nextid;
        public event Action<string> LogMessage;

        static BaseBankClient()
        {
            nextid = 0;
        }

        public BaseBankClient()
        {
            id = nextid++;
            Accaunts = new ObservableCollection<Accaunt>();
        }

        public int Id

        {
            get { return id; }
            //set { id = value; }
        }


        /// <summary>
        /// полное имя клиента для отображения
        /// </summary>
        string clientFullName;
        public string ClientFullName 
        { get { return clientFullName; } 
            set 
            { 
                clientFullName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ClientFullName)));
            }
        }

 
        public void GenerateAccaunts()
        {
            Random rnd = new Random(id);
            AccauntsType acc;
            bool tmp_cap = false;
            switch (rnd.Next(0, 3))
            {
                case 0: acc =  AccauntsType.Simple; break;
                case 1: 
                    acc = AccauntsType.Deposite;
                    tmp_cap = (rnd.Next(0, 2) == 0 ? false : true);
                        break;
                default: acc = AccauntsType.Credit; break;
            }
            AddAccaunt(new Accaunt(acc, this) { 
                Balans = rnd.Next(0,10)*rnd.Next(0,1000),
                EndDate = DateTime.Now.AddMonths(12*rnd.Next(1, 4)),
                Capitalization = tmp_cap
            });;
        }


        private int id;

        private DateTime birthday;
        private ClientType clientType;
        private bool haveGoodHistory;

        public ClientType ClientType
        {
            get
            {
                return clientType;
            }
            set { clientType = value; }
        }

        public int Accs { get
            {
            return Accaunts.Count;
            }
        }


        public void AddAccaunt(Accaunt accauntforadd)
        {
            accauntforadd.LogMessage += LogMessage;
            this.Accaunts.Add(accauntforadd);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Accs)));
            LogMessage?.Invoke($"Клиенту id:{id} ({clientFullName}) добавлен счет {accauntforadd.Id}");
        }

        public bool HaveGoodHistory
        {
            get
            {
                return haveGoodHistory;
            }
            set
            {
                haveGoodHistory = value;
            }
        }

        public string HaveGoodHistoryDesc
        {
            get
            {
                return haveGoodHistory ? "Хорошая": "";
            }
        }
        //private float salary;
        //private float salary;


    }

    public class Organisation : BaseBankClient
    {
        string organisationName;
        public string OrganisationName 
        { 
            get { return organisationName; }
            set { ClientFullName = organisationName = value; } 
        }
    }


    public class BankClient : BaseBankClient , INotifyPropertyChanged
    {
        private string firstname;
        private string midlename;
        private string lastname;

        public string LastName
        {
            get { return lastname; }
            set
            {
                lastname = value;
                ClientFullName = FirstName + " " + MidleName + " " + LastName;
            }
        }
        public string MidleName
        {
            get { return midlename; }
            set
            {
                midlename = value;
                ClientFullName = FirstName + " " + MidleName + " " + LastName;
            }
        }

        public string FirstName
        {
            get { return firstname; }
            set
            {
                firstname = value;
                ClientFullName = FirstName + " " + MidleName + " " + LastName;
            }
        }
    }
}
