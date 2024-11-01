using QuanLyNhanVien.Models;
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
   
    public partial class Login : Form
    {
        HRmanagementEntities context= new HRmanagementEntities();
        public Login()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShow.Checked == true)
            {
                tbPassword.UseSystemPasswordChar = false;
            }
            else {
                tbPassword.UseSystemPasswordChar = true; 
            }


        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            var tk = new TaiKhoan();
            string hashPass=Md5.CreateMD5(tbPassword.Text);
            var account = context.Accounts.Where(r => r.username == tbUsername.Text && r.password == hashPass).ToList();
            if (account.Count > 0) 
            {
                var db = new DashBoard();
                db.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu");
            }
        }
    }
}
