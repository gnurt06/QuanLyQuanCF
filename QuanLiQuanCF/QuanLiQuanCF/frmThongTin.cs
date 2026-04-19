using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiQuanCF
{
    public partial class frmThongTin : Form
    {
        KetNoi kn = new KetNoi();
        // Hàm load Nhân viên
        private void LoadNhanVien()
        {
            dgvNhanVien.DataSource = kn.LayDuLieu("SELECT * FROM NhanVien");
        }

        // Hàm load Khách hàng
        private void LoadKhachHang()
        {
            dgvKhachHang.DataSource = kn.LayDuLieu("SELECT * FROM KhachHang");
        }

        // Hàm load Nhà cung cấp
        private void LoadNCC()
        {
            dgvNCC.DataSource = kn.LayDuLieu("SELECT * FROM NhaCungCap");
        } 
        public frmThongTin()
        {
            InitializeComponent();
        }

        private void frmThongTin_Load(object sender, EventArgs e)
        {
            LoadNhanVien();
            LoadKhachHang();
            LoadNCC();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                txtMaNV.Text = dgvNhanVien.Rows[i].Cells[0].Value.ToString();
                txtHoTenNV.Text = dgvNhanVien.Rows[i].Cells[1].Value.ToString();
                txtSDTNV.Text = dgvNhanVien.Rows[i].Cells[2].Value.ToString();

                string gioiTinh = dgvNhanVien.Rows[i].Cells[3].Value.ToString();
                if (gioiTinh == "Nam") rbtnNVNam.Checked = true; else rbtnNVNu.Checked = true;

                dtpNgaySinhNV.Value = Convert.ToDateTime(dgvNhanVien.Rows[i].Cells[4].Value);
                txtDiaChiNV.Text = dgvNhanVien.Rows[i].Cells[5].Value.ToString();
            }
        }
        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                txtMaKH.Text = dgvKhachHang.Rows[i].Cells[0].Value.ToString();
                txtHoTenKH.Text = dgvKhachHang.Rows[i].Cells[1].Value.ToString();
                txtSDTKH.Text = dgvKhachHang.Rows[i].Cells[2].Value.ToString();

                string gioiTinh = dgvKhachHang.Rows[i].Cells[3].Value.ToString();
                if (gioiTinh == "Nam") rbtnKHNam.Checked = true; else rbtnKHNu.Checked = true;

                dtpNgaySinhKH.Value = Convert.ToDateTime(dgvKhachHang.Rows[i].Cells[4].Value);
                txtDiaChiKH.Text = dgvKhachHang.Rows[i].Cells[5].Value.ToString();
            }

        }
        private void dgvNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                txtMaNCC.Text = dgvNCC.Rows[i].Cells[0].Value.ToString();
                txtTenNCC.Text = dgvNCC.Rows[i].Cells[1].Value.ToString();
                txtSDTNCC.Text = dgvNCC.Rows[i].Cells[2].Value.ToString();
                txtDiaChiNCC.Text = dgvNCC.Rows[i].Cells[3].Value.ToString();
            }

        }

        private void tbpNhanVien_Click(object sender, EventArgs e)
        {

        }

        private void tbpNCC_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nút đã hoạt động!"); // Thêm dòng này

            string gioiTinh = rbtnNVNam.Checked ? "Nam" : "Nữ";
            string sql = string.Format(
                "INSERT INTO NhanVien VALUES('{0}', N'{1}', '{2}', N'{3}', '{4}', N'{5}')",
                txtMaNV.Text, txtHoTenNV.Text, txtSDTNV.Text, gioiTinh,
                dtpNgaySinhNV.Value.ToString("yyyy-MM-dd"), txtDiaChiNV.Text
            );

            if (kn.ThucThi(sql))
            {
                MessageBox.Show("Thêm nhân viên thành công!");
                LoadNhanVien(); // Load lại để hiện dữ liệu mới
            }
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            string gioiTinh = rbtnNVNam.Checked ? "Nam" : "Nữ";
            string sql = string.Format(
                "UPDATE NhanVien SET HoTen = N'{1}', SDT = '{2}', GioiTinh = N'{3}', NgaySinh = '{4}', DiaChi = N'{5}' WHERE MaNV = '{0}'",
                txtMaNV.Text, txtHoTenNV.Text, txtSDTNV.Text, gioiTinh,
                dtpNgaySinhNV.Value.ToString("yyyy-MM-dd"), txtDiaChiNV.Text
            );

            if (kn.ThucThi(sql))
            {
                MessageBox.Show("Cập nhật thông tin thành công!");
                LoadNhanVien();
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sql = "DELETE FROM NhanVien WHERE MaNV = '" + txtMaNV.Text + "'";
                if (kn.ThucThi(sql))
                {
                    LoadNhanVien();
                    MessageBox.Show("Đã xóa nhân viên!");
                }
            }
        }

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nút đã hoạt động!"); // Thêm dòng này

            string gioiTinh = rbtnKHNam.Checked ? "Nam" : "Nữ";
            string sql = string.Format(
                "INSERT INTO KhachHang VALUES('{0}', N'{1}', '{2}', N'{3}', '{4}', N'{5}')",
                txtMaKH.Text, txtHoTenKH.Text, txtSDTKH.Text, gioiTinh,
                dtpNgaySinhKH.Value.ToString("yyyy-MM-dd"), txtDiaChiKH.Text
            );

            if (kn.ThucThi(sql))
            {
                MessageBox.Show("Thêm nhân viên thành công!");
                LoadKhachHang(); // Load lại để hiện dữ liệu mới
            }
        }

        private void btnSuaKH_Click(object sender, EventArgs e)
        {
            string gioiTinh = rbtnKHNam.Checked ? "Nam" : "Nữ";
            string sql = string.Format(
                "UPDATE KhachHang SET HoTen = N'{1}', SDT = '{2}', GioiTinh = N'{3}', NgaySinh = '{4}', DiaChi = N'{5}' WHERE MaNV = '{0}'",
                txtMaKH.Text, txtHoTenKH.Text, txtSDTKH.Text, gioiTinh,
                dtpNgaySinhKH.Value.ToString("yyyy-MM-dd"), txtDiaChiKH.Text
            );

            if (kn.ThucThi(sql))
            {
                MessageBox.Show("Cập nhật thông tin thành công!");
                LoadKhachHang();
            }
        }

        private void btnXoaKH_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sql = "DELETE FROM KhachHang WHERE MaKH = '" + txtMaKH.Text + "'";
                if (kn.ThucThi(sql))
                {
                    LoadKhachHang();
                    MessageBox.Show("Đã xóa nhân viên!");
                }
            }
        }
        private void btnThemNCC_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nút đã hoạt động!"); // Thêm dòng này
            string sql = string.Format("INSERT INTO NhaCungCap VALUES('{0}', N'{1}', '{2}', N'{3}')",txtMaNCC.Text, txtTenNCC.Text, txtSDTNCC.Text , txtDiaChiNCC.Text
            );

            if (kn.ThucThi(sql))
            {
                MessageBox.Show("Thêm nhân viên thành công!");
                LoadNCC(); // Load lại để hiện dữ liệu mới
            }
        }
        private void btnSuaNCC_Click(object sender, EventArgs e)
        {
            string sql = string.Format("INSERT INTO NhaCungCap VALUES('{0}', N'{1}', '{2}', N'{3}')", txtMaNCC.Text, txtTenNCC.Text, txtSDTNCC.Text, txtDiaChiNCC.Text
            );

            if (kn.ThucThi(sql))
            {
                MessageBox.Show("Cập nhật thông tin thành công!");
                LoadNCC();
            }
        }
        private void btnXoaNCC_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sql = "DELETE FROM NhaCungCap WHERE MaNCC = '" + txtMaNCC.Text + "'";
                if (kn.ThucThi(sql))
                {
                    LoadNCC();
                    MessageBox.Show("Đã xóa nhà cung cấp !");
                }
            }
        }
    }
}
