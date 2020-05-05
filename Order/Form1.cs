using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Order
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 填入銷售項目到菜單中
            DataGridViewRowCollection rows = dataGridViewMenu.Rows;
            rows.Add(new Object[] { "滾珠軸承", 25 });
            rows.Add(new Object[] { "聯軸器",850  });
            rows.Add(new Object[] { "線性導桿", 150 });
            rows.Add(new Object[] { "線性襯套", 180 });
            rows.Add(new Object[] { "間隔環", 200 });
            rows.Add(new Object[] { "軸承座", 150 });
            rows.Add(new Object[] { "軸節機構", 750 });
            rows.Add(new Object[] { "旋轉軸", 800 });
            rows.Add(new Object[] { "固定環", 300 });
            rows.Add(new Object[] { "M4螺絲", 500 });
            rows.Add(new Object[] { "M4螺帽", 500 });

            buttonAdd.Enabled = false;

        }

        private void dataGridViewMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 選取某項目，將該項目的品名價格放入待購項目中。
            if (dataGridViewMenu.Rows[e.RowIndex].Cells[0].Value == null)
                return;
            buttonName.Text = dataGridViewMenu.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBoxPrice.Text = dataGridViewMenu.Rows[e.RowIndex].Cells[1].Value.ToString();
            numericUpDownNumber.Value = 1;
            buttonAdd.Enabled = true;

        }

        private void numericUpDownNumber_ValueChanged(object sender, EventArgs e)
        {
            calculate();
        }

        String name;
        double price;
        double number;
        double sumtotal;

        private void calculate()
        {
            name = buttonName.Text;
            price = double.Parse(textBoxPrice.Text);
            number = (double)numericUpDownNumber.Value;
            sumtotal = price * number;
            textBoxTotal.Text = sumtotal.ToString();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            calculate();
            dataGridViewOrder.Rows.Add(new Object[] { name, price, number, sumtotal });
        }

        private void calculatetotal()
        {
            double sum = 0.0;
            for (int i = 0; i < dataGridViewOrder.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridViewOrder.Rows[i]; //和前面 DataGridViewRow使用不一樣
                if (row.Cells[0].Value != null)
                    sum = sum + (double)row.Cells[3].Value;
            }
            textBox3Total.Text = sum.ToString();

        }

        private void button3Total_Click(object sender, EventArgs e)
        {
            calculatetotal();

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = "Data Source=(local);Initial Catalog=OrderTotal;Integrated Security=True";
            cn.Open();
            int k = dataGridViewOrder.Rows.Count - 1;
            string empId, empPrice, empNumber, empTotal, a;
            for (int i = 0; i < k; i++)
            {
                empId = dataGridViewOrder.Rows[i].Cells[0].Value.ToString();
                empPrice = dataGridViewOrder.Rows[i].Cells[1].Value.ToString();
                empNumber = dataGridViewOrder.Rows[i].Cells[2].Value.ToString();
                empTotal = dataGridViewOrder.Rows[i].Cells[3].Value.ToString();
                a = "INSERT INTO OrderTotal(品名,單價,數量,總額小計)" +
                    "VALUES('" + empId + "','" + empPrice + "','" + empNumber + "','" + empTotal + "')";

                SqlCommand cmd = new SqlCommand(a, cn);
                cmd.ExecuteNonQuery();

            }
            cn.Close();
            dataGridViewOrder.Rows.Clear();
        }

        private void textBoxTotal_TextChanged(object sender, EventArgs e)
        {

        }
    }

      
 }   
