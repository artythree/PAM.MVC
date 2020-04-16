using PAM.DataN;
using PAM.ServicesN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PAM.MVC.Controllers
{
    public class SearchController : Controller
    {
        private SearchServices sService = new SearchServices();
        private MarketDataServices mService = new MarketDataServices();
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Search
        public ActionResult Index()
        {
            return View(_db.Searches.ToList());
        }
        // GET: Search/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Search/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Search search)
        {
            if (ModelState.IsValid)
            {


                if (sService.HasBeenSearched(search.Stock))
                {

                    //IQueryable<MarketData> dataQueryOne =
                    //from data in _db.MarketDataPoints
                    //where data.Ticker == search.Stock
                    //select data;
                    return View(_db.Searches.Where(x=>x.Stock==search.Stock));

                }
                await mService.GetMarketData(search.Stock);
                
                return View(search);
            }
            return View(search);
        }
        
    }
}