using System;
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
        SqlDataAdapter da_organisations;
        SqlDataAdapter da_accaunts;
        public DataTable dt_clients;
        public DataTable dt_Accaunts;
        public DataTable dt_Organisations;
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
            dt_Organisations = new DataTable();

            dtClientsTypes = new DataTable();
            dtAccauntTypes = new DataTable();
            dtRateTypes = new DataTable();

            da_clients = new SqlDataAdapter();
            da_accaunts = new SqlDataAdapter();
            da_organisations = new SqlDataAdapter();


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
    c.GoodHistory,
    isnull(ac.accs,0) as 'accs'
from (select 
        id,FullName as N'FullName',ClientType,GoodHistory
    from [Clients] 
--    union 
--    select 
--        id, OrganisationName as'FullName',ClientType,GoodHistory
--    from Organisations
) as c
    left join[dbo].[ClientType] as ct
on c.ClientType = ct.id
    left join(select OwnerId, cast (count(*) as int) as 'accs' from[dbo].Accaunts group by OwnerId ) as ac
    on c.id = ac.OwnerId
    order by c.id;
";
            da_clients.SelectCommand = new SqlCommand(sql, con);

            #endregion
            #region delete Clients

            string sql_client_dell = "DELETE FROM [dbo].[Clients] WHERE id = @id";

            da_clients.DeleteCommand = new SqlCommand(sql_client_dell, con);
            da_clients.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

            #endregion
            da_clients.Fill(dt_clients);

            #region selectOrgs
            string sql_selectOrgs = @"select
    c.id,
    c.FullName,
    ct.[description] as 'ClientType',
    c.GoodHistory,
    isnull(ac.accs,0) as 'accs',
    c.BankDetails,
    c.Adress
from (
--select 
--        id,FullName as N'FullName',ClientType,GoodHistory
--    from [Clients] 
--    union 
    select 
        id, OrganisationName as'FullName',ClientType,GoodHistory,BankDetails,Adress
    from Organisations
) as c
    left join[dbo].[ClientType] as ct
on c.ClientType = ct.id
    left join(select OwnerId, cast (count(*) as int) as 'accs' from[dbo].Accaunts group by OwnerId ) as ac
    on c.id = ac.OwnerId
    order by c.id;
";
            da_organisations.SelectCommand = new SqlCommand(sql_selectOrgs, con);

            #endregion
            #region delete Orgs

            sql = "DELETE FROM [dbo].[Organisations] WHERE id = @id";

            da_organisations.DeleteCommand = new SqlCommand(sql, con);
            da_organisations.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

            #endregion
            da_organisations.Fill(dt_Organisations);



            #region SelectAccs
            string sql_select_accs =
