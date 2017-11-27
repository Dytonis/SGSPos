using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSPos.Service
{
    public class CashManagement
    {
        public static void RecordCashIn(decimal amount, string date, User u)
        {

        }

        /// <summary>
        /// Removes a line from the cdt from the bottom up
        /// </summary>
        /// <param name="index">the number of lines from the bottom up to remove (0 is the last line)</param>
        public static void RemoveLine(int index)
        {

        }

        public static void RecordCashOut(decimal amount, string date, User u)
        {

        }

        public static decimal GetBalanceByDate(string date, User u)
        {
            return 0m;
        }
    }
}
