namespace CMS.WebApp.Helper
{
    public class DateHelper
    {
        public static string getTimespan(DateTime dt)
        {
            TimeSpan span = DateTime.Now.Subtract(dt);
            if (span.Days <= 0 && span.Hours <= 0)
            {

                return span.Minutes.ToString() + " min ago";

            }
            else
            {
                return dt.ToString("HH:mm dd/MM/yyyy");
            }
        }
    }
}
