using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BankNameSpase
{
    /// <summary>
    /// универсальный счет.
    /// </summary>
    public class Accaunt : INotifyPropertyChanged
    {
        static int nextid;
        static float standartRates;
        static float extraratesGoodHistory;
        public static string FloadtSeparatorformat;
        public event Action<string> LogMessage; 
        static Accaunt()
        {
            nextid = 0;
            standartRates = 4.5f;
            extraratesGoodHistory = 1.0f;
            FloadtSeparatorformat = System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
        }

        /// <summary>
        /// 
        /// </summary>
        public static float GetUserValue(string stringunput)
        {
            float value = 0;
            if (!float.TryParse(stringunput, out value))
                                MessageBox.Show($"Введенное число некоректно: {stringunput}\n " +
                    $"в системе установлен разделитель длобной и целой части \"{FloadtSeparatorformat}\""
                    );
            else if (value <= 0)
            {
                MessageBox.Show("Введено отрицательное число или введенное число ноль.");
            }
            return value;
        }

            /// <summary>
            /// перевод денег на другой счет
            /// </summary>
            /// <param name="targetAccaunt">Целевой счет </param>
            /// <param name="valuetosend">количество денег</param>
        public void SendMoneyToOtherAccaunt(Accaunt targetAccaunt, float valuetosend)
        {
            //float valuetosend = Accaunt.GetUserValue(inputSting);
            if ((targetAccaunt != null) && (valuetosend > 0) && (Balans >= valuetosend))
            {
                {
                    PopMoney(valuetosend);
                    targetAccaunt.PushMoney(valuetosend);
                }
            }
        }


        protected int id;
        /// <summary>
        /// свойство id;
        /// </summary>
        public int Id { get { return id; } }
        
        /// <summary>
        /// дата открытия счета. в нее будем начислять проценты.
        /// </summary>
        private DateTime opendate;
        private DateTime enddate;
        
        public DateTime OpenDate { get { return opendate; } }
        public DateTime EndDate { 
            get { return (type==AccauntsType.Simple)? DateTime.MaxValue:enddate; }  
            set { enddate = value; } }

        /// <summary>
        /// баланс счета
        /// </summary>
        /// 
        public float Balans {
            get { return balans; }
            set { balans = value; } }
        /// <summary>
        /// Свойство %
        /// </summary>
        public float Rates
        {
            get {
                float trates;
                float extrates;
                extrates = (owner.ClientType == ClientType.VIP ? -1 : owner.ClientType == ClientType.Organisation ? 5 : 0);
                extrates += owner.HaveGoodHistory  ? -extraratesGoodHistory : 0;
                switch (type)
                {
                    case AccauntsType.Credit: trates = rates + extrates; break ;
                    case AccauntsType.Deposite: trates = rates - extrates; break ;
                    default: trates = 0; break; //simple
                }
                return trates > 0? trates:0; }
        }
        /// <summary>
        /// конструктор
        /// </summary>
        public Accaunt(AccauntsType type, BaseBankClient client)
        {
            rates = standartRates;
            balans = 0;
            id = nextid++;
            opendate = DateTime.UtcNow;
            this.type = type;
            this.owner = client;
            if (type == AccauntsType.Simple)
            {
                this.ratesType = RatesPeriods.Zero;
            }
            else
            {
                this.ratesType = RatesPeriods.Mouth;
            }
            
        }

        public string TypeName {get { return this.type.ToString(); } }
        //public string TypeName {get { return this.GetType().ToString(); } }
 
         /// <summary>
        /// выдача 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool PopMoney(float count)
        {
            if (balans >= count)
            {
                balans -= count;
                LogMessage($"счет {id} списано {count} , новый баланс {balans} ");
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Balans)));
                return true;
            }
            else
            {
                LogMessage($"счет {id} Недостаточно средств, баланс {balans}, запрошено {count}");
                return false;
            }
        }

        /// <summary>
        /// пополнение
        /// </summary>
        /// <param name="count"></param>
        public void PushMoney(float count) {
            balans += count; 
                LogMessage($"счет {id} пополнен на {count} , новый баланс {balans} ");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Balans)));
        }


        /// <summary>
        /// ставка процентов
        /// </summary>
        protected float rates;

        /// <summary>
        /// текущий баланс
        /// </summary>
        protected float balans;

        /// <summary>
        /// 
        /// 
        /// 
        /// тип счета
        /// </summary>
        private AccauntsType type;

        /// <summary>
        /// тип начисления процентов
        /// </summary>
        public RatesPeriods ratesType;

        /// <summary>
        /// для отображения типа начисления процентов
        /// </summary>
        public string RatesPeriodDesc
        { get 
            {
                //string s;
                //switch (ratesType) 
                //{
                //    case RatesPeriods.Zero: s = RatesPeriods.Zero.GetType().ToString() ; break;
                //    case RatesPeriods.Daily:; break;
                //    case RatesPeriods.Mouth:; break;
                //    case RatesPeriods.Year:; break;
                //    case RatesPeriods.End:; break;
                //    case RatesPeriods.Infinity:; break;
                //    default: break;
                //}
                return ratesType.ToString();
            } }

        /// <summary>
        /// капитализация процентов ... 1-Да/ 0 - нет
        /// </summary>
        public bool Capitalization;
        public string CapitalisationDecs { get
            {
                return Capitalization ? "Yes" : "No";
            } }
        

        /// <summary>
        /// лимит кредита
        /// </summary>
        public float CreditLimit { get; set; }

        private BaseBankClient owner;
        public string OwnerFullName { get
            {
                if (this.owner != null)
                    return owner.ClientFullName;
                else
                    return "";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// возвращает коллекцию для отображения процентов
        /// </summary>
        public ObservableCollection<PercentsView> percentsViews
        {
            get
            {
                ObservableCollection<PercentsView> views = new ObservableCollection<PercentsView>();
                if (type == AccauntsType.Deposite)
                {
                    DateTime tmp_date = opendate;
                    float tmp_balans = balans;
                    float tmp_extrabalans = 0;
                    views.Add(new PercentsView()
                    {
                        Date = opendate,
                        ExtraBalans = 0,
                        BalansToEndofPerriod = balans,
                        Cache = -balans
                    });
                    while (tmp_date < enddate)
                    {
                        DateTime tmp_enddate;
                            
                        switch (ratesType)
                        {
                            case RatesPeriods.Mouth: tmp_enddate = tmp_date.AddMonths(1); break;
                            case RatesPeriods.Daily: tmp_enddate = tmp_date.AddDays(1); break;
                            default: tmp_enddate = enddate; break;
                        }
                        float rates = Rates / 365f * tmp_enddate.Subtract(tmp_date).Days;
                        tmp_extrabalans = balans * rates / 100;
                        tmp_date =  (tmp_enddate <= enddate) ? tmp_enddate : enddate;
                        if (Capitalization)
                        {
                            views.Add(new PercentsView() { 
                                Date = tmp_date, 
                                ExtraBalans = tmp_extrabalans, 
                                BalansToEndofPerriod = balans+=tmp_extrabalans,
                                Cache = 0 
                            });
                        }
                        else
                        {
                            views.Add(new PercentsView() { 
                                Date = tmp_date,
                                ExtraBalans = 0,
                                BalansToEndofPerriod = balans, 
                                Cache =  tmp_extrabalans
                            });
                        }

                    }
                }
                else
                { 
                views.Add(new PercentsView() { Date = OpenDate, ExtraBalans = 0f, BalansToEndofPerriod = balans , Cache = -balans });
                }
                return views;
            }
        }

    }

    //class DepositAccaunt : Accaunt
    //{


    //    /// <summary>
    //    /// тип начисления процентов
    //    /// </summary>
    //    public TimePeriods ratesType;
    //    /// <summary>
    //    /// капитализация процентов ... 1-Да/ 0 - нет
    //    /// </summary>
    //    public bool Capitalization;


    //}
    //class CreditAccaunt : Accaunt
    //{
    //    /// <summary>
    //    /// признак того что счет кредитный
    //    /// </summary>
    //    public bool Credit { get; set; }

    //    /// <summary>
    //    /// лимит кредита
    //    /// </summary>
    //    public float CreditLimit { get; set; }

    //}

}
