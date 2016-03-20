using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using AUTO_IPGW;
using System.IO;
using System.Threading;

namespace AUTO_IPGW
{
    public partial class MainForm : Form
    {
        public static Thread thread;
        public MainForm()
        {
            InitializeComponent();
            MainForm.thread = new Thread(Func);
        }        

        private void button1_Click(object sender, EventArgs e)
        {
            Program.saveUserInfo(userName.Text, passWord.Text);
            Program.IPGW_connect(Program.FREE, this.userName.Text, this.passWord.Text);
            if (thread.IsAlive)
            {
                thread.Abort();
            }
            this.connInfo.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.saveUserInfo(userName.Text, passWord.Text);
            bool suc = Program.IPGW_connect(Program.GLOBAL, this.userName.Text, this.passWord.Text);
            if (!suc) return;
            this.connInfo.Hide();
            if (this.checkBox1.Checked)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
                thread = new Thread(Func);
                thread.Start();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.saveUserInfo(userName.Text, passWord.Text);
            Program.IPGW_connect(Program.DISCONNECT, this.userName.Text, this.passWord.Text);
            if (thread.IsAlive)
            {
                thread.Abort();
            }
            this.connInfo.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Program.saveUserInfo(userName.Text, passWord.Text);
            Program.IPGW_connect(Program.DIS_ALL, this.userName.Text, this.passWord.Text);
            if (thread.IsAlive)
            {
                thread.Abort();
            }
            this.connInfo.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            string _uid = "username";
            string _password = "password";
            if (Program.readUserInfo(ref _uid, ref _password))
            {
                this.userName.Text = _uid;
                this.passWord.Text = _password;
            }
        }

        private void connInfo_Click(object sender, EventArgs e)
        {
            DialogResult dr= MessageBox.Show("Cancel auto disconnection？","Cancel?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                //点确定的代码
                if(thread.IsAlive)
                {
                    thread.Abort();
                }
                this.connInfo.Hide();
            }
            else
            {   //点取消的代码 
            }
        }

        

        private void Func()
        {
            int hour = 0;
            int minute = 0;
            int second = 0;
            try
            {
                minute = Convert.ToInt32(this.textBox1.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show("时间输入错误：\n" + e.Message);
                return;
            }
            Console.WriteLine(minute);
            if (minute == 0) return;
            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }
            while (hour > 0 || minute>0 || second>0)
            {
                this.Invoke((EventHandler)delegate {
                    
                    if (second < 0)
                    {
                        second = 59;
                        minute--;                    
                    }
                    if (minute < 0)
                    {
                        minute = 59;
                        hour--;
                    }
                    this.connInfo.Show();
                    this.connInfo.Text = "Disconnect in\n" +  hour.ToString() + ": " + minute.ToString() + ": " + second.ToString() ;
                });
                Thread.Sleep(1000);
                second--;
            }
            Program.IPGW_connect(Program.FREE, this.userName.Text, this.passWord.Text);
            this.connInfo.Hide();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread.IsAlive)
            {
                thread.Abort();
            }
        }    
    }
}
