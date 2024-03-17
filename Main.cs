using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Fitnes_Club
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public static NpgsqlConnection connection = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=qwerty; Database=tasks;");

        public static DataSet ds = new DataSet();

        public static TabControl tabControl1 = new TabControl();

        public static void Table_Fill(string name, string sql)
        {
            if (ds.Tables[name] != null)
                ds.Tables[name].Clear();
            NpgsqlDataAdapter dat;
            dat = new NpgsqlDataAdapter(sql, connection);
            dat.Fill(ds, name);
            connection.Close();
        }

        public static bool Modification_Execute(string sql)
        {
            NpgsqlCommand com;
            com = new NpgsqlCommand(sql, connection);
            connection.Open();
            try
            {
                com.ExecuteNonQuery();
            }
            catch (NpgsqlException)
            {
                MessageBox.Show("Обновление базы данных не было выполнено либо из-за некорректно указанных" + " обновляемых данных  либо отсутствующих, но при этом обязательных!!!", "Ошибка");
                connection.Close(); return false;
            }
            connection.Close();
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.Size = new Size(1184, 647);
            this.Controls.Add(tabControl1);

            LogIn login = new LogIn();
            tabControl1.Controls.Add(login.tabControl1.TabPages[0]);
        }
    }
}
