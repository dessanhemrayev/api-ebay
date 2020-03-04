using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client_EBAY
{
    class DB
    {

        MySqlConnection connection = new MySqlConnection("server=mydb.cmkwiytbbvat.eu-central-1.rds.amazonaws.com; port=3306; username=admin_amazon;password=Amazon_2019; database=api_ebay");
        
        
        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }



        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
    
        public MySqlConnection GetConnection()
        {
            return connection; 

        }
    }



}
