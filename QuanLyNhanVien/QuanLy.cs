using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using QuanLyNhanVien.Models;
namespace QuanLyNhanVien
{
    
    public partial class QuanLy : Form
    {
        HRmanagementEntities context= new HRmanagementEntities();
        List<Models.Employee> lst;
        BindingSource bs= new BindingSource();
        string fileName = "";
        public QuanLy()
        {
            InitializeComponent();
        }
        public List<Employee> getData()
        {
            var employee = context.Employees.ToList();
            return employee;
            
        }
        public List<Department> getDepartment() 
        { 
            return context.Departments.ToList();
        }
        private void loadDepartmentCombobox()
        {
            var department = getDepartment();
            cbDepartment.DataSource= department;
            cbDepartment.DisplayMember = "name";
            cbDepartment.ValueMember = "id";
            cbDepartment.SelectedIndexChanged += cbDepartment_SelectedIndexChanged;
        }
        public List<Role> getRole()
        {
            if (cbDepartment.SelectedItem != null)
            {
                
                var selectedDepartment = cbDepartment.SelectedItem as Department; 
                if (selectedDepartment != null)
                {
                    int departmentId = selectedDepartment.id; 
                    var role = context.Roles.Where(r => r.department_id == departmentId).ToList();
                    return role;
                }
            }
            return new List<Role>();
        }
        private void loadRoleCombobox()
        {
            var role = getRole();
            cbRole.DataSource= role;
            cbRole.DisplayMember = "name";
            cbRole.ValueMember = "id";
        }
        private void QuanLy_Load(object sender, EventArgs e)
        {
            lst = getData();
            bs.DataSource = lst;
            dgvEmployee.DataSource = bs;
            loadDepartmentCombobox();
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            var em = new Employee();
            try
            {
                em.id = int.Parse(tbId.Text);
                em.name = tbName.Text;
                em.address = tbAddress.Text;
                em.dob = dtpDOB.Value.Date;
                em.gender = ckGender.Checked;
                em.deparment_id = (int)cbDepartment.SelectedValue;
                em.role_id = (int)cbRole.SelectedValue;
                if (!string.IsNullOrEmpty(fileName))
                {
                    string imageFolder = @"D:\TaiLieu\winform\QuanLyNhanVien\QuanLyNhanVien\Image";
                    string destinationPath = Path.Combine(imageFolder, fileName);
                    if (pbImage.ImageLocation != null && !File.Exists(destinationPath))
                    {
                        File.Copy(pbImage.ImageLocation, destinationPath);
                    }
                    em.image = fileName;
                }
                context.Employees.Add(em);
                context.SaveChanges();
                lst.Add(em);
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Lỗi" +ex);
            }
            
            bs.ResetBindings(false);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
            if (dgvEmployee.CurrentCell == null )
            {
                MessageBox.Show("Không có dữ liệu để xóa");
                return; 
            }        
            int idx = dgvEmployee.CurrentCell.RowIndex;          
            var em = context.Employees.Find(lst[idx].id);
            if (em == null)
            {
                MessageBox.Show("Nhân viên không tồn tại");
                return; 
            }
            context.Employees.Remove(em);
            context.SaveChanges();
            lst.RemoveAt(idx);
            bs.ResetBindings(false);
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvEmployee.CurrentCell == null)
            {
                MessageBox.Show("Không tìm thấy nhân viên");
                return;
            }
            int idx = dgvEmployee.CurrentCell.RowIndex;
            var em = context.Employees.Find(lst[idx].id);
            if (em != null)
            {
                em.id = int.Parse(tbId.Text);
                em.name = tbName.Text;
                em.address = tbAddress.Text;
                em.dob = dtpDOB.Value.Date;
                em.gender = ckGender.Checked;
                em.deparment_id = (int)cbDepartment.SelectedValue;
                em.role_id = (int)cbRole.SelectedValue;
                if (!string.IsNullOrEmpty(fileName))
                {
                    string imageFolder = @"D:\TaiLieu\winform\QuanLyNhanVien\QuanLyNhanVien\Image";
                    string destinationPath = Path.Combine(imageFolder, fileName);
                    if (pbImage.ImageLocation != null && !File.Exists(destinationPath))
                    {
                        File.Copy(pbImage.ImageLocation, destinationPath);
                    }
                    em.image = fileName;
                }
                context.Entry(em).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                bs.ResetBindings(false);
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên");
            }
        }

        private void btnImgPicker_Click(object sender, EventArgs e)
        {
            pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Image";
            dlg.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pbImage.ImageLocation = dlg.FileName;
                string fileExtension=Path.GetExtension(dlg.FileName);
               fileName=GenerateSlug(tbName.Text) + fileExtension;

            }
        }
        public static string GenerateSlug(string input)
        {
            // Chuyển chuỗi sang chữ thường
            string slug = input.ToLowerInvariant();

            // Loại bỏ các ký tự không hợp lệ
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Thay thế khoảng trắng và các ký tự không hợp lệ bằng dấu gạch ngang
            slug = Regex.Replace(slug, @"\s+", "-").Trim('-');

            // Loại bỏ các dấu gạch ngang thừa
            slug = Regex.Replace(slug, @"-+", "-");

            // Cắt chuỗi nếu cần (ví dụ: giới hạn 200 ký tự)
            if (slug.Length > 200)
            {
                slug = slug.Substring(0, 200).Trim('-');
            }

            return slug;
        }

        private void dgvEmployee_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= lst.Count) return;
            var emm = context.Employees.Find(lst[e.RowIndex].id);
            if (emm != null)
            {
                tbName.Text = emm.name;
                tbId.Text = emm.id.ToString();
                tbAddress.Text = emm.address;
                if (emm.role_id.HasValue)
                {
                    cbRole.SelectedValue = emm.role_id;
                }

                if (emm.deparment_id.HasValue)
                {
                    cbDepartment.SelectedValue = emm.deparment_id;
                }
                dtpDOB.Value = emm.dob ?? DateTime.Now; 

                ckGender.Checked = emm.gender.HasValue && emm.gender.Value == true;

                if (emm.image != null)
                {
                    pbImage.ImageLocation = System.IO.Path.Combine(@"D:\TaiLieu\winform\QuanLyNhanVien\QuanLyNhanVien\Image", emm.image);
                }
            }
            else
            {
                MessageBox.Show("Dữ liệu rỗng");
            }
        }


        private void cbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDepartment.SelectedValue != null)
            {
                loadRoleCombobox();
            }
        }
    }
}
