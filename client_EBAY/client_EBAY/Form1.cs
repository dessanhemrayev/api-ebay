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
using IronPython.Hosting;
using MySql.Data.MySqlClient;

using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace client_EBAY
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listBox1.Items.Clear();


            this.chart1.Series.Clear();

            this.chart1.Titles.Add("График измение цены");

         
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT `name_category` FROM `category_product`", db.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);



            var myData = table.Select();
            for (int i = 0; i < myData.Length; i++)
            {
                
                    comboBox1.Items.Add(myData[i].ItemArray[0]);

            }


            

        }

        DataTable table1 = new DataTable();
      
        public void grafik(string arg)
        {
            
            DB db = new DB();
            MySqlDataAdapter adapter2 = new MySqlDataAdapter();
            //MySqlCommand command2 = new MySqlCommand("SELECT `price_product`.`price`,`price_product`.`date`FROM `api_ebay`.`price_product`  where `price_product`.`product_id` = @ucat_name ", db.GetConnection());
           // MySqlCommand command2 = new MySqlCommand("SELECT `price_product`.`price`,`price_product`.`date` FROM `price_product`WHERE `price_product`.`date`>= DATE_SUB(CURRENT_DATE, INTERVAL 7 DAY) and `price_product`.`product_id` =@ucat_name ", db.GetConnection());

            MySqlCommand command2 = new MySqlCommand("SELECT `price_product`.`price`,`price_product`.`date` FROM `price_product`WHERE `price_product`.`date`>= DATE_SUB(CURRENT_DATE, INTERVAL 25 DAY) and `price_product`.`product_id` =@ucat_name ", db.GetConnection());

            command2.Parameters.Add("@ucat_name", MySqlDbType.VarChar).Value = arg;
            adapter2.SelectCommand = command2;
            DataTable table3 = new DataTable();
            _ = adapter2.Fill(table3);

            var myData3 = table3.Select();

            chart1.Series.Clear();
          
            if (chart1.Series.Count==0)
            {
                Series series = chart1.Series.Add("Цена");
                series.ChartType = SeriesChartType.Spline;
                int kl = myData3.Length;

                for (int i = 0; i < myData3.Length; i++)
                {
                    
                        series.Points.AddXY(myData3[i].ItemArray[1], myData3[i].ItemArray[0]);

                    
                }
                
                
            }
           

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string str = listBox1.SelectedItem.ToString();
            int l = listBox1.SelectedIndex;
            var myData1 = table1.Select();

         
            label5.Text = myData1[l].ItemArray[1].ToString();

            if (Convert.ToDouble(myData1[l].ItemArray[2].ToString())==0)
            {
                label3.Text = " Товар снять с продажи ";

            }
            else
            {
                label3.Text = myData1[l].ItemArray[2].ToString() + "$";

            }
            
           
            linkLabel1.Text= myData1[l].ItemArray[3].ToString();
            string arg = myData1[l].ItemArray[0].ToString();
            
            DB db = new DB();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("select `seller_product`.`name_seller`,`image_product`.`adress_save` from `seller_product`, `image_product` where `seller_product`.`product_id`=@ucat_name and `image_product`.`product_id`=@ucat_name  ", db.GetConnection());


            command.Parameters.Add("@ucat_name", MySqlDbType.VarChar).Value = arg;
            adapter.SelectCommand = command;
            DataTable table2 = new DataTable();
            _ = adapter.Fill(table2);

            var myData2 = table2.Select();

           
            
            label4.Text = myData2[0].ItemArray[0].ToString();

            byte[] img = (byte[])table2.Rows[0][1];

            MemoryStream ms = new MemoryStream(img);

           pictureBox1.Image=Image.FromStream(ms);
            grafik(myData1[l].ItemArray[0].ToString());
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            listBox1.Items.Clear();
            string str1 = comboBox1.SelectedItem.ToString();
            int l = comboBox1.SelectedIndex + 1;

            //MessageBox.Show(l.ToString());
            DB db = new DB();

            

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            table1.Rows.Clear();
            MySqlCommand command = new MySqlCommand("SELECT `product`.`id`,`product`.`title`,`product`.`price`,`product`.`url`,`product`.`category_product_id` FROM `api_ebay`.`product` where   `product`.`category_product_id`= @ucat_name ", db.GetConnection());
            command.Parameters.Add("@ucat_name", MySqlDbType.VarChar).Value = l.ToString();
            adapter.SelectCommand = command;
            _ = adapter.Fill(table1);
            /*
             
             for (int i = 0; i < myData1.Length; i++)
            {
                for (int j = 0; j < myData1[i].ItemArray.Length; j++)
                    listBox1.Items.Add(myData1[i].ItemArray[0]);

            }
             
             */
            
            

            var myData1 = table1.Select();
            
            for (int i = 0; i < myData1.Length; i++)
            {
              listBox1.Items.Add(myData1[i].ItemArray[1]);

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DB db = new DB();
            db.closeConnection();
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            db.closeConnection();
            Close();

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Это программа написано для слежения за изменением товаров в интернет площадке EBAY ", "О программе");
        }
    }
}