@"    select
       a.id,
       act.Description as 'TypeDesc',
       rt.Description as 'RatesType',
       c.FullName as 'Owner',
       a.Balans,
       a.OpenDate,
       a.EndDate,
       a.rates,
       a.OwnerId,
       a.ratesTypeid,
       a.Capitalisation,
       a.TypeId,
       c.ClientType      
        
    from
    [Accaunts] as a 
    left join AccauntType as act on a.[TypeId] = act.id  
    left join ratesType as rt on a.[ratesTypeid] = rt.id
    left join (select 
        id,FullName as 'FullName',ClientType
    from [Clients] 
    union 
    select 
        id, OrganisationName as'FullName',ClientType
    from Organisations) as c on a.OwnerId = c.id";

            da_accaunts.SelectCommand = new SqlCommand(sql_select_accs, con);
            #endregion

            #region insertAccaunt

            sql = @"declare @res int; 
                 exec @res = dbo.NextAccauntId; 
                INSERT INTO [Accaunts]  ( id, OwnerId, TypeId, OpenDate, EndDate, rates, ratesTypeid) 
                                 VALUES (@res, @OwnerId, @TypeId, @OpenDate,  @EndDate, @rates, @ratesTypeid); 
                 SET @id = @@IDENTITY;
                     ";
            //sql = @"INSERT INTO [Accaunts] (OwnerId, Type, OpenDate, EndDate, rates, ratesTypeid) 
            //                     VALUES (3, 1, @OpenDate,@EndDate,6,1); 
            //         ";

            da_accaunts.InsertCommand = new SqlCommand(sql, con);

            da_accaunts.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id").Direction = ParameterDirection.Output;
            da_accaunts.InsertCommand.Parameters.Add("@OwnerId", SqlDbType.Int, 4, "OwnerId");
            da_accaunts.InsertCommand.Parameters.Add("@TypeId", SqlDbType.Int, 4, "TypeId");
            da_accaunts.InsertCommand.Parameters.Add("@OpenDate", SqlDbType.Date, 3, "OpenDate");
            da_accaunts.InsertCommand.Parameters.Add("@EndDate", SqlDbType.Date, 3, "EndDate");
            da_accaunts.InsertCommand.Parameters.Add("@rates", SqlDbType.Real, 4, "rates");
            da_accaunts.InsertCommand.Parameters.Add("@ratesTypeid", SqlDbType.Int, 4, "ratesTypeid");
            da_accaunts.InsertCommand.Parameters.Add("@Capitalisation", SqlDbType.Bit, 1, "Capitalisation");

            #endregion

            #region delete Accs

            string sql_acc_dell = "DELETE FROM [dbo].[Accaunts] WHERE id = @id";

            da_accaunts.DeleteCommand = new SqlCommand(sql_acc_dell, con);
            da_accaunts.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

            #endregion


            da_accaunts.Fill(dt_Accaunts);
            //dt_Accaunts.DefaultView.RowFilter = "OwnerId = -1";

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
        public void DeleteOrgs(DataRowView row)
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
                da_organisations.Update(dt_Organisations);
            }
        }

        public void ReFilldtClients()
        {
            dt_clients.Clear();
            da_clients.Fill(dt_clients);
        }
        public void ReFilldtOrgs()
        {
            dt_Organisations.Clear();
            da_organisations.Fill(dt_Organisations);
        }
        public void ReFilldtAcc()
        {
            dt_Accaunts.Clear();
            da_accaunts.Fill(dt_Accaunts);
        }


        public void AddClient(string FirstName, string MidleName, string LastName, string Documents,  bool GoodHistory)
        {

            SqlCommand sqlCommand = new SqlCommand(
                    $"declare @res int;"+
                    "exec @res = dbo.NextClientId;" +
                    $"" +
                    $"insert into [dbo].[Clients] " +
                   $"(id" +
                   $",FirstName" +
                   $",MidleName," +
                   $"LastName," +
                   $"ClientType," +
                   $"Documents," +
                   $"GoodHistory)  " +
                   $"Values(" +
                   $"@res," +
                   $"N'{FirstName}'," +
                   $"N'{MidleName}'," +
                   $"N'{LastName}'," +
                   $"0," +
                   $"N'{Documents}',"+
                   $"'{ GoodHistory}')", con);
            con.Open();
            if (sqlCommand.ExecuteNonQuery() > 0)
            {
                dt_clients.Clear();
                da_clients.Fill(dt_clients);
                newLogMessage?.Invoke($"Добавлен пользоваетль клиент : " +
                   $"{FirstName}," +
                   $",MidleName : " +
                   $"{MidleName}'," +
                   $"LastName : " +
                   $"{LastName}', " +
                   $"Documents : " +
                   $"{Documents}'," +
                   $"GoodHistory :  " +
                   $"'{ GoodHistory}')");
            }
            con.Close();

        }

        public void AddOrganisation( string OrganisationName, string BankDetails, string Adress,  bool GoodHistory)
        {

            SqlCommand sqlCommand = new SqlCommand(
                    $"declare @res int;" +
                    "exec @res = dbo.NextClientId;" +
                    $"" +
                    $"insert into [dbo].[Organisations]( " +
                    $"id," +
                   $"OrganisationName," +
                   $"ClientType," +
                   $"BankDetails," +
                   $"Adress," +
                   $"GoodHistory)  " +
                   $"Values(" +
                   $"@res," +
                   $"N'{OrganisationName}'," +
                   $"1," +
                   $"N'{BankDetails}'," +
                   $"N'{Adress}'," +
                   $"'{ GoodHistory}')", con);
            con.Open();
            if (sqlCommand.ExecuteNonQuery() > 0)
            {
                dt_Organisations.Clear();
                da_organisations.Fill(dt_Organisations);
                newLogMessage?.Invoke($"" +
                    $"Добавлена Организация : " +
                   $"{OrganisationName}," +
                    $"BankDetails : " +
                   $"{BankDetails}," + 
                   $"Adress : " +
                   $"{Adress}," +
                   $"GoodHistory :  " +
                   $"'{ GoodHistory}')");
            }
            con.Close();

        }
        public void AddAccaunt(DataRow row, int ClientType)
        {
            dt_Accaunts.Rows.Add(row);
            newLogMessage?.Invoke(
                $"Добавлен счет : " + $"{row["id"]}" +

                   $",Type : " + $"'{row["TypeId"]}'" +
                   $",Ownerid : " + $"'{row["OwnerId"]}'" +
                   $",OpenDate : " + $"'{row["OpenDate"]}'" +
                   $",EndDate : " + $"'{row["EndDate"]}'" +
                   $",rates : " + $"'{row["rates"]}'" +
                   $",RateTypeId : " + $"'{row["RatesTypeid"]}'" +
                   $",Capitalisation : " + $"'{row["Capitalisation"]}'" +
                   $"");
            da_accaunts.Update(dt_Accaunts);
            ReFilldtAcc();
            if (ClientType == 0)
            {

                ReFilldtClients();
            }
            else
            {
                ReFilldtOrgs();
            }
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


        public void DeleteAcc(DataRowView row, int CLientType)
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
                if (CLientType == 0)
                {

                ReFilldtClients();
                }
                else
                {
                    ReFilldtOrgs();
                }
            }
        }


    }
}

