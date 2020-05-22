﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Imaging;


namespace Laba8
{
    public partial class Form1 : Form
    {
        private Bitmap bmp = null;
        private string extractedText = string.Empty;
        public enum State
        {
            Hiding,
            Filling_With_Zeros
        };
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files (*.jpeg; *.png; *.bmp)|*.jpg; *.png; *.bmp";

            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                imagePictureBox.Image = Image.FromFile(open_dialog.FileName);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog save_dialog = new SaveFileDialog();
            save_dialog.Filter = "Png Image|*.png|Bitmap Image|*.bmp";
            if (save_dialog.ShowDialog() == DialogResult.OK)
            {
                switch (save_dialog.FilterIndex)
                {
                    case 0:
                        {
                            bmp.Save(save_dialog.FileName, ImageFormat.Png);
                        }
                        break;
                    case 1:
                        {
                            bmp.Save(save_dialog.FileName, ImageFormat.Bmp);
                        }
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bmp = (Bitmap)imagePictureBox.Image;
            string text = textBox1.Text;
            if (text.Equals("")) { MessageBox.Show("Текст не может быть пустым"); return; }
            bmp = EmbedText(text, bmp);
            MessageBox.Show("Успешно");
        }

        public static Bitmap EmbedText(string text, Bitmap bmp)
        {
            State state = State.Hiding;
            int charIndex = 0, charValue = 0, zeros = 0;
            long pixelElementIndex = 0;
            int R = 0, G = 0, B = 0;

            for (int i = 0; i < bmp.Height; i+=3)
            {
                for (int j = 0; j < bmp.Width; j+=5)
                {
                    Color pixel = bmp.GetPixel(j, i);
                    R = pixel.R - pixel.R % 2;
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;
                    for (int n = 0; n < 3; n++)
                    {
                        if (pixelElementIndex % 8 == 0)
                        {
                            if (state == State.Filling_With_Zeros && zeros == 8)
                            {
                                if ((pixelElementIndex - 1) % 3 < 2)
                                {
                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                return bmp;
                            }

                            if (charIndex >= text.Length)
                            {
                                state = State.Filling_With_Zeros;
                            }
                            else
                            {
                                charValue = text[charIndex++];
                            }
                        }
                        switch (pixelElementIndex % 3)
                        {
                            case 0:
                                {
                                    if (state == State.Hiding)
                                    {
                                        R += charValue % 2;
                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (state == State.Hiding)
                                    {
                                        G += charValue % 2;
                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (state == State.Hiding)
                                    {
                                        B += charValue % 2;
                                        charValue /= 2;
                                    }
                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                break;
                        }
                        pixelElementIndex++;
                        if (state == State.Filling_With_Zeros)
                        {
                            zeros++;
                        }
                    }
                }
            }
            return bmp;
        }

        public static string ExtractText(Bitmap bmp)
        {
            int colorUnitIndex = 0;
            int charValue = 0;
            string extractedText = String.Empty;
            for (int i = 0; i < bmp.Height; i+=3)
            {
                for (int j = 0; j < bmp.Width; j+=5)
                {
                    Color pixel = bmp.GetPixel(j, i);
                    for (int n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex % 3)
                        {
                            case 0:
                                {
                                    charValue = charValue * 2 + pixel.R % 2;
                                }
                                break;
                            case 1:
                                {
                                    charValue = charValue * 2 + pixel.G % 2;
                                }
                                break;
                            case 2:
                                {
                                    charValue = charValue * 2 + pixel.B % 2;
                                }
                                break;
                        }
                        colorUnitIndex++;
                        if (colorUnitIndex % 8 == 0)
                        {
                            charValue = reverseBits(charValue);
                            if (charValue == 0)
                            {
                                return extractedText;
                            }
                            char c = (char)charValue;
                            extractedText += c.ToString();
                        }
                    }
                }
            }

            return extractedText;
        }
        public static int reverseBits(int n)
        {
            int result = 0;
            for (int i = 0; i < 8; i++)
            {
                result = result * 2 + n % 2;
                n /= 2;
            }
            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bmp = (Bitmap)imagePictureBox.Image;
            string extractedText = ExtractText(bmp);

            textBox1.Text = extractedText;
        }
    }
}
