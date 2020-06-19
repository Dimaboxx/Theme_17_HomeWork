﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;

namespace Theme_17_HomeWork
{
    public class MSSqlRep
    {
        public event Action<string> newLogMessage;
        public SqlConnection con;
        SqlDataAdapter da_clients;
        SqlDataAdapter da_accaunts;
        public DataTable dt_clients;
        public DataTable dt_Accaunts;
        // DataRowView row;
        public DataTable DtClientsTypes { get { return dtClientsTypes; } }
        private DataTable dtClientsTypes;
        public DataTable DtAccauntTypes { get { return dtAccauntTypes; } }
        private DataTable dtAccauntTypes;
        public DataTable DtRateTypes { get { return dtRateTypes; } }
        private DataTable dtRateTypes;

        public MSSqlRep()
        {

            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                InitialCatalog = "MSSQLLocalDemo",
                IntegratedSecurity = true,
                Pooling = false
                //, MinPoolSize = 10
            };

            con = new SqlConnection(connectionStringBuilder.ConnectionString);

            dt_clients = new DataTable();
            dt_Accaunts = new DataTable();
            dtClientsTypes = new DataTable();
            dtAccauntTypes = new DataTable();
            dtRateTypes = new DataTable();

            da_clients = new SqlDataAdapter();
            da_accaunts = new SqlDataAdapter();


            #region gettypes

            SqlCommand sqlcc = new SqlCommand("select * from [dbo].ClientType", con);
            con.Open();
            var res = sqlcc.ExecuteReader();
            dtClientsTypes.Load(res);

            sqlcc.CommandText = "select* from[dbo].AccauntType";
            res = sqlcc.ExecuteReader();
            dtAccauntTypes.Load(res);

            sqlcc.CommandText = "select* from[dbo].ratesType";
            res = sqlcc.ExecuteReader();
            dtRateTypes.Load(res);

            con.Close();
            #endregion


            #region selectClients
            string sql = @"select
    c.id,
    c.FullName,
    ct.[description] as 'ClientType', 
    isnull(ac.accs,0) as 'accs'
from[dbo].[Clients] as c
    left join[dbo].[ClientType] as ct
on c.ClientType = ct.id
    left join(select OwnerId, cast (count(*) as int) as 'accs' from[dbo].Accaunts group by OwnerId ) as ac
    on c.id = ac.OwnerId
    order by c.id;
";
            da_clients.SelectCommand = new SqlCommand(sql, con);

            #endregion
            #region delete Clients

            sql = "DELETE FROM [dbo].[Clients] WHERE id = @id";

            da_clients.DeleteCommand = new SqlCommand(sql, con);
            da_clients.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

            #endregion
            da_clients.Fill(dt_clients);



            #region SelectAccs
            string sql_select_accs =
@"    select
       a.id,
       act.Description as 'TypeDesc',
       a.Balans,
       a.OpenDate,
       a.EndDate,
       a.rates,
       rt.Description as 'RatesType',
       a.OwnerId,
       a.ratesTypeid,
       a.Capitalisation,
       a.[Type]
        
    from
    [Accaunts] as a 
    left join AccauntType as act on a.[Type] = act.id  
    left join ratesType as rt on a.[ratesTypeid] = rt.id ";

            da_accaunts.SelectCommand = new SqlCommand(sql_select_accs, con);
            #endregion

            #region insertAccaunt

            sql = @"INSERT INTO [Accaunts]  ( OwnerId, Type, OpenDate, EndDate, rates, ratesTypeid) 
                                 VALUES ( @OwnerId, @Type, @OpenDate,  @EndDate, @rates, @ratesTypeid); 
                 SET @id = @@IDENTITY;
                     ";
            //sql = @"INSERT INTO [Accaunts] (OwnerId, Type, OpenDate, EndDate, rates, ratesTypeid) 
            //                     VALUES (3, 1, @OpenDate,@EndDate,6,1); 
            //         ";

            da_accaunts.InsertCommand = new SqlCommand(sql, con);

            da_accaunts.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id").Direction = ParameterDirection.Output;
            da_accaunts.InsertCommand.Parameters.Add("@OwnerId", SqlDbType.Int, 4, "OwnerId");
            da_accaunts.InsertCommand.Parameters.Add("@Type", SqlDbType.Int, 4, "Type");
            da_accaunts.InsertCommand.Parameters.Add("@OpenDate", SqlDbType.Date, 3, "OpenDate");
            da_accaunts.InsertCommand.Parameters.Add("@EndDate", SqlDbType.Date, 3, "EndDate");
            da_accaunts.InsertCommand.Parameters.Add("@rates", SqlDbType.Real, 4, "rates");
            da_accaunts.InsertCommand.Parameters.Add("@ratesTypeid", SqlDbType.Int, 4, "ratesTypeid");
            da_accaunts.InsertCommand.Parameters.Add("@Capitalisation", SqlDbType.Bit, 1, "Capitalisation");

            #endregion

            #region delete Accs

