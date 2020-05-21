using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba3
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ra97\Source\Repos\AlexeyR7\Lab_crypto1\Laba3\Laba3\Database1.mdf;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form reg = new Form2();
            reg.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") { MessageBox.Show("Поле Логин не может быть пустым!"); return; };
            string sqlExpression = "SELECT PassHash, Name FROM Users WHERE Login ='" + textBox1.Text + "'";
            SqlDataReader reader;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                reader = command.ExecuteReader();
                if (!reader.HasRows) { MessageBox.Show("Пользователь с таким логином не найден!"); return; }
                reader.Read();
                string s1 = reader[0].ToString();
                string s2 = Crypto.GetHash(textBox2.Text);
                if (reader[0].ToString().TrimEnd() == Crypto.GetHash(textBox2.Text))
                {
                    MessageBox.Show("Здравствуйте, " + reader["Name"].ToString() + "!", "Успешно!");
                }
                else { MessageBox.Show("Неверный логин или пароль!"); return; }
            }
        }
    }
}
