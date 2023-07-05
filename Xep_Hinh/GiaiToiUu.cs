using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xep_Hinh
{
    class GiaiToiUu
    {
        State trangThaiDau;
        State trangThaiCuoi;
        List<State> TrangThaiDaDuyet = null;
        public int dem = 0;

        public GiaiToiUu(State trangThaiDau,State trangThaiCuoi)
        {
            this.trangThaiDau = trangThaiDau;
            this.trangThaiCuoi = trangThaiCuoi;
            TrangThaiDaDuyet = new List<State>();
        }
        public bool KiemTraTrangThaiTrungNhau(State a, State b)
        {
            bool trung = true;
            for (int i = 0; i < a.TrangThai.Count; i++)
            {
                if (a.TrangThai[i] != b.TrangThai[i]) trung = false;
            }
            return trung;
        }
        //kiểm tra một trạng thái đã có trong danh sách các trạng thái đã duyệt hay chưa?
        public bool KiemTraDaDuyet(List<State> listState, State state)
        {
            bool duyet = false;
            foreach (var item in listState)
            {
                if (KiemTraTrangThaiTrungNhau(item, state) == true) duyet = true;
            }
            return duyet;
        }
        public void LanVet(List<State> ketQua, State item)
        {
            State hienTai = item;
            ketQua.Add(hienTai);
            while (hienTai.Father != null)
            {
                hienTai = hienTai.Father;
                ketQua.Add(hienTai);

            }

        }
        public void SapXepHueristic(List<State> trangThaiDinhDuyet)
        {
            for(int i = 0; i<trangThaiDinhDuyet.Count - 1; i++)
            {
                for(int j = i + 1; j < trangThaiDinhDuyet.Count; j++)
                {
                    if(trangThaiDinhDuyet[i].heuristic > trangThaiDinhDuyet[j].heuristic)
                    {
                        State temp = trangThaiDinhDuyet[i];
                        trangThaiDinhDuyet[i] = trangThaiDinhDuyet[j];
                        trangThaiDinhDuyet[j] = temp;

                    }

                }
            }
        }
        public  List<State> ThuatGiaiToiUu()
        {
            dem = 0;
            List<State> ketQua = new List<State>();
            List<State> trangThaiDinhDuyet = new List<State>();
            trangThaiDinhDuyet.Add(trangThaiDau); 
            while(trangThaiDinhDuyet.Count > 0)
            {
                State trangThaiDangDuyet = trangThaiDinhDuyet[0];
                dem++;
                trangThaiDinhDuyet.RemoveAt(0); 
                TrangThaiDaDuyet.Add(trangThaiDangDuyet);
             
                if(trangThaiDangDuyet.Check_Dich(trangThaiCuoi) == true)
                {
                    Console.WriteLine("Chúc mừng bạn đã chiến thắng"); break; 
                }
                List<State> trangThaiPhatSinhTuTrangThaiDangDuyet = trangThaiDangDuyet.ChiaTrangThai(); 
                foreach(var item in trangThaiPhatSinhTuTrangThaiDangDuyet)
                {
                    if(item.Check_Dich(trangThaiCuoi) == true)
                    {
                        Console.WriteLine("Chúc mừng bạn đã chiến thắng");
                        LanVet(ketQua, item);
                        return ketQua;
                    }
                    if (!KiemTraDaDuyet(TrangThaiDaDuyet, item))
                    {
                        item.Heuristic(trangThaiCuoi);
                        trangThaiDinhDuyet.Add(item);
                        SapXepHueristic(trangThaiDinhDuyet);

                    }
                }
              
            }
            return ketQua;
        }
    
    }
}
