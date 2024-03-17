using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace Fitnes_Club
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        public static int projID = 0;

        public void Iden(int n)
        {
            projID = n;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Excel.Application Excel_ = new Excel.Application();
            Excel_.Visible = true;
            Excel.Workbook WorkBook_ = Excel_.Workbooks.Add();
            Excel.Worksheet Sheet_ = (Excel.Worksheet)WorkBook_.Sheets[1];

            Sheet_.Cells[1, 1].Value = "Отчёт";
            Sheet_.Range[Sheet_.Cells[1, 1], Sheet_.Cells[1, 6]].Merge();
            Sheet_.Cells[1, 1].HorizontalAlignment = 3;

            // Вывод в ячейки заголовков таблицы
            Sheet_.Cells[2, 1].Value = dataGridView1.Columns[0].HeaderText; // Название проекта
            Sheet_.Cells[2, 2].Value = dataGridView1.Columns[1].HeaderText; // Название задачи
            Sheet_.Cells[2, 3].Value = dataGridView1.Columns[2].HeaderText; // Статус
            Sheet_.Cells[2, 4].Value = dataGridView1.Columns[3].HeaderText; // Дата создания

            Sheet_.Range[Sheet_.Cells[2, 1], Sheet_.Cells[2, 6]].HorizontalAlignment = 3;

            int n = 3;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                Sheet_.Cells[n, 1].Value = dataGridView1.Rows[i].Cells[0].Value; // Название проекта
                Sheet_.Cells[n, 2].Value = dataGridView1.Rows[i].Cells[1].Value; // Название задачи
                Sheet_.Cells[n, 3].Value = dataGridView1.Rows[i].Cells[2].Value; // Статус
                Sheet_.Cells[n, 4].Value = dataGridView1.Rows[i].Cells[3].Value; // Дата создания

                n++;
            }

            // Выравнивание содержимого диапазона ячеек с логическими значениями по центру
            Sheet_.Range[Sheet_.Cells[3, 4], Sheet_.Cells[3 + dataGridView1.RowCount, 5]].HorizontalAlignment = 3;
            // Выравнивание содержимого диапазона ячеек с денежными значениями по правому краю
            Sheet_.Range[Sheet_.Cells[3, 6], Sheet_.Cells[3 + dataGridView1.RowCount, 6]].HorizontalAlignment = 4;
            // Установка границ ячеек таблицы
            Sheet_.Range[Sheet_.Cells[2, 1], Sheet_.Cells[2 + dataGridView1.RowCount, 6]].Borders.LineStyle =
                Excel.XlLineStyle.xlContinuous;
            Sheet_.Cells.Columns.EntireColumn.AutoFit();

            // Закрытие приложения Excel
            WorkBook_.Close();
            Excel_.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(Excel_);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string message = "Вы точно хотите удалить информацию обо всех Отчётах?";
            string caption = "Очистка Отчётов";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
       
            string sql = "DELETE FROM agreement";
            Main.Modification_Execute(sql);
            Main.ds.Tables["Отчёт"].Clear(); 


        }

        private void button4_Click(object sender, EventArgs e)
        {
            string kod;
            try
            {
                kod = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Код"].Value.ToString();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Hе указан удаляемый экземпляр!!!", "Ошибка"); return;
            }
;
            string message = "Вы точно хотите удалить Отчёт " + kod + "?";
            string caption = "Удаление Отчёта";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult rezult = MessageBox.Show(message, caption, buttons);
            if (rezult == DialogResult.No) { return; }

            string sql = "DELETE FROM agreement WHERE id=" + kod;
            Main.Modification_Execute(sql);

            for (int i = Main.ds.Tables["Отчёт"].Rows.Count - 1; i >= 0; i--)
            {
                if (Main.ds.Tables["Отчёт"].Rows[i]["Код"].ToString() == kod)
                {
                    Main.ds.Tables["Отчёт"].Rows.RemoveAt(i);
                    dataGridView1.CurrentCell = null;
                    return;
                }
            }
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            string sql = "SELECT p.name AS \"Название проекта\", t.name AS \"Название задачи\", t.status AS Cтатус, t.\"createAt\" AS \"Дата создания\"" +
                    "FROM \"projectTask\" pt " +
                    $"JOIN project p ON pt.\"projectId\" = p.id JOIN task t ON pt.\"taskId\" = t.id WHERE pt.\"projectId\" = {projID} and t.status = 'Завершено'";

            Main.Table_Fill("Отчёт", sql);

            dataGridView1.DataSource = Main.ds.Tables["Отчёт"].DefaultView;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Enabled = false;
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
        }
    }
}
