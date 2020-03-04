using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace admin_client_ebay
{
    public partial class Form1 : Form
    {
        DataTable table = new DataTable();
        public Form1()
        {
            InitializeComponent();
            listBox1.Items.Clear();

      
            DB db = new DB();

          

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT `id` , `title` FROM `product` where price=0", db.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);


            var myData = table.Select();
            for (int i = 0; i < myData.Length; i++)
            {
              
                    listBox1.Items.Add(myData[i].ItemArray[1]);

            }


        }
        private void button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            textBox1.Enabled = false;
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите название товара");
            }
            else
            {
                
                
                var psi = new ProcessStartInfo();
                psi.FileName = @"C:\Users\dessa\AppData\Local\Programs\Python\Python37\python.exe";

                var script = @"D:\projects\курсовые\базы_данных\client_EBAY\api_get.py";
                string zapros = textBox1.Text;
                psi.Arguments = $" \"{script}\" \"{zapros}\"";

                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;

                var errors = "";
                var results = "";


                using (var process = Process.Start(psi))
                {

                    errors = process.StandardError.ReadToEnd();
                    results = process.StandardOutput.ReadToEnd();
                }

                MessageBox.Show("Товар успешно добавлен!");
             
            }


            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            textBox1.Enabled = true;




        }

        private void button2_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            textBox1.Enabled = false;

            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Users\dessa\AppData\Local\Programs\Python\Python37\python.exe";

            var script = @"D:\projects\курсовые\базы_данных\client_EBAY\update_price.py";
          
            psi.Arguments = $" \"{script}\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var errors = "";
            var results = "";


            using (var process = Process.Start(psi))
            {
                             
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

                     MessageBox.Show("Цены товаров обновлены!");
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            textBox1.Enabled = true;
        }

        static void remove_product(string arg) 
        {
            DB db = new DB();

            db.openConnection();

            MySqlDataAdapter adapter2 = new MySqlDataAdapter();
            MySqlCommand command2 = new MySqlCommand("DELETE FROM `api_ebay`.`product` WHERE `product`.`id`=@id ", db.GetConnection());
   

            command2.Parameters.Add("@id", MySqlDbType.VarChar);

            command2.Parameters["@id"].Value = arg;


            if (command2.ExecuteNonQuery() == 1)
            {
            //    MessageBox.Show("Удалено");
            }

            db.closeConnection();

        }


        static void remove_price(string arg)
        {
            DB db = new DB();
            db.openConnection();

            MySqlCommand command2 = new MySqlCommand("DELETE FROM `api_ebay`.`price_product` WHERE `price_product`.`product_id`=@id ", db.GetConnection());
            command2.Parameters.Add("@id", MySqlDbType.VarChar);

            command2.Parameters["@id"].Value = arg;


            if (command2.ExecuteNonQuery() == 1)
            {
              //  MessageBox.Show("Удалено");
            }

            db.closeConnection();
        }

        static void remove_image(string arg)
        {
            DB db = new DB();
            db.openConnection();
         
            MySqlCommand command2 = new MySqlCommand("DELETE FROM `api_ebay`.`image_product` WHERE `image_product`.`product_id`=@id ", db.GetConnection());
            command2.Parameters.Add("@id", MySqlDbType.VarChar);

            command2.Parameters["@id"].Value = arg;


            if (command2.ExecuteNonQuery() == 1)
            {
               // MessageBox.Show("Удалено");
            }

            db.closeConnection();
        }



        static void remove_seller(string arg)
        {
            DB db = new DB();

            db.openConnection();
            MySqlCommand command2 = new MySqlCommand("DELETE FROM `api_ebay`.`seller_product` WHERE `seller_product`.`product_id`=@id ", db.GetConnection());
            command2.Parameters.Add("@id", MySqlDbType.VarChar);

            command2.Parameters["@id"].Value = arg;


            if (command2.ExecuteNonQuery() == 1)
            {
            //    MessageBox.Show("Удалено");
            }

            db.closeConnection();
        }




        private void button3_Click(object sender, EventArgs e)
        {
           


            var myData = table.Select();
            for (int i = 0; i < myData.Length; i++)
            {
              
                remove_seller(myData[i].ItemArray[0].ToString());
                remove_image(myData[i].ItemArray[0].ToString());
                remove_price(myData[i].ItemArray[0].ToString());
                remove_product(myData[i].ItemArray[0].ToString());

            }

            listBox1.Items.Clear();




        }
    }
}
