using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xep_Hinh
{
    class BFS
    {
        State trangThaiDau;
        State trangThaiDich;
        List<State> TrangThaiDaDuyet = null;
        public int dem = 0;

        public BFS(State trangThaiDau, State trangThaiDich)
        {
            this.trangThaiDau = trangThaiDau;
            this.trangThaiDich = trangThaiDich;
            TrangThaiDaDuyet = new List<State>(); 
        }
        public bool KiemTraTrangThaiTrungNhau(State a, State b)
        {
            bool trung = true; 
            for(int i = 0; i< a.TrangThai.Count; i++)
            {
                if (a.TrangThai[i] != b.TrangThai[i]) trung = false; 
            }
            return trung;
        }
        //kiểm tra một trạng thái đã có trong danh sách các trạng thái đã duyệt hay chưa?
        public bool KiemTraDaDuyet(List<State> listState,State state)
        {
            bool duyet = false; 
           foreach(var item in listState)
            {
                if (KiemTraTrangThaiTrungNhau(item, state) == true) duyet = true; 
            }
            return duyet; 
        }

        public List<State> ThuatGiai()
        {
            dem = 0;
            List<State> KetQua = new List<State>();
            Queue<State> queue_trangThaiDinhDuyet = new Queue<State>();
            queue_trangThaiDinhDuyet.Enqueue(trangThaiDau); 
            while(queue_trangThaiDinhDuyet.Count > 0)
            {
                State trangThaiDangDuyet = queue_trangThaiDinhDuyet.Dequeue(); // trang thái sẽ lấy ra để duyệt
                TrangThaiDaDuyet.Add(trangThaiDangDuyet); // (xet nó duyệt chưa với trùng )
                dem++; 
                if(trangThaiDangDuyet.Check_Dich(trangThaiDich) == true)
                {
                    Console.WriteLine("Chúc mứng bạn đã chiến thắng... ");
                    break;
                }
                List<State> TrangThaiKhac_Tu_TrangThaiDangDuyet = trangThaiDangDuyet.ChiaTrangThai();
                foreach(var item in TrangThaiKhac_Tu_TrangThaiDangDuyet)
                {
                   
                    if(item.Check_Dich(trangThaiDich) == true) 
                    {
                        Console.WriteLine("Chúc mứng bạn đã chiến thắng... ");
                        LanVet(KetQua, item);
                        return KetQua;

                    }
                    if (!KiemTraDaDuyet(TrangThaiDaDuyet, item)) queue_trangThaiDinhDuyet.Enqueue(item);
                }
            }
            return KetQua;
        }
        public void LanVet(List<State> DuongDi, State n)
        {
            State hienTai = n;
            DuongDi.Add(hienTai); 
            while(hienTai.Father != null)
            {
                hienTai = hienTai.Father;
                DuongDi.Add(hienTai);
            }
        }
    }
}
