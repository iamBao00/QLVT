using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT
{
   
    public partial class Xrpt_Hoat_Dong_Nhan_Vien : DevExpress.XtraReports.UI.XtraReport
    {
        public Xrpt_Hoat_Dong_Nhan_Vien()
        {
    
        }
        public static string NumberToText(int inputNumber, bool suffix = true)
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNumber = inputNumber.ToString("#");
            int number = Convert.ToInt32(sNumber);
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first

            string result = " ";


            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3) placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0)) result = "lẻ " + result;
                        if (tens == 1) result = "mười " + result;
                        if (tens > 1) result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0) break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }
            result = result.Trim();
            if (isNegative) result = "Âm " + result;
            return result + (suffix ? " đồng" : "");
        }
        public Xrpt_Hoat_Dong_Nhan_Vien(int manv, DateTime batdau, DateTime ketthuc)
        {
            InitializeComponent();
            this.sqlDS_Hoat_Dong_Nhan_Vien.Connection.ConnectionString = Program.connstr;
            this.sqlDS_Hoat_Dong_Nhan_Vien.Queries[0].Parameters[0].Value = manv;
            this.sqlDS_Hoat_Dong_Nhan_Vien.Queries[0].Parameters[1].Value = batdau;
            this.sqlDS_Hoat_Dong_Nhan_Vien.Queries[0].Parameters[2].Value = ketthuc;
            this.sqlDS_Hoat_Dong_Nhan_Vien.Fill();
            Console.WriteLine(this.Xrpt_Sum_Tien_Gia.Text.ToString());
            Console.WriteLine(this.Xrpt_Sum_Tien.Text.ToString());

            // this.Xrpt_lbtTienChu.Text = NumberToText(int.Parse(this.Xrpt_Sum_Tien_Gia.Text.ToString()));

        }
    }
}
