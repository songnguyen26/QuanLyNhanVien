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
    public partial class TaiKhoan : Form
    {
        HRmanagementEntities context = new HRmanagementEntities();
        List<Account> lst = new List<Account>();
        BindingSource bs = new BindingSource();
        
        public TaiKhoan()
        {
            InitializeComponent();
        }
        public List<Account> getData()
        {
            return context.Accounts.ToList();
            
        }
        private void Account_Load(object sender, EventArgs e)
        {
             lst=getData();
            bs.DataSource=lst;
            dgvAccount.DataSource=bs;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            string hashPass=Md5.CreateMD5(tbPassword.Text);
            if(tbPassword.Text != tbRePassword.Text)
            {
                MessageBox.Show("Mật khẩu không khớp");
                return;
            }
            if (tbUsername.Text == "")
            {
                MessageBox.Show("Nhập tên đăng nhập");
                return;
            }
            try
            {
                account.username = tbUsername.Text;
                account.password = hashPass;
                context.Accounts.Add(account);
                context.SaveChanges();
                lst.Add(account);
            }
            catch
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại");
            }
            bs.ResetBindings(false);
        }
        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAccount.CurrentCell==null)
            {
                MessageBox.Show("Tài khoản không tồn tại");
                return;
            }
            int idx=dgvAccount.CurrentCell.RowIndex;
            var account = context.Accounts.Find(lst[idx].username);
            if (account == null) 
            {
                MessageBox.Show("Tài khoản không tồn tại");
                return;
            }
            context.Accounts.Remove(account);
            context.SaveChanges();
            lst.RemoveAt(idx);
            bs.ResetBindings(false);
        }

        private void cbShow_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShow.Checked) 
            {
                tbPassword.UseSystemPasswordChar = false;
                tbRePassword.UseSystemPasswordChar = false;
            }
            else
            {
                tbPassword.UseSystemPasswordChar = true;
                tbRePassword.UseSystemPasswordChar = true;
            }
        }
    }
}
