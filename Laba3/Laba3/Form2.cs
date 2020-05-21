using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba3
{
    public partial class Form2 : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ra97\Source\Repos\AlexeyR7\Lab_crypto1\Laba3\Laba3\Database1.mdf;Integrated Security=True";
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") { MessageBox.Show("Поле Имя не может быть пустым!"); return; };
            if (textBox2.Text == "") { MessageBox.Show("Поле Логин не может быть пустым!"); return; };
            if (textBox3.Text == "") { MessageBox.Show("Поле Пароль не может быть пустым!"); return; };
            int number = 10;
            string sqlExpression = "SELECT COUNT(*) FROM Users WHERE Login ='" + textBox2.Text + "'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                number = (int)command.ExecuteScalar();
            }
            if(number > 0) { MessageBox.Show("Пользователь с таким логином уже существует!"); return; }
            string Hash = Crypto.GetHash(textBox3.Text);
            sqlExpression = "INSERT INTO Users (Name, Login, PassHash) VALUES ('"+ textBox1.Text + "', '" + textBox2.Text + "','" + Hash + "')";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                number = command.ExecuteNonQuery();
            }
            if (number == 1) { MessageBox.Show("Успешно!"); Close(); } else { MessageBox.Show("Ошибка при создании нового пользователя");}
        }
    }
}
