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
            IPGW.saveUserInfo(userName.Text, passWord.Text);
            var isSuc = IPGW.IPGW_connect(IPGW.FREE, this.userName.Text, this.passWord.Text);
            if (isSuc)
            {
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.ShowBalloonTip(1000, "提示", "连接免费网络成功", ToolTipIcon.Info);
            }
            else
            {
                MessageBox.Show(IPGW.parseCode[IPGW.lastResultCode]);
            }
            if (thread.IsAlive)
            {
                thread.Abort();
            }
            this.connInfo.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IPGW.saveUserInfo(userName.Text, passWord.Text);
            bool isSuc = IPGW.IPGW_connect(IPGW.GLOBAL, this.userName.Text, this.passWord.Text);
            if (isSuc)
            {
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.ShowBalloonTip(1000, "提示", IPGW.parseCode[IPGW.lastResultCode], ToolTipIcon.Info);
            }
            else
            {
                MessageBox.Show(IPGW.parseCode[IPGW.lastResultCode]);
            }
            if (!isSuc) return;
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
            else
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
                this.connInfo.Hide();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IPGW.saveUserInfo(userName.Text, passWord.Text);
            var isSuc = IPGW.IPGW_connect(IPGW.DISCONNECT, this.userName.Text, this.passWord.Text);
            MessageBox.Show(IPGW.parseCode[IPGW.lastResultCode]);
            if (thread.IsAlive)
            {
                thread.Abort();
            }
            this.connInfo.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            IPGW.saveUserInfo(userName.Text, passWord.Text);
            IPGW.IPGW_connect(IPGW.DIS_ALL, this.userName.Text, this.passWord.Text);
            MessageBox.Show(IPGW.parseCode[IPGW.lastResultCode]);
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
            if (IPGW.readUserInfo(ref _uid, ref _password))
            {
                this.userName.Text = _uid;
                this.passWord.Text = _password;
            }
        }

        private void connInfo_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Cancel auto disconnection？", "Cancel?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                //点确定的代码
                if (thread.IsAlive)
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
            while (hour > 0 || minute > 0 || second > 0)
            {
                this.Invoke((EventHandler)delegate
                {

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
                    this.connInfo.Text = "Disconnect in\n" + hour.ToString() + ": " + minute.ToString() + ": " + second.ToString();
                });
                Thread.Sleep(1000);
                second--;
            }
            IPGW.IPGW_connect(IPGW.FREE, this.userName.Text, this.passWord.Text);
            this.connInfo.Hide();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread.IsAlive)
            {
                var confirm = MessageBox.Show("是否断开收费网络？", "退出确认", MessageBoxButtons.YesNoCancel);
                if (confirm == DialogResult.Yes)
                {
                    thread.Abort();
                    IPGW.IPGW_connect(IPGW.FREE, this.userName.Text, this.passWord.Text);
                    return;
                }
                else if (confirm == DialogResult.No)
                {
                    thread.Abort();
                    return;
                }
                else if (confirm == DialogResult.Cancel)
                {
                    e.Cancel = true;    //cancel closing.
                }
                else
                    throw new Exception("Dialog result unexpected");
            }
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                this.notifyIcon1.BalloonTipTitle = "提示";
                this.notifyIcon1.BalloonTipText = "自动网关已经最小化到系统托盘";
                //this.notifyIcon1.ShowBalloonTip(1000, this.notifyIcon1.BalloonTipTitle, this.notifyIcon1.BalloonTipText, ToolTipIcon.Info);
                this.Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //notifyIcon1.Visible = false;
            this.Show();
            WindowState = FormWindowState.Normal;
            this.Focus();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.notifyIcon1_MouseDoubleClick(sender, e);
            }
            else if (e.Button == MouseButtons.Right)
            {

            }
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SoftUpdate app = new SoftUpdate(Application.ExecutablePath, "AUTO_IPGW");
            app.UpdateFinish += new UpdateState(app_UpdateFinish);
            try
            {
                if (app.IsUpdate && MessageBox.Show("检查到新版本，是否更新？", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    Thread update = new Thread(new ThreadStart(app.Update));
                    update.Start();
                }
                else if (!app.IsUpdate)
                {
                    MessageBox.Show("当前已是最新版本");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void app_UpdateFinish()
        {
            MessageBox.Show("更新完成，请重新启动程序！", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
