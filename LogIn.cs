using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fitnes_Club
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.UseSystemPasswordChar = false;
            else
                textBox2.UseSystemPasswordChar = true;
        }

        private void tabPage1_Paint(object sender, PaintEventArgs e)
        {
            string sql = "select name as \"Название\", describtion as \"Описание\" " +
                "from project;";

            Main.Table_Fill("Проект", sql);
            Main.ds.Tables["Проект"].DefaultView.Sort = "Название";

            dataGridView1.DataSource = Main.ds.Tables["Проект"].DefaultView;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Enabled = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Main.Table_Fill("Пользователь", $"select * from usser where login = '{textBox1.Text}' and password = '{textBox2.Text}'");

            try
            {
                if (Main.ds.Tables["Пользователь"].Rows[0]["login"].ToString() == textBox1.Text && Main.ds.Tables["Пользователь"].Rows[0]["password"].ToString() == textBox2.Text)
                {
                    Main.Table_Fill("Проект", "select * from project");

                    BD bd = new BD();
                    Main.tabControl1.TabPages.RemoveAt(0);
                    Main.tabControl1.Controls.Add(bd.tabControl1.TabPages[0]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Неправильный логин или пароль");
            }        
        }
    }
}
