using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Helper
{
    public static class UnixHelper
    {
        #region  获取时间戳13位
        // <summary>  
        /// 获取时间戳  13位
        /// </summary>  
        /// <returns></returns>  
        public static long GetTimeStamp(DateTime datetime)
        {
            TimeSpan ts = datetime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds * 1000);
        }
        #endregion

        #region  获取时间戳 10位
        /// <summary> 
        /// 获取时间戳 10位
        /// </summary> 
        /// <returns></returns> 
        public static long GetTimeStampTen(DateTime datetime)
        {
            return (datetime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
        #endregion 

        #region 将时间戳转换为日期类型，并格式化
        /// <summary>
        /// 将时间戳转换为日期类型，并格式化
        /// </summary>
        /// <param name="longDateTime">时间戳格式</param>
        /// <returns>yyyy-MM-dd HH:mm:ss</returns>
        public static string LongDateTimeToDateTimeString(string longDateTime)
        {
            //用来格式化long类型时间的,声明的变量
            long unixDate;
            DateTime start;
            DateTime date;
            //ENd

            unixDate = long.Parse(longDateTime);
            start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            date = start.AddMilliseconds(unixDate).ToLocalTime();

            return date.ToString("yyyy-MM-dd HH:mm:ss");

        }
        #endregion 

    }
}