using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xep_Hinh
{
    public partial class frmXephinh : Form
    {
        int ChiSoOTrong, SoBuocDi = 0;
        System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch(); 
        List<Bitmap> MangGoc = new List<Bitmap>();
        List<int> mangCuoi = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<List<Bitmap>> bitmaps = new List<List<Bitmap>>();
        List<Bitmap> Mang1 = new List<Bitmap> { Properties.Resources._1, Properties.Resources._2, Properties.Resources._3, Properties.Resources._4, Properties.Resources._5, Properties.Resources._6, Properties.Resources._7, Properties.Resources._8, Properties.Resources._0 };
        List<Bitmap> Mang2 = new List<Bitmap> { Properties.Resources.Untitled_1_01, Properties.Resources.Untitled_1_02, Properties.Resources.Untitled_1_03, Properties.Resources.Untitled_1_04, Properties.Resources.Untitled_1_05, Properties.Resources.Untitled_1_06, Properties.Resources.Untitled_1_07, Properties.Resources.Untitled_1_08, Properties.Resources._0 };
        List<Bitmap> Mang3 = new List<Bitmap> { Properties.Resources._1_01_01, Properties.Resources._1_01_02, Properties.Resources._1_01_03, Properties.Resources._1_01_04, Properties.Resources._1_01_05, Properties.Resources._1_01_06, Properties.Resources._1_01_07, Properties.Resources._1_01_08, Properties.Resources._0 };
        List<Bitmap> Mang4 = new List<Bitmap> { Properties.Resources._4_1, Properties.Resources._4_2, Properties.Resources._4_3, Properties.Resources._4_4, Properties.Resources._4_5, Properties.Resources._4_6, Properties.Resources._4_7, Properties.Resources._4_8, Properties.Resources._0 };
        List<Bitmap> Mang5 = new List<Bitmap> { Properties.Resources._5_01, Properties.Resources._5_02, Properties.Resources._5_03, Properties.Resources._5_04, Properties.Resources._5_05, Properties.Resources._5_06, Properties.Resources._5_07, Properties.Resources._5_08, Properties.Resources._0 };
        List<Bitmap> AnhGoc = new List<Bitmap> { Properties.Resources.anhgoc, Properties.Resources.Untitled_1, Properties.Resources.x,Properties.Resources._41,Properties.Resources._61 };


        List<State> ketQuaCuoiCung = new List<State>();
        int currentState = 0;



        List<List<int>> mangTest = new List<List<int>>();
        List<int> tesCase1 = new List<int> { 1, 2, 9, 3, 4, 6, 7, 5, 8 }; //trường hợp đb bfs 15/ tối ưu 89
        List<int> tesCase2 = new List<int> { 4, 5, 9, 3, 1, 6, 7, 2, 8 }; //15 nhưng khác số bước duyệt
        List<int> tesCase3 = new List<int> { 4, 9, 5, 3, 1, 6, 7, 2, 8 }; //14 tương tự như 15
        List<int> tesCase4 = new List<int> { 9, 1, 2, 3, 6, 5, 4, 8, 7 }; //bfs không ra, tối ưu ra 53
        List<int> tesCase5 = new List<int> { 9, 1, 3, 2, 6, 5, 4, 7, 8 };
        public frmXephinh()
        {
            InitializeComponent();
            MangGoc.AddRange(new Bitmap[] { Properties.Resources._1, Properties.Resources._2, Properties.Resources._3, Properties.Resources._4, Properties.Resources._5, Properties.Resources._6, Properties.Resources._7, Properties.Resources._8, Properties.Resources._0 });
            bitmaps.Add(Mang1);
            bitmaps.Add(Mang2);
            bitmaps.Add(Mang3);
            bitmaps.Add(Mang4);
            bitmaps.Add(Mang5);




            mangTest.Add(tesCase1);
            mangTest.Add(tesCase2);
            mangTest.Add(tesCase3);
            mangTest.Add(tesCase4);
            mangTest.Add(tesCase5);
            lblBuocdi.Text += SoBuocDi;
            lblThoigiandem.Text = "00:00:00";


        }

        private void btnLienhe_Click(object sender, EventArgs e)
        {
            //gọi from liên hệ 
            frmLienHe lh = new frmLienHe();
            lh.ShowDialog();
        }
        /// <summary>
        /// Kiểm tra xem có muốn thoát khỏi chương trình không 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_exit(object sender, FormClosingEventArgs e)
        {
            DialogResult yesorno = MessageBox.Show("Bạn có muốn thoát trò chơi?", "Trò chơi xếp hình", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (yesorno == DialogResult.No && (sender as Button) != btnThoat) e.Cancel = true;
            if (yesorno == DialogResult.Yes && (sender as Button) == btnThoat) this.Close(); 


        }

        private void frmXephinh_Load(object sender, EventArgs e)
        {
            ranDom();
            ChoiLai();
            gbKhungChinh.Visible = false;
        }
        private void DiChuyen(object sender, EventArgs e)
        {

            if (lblThoigiandem.Text == "00:00:00")
            {
                time.Start();
            }
            int O_click = gbKhung.Controls.IndexOf(sender as Control);
            if (ChiSoOTrong != O_click)
            {
                // các ô mà ô được người dùng click có thể di chuyễn được VD: ô nằm ở vị trí trung tâm : ô mà nó duy chuyển được là trên dước trái phải  
                // o_click % 3 == 0 thì vị trí của ô click sẽ là ngoài cùng bên trái (không có nên cho là -1 ) và không chia hết cho 3 thì sẽ tìm đươc ô phía trên bên phải (O_click -1) và ô ở trên bên trái 
                // o_clic % 3 == 2 thì vị trí của ô click sẽ là ngoài cùng bên phải (ở trên đã tìm rồi nên -1) và mà sai ô tìm được ô phía dưới bên trái + 1 và ô ngoài cùng phía dưới bên phải + 3 
                List<int> O_LienQuan = new List<int>(new int[] { ((O_click % 3 == 0) ? -1 : O_click - 1), O_click - 3, (O_click % 3 == 2) ? -1 : O_click + 1, O_click + 3 });
                if (O_LienQuan.Contains(ChiSoOTrong))
                {
                    ((PictureBox)gbKhung.Controls[ChiSoOTrong]).Image = ((PictureBox)gbKhung.Controls[O_click]).Image;
                    ((PictureBox)gbKhung.Controls[O_click]).Image = MangGoc[8];
                    ChiSoOTrong = O_click;
                    lblBuocdi.Text = "Số bước :" + (++SoBuocDi);
                    if (Check_win())
                    {
                        time.Stop();
                        ((PictureBox)gbKhung.Controls[8]).Image = MangGoc[8];
                        MessageBox.Show("Chúc mứng bạn đã chiến thắng...\nThời gian là :" + time.Elapsed.ToString().Remove(8) + "Số bước đi:" + SoBuocDi, "Trò chơi xếp hình");
                        SoBuocDi = 0;
                        lblThoigiandem.Text = "00:00:00";
                        time.Reset();
                        ChoiLai();
                    }

                }
            }
        }
        private void ThoiGianDem(object sender, EventArgs e)
        {
            // khi người dùng click vào ô thì thời gian sẽ chạy, ngoại trừ ô màu trắng
            if (time.Elapsed.ToString() != "00:00:00")
                lblThoigiandem.Text = time.Elapsed.ToString().Remove(8);
            if (time.Elapsed.ToString() == "00:00:00")
                btnTamdung.Enabled = false;
            else
                btnTamdung.Enabled = true;
        }
        private bool Check_win()
        {
            int i ;
            for ( i = 0; i < 8; i++) {
                if ((gbKhung.Controls[i] as PictureBox).Image != MangGoc[i]) break;
            }
            if (i == 8) return true;
            else return false;

        }
        private List<int> ChoiLai()
        {
            Random r = new Random();
            int j = r.Next(0, 5);
            List<int> mang_Random = mangTest[j];
            do
            {
                for(int i = 0; i< 9; i++)
                {
                    ((PictureBox)gbKhung.Controls[i] ).Image = MangGoc[(mang_Random[i] - 1)];
                    if (mang_Random[i] == 9) ChiSoOTrong = i;

                }
            } while (Check_win());
            return mang_Random;
        }
      
        private void btnChoilai_Click(object sender, EventArgs e)
        {
            DialogResult yerorno = new DialogResult(); 
            if(lblThoigiandem.Text != "00:00:00")
            {
                yerorno = MessageBox.Show("Bạn có muốn chơi lại hay không?", "Trò chơi xếp hình", MessageBoxButtons.YesNo, MessageBoxIcon.Question); 
            }
            if(yerorno == DialogResult.Yes || lblThoigiandem.Text == "00:00:00")
            {
                ChoiLai();
                time.Stop();
                time.Reset();
                lblThoigiandem.Text = "00:00:00";
                lblThoigiangiai.Text = "Thời Gian Giải :0.0 ms";
                lblBuocduyet.Text = "Số Bước Duyệt:"; 
                SoBuocDi = 0;
                lblBuocdi.Text = "Số bước: 0" ;
            }
        }
        
        private void btnTamdung_Click(object sender, EventArgs e)
        {
            

            if (btnTamdung.Text == "Tạm dừng")
            {
                time.Stop();
                btnTamdung.Text = "Tiếp Tục";
                btnTamdung.BackColor = Color.Aqua;
                gbKhung.Visible = false;
                gbKhungChinh.Visible = true;
                btnAnhtieptheo.Enabled = false;
                btnChoilai.Enabled = false;
            }
           else
            {
               
                time.Start();
                btnTamdung.Text = "Tạm dừng";
                btnTamdung.BackColor = Color.Orange;
                gbKhung.Visible = true;
                gbKhungChinh.Visible = false;
                btnAnhtieptheo.Enabled = true;
                btnChoilai.Enabled = true;


            }
        }

        private void btnGiaibfs_Click(object sender, EventArgs e)
        {
            List<int> MangDau = ChoiLai();
            State TrangThaiDau = new State(MangDau);
            State TrangThaiCuoi = new State(mangCuoi);
            BFS bfs = new BFS(TrangThaiDau, TrangThaiCuoi);
            Stopwatch time = new Stopwatch();

            time.Reset();

            lblThoigiangiai.Text = "Thời Gian Giải :0.0 ms";
            time.Start();
            this.ketQuaCuoiCung = bfs.ThuatGiai();
            time.Stop();

            lblThoigiangiai.Text = "Thời Gian Giải :" + time.Elapsed.TotalMilliseconds.ToString() + "ms";
            ketQuaCuoiCung.Reverse(); // đường đi trả về bị nguọc 3 - 2- 1 -> 1 2 3 

            lblBuocduyet.Text = "Số Bước Duyệt:" + bfs.dem.ToString();
            this.currentState = 0;
            lblBuocdi.Text = "Số Bước Đi :" + (currentState + 1).ToString() + "/" + this.ketQuaCuoiCung.Count.ToString();
            State temp = this.ketQuaCuoiCung[currentState];  // (1, 2 3  4,5,34)
            List<int> mang = temp.TrangThai; 
            for(int i = 0; i < mang.Count; i++)
            {
                ((PictureBox)gbKhung.Controls[i]).Image = MangGoc[mang[i] - 1];
                // 1 2 3 4 5 6 7 8 9.     3 
            }
        }

        private void btnDilui_Click(object sender, EventArgs e)
        {
            if(currentState > 0)
            {
                currentState--;
                lblBuocdi.Text = "Số bước đi:" + (currentState + 1).ToString() + "/" + ketQuaCuoiCung.Count.ToString();
                State temp = ketQuaCuoiCung[currentState]; 
                for(int i = 0; i<temp.TrangThai.Count; i++)
                {
                    ((PictureBox)gbKhung.Controls[i]).Image = MangGoc[temp.TrangThai[i] - 1 ];
                }
            }
        }

        private void btnDitoi_Click(object sender, EventArgs e)
        {
            if(currentState < ketQuaCuoiCung.Count - 1 )
            {
                currentState++;
                lblBuocdi.Text = "Số bước đi:" + (currentState + 1).ToString() + "/" + ketQuaCuoiCung.Count.ToString();
                State temp = ketQuaCuoiCung[currentState];
                for(int i = 0; i< temp.TrangThai.Count; i++)
                {
                    ((PictureBox)gbKhung.Controls[i]).Image = MangGoc[temp.TrangThai[i] - 1];
                }

            }
        }

        private void btnGiaitoiuu_Click(object sender, EventArgs e)
        {
            List<int> mangDau = ChoiLai();
            State TrangThaiDau = new State(mangDau);
            State TrangThaiCuoi = new State(mangCuoi);
            GiaiToiUu bfs = new GiaiToiUu(TrangThaiDau, TrangThaiCuoi);
            Stopwatch time = new Stopwatch();
            time.Restart();
            lblThoigiangiai.Text = "Thời Gian Giải :0.0 ms";
            time.Start();
            this.ketQuaCuoiCung = bfs.ThuatGiaiToiUu();
            time.Stop();
            lblThoigiangiai.Text = "Thời gian giải: " + time.Elapsed.TotalMilliseconds.ToString() + " ms";
            this.ketQuaCuoiCung.Reverse();
            lblBuocduyet.Text = "Số Bước Duyệt:" + bfs.dem.ToString();
            this.currentState = 0;
            this.lblBuocdi.Text = "Số Bước Đi: " + (currentState + 1).ToString() + "/" + this.ketQuaCuoiCung.Count.ToString();
            State state = ketQuaCuoiCung[this.currentState];
            List<int> mang = state.TrangThai;
            for (int i = 0; i < mang.Count; i++)
            {
                ((PictureBox)gbKhung.Controls[i]).Image = MangGoc[mang[i] - 1];
            }
        }
        public void ranDom()
        {
            
                Random i = new Random();
                int j = i.Next(0, 5);
                Console.WriteLine(j);
                MangGoc = bitmaps[j];
                ((PictureBox)pbxAnhGoc).Image = AnhGoc[j];
                ChoiLai();
           
        }
        private void btnAnhtieptheo_Click(object sender, EventArgs e)
        {
            
            ranDom();
            time.Stop();
            lblThoigiandem.Text = "00:00:00";
            time.Reset();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            //gọi check exit 
            Check_exit(sender, e as FormClosingEventArgs);
        }
    
    }
}
