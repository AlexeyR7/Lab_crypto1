using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba2_Vernam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            encryptdecrypt();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            encryptdecrypt();
        }

        void encryptdecrypt()
        {
            try
            {
                openFileDialog1.Title = "Открыть исходный файл";
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename1 = openFileDialog1.FileName;
                string text = System.IO.File.ReadAllText(filename1);
                textBox1.Text = text;
                openFileDialog1.Title = "Открыть файл ключа";
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string filenameKey = openFileDialog1.FileName;
                string key_ = System.IO.File.ReadAllText(filenameKey);
                textBox2.Text = key_;

                if (text.Length > key_.Length) { MessageBox.Show("Ошибка! Длина ключа меньше длины сообщения"); return; };
                string buff = ""; int jj = 0;
                foreach (char character in text)
                {
                    buff += (char)(character ^ key_[jj++]);
                }

                saveFileDialog1.Title = "Сохранить результат";
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename2 = saveFileDialog1.FileName;
                textBox3.Text = buff;
                System.IO.File.WriteAllText(filename2, buff);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        
    }
}
