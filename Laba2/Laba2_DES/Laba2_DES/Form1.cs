using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace laba2_DES
{
    public partial class Form1 : Form
    {
        DES DESalg;
        public Form1()
        {
            InitializeComponent();
            DESalg = DES.Create();
            byte[] b = {241,227,34,28,171,255,175,35};
            DESalg.IV = b;
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            printkey();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename = saveFileDialog1.FileName;
                string sData = textBox1.Text;
                EncryptTextToFile(sData, filename, DESalg.Key, DESalg.IV);
              
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string filename = openFileDialog1.FileName;
                string Final = DecryptTextFromFile(filename, DESalg.Key, DESalg.IV);
                textBox1.Text = Final;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
        static void startDES(string FileName, string sData)
        {
            try
            {
                // Create a new DES object to generate a key
                // and initialization vector (IV).
                DES DESalg = DES.Create();

                // Create a string to encrypt.
                

                // Encrypt text to a file using the file name, key, and IV.
                EncryptTextToFile(sData, FileName, DESalg.Key, DESalg.IV);

                // Decrypt the text from a file using the file name, key, and IV.
                string Final = DecryptTextFromFile(FileName, DESalg.Key, DESalg.IV);

                // Display the decrypted string to the console.
                MessageBox.Show(Final);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void EncryptTextToFile(String Data, String FileName, byte[] Key, byte[] IV)
        {
            try
            {
                // Create or open the specified file.
                FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate);

                // Create a new DES object.
                DES DESalg = DES.Create();

                // Create a CryptoStream using the FileStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(fStream,
                    DESalg.CreateEncryptor(Key, IV),
                    CryptoStreamMode.Write);

                // Create a StreamWriter using the CryptoStream.
                StreamWriter sWriter = new StreamWriter(cStream);

                // Write the data to the stream 
                // to encrypt it.
                sWriter.WriteLine(Data);

                // Close the streams and
                // close the file.
                sWriter.Close();
                cStream.Close();
                fStream.Close();
            }
            catch (CryptographicException e)
            {
                MessageBox.Show("A Cryptographic error occurred: {0}", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("A file error occurred: {0}", e.Message);
            }
        }

        public static string DecryptTextFromFile(String FileName, byte[] Key, byte[] IV)
        {
            try
            {
                // Create or open the specified file. 
                FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate);

                // Create a new DES object.
                DES DESalg = DES.Create();

                // Create a CryptoStream using the FileStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(fStream,
                    DESalg.CreateDecryptor(Key, IV),
                    CryptoStreamMode.Read);

                // Create a StreamReader using the CryptoStream.
                StreamReader sReader = new StreamReader(cStream);

                // Read the data from the stream 
                // to decrypt it.
                string val = sReader.ReadLine();

                // Close the streams and
                // close the file.
                sReader.Close();
                cStream.Close();
                fStream.Close();

                // Return the string. 
                return val;
            }
            catch (CryptographicException e)
            {
                MessageBox.Show("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("A file error occurred: {0}", e.Message);
                return null;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, textBox1.Text);
            System.IO.File.WriteAllBytes(filename, DESalg.Key);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            DESalg.Key = System.IO.File.ReadAllBytes(filename);
            printkey();
        }
        
        void printkey()
        {
            string buff = "";
            foreach (var item in DESalg.Key)
            {
                buff += item.ToString() + " ";
            }
            textBox2.Text = buff;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
        }
    }
}
