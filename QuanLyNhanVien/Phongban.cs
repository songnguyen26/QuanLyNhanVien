using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhanVien.Models;
namespace QuanLyNhanVien
{
    public partial class Phongban : Form
    {
        HRmanagementEntities context = new HRmanagementEntities();
        List<Department> lst;
        BindingSource bs = new BindingSource();
        public Phongban()
        {
            InitializeComponent();
        }
        public List<Department> getData()
        {
            return context.Departments.ToList();
        }
        

        private void Phongban_Load(object sender, EventArgs e)
        {
            lst=getData();
            bs.DataSource = lst;
            dgvDepartment.DataSource = bs;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Department dep = new Department();
            try
            {
                dep.id = int.Parse(tbId.Text);
                dep.name = tbName.Text;
                context.Departments.Add(dep);
                context.SaveChanges();
                lst.Add(dep);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm không thành công do mã phòng ban đã tồn tại hoặc bạn nhập chưa đầy đủ thông tin");
            }
            bs.ResetBindings(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(dgvDepartment.CurrentCell == null)
            {
                MessageBox.Show("Phòng ban không tồn tại");
                return;
            }
            else
            {
                int idx = dgvDepartment.CurrentCell.RowIndex;
                var dep = context.Departments.Find(lst[idx].id);
                context.Departments.Remove(dep);
                context.SaveChanges();
                lst.RemoveAt(idx);
                bs.ResetBindings(false);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(dgvDepartment.CurrentCell == null)
            {
                MessageBox.Show("Phòng ban không tồn tại");
                return;
            }
            int idx = dgvDepartment.CurrentCell.RowIndex;
            var dep = context.Departments.Find(lst[idx].id);
            if(dep != null)
            {
                try
                {
                    dep.id = int.Parse(tbId.Text);
                    dep.name = tbName.Text;
                    context.Entry(dep).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Sửa không thành công do mã phòng ban đã tồn tại hoặc bạn nhập chưa đầy đủ thông tin");
                }
                bs.ResetBindings(false);
            }
            else
            {
                MessageBox.Show("Phòng ban không tồn tại");
            }
        }

        

        private void dgvDepartment_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= lst.Count) return;
            int idx = e.RowIndex;
            var dep = context.Departments.Find(lst[idx].id);
            
            if (dep != null)
            {
                tbId.Text = dep.id.ToString();
                tbName.Text = dep.name.ToString();
            }
            else
            {
                tbId.Text = string.Empty;
                tbName.Text = string.Empty;
            }
            
        }
    }
}
