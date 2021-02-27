using System.Web;
using System.Web.Mvc;

namespace LogiTax_Avinash
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
