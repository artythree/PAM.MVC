using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.BatchQuotes;
using AlphaVantage.Net.Stocks.TimeSeries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PAM.Data
{
    //UI2LXNQXDODSOR3L is API key
    public class APIConnection
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public async Task GetStocksAll(String ticker)
        {
            string apiKey = "UI2LXNQXDODSOR3L";

            var client = new AlphaVantageStocksClient(apiKey);

            StockTimeSeries timeSeries = await client.RequestDailyTimeSeriesAsync($"{ticker}", TimeSeriesSize.Full, true);

            int perceptronId = 0;
            foreach (var perceptron in context.Perceptrons)
            {
                if (perceptron.Stock == ticker)
                {
                    perceptronId = perceptron.PerceptronId;
                }
            }
            if (perceptronId == 0)
            {
                Perceptron perceptron = new Perceptron();
                perceptron.Stock = ticker;

                context.Perceptrons.Add(perceptron);
                perceptronId = context.Perceptrons.Find(perceptron).PerceptronId;
                await context.SaveChangesAsync();
            }
            foreach (var dataPoint in timeSeries.DataPoints)
            {
                MarketData DataPoint = new MarketData();
                DataPoint.Date = dataPoint.Time.Date;
                DataPoint.Ticker = ticker;
                DataPoint.PercentChange = (double)(dataPoint.OpeningPrice / dataPoint.ClosingPrice);
                

                context.MarketDataPoints.Add(DataPoint);
                await context.SaveChangesAsync();
            }

        }
        public async Task GetStocksToday(String ticker)
        {
            string apiKey = "UI2LXNQXDODSOR3L";

            var client = new AlphaVantageStocksClient(apiKey);
            StockTimeSeries timeSeries = await client.RequestDailyTimeSeriesAsync($"{ticker}", TimeSeriesSize.Compact, true);

            int perceptronId = 0;
            foreach (var perceptron in context.Perceptrons)
            {
                if (perceptron.Stock == ticker)
                {
                    perceptronId = perceptron.PerceptronId;
                }
            }
            if (perceptronId == 0)
            {
                Perceptron perceptron = new Perceptron();
                perceptron.Stock = ticker;

                context.Perceptrons.Add(perceptron);
                perceptronId = context.Perceptrons.Find(perceptron).PerceptronId;
                await context.SaveChangesAsync();
            }
            foreach (var dataPoint in timeSeries.DataPoints)
            {
                if (dataPoint.Time.Date == DateTime.Today.AddDays(-1))
                {
                    MarketData DataPoint = new MarketData();
                    DataPoint.Date = dataPoint.Time.Date;
                    DataPoint.Ticker = ticker;
                    DataPoint.PercentChange = (double)(dataPoint.OpeningPrice / dataPoint.ClosingPrice);
                    

                    context.MarketDataPoints.Add(DataPoint);
                    await context.SaveChangesAsync();
                }
            }
        }

    }
}
