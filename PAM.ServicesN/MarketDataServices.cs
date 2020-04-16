using PAM.DataN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAM.ServicesN
{
    public class MarketDataServices
    {
        APIConnection connection = new APIConnection();
        ApplicationDbContext context = new ApplicationDbContext();


        public async Task<string> GetMarketData(string ticker)
        {
            int counter = 0;
            bool dateIncluded = false;
            foreach (MarketData data in context.MarketDataPoints)
            {
                if (data.Ticker == ticker)
                {
                    counter += 1;
                }
                if (data.Date.Date == DateTime.Today.AddDays(-1))
                {
                    dateIncluded = true;
                }
            }
            if (counter == 0 )
            {
                await connection.GetStocksAll(ticker);

                return "Added all stocks";
            }
            else if (counter != 0 && dateIncluded == false)
            {
                await connection.GetStocksToday(ticker);

                return "Added stock for today";
            }
            return "Value already in directory no change.";

        }
    }
}
