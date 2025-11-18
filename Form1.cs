namespace loan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {   // 表單載入時執行此事件處理函式
            int loan, year;
            double rate, payRate;
            loan = 500000;
            rate = 0.02;      //年利率
            year = 20;
            // 計算每月應付本息金額之平均攤還率
            payRate = ((Math.Pow((1 + rate / 12), year * 12) * rate / 12)) /
                       (Math.Pow((1 + rate / 12), year * 12) - 1);
            // 顯示本金
            LblLoan.Text += String.Format("{0} 元", loan);
            // 顯示年利率
            LblRate.Text += $"{rate * 100} %";
            // 顯示貸款年數
            LblYear.Text += $"{year} 年";
            // 顯示每月應付的本息
            LblPay.Text += ((int)(loan * payRate + 0.5)).ToString() + " 元";
        }
    }
}