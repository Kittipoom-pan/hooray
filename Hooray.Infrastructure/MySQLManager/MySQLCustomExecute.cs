using Hooray.Infrastructure.DBContexts;
using Hooray.Infrastructure.Manager;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Hooray.Infrastructure.MySQLManager
{
    public class MySQLCustomExecute
    {
        private readonly devhoorayContext _context;
        public List<MySqlParameter> Parameters { get; set; }
        public string procedureName { get; set; }
        private string conStr { get; set; }
        private string connectionStr
        {
            get
            {
                MySqlConnection conn = new MySqlConnection("server=203.154.162.242;port=3309;user id=dev-hooray;password=fa682223e721054945a335b157e99de232c2b574ffd277f27a71c90d6b4165d0;database=dev-hooray;convertzerodatetime=True");
                return conn.ConnectionString.ToString();
            }
            set
            {
                this.connectionStr = value;
            }
        }

        public MySQLCustomExecute(string procedureName)
        {
            this.procedureName = procedureName;
            this.Parameters = new List<MySqlParameter>();
        }

        public DataTable executeProcedureWithReturnTable()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
            DataTable table = new DataTable();
            MySqlConnection con = new MySqlConnection(this.connectionStr);
            //MySqlConnection con = new MySqlConnection(_conn.ConnectionString.ToString());

            try
            {
                con.Open();

                using (MySqlCommand cmd = con.CreateCommand())
                {
                    //cmd.CommandTimeout = 60;
                    cmd.CommandText = this.procedureName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (MySqlParameter param in this.Parameters)
                        cmd.Parameters.Add(param);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Load(reader);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                HoorayLogManager.FeyverLog.WriteExceptionLog(ex, "MySQLManager");
            }
            finally
            {
                if (con != null)
                {
                    con.Dispose();
                }
            }

            return table;
        }
    }
}
