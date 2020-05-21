using MetroFramework.Forms;
using MetroFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Pos
{
    public partial class orderdetailForm : MetroForm
    {
        private int TableNum;

        public orderdetailForm()
        {
            InitializeComponent();
        }
        
        public void SetTableNumLabel(int PassValueTableNum)
        {
            TableNum = PassValueTableNum;
            TableNumLabel.Text = TableNum + "번 테이블";
            SetOrderDetailFormDefaultValue();
            addMenulist();
        }

        private void SetOrderDetailFormDefaultValue()
        {
            string strconn = "Server=localhost;Database=test;User Id=root;Password=1234;Charset=utf8";
            List<string> OrderDetailList = new List<string>();

            MySqlConnection conn = new MySqlConnection(strconn);
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                conn.Open();

                string strOrderDetailListViewData = "select mname, mquantity, (mprice*mquantity) as price from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum " +
                    "inner join menu on orderdetail.mnum = menu.mnum where ostate = 1 and otablenum = " + TableNum;

                cmd.Connection = conn;
                cmd.CommandText = strOrderDetailListViewData;

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    OrderDetailList.Add(rdr["mname"].ToString());
                    OrderDetailList.Add(rdr["mquantity"].ToString());
                    OrderDetailList.Add(rdr["price"].ToString());
                    ListViewItem OrderDetaillvtItem = new ListViewItem(OrderDetailList.ToArray());
                    listViewOrderdetail.Items.Add(OrderDetaillvtItem);

                    OrderDetailList.Clear();
                }
                rdr.Close();

                string strTotalPriceData = "select sum(mquantity * mprice) as totalprice from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum " +
                    "inner join menu on orderdetail.mnum = menu.mnum where ostate = 1 and otablenum = " + TableNum;

                cmd.CommandText = strTotalPriceData;
                rdr = cmd.ExecuteReader();
                rdr.Read();

                TotalPriceTextBox.Text = rdr["totalprice"].ToString();
                DueMoneyTextBox.Text = rdr["totalprice"].ToString();
                rdr.Close();
            }
            catch (Exception E)
            {
                MetroMessageBox.Show(this, "오류입니다\n" + E.Message + "\n" + E.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
            finally
            {
                cmd.Dispose();
                conn.Dispose();
                conn.Close();
            }
        }

        private void UpdateOrderState()
        {
            string strconn = "Server=localhost;Database=test;User Id=root;Password=1234;Charset=utf8";
            string strUpdateOrderState = "update orderlist set ostate = 0 where ostate = 1 and otablenum = " + TableNum;
            string strUpdateTableState = "update shoptable set stablestate = 0 where stablestate = 1 and stablenum = " + TableNum;

            MySqlConnection conn = new MySqlConnection(strconn);
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = strUpdateOrderState;
                cmd.ExecuteNonQuery();

                cmd.CommandText = strUpdateTableState;
                cmd.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                MetroMessageBox.Show(this, "DB 업데이트 오류 : \n" + E.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void Calc()
        {
            int CalcDueMoney = Convert.ToInt32(DueMoneyTextBox.Text) - Convert.ToInt32(CalcTextBox.Text);
            int CalcTakeMoney = Convert.ToInt32(TakeMoneyTextBox.Text) + Convert.ToInt32(CalcTextBox.Text);
            
            if (Convert.ToInt32(DueMoneyTextBox.Text) == 0)
            {
                MetroMessageBox.Show(this, "계산이 완료되었습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                /* 초과 계산 원할 경우
                int CalcChangeMoney = Convert.ToInt32(ChangeMoneyTextBox.Text) + Convert.ToInt32(CalcTextBox.Text);
                ChangeMoneyTextBox.Text = CalcChangeMoney.ToString();
                TakeMoneyTextBox.Text = CalcTakeMoney.ToString();
                */
            }
            else
            {
                if (CalcDueMoney <= 0)
                {
                    DueMoneyTextBox.Text = "0";
                    TakeMoneyTextBox.Text = CalcTakeMoney.ToString();
                    ChangeMoneyTextBox.Text = CalcDueMoney.ToString().Replace("-", "");

                    UpdateOrderState();
                }
                else
                {
                    DueMoneyTextBox.Text = CalcDueMoney.ToString();
                    TakeMoneyTextBox.Text = CalcTakeMoney.ToString();
                }
            }
            CalcTextBox.Clear();
        }

        private void addMenulist()
        {
            string strconn = "Server=localhost;Database=test;User Id=root;Password=1234;Charset=utf8";
       
            MySqlConnection conn = new MySqlConnection(strconn);
            MySqlCommand cmd = new MySqlCommand();

            Queue<int> UsingMenuNumq = new Queue<int>();

            try
            {
                conn.Open();
                string getMenuNum = "select mNum from menu order by mnum ASC";
                string getMenuBtnNum;

                cmd.Connection = conn;
                cmd.CommandText = getMenuNum;

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UsingMenuNumq.Enqueue((int)rdr[0]);
                }

                rdr.Close();

                foreach (int MenuNum in UsingMenuNumq)
                {
                    getMenuBtnNum = "menu" + MenuNum;
                    string strGetMenuList = "select mname, mprice from menu where mnum = " + MenuNum;
                    cmd.CommandText = strGetMenuList;
                    rdr = cmd.ExecuteReader();

                    StringBuilder menusb = new StringBuilder();

                    while (rdr.Read())
                    {
                        menusb.Append(rdr["mname"].ToString() + "\n" + rdr["mprice"].ToString() + "\n");
                    }

                    rdr.Close();

                    Button OrderDetailAddMenubtn = this.Controls.Find(getMenuBtnNum, true).FirstOrDefault() as Button;

                    if (OrderDetailAddMenubtn != null)
                    {
                        OrderDetailAddMenubtn.Text = menusb.ToString();
                    }

                    menusb.Clear();
                    }
                }
            catch (Exception E)
            {
                MetroMessageBox.Show(this, "오류입니다\n" + E.Message + "\n" + E.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
            finally
            {
                cmd.Dispose();
                conn.Dispose();
                conn.Close();
            }        
        }

        private void Menu1_Click(object sender, EventArgs e)
        {
            listViewOrderdetail.BeginUpdate();
            ListViewItem item;
            item = new ListViewItem("아메리카노");
            item.SubItems.Add("1");
            item.SubItems.Add("1000");
            listViewOrderdetail.Items.Add(item);
            listViewOrderdetail.EndUpdate();
        }

        private void listViewOrderdetail_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listViewOrderdetail.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void num1_Click(object sender, EventArgs e)
        {
            CalcTextBox.AppendText("1");
        }

        private void num2_Click(object sender, EventArgs e)
        {
            CalcTextBox.AppendText("2");
        }

        private void num3_Click(object sender, EventArgs e)
        {
            CalcTextBox.AppendText("3");
        }

        private void num4_Click(object sender, EventArgs e)
        {
            CalcTextBox.AppendText("4");
        }

        private void num5_Click(object sender, EventArgs e)
        {
            CalcTextBox.AppendText("5");
        }

        private void num6_Click(object sender, EventArgs e)
        {
            CalcTextBox.AppendText("6");
        }

        private void num7_Click(object sender, EventArgs e)
        {
            CalcTextBox.AppendText("7");
        }

        private void num8_Click(object sender, EventArgs e)
        {
            CalcTextBox.AppendText("8");
        }

        private void num9_Click(object sender, EventArgs e)
        {
            CalcTextBox.AppendText("9");
        }

        private void num0_Click(object sender, EventArgs e)
        {
            CalcTextBox.AppendText("0");
        }

        private void num00_Click(object sender, EventArgs e)
        {
            CalcTextBox.AppendText("00");
        }

        private void clr_Click(object sender, EventArgs e)
        {
            CalcTextBox.Text = CalcTextBox.Text.Remove(CalcTextBox.Text.Length-1, 1);
        }

        private void cash_Click(object sender, EventArgs e)
        {
            Calc();
        }

        private void card_Click(object sender, EventArgs e)
        {
            Calc();
        }
    }
}
