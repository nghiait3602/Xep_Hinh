using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xep_Hinh
{
    class State
    {
        private List<int> trangThai;
        private State father; // dùng dễ lần vết (do giải thuật bfs tìm đường ngượt lại )
        public int heuristic; // là các hình không nằm đúng vị trí so với hình góc 

        public List<int> TrangThai { get => trangThai; set => trangThai = value; }
        internal State Father { get => father; set => father = value; }

        public State() { }

        public State(List<int> state)
        {
            this.TrangThai = state;
        }
        // hàm tìm ô sai , số ô sai là giá trị của heuristic  (bfs) 
        public int Heuristic(State trangThaiDich)
        {
            int O_sai = 0;
            for (int i = 0; i < trangThaiDich.TrangThai.Count; i++)
            {
                if (this.TrangThai[i] != trangThaiDich.TrangThai[i]) O_sai++;
            }
            this.heuristic = O_sai;
            return O_sai;

        }
        // mảng bất kỳ, khi người dùng di chuyển các ô sẽ sinh ra mảng mới, truyền vào mỗi bước đi (trên dưới trái phải)
        public List<List<int>> MangPhatSinh(List<int> MangBanDau)
        {
            List<List<int>> MangPhatSinh = new List<List<int>>();
            int i = MangBanDau.IndexOf(9);
            if (i % 3 < 2) // đi sang phải 
            {
                // khi đi sang phải thì sẽ sinh ra mảng mới khác với mảng ban đầu và sẽ được lưu vào mảng phát sinh
                List<int> copy_mangbandau = new List<int>(MangBanDau);
                int temp = copy_mangbandau[i];
                copy_mangbandau[i] = copy_mangbandau[i + 1];
                copy_mangbandau[i + 1] = temp;
                MangPhatSinh.Add(copy_mangbandau);
            }
            if (i % 3 > 0) // đi sang trái 
            {
                List<int> copy_mangbandau = new List<int>(MangBanDau);
                int temp = copy_mangbandau[i];
                copy_mangbandau[i] = copy_mangbandau[i - 1];
                copy_mangbandau[i - 1] = temp;
                MangPhatSinh.Add(copy_mangbandau);
            }
            if (i - 3 >= 0) // đi lên trên 
            {
                List<int> copy_mangbandau = new List<int>(MangBanDau);
                int temp = copy_mangbandau[i];
                copy_mangbandau[i] = copy_mangbandau[i - 3];
                copy_mangbandau[i - 3] = temp;
                MangPhatSinh.Add(copy_mangbandau);
            }
            if (i + 3 < MangBanDau.Count) // đi xuống dưới 
            {
                List<int> copy_mangbandau = new List<int>(MangBanDau);
                int temp = copy_mangbandau[i];
                copy_mangbandau[i] = copy_mangbandau[i + 3];
                copy_mangbandau[i + 3] = temp;
                MangPhatSinh.Add(copy_mangbandau);
            }
            return MangPhatSinh;
        }
        
        public List<State> ChiaTrangThai()
        {
            // chia mảng phát sinh thành từng trạng thái khác nhau 
            List<State> trangThaiPhatSinh = new List<State>();
            List < List<int> >mangPhatSinh = MangPhatSinh(this.trangThai);
            for(int i = 0; i<mangPhatSinh.Count; i++)
            {
                State Gop = new State(mangPhatSinh[i]);
                trangThaiPhatSinh.Add(Gop);
                Gop.father = this;

            }
            return trangThaiPhatSinh;
        }
        public bool Check_Dich(State mangGoc) // kiểm tra đến đích hay chưa 
        {
            bool Dich = true;
            for(int i = 0; i < mangGoc.trangThai.Count; i++ )
            {
                if (this.trangThai[i] != mangGoc.trangThai[i])
                {
                    Dich = false; break;
                }
            }
            return Dich;
        }

    }
}
