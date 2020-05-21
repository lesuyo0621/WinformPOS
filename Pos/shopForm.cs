using MetroFramework;
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
        int TableCount = 10;

        // 999 = null
        int ButtonFlag1 = 999;
        int ButtonFlag2 = 999;

        public shopForm()
        {
            InitializeComponent();
            SetShopFormDefaultValue();
        }

        private void SetShopFormDefaultValue()
        {
            string getTableNum;
            string strconn = "Server=localhost;Database=test;User Id=root;Password=1234;Charset=utf8";
            Queue<int> UsingTableNumq = new Queue<int>();

            MySqlConnection conn = new MySqlConnection(strconn);
            MySqlCommand cmd = new MySqlCommand();

            for (int TableNum = 1; TableNum <= TableCount; TableNum++)
            {
                Button SetForInputTextbtn = this.Controls.Find("Table" + TableNum, true).FirstOrDefault() as Button;
                SetForInputTextbtn.Text = TableNum + "번 테이블";
            }

            try
            {
                conn.Open();
                string strTableStateCheckQ = "select stablenum from shoptable where stablestate = 1 or stablecookstate = 1";

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
                    string strGetOrder = "select orderdetail.onum, menu.mname, orderdetail.mquantity from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum " +
                        "inner join menu on orderdetail.mnum = menu.mnum where (orderlist.ostate = 1 or orderlist.ocookstate = 1) and otablenum = " + TableNum;
                    cmd.CommandText = strGetOrder;
                    rdr = cmd.ExecuteReader();

                    StringBuilder menusb = new StringBuilder();

                    while (rdr.Read())
                    {
                        menusb.Append(rdr["mname"].ToString() + " " + rdr["mquantity"].ToString() + "\n");
                    }

                    rdr.Close();

                    string strGetTotalPrice = "select sum(orderdetail.mquantity * menu.mprice) as totalprice from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum " +
                        "inner join menu on orderdetail.mnum = menu.mnum where (orderlist.ostate = 1 or orderlist.ocookstate = 1) and otablenum = " + TableNum;
                    cmd.CommandText = strGetTotalPrice;
                    rdr = cmd.ExecuteReader();

                    rdr.Read();

                    menusb.Append("\n" + rdr["totalprice"].ToString() + "원");

                    Button ShopTablebtn = this.Controls.Find(getTableNum, true).FirstOrDefault() as Button;

                    if (ShopTablebtn != null)
                    {
                        ShopTablebtn.Text = ShopTablebtn.Text + "\n\n" + menusb.ToString();
                        ShopTablebtn.BackColor = Color.White;
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
                ButtonFlagSetDefault();
            }
        }

        private void ActErrorMsg()
        {
            MetroMessageBox.Show(this, "잘못된 동작입니다.\n버튼을 다시 선택하여 주세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ButtonFlag1 = 999;
            ButtonFlag2 = 999;
        }

        private void ButtonFlagSetting(int SetButtonFlagValue)
        {
            if (ButtonFlag1 == 999)
            {
                ButtonFlag1 = SetButtonFlagValue;
            }
            else if (ButtonFlag1 != 999 && ButtonFlag2 == 999)
            {
                ButtonFlag2 = SetButtonFlagValue;
            }else
            {
                ActErrorMsg();
            }
        }

        private void ButtonFlagSetDefault()
        {
            ButtonFlag1 = 999;
            ButtonFlag2 = 999;
        }

        private void Table1_Click(object sender, EventArgs e)
        {
            ButtonFlagSetting(1);
            Table1.BackColor = Color.LightGray;
        }

        private void Table2_Click(object sender, EventArgs e)
        {
            ButtonFlagSetting(2);
            Table2.BackColor = Color.LightGray;
        }

        private void Table3_Click(object sender, EventArgs e)
        {
            ButtonFlagSetting(3);
            Table3.BackColor = Color.LightGray;
        }

        private void Table4_Click(object sender, EventArgs e)
        {
            ButtonFlagSetting(4);
            Table4.BackColor = Color.LightGray;
        }

        private void Table5_Click(object sender, EventArgs e)
        {
            ButtonFlagSetting(5);
            Table5.BackColor = Color.LightGray;
        }

        private void Table6_Click(object sender, EventArgs e)
        {
            ButtonFlagSetting(6);
            Table6.BackColor = Color.LightGray;
        }

        private void Table7_Click(object sender, EventArgs e)
        {
            ButtonFlagSetting(7);
            Table7.BackColor = Color.LightGray;
        }

        private void Table8_Click(object sender, EventArgs e)
        {
            ButtonFlagSetting(8);
            Table8.BackColor = Color.LightGray;
        }

        private void Table9_Click(object sender, EventArgs e)
        {
            ButtonFlagSetting(9);
            Table9.BackColor = Color.LightGray;
        }

        private void Table10_Click(object sender, EventArgs e)
        {
            ButtonFlagSetting(10);
            Table10.BackColor = Color.LightGray;
        }

        private void move_Click(object sender, EventArgs e)
        {
            if(ButtonFlag1 != 999 && ButtonFlag2 != 999)
            {
                
            }
            else
            {
                ActErrorMsg();
            }
            ButtonFlagSetDefault();
        }

        private void reservation_Click(object sender, EventArgs e)
        {
            if(ButtonFlag1 != 999 && ButtonFlag2 == 999)
            {
                string getTableNum = "Table" + ButtonFlag1;
                Button ShopTablebtn = this.Controls.Find(getTableNum, true).FirstOrDefault() as Button;
                //MetroMessageBox.Show(this, ButtonFlag1 + "\n" + ButtonFlag2, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if(ShopTablebtn != null)
                {
                    ShopTablebtn.Text = ShopTablebtn.Text + "\n\n예약중";
                }
            }
            else
            {
                ActErrorMsg();
            }

            ButtonFlagSetDefault();
        }

        private void order_Click(object sender, EventArgs e)
        {
            if(ButtonFlag1 != 999 && ButtonFlag2 == 999)
            {
                orderdetailForm orderdetailfrm = new orderdetailForm();
                orderdetailfrm.SetTableNumLabel(ButtonFlag1);
                orderdetailfrm.ShowDialog();

                ButtonFlagSetDefault();
            }
            else
            {
                ActErrorMsg();
            }
        }

        private void inout_Click(object sender, EventArgs e)
        {
            //임시로 새로고침버튼
            SetShopFormDefaultValue();
        }
    }
}
