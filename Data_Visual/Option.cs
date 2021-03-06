﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Data_Visual
{
    public partial class Option : Form
    {
        public Option()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimeMap f_time = new TimeMap();
            f_time.Owner = this;
            //Hide();
            f_time.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GridMap f_grid = new GridMap();
            f_grid.Owner = this;
            //Hide();
            f_grid.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NinoMap f_nino = new NinoMap();
            f_nino.Owner = this;
            //Hide();
            f_nino.ShowDialog();
        }

        DataProcess f_data = new DataProcess(); //预加载
        private void button4_Click(object sender, EventArgs e)
        {
            f_data.Owner = this;
            Hide();
            f_data.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
            Owner.Show();
        }
        /*
         当前窗口调用逻辑上的子窗口时，可以将子窗口的Owner属性设置为当前窗口，并隐藏当前窗口，子窗口退出时，
         再用Owner.Show()恢复当前窗口，这样保持了界面的连贯、窗口的一致和占用资源最小；
         如果每次调用时关闭当前窗口、返回时再构造当前窗口，实际上会产生多个进程，造成资源冗余
        */
        /*
         Close不负责销毁对象，仅实现“关闭”；Dispose则是销毁对象并释放资源。
         Close后可以使用Open再次打开数据库连接，而Dispose后不能直接使用Open，而需要重新创建一个SQLConnection对象。
         使用完数据库，需要关闭与数据库的连接，释放占用的资源。通过Dispose方法可以关闭数据库连接。
         关于二者的关系，Close()内部并不会调用Dispose()，Dispose()似乎包含Close()；总结而言，
         想要重用窗体属性或窗体对象时调用Close(),不需要再重复使用资源时则就调用Dispose()。
        */
        private void Option_Load(object sender, EventArgs e)
        {
            if (登录界面.type  != 0)
                button4.Visible = false;
            if (登录界面.type == 0)
                button4.Visible = true;
        }
    }
}
