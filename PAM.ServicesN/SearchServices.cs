using PAM.DataN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAM.ServicesN
{
    public class SearchServices
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        public bool HasBeenSearched(string ticker)
        {
            IQueryable<MarketData> dataQuery =
                    from data in _db.MarketDataPoints
                    where data.Ticker == ticker
                    select data;
            if (dataQuery.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
