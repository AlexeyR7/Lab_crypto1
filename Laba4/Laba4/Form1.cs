using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba4
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e_)
        {
            textBox2.Text = "";
            LOG("Шифр RSA");
            var rand = new Random();
            Int64 p, q, n, fi, d, e;
            Int64 m1, m2, encrM;
            m1 = Convert.ToInt64(numericUpDown1.Value);
            p = SieveEratosthenes((Int64)rand.Next(4001, 6000)).Last();
            LOG("Выбор случайного простого числа \t p=", p);
            q = SieveEratosthenes((Int64)rand.Next(4001, 6000)).Last();
            LOG("Выбор случайного простого числа \t q=", q);
            n = p * q;
            LOG("Вычисление произведения p*q \t n=", n);
            fi = (p - 1) * (q - 1);
            LOG("Вычисление (p-1)*(q-1) \t φ=", fi);
            e = get_e(fi);
            LOG("Выбор секретного ключа \t e=", e);
            d = get_d(e, fi);
            LOG("Вычисление секретного ключа \t d=", d);
            encrM = Axmodp(m1, e, n);
            LOG("Шифрование исходного сообщения: ", encrM);
            LOG("Передача сообщение и открытого ключа");

            m2 = Axmodp(encrM, d, n);
            LOG("Расшифрование переданного сообщения: ", m2);
        }


        Int64 Axmodp(Int64 a, Int64 x, Int64 p)
        {
            Int64 y, s;
            Char [] xi = Convert.ToString(x, 2).ToCharArray();
            Array.Reverse(xi);
            y = 1;
            s = a;
            for (int i = 0; i < xi.Length; i++)
            {
                if (xi[i] == '1') { 
                    y = (y * s) % p; 
                }
                s = (s * s) % p;
            }
            return y;
        }
        static List<Int64> SieveEratosthenes(Int64 n)
        {
            var numbers = new List<Int64>();
            for (var i = 2u; i < n; i++){
                numbers.Add(i);
            }
            for (var i = 0; i < numbers.Count; i++)
            {
                for (var j = 2u; j < n; j++){
                    numbers.Remove(numbers[i] * j);
                }
            }
            return numbers;
        }
        Int64 get_e(Int64 fi)
        {
            Int64 F=3, buf;
            for (int i = 0; i < 10; i++)
            {
                buf = (Int64)Math.Pow(2, Math.Pow(2, i)) + 1;
                if (buf < fi) {
                    if (IsPrimeNumber(buf)) F = buf;
                }
                else break;
            }
            return F;
        }
        Int64 get_d(Int64 d, Int64 fi)
        {
            Int64 e = 10;
            while (true)
            {
                if ((e * d) % fi == 1)
                    break;
                else
                    e++;
            }
            return e;
        }


        public static bool IsPrimeNumber(Int64 n)
        {
            var result = true;
            if (n > 1) {
                for (var i = 2u; i < n; i++) {
                    if (n % i == 0) {
                        result = false;
                        break;
                    }
                }
            }
            else { 
                result = false;
            }
            return result;
        }




        void LOG(string s, Int64 i)
        {
            textBox2.Text += s + " "+i.ToString() + Environment.NewLine;
            textBox2.Update();
        }
        void LOG(string s)
        {
            textBox2.Text += s + Environment.NewLine;
            textBox2.Update();
        }

    }
}
