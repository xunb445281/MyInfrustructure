using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Helpers
{
    /// <summary>
    /// 时间帮助类
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// 生成时间戳
        /// </summary>
        /// <param name="dateTimeType"></param>
        /// <returns></returns>
        public static string CreateTimestamp(DateTimeType dateTimeType = DateTimeType.Milliseconds)
        {
            var dt = DateTime.UtcNow - new DateTime(1970, 1, 1);

            double timestamp = 0.0;

            switch(dateTimeType)
            {
                case DateTimeType.Day:
                    timestamp = dt.TotalDays;
                    break;
                case DateTimeType.Hour:
                    timestamp = dt.TotalHours;
                    break;
                case DateTimeType.Minutes:
                    timestamp = dt.TotalMinutes;
                    break;
                case DateTimeType.Seconds:
                    timestamp = dt.TotalSeconds;
                    break;
                default:
                    timestamp = dt.TotalMilliseconds;
                    break;
            }

            return timestamp.ToString();
        }
    }

    /// <summary>
    /// 日期类型
    /// </summary>
    public enum DateTimeType
    {
        Year,
        Month,
        Day,
        Hour,
        Minutes,
        Seconds,
        Milliseconds
    }
}
