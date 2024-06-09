using System.Globalization;

namespace LocationConsumer.Helper;

public class BucketCalculation
{
    //follows the rules of ISO 8601 
    public static string BucketWeekYear(DateTime timestamp)
    {
        var week = ISOWeek.GetWeekOfYear(timestamp);
        
        var year = ISOWeek.GetYear(timestamp);
        
        //examples 1-2024 or 12-2025
        var result = week + "-" + year;

        return result;
    }
    
    public static string BucketHourDate(DateTime timestamp)
    {
        //examples 11-30-00-2024 ie "hh-dd-mm-yyyy"
        //CHANGING the capital in formating for HH to hh would make it 12formating instead of 24
        var hourDate = timestamp.ToString("HH-dd-MM-yyyy");
        
        return hourDate;
    }
}