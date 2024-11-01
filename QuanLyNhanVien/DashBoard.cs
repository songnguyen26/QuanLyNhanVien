using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanVien
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var nhanvien = new QuanLy();
            nhanvien.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void phòngBanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var phongban=new Phongban();
            phongban.Show();
        }

        private void chứcVụToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var chucvu= new ChucVu();
            chucvu.Show();
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            pbImg.ImageLocation= @"D:\TaiLieu\winform\QuanLyNhanVien\QuanLyNhanVien\Image\DashBoardIcon.png";
        }

        private void tàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var taikhoan=new TaiKhoan();
            taikhoan.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var login=new Login();
            login.Show();
            this.Close();
        }
    }
}
