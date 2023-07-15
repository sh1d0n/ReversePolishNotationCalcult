using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalcutSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            label1.Text = "Обратная польская нотация: ";
            label2.Text = "Результат: ";
            label3.Text = "";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            ReversePolishCalculator start = new ReversePolishCalculator();
            string[] results = start.Start(textBox1.Text);

            if (results[2] != "")
            {
                label3.Text = results[2];
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        label1.Text = "Обратная польская нотация: " + results[0];
                    }
                    else
                    {
                        label2.Text = "Результат: " + results[1];
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
            if(textBox1.Text == "")
            {
                textBox1.Text = "0";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Button B = (Button)sender;
            if(textBox1.Text == "0" && B.Text != ",")
            {
                textBox1.Text = "";
            }
            textBox1.Text += B.Text;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
