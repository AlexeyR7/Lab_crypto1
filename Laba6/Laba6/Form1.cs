using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Int64 Dlogarithm(Int64 y, Int64 a, Int64 p)
        {
            for (Int64 i = 0; i < p; i++)
            {
                if (Axmodp(a, i, p) == y) return i;
            }
            return -1;
        }
        Int64 Axmodp(Int64 a, Int64 x, Int64 p)
        {
            Int64 y, s;
            Char[] xi = Convert.ToString(x, 2).ToCharArray();
            Array.Reverse(xi);
            y = 1;
            s = a;
            for (Int64 i = 0; i < xi.Length; i++)
            {
                if (xi[i] == '1')
                {
                    y = (y * s) % p;
                }
                s = (s * s) % p;
            }
            return y;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int64 y = Convert.ToInt64(numericUpDown1.Value);
            Int64 a = Convert.ToInt64(numericUpDown2.Value);
            Int64 p = Convert.ToInt64(numericUpDown3.Value);
            textBox1.Text = "";
            Int64 result;
            result = Dlogarithm(y, a, p);
            textBox1.Text = "X = "+ result + "";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Int64 y = Convert.ToInt64(numericUpDown1.Value);
            Int64 a = Convert.ToInt64(numericUpDown2.Value);
            Int64 p = Convert.ToInt64(numericUpDown3.Value);
            textBox1.Text = "";
            Int64 result;
            result = Shanks(y, a, p);
            textBox1.Text += "X = " + result + "";
        }


        public Int64 Shanks(Int64 y, Int64 a, Int64 p)
        {
            Int64 k = 1, m = 1;
            int i=-1,j=-1;
            for (k = 2; k < p; k++)
            {
                for (m = k; m < k + 5; m++)
                {
                    if (k * m > p) break;
                }
                if (k * m > p) break;
            }
            textBox1.Text += "m = " + m.ToString() + "\t k=" + k.ToString() + Environment.NewLine;

            List<Int64> list1 = new List<Int64>(), list2 = new List<Int64>();

            for (Int64 im = 0; im < m; im++)
            {
                list1.Add((Int64)(Axmodp(a,im,p) * y % p));
            }
            for (Int64 ik = 1; ik <= k; ik++)
            {
                list2.Add((Int64)(Axmodp(a, ik * m, p)));
            }
            textBox1.Text += "1 ряд чисел: " + Environment.NewLine;
            foreach (var item in list1) { 
                textBox1.Text += " " + item.ToString(); 
            }
            textBox1.Text += Environment.NewLine + "2 ряд чисел: " + Environment.NewLine;
            foreach (var item in list2)
            {
                textBox1.Text += " " + item.ToString();
            }
            textBox1.Text += Environment.NewLine;

            bool s = false;
            for (i = 1; i < list2.Count; i++)
            {
                for (j = 0; j < list1.Count; j++)
                {
                    if (list2[i-1] == list1[j]){ s = true; break;}
                }
                if (s) break;
            }
            textBox1.Text += "Одинаковые числа:" + list2[i-1].ToString() + "=" + list1[j].ToString() + Environment.NewLine;
            textBox1.Text += "j = " + j.ToString() + "\t i=" + i.ToString() + Environment.NewLine;

            return i*m-j;
        }


    }
}
