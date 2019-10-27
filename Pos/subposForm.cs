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
    public partial class subposForm : MetroForm
    {
        string getTableNumFromListView;
        string strconn = "Server=localhost;Database=test;User Id=root;Password=1234;Charset=utf8";

        public subposForm()
        {
            InitializeComponent();
            SetDefaultValue();
        }

        private void SetDefaultValue()
        {
            panel2.Hide();
            panel3.Hide();
            panel4.Hide();
            panel5.Hide();
            panel6.Hide();
            panel7.Hide();
            panel8.Hide();
            panel9.Hide();
            panel10.Hide();
            panel11.Hide();

            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();
            listView5.Items.Clear();
            listView6.Items.Clear();
            listView7.Items.Clear();
            listView8.Items.Clear();
            listView9.Items.Clear();
            listView10.Items.Clear();

            /* 이 방식으로 리스트 추가됨
            List<string> test = new List<string>();
            test.Add("하이");
            test.Add("하이");
            test.Add("하이");
            ListViewItem lvt = new ListViewItem(test.ToArray());
            listView1.Items.Add(lvt);
            */

            string strgetMenuinfoQ;
            string getPanelNum;
            string getListViewNum;

            List<string> MenuInfoList = new List<string>();
            int a = 1;

            Queue<int> NotCookingOrderNumq = new Queue<int>();

            MySqlConnection conn = new MySqlConnection(strconn);
            MySqlCommand cmd = new MySqlCommand();


            try
            {
                conn.Open();
                string strNotCookingOrderNumorderbytimeQ = "select onum, otime from orderlist where ocookstate = 1 order by otime ASC";

                cmd.Connection = conn;
                cmd.CommandText = strNotCookingOrderNumorderbytimeQ;

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    NotCookingOrderNumq.Enqueue((int)rdr[0]);
                }
                rdr.Close();

                foreach (int OrderNum in NotCookingOrderNumq)
                {

                    getPanelNum = "panel" + (a + 1);
                    getListViewNum = "listView" + a;

                    Panel SubPosOrderPanel = this.Controls.Find(getPanelNum, true).FirstOrDefault() as Panel;
                    ListView SubPosListView = SubPosOrderPanel.Controls.Find(getListViewNum, true).FirstOrDefault() as ListView;

                    SubPosOrderPanel.Show();

                    strgetMenuinfoQ = "select otablenum, mname, mquantity from orderlist inner join orderdetail on orderlist.onum = orderdetail.onum " +
                        "inner join menu on orderdetail.mnum = menu.mnum where orderlist.ocookstate = 1 and orderlist.onum = " + OrderNum;

                    cmd.CommandText = strgetMenuinfoQ;
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        MenuInfoList.Add(rdr["otablenum"].ToString());
                        MenuInfoList.Add(rdr["mname"].ToString());
                        MenuInfoList.Add(rdr["mquantity"].ToString());
                        ListViewItem SubPoslvtItem = new ListViewItem(MenuInfoList.ToArray());
                        SubPosListView.Items.Add(SubPoslvtItem);

                        MenuInfoList.Clear();
                    }
                    rdr.Close();
                    a++;
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cmd.Dispose();
                conn.Dispose();
                conn.Close();
                a = 1;
            }
        }

        private void UpdateCookState()
        {
            getTableNumFromListView = listView1.Items[0].SubItems[0].Text;

            MySqlConnection conn = new MySqlConnection(strconn);
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                conn.Open();
                string strUpdateCookStateAtOrderList = "update orderlist set ocookstate = 0 where ocookstate = 1 and otablenum = " + getTableNumFromListView;
                string strUpdateCookStateAtShopTable = "update shoptable set stablecookstate = 0 where stablecookstate = 1 and stablenum = " + getTableNumFromListView;

                cmd.Connection = conn;

                cmd.CommandText = strUpdateCookStateAtOrderList;
                cmd.ExecuteNonQuery();

                cmd.CommandText = strUpdateCookStateAtShopTable;
                cmd.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                MetroMessageBox.Show(this, "DB 업데이트 오류 :\n" + E.Message + "\n" + E.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                cmd.Dispose();
                conn.Dispose();
                conn.Close();
            }
        }

        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void listView2_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void listView3_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void listView4_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void listView5_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void listView6_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void listView7_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void listView8_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void listView9_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void listView10_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void comp1_Click(object sender, EventArgs e)
        {
            UpdateCookState();
            SetDefaultValue();
        }

        private void comp2_Click(object sender, EventArgs e)
        {

        }

        private void comp3_Click(object sender, EventArgs e)
        {

        }

        private void comp4_Click(object sender, EventArgs e)
        {

        }

        private void comp5_Click(object sender, EventArgs e)
        {

        }

        private void comp6_Click(object sender, EventArgs e)
        {

        }

        private void comp7_Click(object sender, EventArgs e)
        {

        }

        private void comp8_Click(object sender, EventArgs e)
        {

        }

        private void comp9_Click(object sender, EventArgs e)
        {

        }

        private void comp10_Click(object sender, EventArgs e)
        {

        }
    }
}
