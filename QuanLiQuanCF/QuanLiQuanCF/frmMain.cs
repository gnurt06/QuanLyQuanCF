using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace QuanLiQuanCF
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            if (Session.LoaiTK == "NhanVien")
            {
                btnThongKe.Visible = true;
                btnThongTin.Visible = true;
                // Nhân viên chỉ thấy Bán hàng, Hóa đơn, Kho hàng
            }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK) Application.Exit();
        }

        private void btnBanHang_Click(object sender, EventArgs e)
        {
            OpenForm(new frmBanHang());
        }

        private void btnKhoHang_Click(object sender, EventArgs e)
        {
            OpenForm(new frmKhoHang());
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            OpenForm(new frmHoaDon());
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            OpenForm(new frmThongKe());
        }

        private void btnThongTin_Click(object sender, EventArgs e)
        {
            OpenForm(new frmThongTin());
        }
        private void OpenForm(Form f)
        {
            foreach (Form child in this.MdiChildren) child.Close();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.FormBorderStyle = FormBorderStyle.None;
            f.Show();
        }
    }
      

    }
