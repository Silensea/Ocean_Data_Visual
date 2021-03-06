﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Word;

namespace Data_Visual
{
    public partial class 注册界面 : Form
    {
        public 注册界面()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
        }
        SqlConnection myconn = new SqlConnection(@"Data Source=" + sql_source.dt_source + " ; Initial Catalog=OT_user;User ID=sa;Password=Cptbtptp123");
        public static string user_email = "";
        private void button1_Click(object sender, EventArgs e)
        {
            if (PasswBox1.Text != PasswBox2.Text) 
                MessageBox.Show("两次密码不一致，请重新输入！");
            else if (EmailBox.Text == "" || PasswBox1.Text == "" || PasswBox2.Text == "") 
                MessageBox.Show("用户名、密码不能为空！");
            else
            {
                 string email = EmailBox.Text;
                string password = PasswBox1.Text;
                string username = NameBox.Text;
                user_email = EmailBox.Text;
                if ( EmailBox.Text.Trim() != "" && NameBox.Text.Trim() != "" && PasswBox1.Text.Trim() != "" && PasswBox2.Text.Trim() !="")
                {
                    string mycmd = "insert into user_info  VALUES('" + email + "','" + password + "','" + username + "',null,1,null,null,'Y',0,null,null)";
                    SqlCommand sqlCommand = new SqlCommand(mycmd, myconn);
                    Console.WriteLine(mycmd);

                    try
                    {
                        myconn.Open();
                        {
                            sqlCommand.ExecuteNonQuery();
                        }
                        myconn.Close();
                    }
                    catch(Exception)
                    {
                        MessageBox.Show("该邮箱已被注册！");
                        return;
                    }
                    
                    注册成功 f1 = new 注册成功();
                    f1.Owner = this.Owner;
                    f1.ShowDialog();
                    Close();
                    
                }
                else
                {
                    MessageBox.Show("请完整输入信息");
                }
            }
        }

        private void EmailBox_TextChanged(object sender, EventArgs e)
        {
            if (!(EmailBox.Text.Contains('@') && EmailBox.Text.Contains('.')))
            {
                HintLabel1.Text = "请输入正确的邮箱格式";
                button1.Enabled = false;
            }
            else
            {
                HintLabel1.Text = "";
            }
        }

        private void PasswBox1_TextChanged(object sender, EventArgs e)
        {
            if (PasswBox1.TextLength < 6)
            {
                HintLabel2.Text = "密码过短";
                button1.Enabled = false;
            }
            else if (PasswBox1.TextLength > 20)
            {
                HintLabel2.Text = "密码过长";
                button1.Enabled = false;
            }
            else
                HintLabel2.Text = "";
        }

        private void PasswBox2_TextChanged(object sender, EventArgs e)
        {
            if (PasswBox2.Text != PasswBox1.Text)
            {
                HintLabel3.Text = "请输入相同密码";
                button1.Enabled = false;
            }
            else
                HintLabel3.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Owner.Show();
        }

        private void 注册界面_MouseMove(object sender, MouseEventArgs e)
        {
            if (HintLabel3.Text == "" && HintLabel2.Text == "" && HintLabel1.Text == "")
                button1.Enabled = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
