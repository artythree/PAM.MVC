using PAM.DataN;
using PAM.ServicesN;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PAM.MVC.Controllers
{
    public class MarketDataController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private MarketDataServices services = new MarketDataServices();


        // GET: MarketData
        public ActionResult Index()
        {
            IQueryable<MarketData> dataQuery =
                    from data in _db.MarketDataPoints
                    where data.Date == DateTime.Today
                    select data;
            return View(dataQuery.ToList());
        }
        // GET: MarketData{ticker}
        public ActionResult ListByTicker(MarketData datum)
        {
            IQueryable<MarketData> dataQuery =
                    from data in _db.MarketDataPoints
                    where data.Ticker == datum.Ticker
                    select data;
            return View(dataQuery.ToList());
        }
        // GET: MarketData/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: MarketData/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string ticker)
        {
            IQueryable<MarketData> dataQuery =
                    from data in _db.MarketDataPoints
                    where data.Ticker == ticker
                    select data;


            List<MarketData> list = dataQuery.ToList();

            if (list.Count == 0)
            {
                var holder = await services.GetMarketData(ticker);

                dataQuery =
                    from data in _db.MarketDataPoints
                    where data.Ticker == ticker
                    select data;
                return View(dataQuery);
            }
            return View(ticker);
        }
        // GET: Perceptron/Delete{id}
        public ActionResult Delete(string ticker)
        {
            IQueryable<MarketData> dataQuery =
                    from data in _db.MarketDataPoints
                    where data.Ticker == ticker
                    select data;
            var dataQueryOne = dataQuery.ToList();
            int x = dataQueryOne.Count();
            return View(dataQueryOne[x - 1]);
        }
        // DELETE: Perceptron/Delete{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(MarketData data)
        {
            IQueryable<MarketData> dataQuery =
                    from d in _db.MarketDataPoints
                    where d.Ticker == data.Ticker
                    select d;

            _db.MarketDataPoints.RemoveRange(dataQuery);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}