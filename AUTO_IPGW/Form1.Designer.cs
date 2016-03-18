namespace AUTO_IPGW
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.userName = new System.Windows.Forms.TextBox();
            this.passWord = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.connInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(30, 82);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 62);
            this.button1.TabIndex = 0;
            this.button1.Text = "Connect Free";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(193, 82);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 62);
            this.button2.TabIndex = 1;
            this.button2.Text = "Connect Global";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(30, 324);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(117, 62);
            this.button3.TabIndex = 2;
            this.button3.Text = "Disconnect";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(193, 324);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(117, 62);
            this.button4.TabIndex = 3;
            this.button4.Text = "Disconnect All";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // userName
            // 
            this.userName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.userName.Location = new System.Drawing.Point(30, 24);
            this.userName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(116, 28);
            this.userName.TabIndex = 4;
            this.userName.Text = "ID";
            this.userName.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // passWord
            // 
            this.passWord.Location = new System.Drawing.Point(193, 24);
            this.passWord.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.passWord.Name = "passWord";
            this.passWord.PasswordChar = '*';
            this.passWord.Size = new System.Drawing.Size(116, 28);
            this.passWord.TabIndex = 5;
            this.passWord.Text = "PASSWORD";
            this.passWord.UseSystemPasswordChar = true;
            // 
            // textBox1
            // 
            this.textBox1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBox1.Location = new System.Drawing.Point(231, 163);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(78, 28);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "15";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(30, 168);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(169, 22);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "auto disconnect";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // connInfo
            // 
            this.connInfo.AutoSize = true;
            this.connInfo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.connInfo.Location = new System.Drawing.Point(45, 237);
            this.connInfo.Name = "connInfo";
            this.connInfo.Size = new System.Drawing.Size(0, 21);
            this.connInfo.TabIndex = 8;
            this.connInfo.Visible = false;
            this.connInfo.Click += new System.EventHandler(this.connInfo_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 413);
            this.Controls.Add(this.connInfo);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.passWord);
            this.Controls.Add(this.userName);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AUTO IPGW";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox userName;
        private System.Windows.Forms.TextBox passWord;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label connInfo;
    }
}

