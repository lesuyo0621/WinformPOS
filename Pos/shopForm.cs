using MetroFramework.Forms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos
{
    public partial class shopForm : MetroForm
    {
        public shopForm()
        {
            InitializeComponent();
            string getTableNum;
            string strconn = "Server=localhost;Database=test;User Id=root;Password=1234;Charset=utf8";
            Queue<int> UsingTableNumq = new Queue<int>();

            MySqlConnection conn = new MySqlConnection(strconn);
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                conn.Open();
                string strTableStateCheckQ = "select stablenum from shoptable where stablestate = 1";

                cmd.Connection = conn;
                cmd.CommandText = strTableStateCheckQ;

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UsingTableNumq.Enqueue((int)rdr[0]);
                }
                rdr.Close();

                foreach (int TableNum in UsingTableNumq)
                {
                    getTableNum = "Table" + TableNum;
                    string strGetOrder = "select orderdetail.onum, menu.mname from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum " +
                        "inner join menu on orderdetail.mnum = menu.mnum where ostate = 1 and otablenum = " + TableNum;
                    cmd.CommandText = strGetOrder;
                    rdr = cmd.ExecuteReader();

                    StringBuilder menusb = new StringBuilder();

                    while (rdr.Read())
                    {
                        menusb.Append(rdr["mname"] + "\n");
                    }

                    Button ShopTablebtn = this.Controls.Find(getTableNum, true).FirstOrDefault() as Button;

                    if (ShopTablebtn != null)
                    {
                        ShopTablebtn.Text = ShopTablebtn.Text + "\n\n" + menusb.ToString();
                    }

                    menusb.Clear();
                    rdr.Close();
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("shopFormInitialize 오류 : \n" + E.Message + "\n" + E.StackTrace);
                throw;
            }
            finally
            {
                cmd.Dispose();
                conn.Dispose();
                conn.Close();
            }

        }

        private void Table1_Click(object sender, EventArgs e)
        {
            orderdetailForm f3 = new orderdetailForm();
            f3.Show();
        }
    }
}
