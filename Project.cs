using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Fitnes_Club
{
    public partial class Project : Form
    {
        public Project()
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
            textBox1.Text = Main.ds.Tables["Проект"].Rows[n]["id"].ToString();
            textBox2.Text = Main.ds.Tables["Проект"].Rows[n]["name"].ToString();
            textBox3.Text = Main.ds.Tables["Проект"].Rows[n]["describtion"].ToString();
            textBox1.Enabled = false;
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            Main.Table_Fill("Проект", "Select * from project");
            if (Main.ds.Tables["Проект"].Rows.Count > n)
            {
                FieldForm_Fill();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (n < Main.ds.Tables["Проект"].Rows.Count) n++;
            if (Main.ds.Tables["Проект"].Rows.Count > n) {
                FieldForm_Fill();
            } else{
                FieldForm_Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FieldForm_Clear();
            n = Main.ds.Tables["Проект"].Rows.Count;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (n > 0) {
                n--; FieldForm_Fill();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (Main.ds.Tables["Проект"].Rows.Count > 0) {
                n = 0; FieldForm_Fill();
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string sql;
            if (n < Main.ds.Tables["Проект"].Rows.Count) {
                sql = "Update project set name = '" + textBox2.Text + "', describtion ='" + textBox3.Text + "'";
                if (!Main.Modification_Execute(sql))
                    return;
            }
            else {
                sql = "Insert into project (name, describtion) values('" + textBox2.Text + "', '" + textBox3.Text +"')";
                if (!Main.Modification_Execute(sql))
                    return;
                textBox1.Enabled = false;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string message = "Вы точно хотите удалить Проект с кодом " + textBox1.Text + "?";
            string caption = "Удаление Проекта";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            Main.Table_Fill("ПроектЗадача", $"select * from \"projectTask\" where \"projectId\" = {textBox1.Text}");
            for (int i = 0; i < Main.ds.Tables["ПроектЗадача"].DefaultView.Count; i++) {
               int taskId = int.Parse(Main.ds.Tables["ПроектЗадача"].DefaultView[i]["taskId"].ToString());
                Main.Modification_Execute("Delete from \"projectTask\" where taskId = " + taskId + "and projectId = " + textBox1.Text);
                Main.Modification_Execute("Delete from task where id = " + taskId);
            }
            string sql = "Delete from project where id=" + textBox1.Text;
            Main.Modification_Execute(sql);
            Main.ds.Tables["Проект"].Rows.RemoveAt(n);
            if (Main.ds.Tables["Проект"].Rows.Count > n)
                FieldForm_Fill();
            else
                FieldForm_Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Main.tabControl1.Controls.Remove(Main.tabControl1.SelectedTab);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Task task = new Task();
            task.Iden(int.Parse(textBox1.Text));
            Main.tabControl1.Controls.Add(task.tabControl1.TabPages[0]);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Report rep = new Report();
            rep.Iden(int.Parse(textBox1.Text));
            Main.tabControl1.Controls.Add(rep.tabControl1.TabPages[0]);
        }
    }
}
