using SQLite;

namespace KakeiboApp.Models
{
    public class Budget
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public decimal Amount { get; set; }

        // 前月から前借りした金額
        public decimal BorrowedAmount { get; set; }
    }
}