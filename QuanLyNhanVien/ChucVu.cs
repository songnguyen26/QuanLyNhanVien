using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhanVien.Models;
namespace QuanLyNhanVien
{
    public partial class ChucVu : Form
    {
        HRmanagementEntities context = new HRmanagementEntities();
        List<Role> lst;
        BindingSource bs = new BindingSource();
        
        public ChucVu()
        {
            InitializeComponent();
        }
        public List<Role> getData()
        {
            return context.Roles.ToList();
        }
        public List<Department> GetDepartments()
        {
             return context.Departments.ToList();
        }
        private void loadDepartmentCombobox()
        {
            var department = GetDepartments();
            cbDepartment.DataSource = department;
            cbDepartment.DisplayMember = "name";
            cbDepartment.ValueMember = "id";
            
        }
        private void ChucVu_Load(object sender, EventArgs e)
        {
            lst= getData();
            bs.DataSource=lst;
            dgvRole.DataSource = bs;
            loadDepartmentCombobox();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Role role = new Role();
            try
            {
                role.id = int.Parse(tbId.Text);
                if (tbName.Text == "")
                {
                    MessageBox.Show("Tên chức vụ không được để trống");
                    return;
                }
                role.name = tbName.Text;
                if (cbDepartment.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn phòng ban");
                    return;
                }
                role.department_id = (int)cbDepartment.SelectedValue;
                context.Roles.Add(role);
                context.SaveChanges();
                lst.Add(role);
            }
            catch(Exception ex) 
            {
                MessageBox.Show("id đã tồn tại");
            }
            bs.ResetBindings(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(dgvRole.CurrentCell == null) 
            {
                MessageBox.Show("Chức vụ không tồn tại");
                return;
            }
            else
            {
                int idx=dgvRole.CurrentCell.RowIndex;
                var role = context.Roles.Find(lst[idx].id);
                context.Roles.Remove(role);
                context.SaveChanges();
                lst.RemoveAt(idx);
                bs.ResetBindings(false);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRole.CurrentCell == null)
            {
                MessageBox.Show("Chức vụ không tồn tại");
                return;
            }
            int idx = dgvRole.CurrentCell.RowIndex;
            var role = context.Roles.Find(lst[idx].id);
            if(role!= null) 
            {
                try
                {
                    role.id = int.Parse(tbId.Text);
                    if (tbName.Text == "")
                    {
                        MessageBox.Show("Tên chức vụ không được để trống");
                        return;
                    }
                    role.name = tbName.Text;
                    if (cbDepartment.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng chọn phòng ban");
                        return;
                    }
                    role.department_id = (int)cbDepartment.SelectedValue;
                    context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Sửa không thành công");
                }
                bs.ResetBindings(false);

            }
            else
            {
                MessageBox.Show("Không tìm thấy chức vụ");
            }
        }

        private void dgvRole_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0 || e.RowIndex >= lst.Count) return;
            int idx = e.RowIndex;
            var role = context.Roles.Find(lst[idx].id);
            if (role != null) 
            {
                tbId.Text=role.id.ToString();
                tbName.Text=role.name.ToString();
                cbDepartment.SelectedValue = role.department_id;
            }
        }
    }
}
