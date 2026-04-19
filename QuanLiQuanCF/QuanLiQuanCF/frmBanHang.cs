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
    public partial class frmBanHang : Form
    {
        KetNoi kn = new KetNoi();
        string maBanDangChon = ""; // Lưu mã bàn (ví dụ: B01)
        bool dangPhucVu = false;

        private void frmBanHang_Load(object sender, EventArgs e)
        {
            // Chỉ load Danh mục loại sản phẩm
            DataTable dtLoai = kn.LayDuLieu("SELECT DISTINCT LoaiSP FROM SanPham");
            cboDanhMucSP.DataSource = dtLoai;
            cboDanhMucSP.DisplayMember = "LoaiSP";
            cboDanhMucSP.SelectedIndex = -1;

            // Load trạng thái bàn
            LoadTrangThaiBan();
        }
        private void LoadTrangThaiBan()
        {
            // 1. Lấy dữ liệu trạng thái tất cả các bàn từ SQL
            DataTable dt = kn.LayDuLieu("SELECT SoBan, TrangThai FROM Ban");

            // 2. Duyệt qua từng dòng dữ liệu
            foreach (DataRow row in dt.Rows)
            {
                string soBan = row["SoBan"].ToString();
                string trangThai = row["TrangThai"].ToString();

                // Tìm nút bấm tương ứng trên Form (ví dụ nút tên là btnBan1, btnBan2...)
                // Lưu ý: Bạn phải đặt tên (Name) của các nút là btnBan1, btnBan2... btnBan12
                Control[] foundButtons = this.Controls.Find("btnBan" + soBan, true);

                if (foundButtons.Length > 0)
                {
                    Button btn = (Button)foundButtons[0];
                    if (trangThai == "Có khách")
                    {
                        btn.BackColor = Color.Orange;
                    }
                    else
                    {
                        btn.BackColor = Color.White;
                    }
                }
            }
        }
        public frmBanHang()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

         }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void TinhTongTien()
{
    double tong = 0;
    
    // Duyệt qua từng dòng trong DataGridView (dgvBanHang)
    foreach (DataGridViewRow row in dgvBanHang.Rows)
    {
        // Kiểm tra nếu dòng đó không phải dòng trống (dòng mới đang chờ nhập)
        if (row.Cells["ThanhTien"].Value != null)
        {
            tong += Convert.ToDouble(row.Cells["ThanhTien"].Value);
        }
    }
    
    // Hiển thị kết quả lên TextBox. Giả sử TextBox của bạn tên là txtTongTien
    // Định dạng #,##0 để có dấu phẩy ngăn cách hàng nghìn (ví dụ: 100,000)
    txtTongTien.Text = tong.ToString("#,##0");
}

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn dòng nào trong DataGridView chưa
            if (dgvBanHang.SelectedRows.Count > 0 || dgvBanHang.CurrentRow != null)
            {
                // Hỏi xác nhận trước khi xóa để tránh bấm nhầm
                DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn bỏ món này khỏi danh sách?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    // 2. Lấy dòng hiện tại đang chọn
                    int rowIndex = dgvBanHang.CurrentCell.RowIndex;

                    // 3. Xóa dòng đó đi
                    dgvBanHang.Rows.RemoveAt(rowIndex);

                    // 4. CỰC KỲ QUAN TRỌNG: Gọi lại hàm tính tổng tiền để cập nhật số liệu mới
                    TinhTongTien();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn món cần xóa trong bảng danh sách!");
            }
        }
        private void button13_Click_1(object sender, EventArgs e)
        {
            string tenMon = lblTenMonDangChon.Text;
            int soLuong = (int)nmSoLuong.Value;

            if (string.IsNullOrEmpty(tenMon) || tenMon == "...")
            {
                MessageBox.Show("Vui lòng chọn món từ danh mục sản phẩm!");
                return;
            }

            DataTable dt = kn.LayDuLieu($"SELECT Gia FROM SanPham WHERE TenSP = N'{tenMon}'");
            if (dt.Rows.Count > 0)
            {
                double gia = Convert.ToDouble(dt.Rows[0]["Gia"]);
                double thanhTien = soLuong * gia;

                // Thêm vào DataGridView - Đảm bảo dgv có đủ 4 cột này
                dgvBanHang.Rows.Add(TenMon, SoLuong, DonGia, ThanhTien);
                TinhTongTien();
            }
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string soBan = btn.Text.Replace("Bàn ", ""); // Lấy số thứ tự bàn

            // Đổi màu để nhận diện bàn có khách
            if (btn.BackColor == Color.White || btn.BackColor == Color.FromKnownColor(KnownColor.Control))
            {
                btn.BackColor = Color.Orange;
                kn.ThucThi("UPDATE Ban SET TrangThai = N'Có khách' WHERE SoBan = " + soBan);
            }
            else
            {
                btn.BackColor = Color.White;
                kn.ThucThi("UPDATE Ban SET TrangThai = N'Trống' WHERE SoBan = " + soBan);
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string maHD = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string maBan = maBanDangChon;
            double tongTien = double.Parse(txtTongTien.Text);

            // LẤY MÃ NHÂN VIÊN TỪ SESSION ĐĂNG NHẬP
            string maNV = Session.MaNV;

            // Lệnh INSERT phải có MaNV
            string sql = string.Format(
                "INSERT INTO HoaDon (MaHD, MaBan, MaNV, GioVao, GioRa, TongTien) " +
                "VALUES ('{0}', '{1}', '{2}', GETDATE(), GETDATE(), {3})",
                maHD, maBan, maNV, tongTien
            );

            if (kn.ThucThi(sql))
            {
                MessageBox.Show("Thanh toán thành công! Người lập: " + Session.TenNV);
            }
        }
        private void cboDanhMucSP_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboDanhMucSP.SelectedIndex == -1) return;

            string loaiChon = cboDanhMucSP.Text;
            // Xóa các nút cũ trong GroupBox trước khi tạo mới
            flpSanPham.Controls.Clear(); // Nên dùng FlowLayoutPanel đặt trong GroupBox để nút tự xếp hàng

            DataTable dt = kn.LayDuLieu($"SELECT TenSP FROM SanPham WHERE LoaiSP = N'{loaiChon}'");

            foreach (DataRow row in dt.Rows)
            {
                Button btn = new Button();
                btn.Text = row["TenSP"].ToString();
                btn.Width = 100;
                btn.Height = 50;
                btn.BackColor = Color.LightBlue;

                // Gán sự kiện click cho từng nút sản phẩm được tạo ra
                btn.Click += (s, ev) => {
                    lblTenMonDangChon.Text = btn.Text; // Hiển thị tên món vừa chọn vào 1 Label ẩn
                    btn.BackColor = Color.Yellow; // Đổi màu để biết đang chọn món này
                };

                flpSanPham.Controls.Add(btn);
            }
        }
        private void grbDanhMucSanPham_Enter(object sender, EventArgs e)
        {
        }
        


        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void dgvBanHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void HanhDongClickBan(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            // Lấy số bàn từ Text của nút (Vd: "Bàn 1" -> "1")
            string soBan = btn.Text.Replace("Bàn ", "").Trim();
            // Quy chuẩn mã bàn để khớp với Database (Vd: "B01")
            string maBanHienTai = "B" + soBan.PadLeft(2, '0');

            // Truy vấn trạng thái thực tế của bàn này từ Database
            DataTable dt = kn.LayDuLieu($"SELECT TrangThai FROM Ban WHERE MaBan = '{maBanHienTai}'");
            if (dt.Rows.Count == 0) return;
            string trangThaiThucTe = dt.Rows[0]["TrangThai"].ToString();

            // TRƯỜNG HỢP 1: Bàn đang trống -> Muốn mở bàn để bán
            if (trangThaiThucTe == "Trống")
             {
                // Nếu nhân viên đang dở tay phục vụ một bàn khác (biến dangPhucVu đang khóa)
                if (dangPhucVu && maBanDangChon != maBanHienTai)
                {
                    MessageBox.Show($"Bạn phải thanh toán hoặc hủy {lblBanDangChon.Text} trước khi mở bàn mới!");
                    return;
                }

                // Tiến hành mở bàn
                maBanDangChon = maBanHienTai;
                dangPhucVu = true;
                lblBanDangChon.Text = btn.Text;
                btn.BackColor = Color.Orange;
                kn.ThucThi($"UPDATE Ban SET TrangThai = N'Có khách' WHERE MaBan = '{maBanHienTai}'");
            }

            else
            {
                // Nếu click vào đúng bàn đang phục vụ -> Cho phép Hủy/Trả bàn
                if (maBanDangChon == maBanHienTai)
                {
                    DialogResult dr = MessageBox.Show("Khách đã về, bạn muốn trả bàn này về trạng thái TRỐNG?", "Xác nhận", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        kn.ThucThi($"UPDATE Ban SET TrangThai = N'Trống' WHERE MaBan = '{maBanHienTai}'");
                        ResetFormBanHang(); // Hàm này sẽ load lại màu nút và set dangPhucVu = false
                    }
                }
                else // Nếu click vào một bàn "Có khách" khác nhưng không phải bàn đang chọn trên Label
                {
                    // Cho phép đổi sang bàn đó để xem/thêm món
                    maBanDangChon = maBanHienTai;
                    dangPhucVu = true;
                    lblBanDangChon.Text = btn.Text;
                    // Ở đây bạn nên viết thêm hàm Load lại các món đã gọi của bàn này lên DataGridView (nếu cần)
                    MessageBox.Show("Đã chuyển sang xem " + btn.Text);
                }
            }
        }
        private void ResetFormBanHang()
        {
            maBanDangChon = "";
            dangPhucVu = false;
            lblBanDangChon.Text = "Chưa chọn bàn";
            lblTenMonDangChon.Text = "...";
            dgvBanHang.Rows.Clear();
            txtTongTien.Clear();

            // Cập nhật lại toàn bộ màu sắc nút bấm dựa trên DB
            LoadTrangThaiBan();
        }
        private void btnHuyHoaDon_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Hủy toàn bộ hóa đơn đang chọn?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dgvBanHang.Rows.Clear();
                txtTongTien.Clear();
            }
        }
        private void LoadDanhMuc()
        {
            try
            {
                DataTable dt = kn.LayDuLieu("SELECT DISTINCT LoaiSP FROM SanPham");

                if (dt != null && dt.Rows.Count > 0)
                {
                    cboDanhMucSP.DataSource = dt;
                    cboDanhMucSP.DisplayMember = "LoaiSP";
                    cboDanhMucSP.ValueMember = "LoaiSP";
                    cboDanhMucSP.SelectedIndex = -1; // Để trống lúc mới mở form
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load danh mục: " + ex.Message);
            }
        }

        private void cboDanhMucSP_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboDanhMucSP.SelectedIndex == -1) return;

            flpSanPham.Controls.Clear();
            string loai = cboDanhMucSP.Text;
            DataTable dt = kn.LayDuLieu($"SELECT TenSP, Gia FROM SanPham WHERE LoaiSP = N'{loai}'");

            foreach (DataRow row in dt.Rows)
            {
                Button btn = new Button
                {
                    Text = row["TenSP"].ToString() + "\n" + string.Format("{0:#,##0}", row["Gia"]),
                    Tag = row["Gia"].ToString(), // Lưu giá vào Tag
                    Size = new Size(100, 60),
                    BackColor = Color.LightBlue
                };

                btn.Click += (s, ev) => {
                    // Khi nhấn vào nút sản phẩm -> Tự động thêm vào lưới luôn
                    ThemMonVaoLuoi(row["TenSP"].ToString(), Convert.ToDouble(row["Gia"]));
                };
                flpSanPham.Controls.Add(btn);
            }
        }
        private void ThemMonVaoLuoi(string tenMon, double gia)
        {
            if (!dangPhucVu) { MessageBox.Show("Vui lòng chọn bàn trước!"); return; }

            int sl = (int)nmSoLuong.Value;
            double tt = sl * gia;
            dgvBanHang.Rows.Add(tenMon, sl, gia, tt);
            TinhTongTien();
        }
    }
}

           