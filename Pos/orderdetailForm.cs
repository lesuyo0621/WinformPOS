using MetroFramework.Forms;
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
    public partial class orderdetailForm : MetroForm
    {
        public orderdetailForm()
        {
            InitializeComponent();
        }


        private void Menu1_Click(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            ListViewItem item;
            item = new ListViewItem("아메리카노");
            item.SubItems.Add("1");
            item.SubItems.Add("1000");
            listView1.Items.Add(item);
            listView1.EndUpdate();
        }
    }
}
