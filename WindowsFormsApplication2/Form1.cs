using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
               InitializeComponent();
    
    this.Size = new Size(1620, 920);
    // ILI odmah fullscreen:
    this.WindowState = FormWindowState.Maximized;
    this.FormBorderStyle = FormBorderStyle.None;
    this.Bounds = Screen.PrimaryScreen.Bounds;
    textBox1.Multiline = true;
    textBox1.Height = 44;
    textBox1.Width = 656; 
    textBox1.ScrollBars = ScrollBars.Vertical; 
    textBox1.Font = new Font("Arial", 14);
    textBox2.Multiline = true;
    textBox2.Height = 40;
    textBox2.Width = 656;
    textBox2.ScrollBars = ScrollBars.Vertical;
    textBox2.Font = new Font("Arial", 14);
    textBox3.Multiline = true;
    textBox3.Height = 40;
    textBox3.Width = 656;
    textBox3.ScrollBars = ScrollBars.Vertical;
    textBox3.Font = new Font("Arial", 14);
    textBox4.Multiline = true;
    textBox4.Height = 40;
    textBox4.Width = 656;
    textBox4.ScrollBars = ScrollBars.Vertical;
    textBox4.Font = new Font("Arial", 14);
    textBox5.Multiline = true;
    textBox5.Height = 50;
    textBox5.Width = 800;
    textBox5.ScrollBars = ScrollBars.Vertical;
    textBox5.Font = new Font("Arial", 14);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close(); // ili vrati normalan prikaz
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void A2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
