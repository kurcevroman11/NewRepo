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
    public partial class User : Form
    {
        public User()
        {
            InitializeComponent();
        }

        public static int n = 0;


        private void FieldForm_Clear()
        {
            textBox1.Text = "0";
            textBox2.Text = "";
            textBox3.Text = "";

            textBox1.Enabled = true; textBox1.Focus();
        }

        private void FieldForm_Fill()
        {
            textBox1.Text = Main.ds.Tables["Пользователь"].Rows[n]["id"].ToString();
            textBox2.Text = Main.ds.Tables["Пользователь"].Rows[n]["fio"].ToString();
            textBox3.Text = Main.ds.Tables["Пользователь"].Rows[n]["login"].ToString();
            textBox4.Text = Main.ds.Tables["Пользователь"].Rows[n]["password"].ToString();

            textBox1.Enabled = false;
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            Main.Table_Fill("Пользователь", "Select * from usser");
            if (Main.ds.Tables["Пользователь"].Rows.Count > n)
            {
                FieldForm_Fill();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (n < Main.ds.Tables["Пользователь"].Rows.Count) n++;
            if (Main.ds.Tables["Пользователь"].Rows.Count > n)
            {
                FieldForm_Fill();
            }
            else 
            {
                FieldForm_Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FieldForm_Clear();
            n = Main.ds.Tables["Пользователь"].Rows.Count;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (n > 0)
            {
                n--; FieldForm_Fill();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Main.ds.Tables["Пользователь"].Rows.Count > 0)
            {
                n = 0; FieldForm_Fill();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string sql;
            if (n < Main.ds.Tables["Пользователь"].Rows.Count)
            {
                sql = $"Update usser set fio = {textBox2.Text}, login = {textBox3.Text}, password = {textBox4.Text} where id = {textBox1.Text}" ;
                if (!Main.Modification_Execute(sql))
                    return;

                Main.ds.Tables["Пользователь"].Rows[n].ItemArray = new object[] { textBox1.Text, textBox3.Text};
            }
            else
            {
                sql = $"Insert into usser (fio, login, password) values('{textBox2.Text}', '{textBox3.Text}', '{textBox4.Text}')";
                if (!Main.Modification_Execute(sql))
                    return;
                textBox1.Enabled = false;

                Main.ds.Tables["Пользователь"].Rows.Add(new object[] { textBox1.Text, textBox2.Text, textBox3.Text });
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string message = "Вы точно хотите удалить Пользоваетеля с кодом " + textBox1.Text + "?";
            string caption = "Удаление Пользоваетеля";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }

          
            string sql = "Delete from task_usser_relation where usser_id =" + textBox1.Text;
            Main.Modification_Execute(sql);

            sql = "Delete from usser where id =" + textBox1.Text;
            Main.Modification_Execute(sql);

            Main.ds.Tables["Пользователь"].Rows.RemoveAt(n);

            if (Main.ds.Tables["Пользователь"].Rows.Count > n)
                FieldForm_Fill();
            else
                FieldForm_Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Main.tabControl1.Controls.Remove(Main.tabControl1.SelectedTab);
        }
    }
}
