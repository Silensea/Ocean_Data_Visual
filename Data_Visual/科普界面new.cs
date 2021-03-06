﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using CCWin.SkinClass;
using System.Drawing.Imaging;
using System.Collections;
using CCWin.SkinControl;

namespace Data_Visual
{
    public partial class 科普界面new : Form
    {
        public 科普界面new()
        {
            InitializeComponent();
        }
        SqlConnection myconn = new SqlConnection(@"Data Source=" + sql_source.dt_source + " ; Initial Catalog=OT_user;User ID=sa;Password=Cptbtptp123");
        string sql;
        int FileCount = 0;//已经通过审核的数量
        int[] alist = new int [1000];//设置1000条科普上限
        Image img;
        private void 科普界面new_Load(object sender, EventArgs e)
        {
            initializeCount();
            countY();
            fetchCollect(1);
        }

        private void initializeCount()
        {
            sql = "Select count(*) from collect_info where approved='Y'";
            SqlCommand cmd = new SqlCommand(sql, myconn);
            try
            {
                myconn.Open();
                FileCount = Convert.ToInt32(cmd.ExecuteScalar());
                myconn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void countY()//用动态数组将id与collect_num对应起来！
        {
            sql = "select collect_num from collect_info where approved='Y'";
            DataSet mydataset2 = new DataSet();
            SqlDataAdapter myadapter = new SqlDataAdapter(sql, myconn);
            mydataset2.Clear();
            myadapter.Fill(mydataset2, "count");
            for (int i=0;i<FileCount;i++)
            {
                int collect_num = Convert.ToInt32(mydataset2.Tables["count"].Rows[i][0]);
                alist[i] = collect_num;
            }
        }
        int status;
        void get_status()
        {
            string mysql = "select u_status from user_info where umail='" + 登录界面.mail + "'";
            SqlDataAdapter myadapter = new SqlDataAdapter(mysql, myconn);
            DataSet mydataset = new DataSet();
            mydataset.Clear();
            myadapter.Fill(mydataset, "status");
            status = Convert.ToInt32(mydataset.Tables["status"].Rows[0][0]);

        }

        private void fetchCollect(int id)
        {
            byte[] bytes = new byte[0];
            string upload_user="default";
            sql = @"select collect_txt, collect_pic, uname from collect_info, user_info 
                    where collect_info.create_by=user_info.umail and collect_num=" + alist[id-1].ToString();
            SqlCommand cmd = new SqlCommand(sql, myconn);
            try
            {
                myconn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                richTextBox1.Text = sdr["collect_txt"].ToString();
                bytes = (byte[])sdr["collect_pic"];
                upload_user = sdr["uname"].ToString();
                sdr.Close();
                myconn.Close();
                MemoryStream mystream = new MemoryStream(bytes);
                //用指定的数据流来创建一个image图片
                img = Image.FromStream(mystream,true);
                skinPictureBox1.Image = img;
                mystream.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            label1.Text = cur.ToString() + "/" + FileCount.ToString();
            label2.Text = "此科普由用户 " + upload_user + " 上传.";
        }

        int cur = 1;
        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            cur = cur - 1;
            if (cur < 1)
                cur = FileCount;
            fetchCollect(cur);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            cur = cur + 1;
            if (cur > FileCount)
                cur = 1;
            fetchCollect(cur);
        }

        private void buttonCollect_Click(object sender, EventArgs e)
        {
            if (登录界面.mail== "")
                MessageBox.Show("未登录！");
            else
            {
                try
                {
                    string mycmd = "insert into collect  VALUES('" + 登录界面.mail + "','" + alist[cur-1] + "',null)";
                    string mycmd1= "select collect_num from collect where umail='" + 登录界面.mail+"'";
                    //统计已经收藏个数
                    DataSet mydataset = new DataSet();
                    SqlDataAdapter myadapter = new SqlDataAdapter(mycmd1, myconn);
                    myadapter.Fill(mydataset, "_email");
                    int count_all = mydataset.Tables["_email"].Rows.Count;
                    if (count_all >= account.N)
                        MessageBox.Show("收藏已达上限！");
                    else
                    {   //收藏
                        SqlCommand sqlCommand = new SqlCommand(mycmd, myconn);
                        Console.WriteLine(mycmd);
                        myconn.Open();
                        {
                            sqlCommand.ExecuteNonQuery();
                        }
                        myconn.Close();
                        MessageBox.Show("收藏成功！", "Ocean");
                    }
 
                }
                catch
                {
                    MessageBox.Show("已收藏！");
                    myconn.Close();
                }
            }
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Close();
            Owner.Show();
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            厄尔尼诺专题 form = new 厄尔尼诺专题();
            form.Owner = this;
            Hide();
            form.ShowDialog();
        }

        private void richTextBox1_MouseLeave(object sender, EventArgs e)
        {
            skinPictureBox1.Focus();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            get_status();
            if (status < 5)
            {
                MessageBox.Show("您的等级不足9级，请继续加油！");
                return;
            }
            科普上传 form = new 科普上传();
            form.Owner = this;
            form.ShowDialog();
            initializeCount();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            SaveFileDialog svdia = new SaveFileDialog();
            svdia.Title = "请选择目录并输入文件名";
            svdia.Filter = "所有文件(*.*)|*.*";
            if (svdia.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = svdia.FileName;
            richTextBox1.SaveFile(filename+".txt", RichTextBoxStreamType.PlainText);
            Bitmap bmp = new Bitmap(img);
            bmp.Save(filename+".jpg", ImageFormat.Jpeg);
            MessageBox.Show("保存成功！", "Ocean");
        }
    }
}