            sql = "DELETE FROM [dbo].[Accaunts] WHERE id = @id";

            da_accaunts.DeleteCommand = new SqlCommand(sql, con);
            da_accaunts.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

            #endregion


            da_accaunts.Fill(dt_Accaunts);
            dt_Accaunts.DefaultView.RowFilter = "OwnerId = -1";

        }

 

        //public void DeleteClient(int id)
        //{
        //    SqlCommand sqlcq_delete = new SqlCommand($"Delete from [dbo].[Clients] where id = {id}", con);
        //    da_clients.Fill(dt_clients);

        //}
        public void DeleteClient(DataRowView row)
        {
            if ((int)row["accs"] > 0)
            {
                MessageBox.Show("Нельзя удалять клиента при наличии открытых счетов");
                newLogMessage?.Invoke($"Уданение клиента id: {row["id"]} невозможно , так как имеются счета : {row["accs"]}");

            }
            else
            {
                newLogMessage?.Invoke($"Уданение клиента id: {row["id"]} , Type : {row["ClientType"]} , {row["FullName"]}");
                row.Row.Delete();
                da_clients.Update(dt_clients);
            }
        }
        public void ReFilldtClients()
        {
            dt_clients.Clear();
            da_clients.Fill(dt_clients);
        }
        public void ReFilldtAcc()
        {
            dt_Accaunts.Clear();
            da_accaunts.Fill(dt_Accaunts);
        }


        public void AddClient(string FirstName, string MidleName, string LastName, string OrganisationName, int ClientType, bool GoodHistory)
        {

            SqlCommand sqlCommand = new SqlCommand(
                    $"insert into [dbo].[Clients] " +
                   $"(FirstName" +
                   $",MidleName," +
                   $"LastName," +
                   $"OrganisationName," +
                   $"ClientType," +
                   $"GoodHistory)  " +
                   $"Values(" +
                   $"N'{FirstName}'," +
                   $"N'{MidleName}'," +
                   $"N'{LastName}'," +
                   $"N'{OrganisationName}'," +
                   $"{ClientType}," +
                   $"'{ GoodHistory}')", con);
            con.Open();
            if (sqlCommand.ExecuteNonQuery() > 0)
            {
                dt_clients.Clear();
                da_clients.Fill(dt_clients);
                newLogMessage?.Invoke($"Добавлен пользоваетль FirstName : " +
                   $"{FirstName}," +
                   $",MidleName : " +
                   $"N'{MidleName}'," +
                   $"LastName : " +
                   $"N'{LastName}'," +
                   $"OrganisationName :" +
                   $"N'{OrganisationName}'," +
                   $",ClientType :" +
                   $"{ClientType}," +
                   $"GoodHistory :  " +
                   $"'{ GoodHistory}')");
            }
            con.Close();

        }


        public void AddAccaunt(DataRow row)
        {
            dt_Accaunts.Rows.Add(row);
            newLogMessage?.Invoke(
                $"Добавлен счет : " + $"{row["id"]}," +

                   $",Ownerid : " + $"'{row["OwnerId"]}'" +
                   $",OpenDate : " + $"'{row["OpenDate"]}'" +
                   $",EndDate : " + $"'{row["EndDate"]}'" +
                   $",Type : " + $"'{row["Type"]}'" +
                   $",rates : " + $"'{row["rates"]}'" +
                   $",Capitalisation : " + $"'{row["Capitalisation"]}'" +
                   $"");
            da_accaunts.Update(dt_Accaunts);
            ReFilldtClients();
        }


        public void AddMoney (int accid, float addetValue)
        {
            SqlCommand sqlCommand = new SqlCommand(
        $"update [dbo].[Accaunts] set Balans = Balans + {addetValue.ToString()} where id = {accid} ;",con);
            con.Open();
            if (sqlCommand.ExecuteNonQuery() > 0)
            {
                ReFilldtAcc();
            }
            con.Close();
        }

        internal void PopMoney(int accid, float addetValue)
        {
            SqlCommand sqlCommand = new SqlCommand(
        $"update [dbo].[Accaunts] set Balans = Balans - {addetValue.ToString()} where id = {accid} ;", con);
            con.Open();
            if (sqlCommand.ExecuteNonQuery() > 0)
            {
                ReFilldtAcc();
            }
            con.Close();
        }


        public void DeleteAcc(DataRowView row)
        {
            if (float.Parse(row["Balans"].ToString()) > 0)
            {
                MessageBox.Show("Нельзя удалять счет при наличии положительного баланса");
                newLogMessage?.Invoke($"Закрыть счет id: {row["id"]}  для клиента {row["OwnerId"]}  невозможно, так как баланс счета : {row["Balans"]}");

            }
            else
            {
                newLogMessage?.Invoke($"Уданение счета id: {row["id"]}  для клиента {row["OwnerId"]} ");
                row.Row.Delete();
                da_accaunts.Update(dt_Accaunts);
                ReFilldtClients();
            }
        }


    }
}

