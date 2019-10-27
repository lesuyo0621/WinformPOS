using MetroFramework.Forms;
using MetroFramework;
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
    public partial class loginForm : MetroForm
    {
        public loginForm()
        {
            InitializeComponent();

        }

        private void LogInbtn_Click(object sender, EventArgs e)
        {
            string loginid = IDTextBox.Text;
            string loginpw = PWTextBox.Text;
            string strconn = "Server=localhost;Database=test;User Id=root;Password=1234;Charset=utf8";

            if (String.IsNullOrEmpty(IDTextBox.Text))
            {
                MetroMessageBox.Show(this, "ID를 입력하여 주세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IDTextBox.Focus();
                return;
            }
            else if (String.IsNullOrEmpty(PWTextBox.Text))
            {
                MetroMessageBox.Show(this, "Password를 입력하여 주세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PWTextBox.Focus();
                return;
            }

            MySqlConnection conn = new MySqlConnection(strconn);
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                conn.Open();
                string strlogincheckQ = "select * from shop where sid = '" + IDTextBox.Text + "'";

                cmd.Connection = conn;
                cmd.CommandText = strlogincheckQ;

                MySqlDataReader rdr = cmd.ExecuteReader();

                if (!rdr.HasRows)
                {
                    var Msgresult = MetroMessageBox.Show(this, "등록되지 않은 가맹점입니다.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (Msgresult == DialogResult.OK) IDTextBox.Focus();
                }

                if (rdr.Read())
                {


                    if (rdr["spw"].ToString() == PWTextBox.Text)
                    {
                        var Msgresult = MetroMessageBox.Show(this, "성공", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (Msgresult == DialogResult.OK)
                        {
                            shopForm shopfrm = new shopForm();
                            //shopfrm.Text = rdr["sname"].ToString();
                            shopfrm.Show();
                            Program.ac.MainForm = shopfrm;
                            subposForm subposfrm = new subposForm();
                            subposfrm.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        var Msgresult = MetroMessageBox.Show(this, "아이디와 패스워드가 일치하지 않습니다.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (Msgresult == DialogResult.OK) IDTextBox.Focus();
                    }
                }
                rdr.Dispose();
                rdr.Close();

            }
            catch (Exception E)
            {
                MetroMessageBox.Show(this, "데이터 베이스 오류 :\n" + E.Message + "\n" + E.StackTrace);
            }
            finally
            {
                cmd.Dispose();
                conn.Dispose();
                conn.Close();
            }
            return;
        }

        private void IDTextBox_Enter(object sender, EventArgs e)
        {
            IDTextBox.Text = "";
        }

        private void PWTextBox_Enter(object sender, EventArgs e)
        {
            PWTextBox.Text = "";
        }

        private void IDTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                PWTextBox.Focus();
            }

            if (e.KeyCode == Keys.Enter)
            {
                LogInbtn_Click(sender, e);
            }
        }

        private void PWTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LogInbtn_Click(sender, e);
            }
        }
    }
}
