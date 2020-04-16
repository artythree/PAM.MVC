using PAM.DataN;
using PAM.ModelsN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PAM.MVC.Controllers
{

    public class PerceptronController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Perceptron
        public ActionResult Index()
        {
            return View(_db.Perceptrons.ToList());
        }
        // GET: Perceptron/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Perceptron/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Perceptron perceptron)
        {

            int x = 0;
            NetworkInitializer networkInitializer = new NetworkInitializer();
            Perceptron p = networkInitializer.CreatePerceptron(perceptron.Stock);

            if (ModelState.IsValid)
            {
                foreach (Perceptron pone in _db.Perceptrons)
                {
                    if (pone.Stock == p.Stock)
                    {
                        x += 1;
                    }
                }
                if (x == 0)
                {
                    _db.Perceptrons.Add(p);
                    _db.SaveChanges();
                    networkInitializer.CreateNetwork(p.Stock);
                    //networkInitializer.InitializeJoiningTables(p.Stock);
                    return RedirectToAction("Index");
                }
            }
            return View(perceptron);
        }
        // GET: Perceptron/Delete{id}
        public ActionResult Delete(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perceptron p = _db.Perceptrons.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }
        // DELETE: Perceptron/Delete{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            
            Perceptron p = _db.Perceptrons.Find(id);
            _db.Perceptrons.Remove(p);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}