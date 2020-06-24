using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LogCenterNameSpace;
using System.Data;
using System.Collections.Specialized;
using Theme_17_HomeWork.Classes;

namespace Theme_17_HomeWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string DateTimeformat;
        public static string Timeformat;
        public static string Dateformat;
        public static NewClient wnd_newClient;
        public static NewOrgs wnd_newOrgs;
        public static w_newAccaunt wnd_w_newAccaunt;
        public MSSqlRep mSSqlRep;

        static MainWindow() {
            Timeformat = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.LongTimePattern;
            Dateformat = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern;
            DateTimeformat = $"{Dateformat} {Timeformat}";
        }


        public MainWindow()
        {
           LogCenter logCenter = new LogCenter();
            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Threading.Thread.CurrentThread.CurrentUICulture.IetfLanguageTag);
            mSSqlRep = new MSSqlRep();
            mSSqlRep.newLogMessage += logCenter.AddMessage;
            mSSqlRep.newLogMessage += scrollLogView;
            mSSqlRep.dt_Accaunts.DefaultView.RowFilter = ($"OwnerId= '-1'");
            InitializeComponent();
            lst_log.ItemsSource = logCenter.records;
            
            logCenter.AddMessage("Репозитарий создан");
//            cbx_ClientType.DataContext = mSSqlRep.DtClientsTypes.DefaultView;
//            dtg_Clients.DataContext = mSSqlRep.dt_clients.DefaultView;
            dtg_Accaunts.DataContext = mSSqlRep.dt_Accaunts.DefaultView;

            cbbx_ClientType.DataContext = mSSqlRep.DtClientsTypes.DefaultView;
            cbbx_ClientType.SelectedIndex = 0;
            //repository.Generate(15);
        }

        private void scrollLogView (string s)
        {
            if (lst_log.Items.Count > 0)
            {
                lst_log.ScrollIntoView(lst_log.Items[lst_log.Items.Count - 1]);
            }
        }

        private void Btn_addClient_Click(object sender, RoutedEventArgs e)
        {

            if (((int)((DataRowView)cbbx_ClientType.SelectedItem)["id"]) == 0)
            {
                if (wnd_newClient == null)
                {
                    wnd_newClient = new NewClient();

                    //wnd_newClient.accTypes = mSSqlRep.DtClientsTypes;
                    wnd_newClient.newClientEvent += mSSqlRep.AddClient;

                    wnd_newClient.Owner = this;

                }
                wnd_newClient.ShowDialog();
                //if (wnd_newClient.DialogResult == true)
                //{
                //    mSSqlRep.ReFilldt();
                //}
                wnd_newClient = null;



            }
            else
            {
                if (wnd_newOrgs == null)
                {
                    wnd_newOrgs = new NewOrgs();

                    //wnd_newClient.accTypes = mSSqlRep.DtClientsTypes;
                    wnd_newOrgs.newOrganisationEvent += mSSqlRep.AddOrganisation;

                    wnd_newOrgs.Owner = this;

                }
                wnd_newOrgs.ShowDialog();
                //if (wnd_newClient.DialogResult == true)
                //{
                //    mSSqlRep.ReFilldt();
                //}
                wnd_newOrgs = null;
                



            }





       }

        private void Btn_addacc_Click(object sender, RoutedEventArgs e)
        {
            if(wnd_w_newAccaunt == null)
            {
                wnd_w_newAccaunt  = new w_newAccaunt();
            }
           // wnd_w_newAccaunt.OwnerId = (int)((DataRowView)(dtg_Clients.SelectedItem))["id"];
            wnd_w_newAccaunt.AccType = mSSqlRep.DtAccauntTypes;
            wnd_w_newAccaunt.RateType = mSSqlRep.DtRateTypes;

            //if (((int)((DataRowView)cbbx_ClientType.SelectedItem)["id"]) == 0)
            //{
            //    wnd_w_newAccaunt.Clients =  mSSqlRep.dt_clients;
            //}
            //else
            //    wnd_w_newAccaunt.Clients =  mSSqlRep.dt_Organisations;

            DataRow row = mSSqlRep.dt_Accaunts.NewRow();
            var id =dtg_Clients.SelectedIndex;
            if (dtg_Clients.SelectedItem != null)
            {
            row["OwnerId"] = (int)((DataRowView)(dtg_Clients.SelectedItem))["id"];
            wnd_w_newAccaunt.NewACCrow = row;

            wnd_w_newAccaunt.ShowDialog();
            if(wnd_w_newAccaunt.DialogResult == true)
            {
                mSSqlRep.AddAccaunt(row, (int)((DataRowView)cbbx_ClientType.SelectedItem)["id"]) ;
                dtg_Clients.SelectedIndex = id;
            }
            }
            wnd_w_newAccaunt = null;
        }



        private void Btn_AddCache_Click(object sender, RoutedEventArgs e)
        {
            mSSqlRep.AddMoney(((int)((DataRowView)dtg_Accaunts.SelectedItem)["id"]), Support.GetUserValue(txtbx_CashValue.Text));
        }

        private void Btn_PopCache_Click(object sender, RoutedEventArgs e)
        {
            float currBalans = float.Parse(((DataRowView)dtg_Accaunts.SelectedItem)["Balans"].ToString());
            float value = (Support.GetUserValue(txtbx_CashValue.Text));
            if (currBalans  >= value)
                mSSqlRep.PopMoney(((int)((DataRowView)dtg_Accaunts.SelectedItem)["id"]), value);
            else
                MessageBox.Show("Введенное число больше остатка на счете");

        }



        // private void Btn_dellClient_Click(object sender, RoutedEventArgs e)
        // {
        //     if(dtg_Clients.SelectedItem!=null)
        //          mSSqlRep.DeleteClient((DataRowView)(dtg_Clients.SelectedItem));
        //}

        private void dtg_Clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtg_Clients.SelectedItem != null)
                mSSqlRep.dt_Accaunts.DefaultView.RowFilter = ($"OwnerId= '{((DataRowView)(dtg_Clients.SelectedItem))["id"]}'");
            else
                mSSqlRep.dt_Accaunts.DefaultView.RowFilter = ($"OwnerId= '-1'");
            //mSSqlRep.dt_Accaunts.DefaultView.RowFilter = "";
        }

        //private void MenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    cbx_ClientType.SelectedIndex = -1;
        //}

        private void Btn_CloseAcc_Click(object sender, RoutedEventArgs e)
        {
            if (dtg_Accaunts.SelectedItem != null)
                mSSqlRep.DeleteAcc((DataRowView)(dtg_Accaunts.SelectedItem), (int)((DataRowView)cbbx_ClientType.SelectedItem)["id"]);
        }

        private void cbbx_ClientType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((int)((DataRowView)cbbx_ClientType.SelectedItem)["id"]) == 0)
            {
                dtg_Clients.DataContext = mSSqlRep.dt_clients.DefaultView;
            }
            else
                dtg_Clients.DataContext = mSSqlRep.dt_Organisations.DefaultView;
        }

        private void Btn_DellClient_Click(object sender, RoutedEventArgs e)
        {

            if (dtg_Clients.SelectedItem != null) {
                if (((int)((DataRowView)cbbx_ClientType.SelectedItem)["id"]) == 0)
                mSSqlRep.DeleteClient((DataRowView)(dtg_Clients.SelectedItem));
            else
                mSSqlRep.DeleteOrgs((DataRowView)(dtg_Clients.SelectedItem));
            }
        }
    }
}
