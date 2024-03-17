using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fitnes_Club
{
    public partial class Task : Form
    {
        public Task()
        {
            InitializeComponent();
        }
        public static int n = 0;
        public static int projID = 0;

        public void Iden(int n)
        {
            projID = n;
        }

        private void FieldForm_Clear()
        {
            textBox1.Text = "0";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";

            textBox1.Enabled = true; textBox1.Focus();
        }

        private void FieldForm_Fill()
        {
            textBox1.Text = Main.ds.Tables["Задача"].Rows[n]["id"].ToString();
            textBox2.Text = Main.ds.Tables["Задача"].Rows[n]["name"].ToString();
            textBox3.Text = Main.ds.Tables["Задача"].Rows[n]["descrption"].ToString();
            textBox4.Text = Main.ds.Tables["Задача"].Rows[n]["createAt"].ToString();
            comboBox1.Text = Main.ds.Tables["Задача"].Rows[n]["status"].ToString();
            comboBox2.Text = Main.ds.Tables["Задача"].Rows[n]["fio"].ToString();

            textBox1.Enabled = false;
            textBox4.Enabled = false;
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            Main.Table_Fill("Пользователи", $"select fio from usser");

            List<string> us = new List<string>();
            for (int i = 0; i < Main.ds.Tables["Пользователи"].DefaultView.Count; i++)
            {
                us.Add(Main.ds.Tables["Пользователи"].DefaultView[i]["fio"].ToString());
            }
            // Присваиваем массив данных свойству DataSource ComboBox
            comboBox2.DataSource = us;

            // Теперь вызываем метод Refresh, чтобы обновить отображение ComboBox
            comboBox2.Refresh();

            textBox1.Enabled = false;

            Main.Table_Fill("Задача", $"SELECT pt.\"taskId\", t.id, t.name, t.descrption, t.\"createAt\", t.status, u.fio " +
                $"FROM \"projectTask\" pt JOIN task t ON pt.\"taskId\" = t.id JOIN task_usser_relation tur ON t.id = tur.task_id " +
                $"JOIN usser u ON tur.usser_id = u.id WHERE pt.\"projectId\" = {projID};");
            if (Main.ds.Tables["Задача"].Rows.Count > n)
            {
                FieldForm_Fill();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (n < Main.ds.Tables["Задача"].Rows.Count) n++;
            if (Main.ds.Tables["Задача"].Rows.Count > n)
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
            n = Main.ds.Tables["Задача"].Rows.Count;
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
            if (Main.ds.Tables["Задача"].Rows.Count > 0)
            {
                n = 0; FieldForm_Fill();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string sql;
            if (n < Main.ds.Tables["Задача"].Rows.Count)
            {
                sql = $"update task set name = {textBox2.Text}, descrption = {textBox3.Text}, status = {comboBox1.Text}";
                if (!Main.Modification_Execute(sql))
                    return;
            }
            else
            {
                Main.Table_Fill("Пользователи", $"select id from usser where fio = '{comboBox2.Text}'");
                int userId = 0;
                for (int i = 0; i < Main.ds.Tables["Пользователи"].DefaultView.Count; i++) {
                    userId = int.Parse(Main.ds.Tables["Пользователи"].DefaultView[i]["id"].ToString());
                }
                DateTime tim = DateTime.Now;
                sql = $"Insert into task (name, descrption, status, \"createAt\")" +
                    $" values('{textBox2.Text}', '{textBox3.Text}', '{comboBox1.Text}', '{tim}')";              
                if (!Main.Modification_Execute(sql))
                    return;
                int id = 0;
                Main.Table_Fill("Задача", $"select id from task where \"createAt\" = '{tim}'");
                for (int i = 0; i < Main.ds.Tables["Задача"].DefaultView.Count; i++) {
                    id = int.Parse(Main.ds.Tables["Задача"].DefaultView[i]["id"].ToString());
                    Main.Modification_Execute($"insert into task_usser_relation (task_id, usser_id) values({id}, {userId})");
                    Main.Modification_Execute($"insert into \"projectTask\" (\"projectId\", \"taskId\") values({projID}, {id})");
                }
                textBox1.Enabled = false;               
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string message = "Вы точно хотите удалить Задачаа с кодом " + textBox1.Text + "?";
            string caption = "Удаление Задачаа";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql = "Delete from \"projectTask\" where \"taskId\" =" + textBox1.Text;
            Main.Modification_Execute(sql);
            sql = "Delete from task where id =" + textBox1.Text;
            Main.Modification_Execute(sql);
            Main.ds.Tables["Задача"].Rows.RemoveAt(n);
            if (Main.ds.Tables["Задача"].Rows.Count > n)
                FieldForm_Fill();
            else
                FieldForm_Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Main.tabControl1.Controls.Remove(Main.tabControl1.SelectedTab);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
