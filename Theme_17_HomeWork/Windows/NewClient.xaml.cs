using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BankNameSpase;
using LogCenterNameSpace;

namespace Theme_17_HomeWork
{
    /// <summary>
    /// Interaction logic for NewClient.xaml
    /// </summary>
    public partial class NewClient : Window
    {
        //       public SqlConnection con { get; set; }
        public DataTable accTypes
        {
            get { return null; }
            set
            {
                cbx_ClientType.DataContext = value.DefaultView;
            }
        }
        public event Action<string, string, string, string, int, bool> newClientEvent;
        public NewClient()
        {

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            //SqlCommand c = new SqlCommand();
            if (cbx_ClientType.Text != "Организация")
            {
                newClientEvent?.Invoke(
                    tbx_FirstName.Text,
                    tbx_MidleName.Text,
                    tbx_LastName.Text,
                    null,
                    ((int)((DataRowView)(cbx_ClientType.SelectedItem))["id"]),
                    (bool)(cb_GoodHistory.IsChecked));
                //if (!String.IsNullOrWhiteSpace(tbx_FirstName.Text) && !String.IsNullOrWhiteSpace(tbx_LastName.Text))
                //{

                //    c.CommandText = $"insert into [dbo].[Clients] " +
                //   $"(FirstName" +
                //   $",MidleName," +
                //   $"LastName" +
                //   $",ClientType," +
                //   $"GoodHistory)  " +
                //   $"Values(" +
                //   $"N'{tbx_FirstName.Text}'," +
                //   $"N'{tbx_MidleName.Text}'," +
                //   $"N'{tbx_LastName.Text}'," +
                //   $"{((DataRowView)(cbx_ClientType.SelectedItem))["id"]}," +
                //   $"'{ (bool)cb_GoodHistory.IsChecked}')";
                //}
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(tbx_OrganistionName.Text))
                {
                    newClientEvent?.Invoke(
                        null,
                        null,
                        null,
                        tbx_OrganistionName.Text,
                        ((int)((DataRowView)(cbx_ClientType.SelectedItem))["id"]),
                        (bool)(cb_GoodHistory.IsChecked));

                    //c.CommandText = $"insert into [dbo].[Clients] " +
                    //    $"(OrganisationName" +
                    //    $",ClientType," +
                    //    $"GoodHistory)  " +
                    //    $"Values(" +
                    //    $"N'{tbx_OrganistionName.Text}'," +
                    //    $"{((DataRowView)(cbx_ClientType.SelectedItem))["id"]}," +
                    //    $"'{ (bool)cb_GoodHistory.IsChecked}')";
                }
                else
                    MessageBox.Show("Имя организации не может быть пустым!", "Обнаружено пустое поле", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            //if (c.CommandText != "")
            //{
            //    c.Connection = con;
            //    con.Open();
            //    c.ExecuteNonQuery();
            //    con.Close();


            //}
            this.Close();


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MainWindow.wnd_newClient = null;
            //this.DialogResult = false;
        }
    }


}
