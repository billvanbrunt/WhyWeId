using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WhyWeID.Models;
using System.Net.Sockets;
using log4net;
using log4net.Config;



namespace WhyWeID.Controllers
{
    public class VisitsController : Controller
    {

        //for error logging take next line and place it into catch change the message to something appropate
        //logger.Error("error message goes");

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private WhyWeIDEntities db = new WhyWeIDEntities();

        // GET: Visits
        public ActionResult Index()
        {
            var visits = db.Visits.Include(v => v.Area).Include(v => v.Reason);

            var model = from x in db.Visits
                        where x.TimeOut == null
                        select x;
            return View(model.ToList());
        }

        // GET: Visits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visit visit = db.Visits.Find(id);
            if (visit == null)
            {
                return HttpNotFound();
            }
            return View(visit);
        }

        // GET: Visits/Create
        public ActionResult Create()
        {
            ViewBag.AreaToVisit = new SelectList(db.Areas, "AreaID", "Description");
            ViewBag.ReasonForVisit = new SelectList(db.Reasons, "ReasonID", "Description");
            return View();
        }

        // POST: Visits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SchoolId,LastName,FirstName,ReasonForVisit,AreaToVisit,SchoolYear,VisitDate,TimeIn,TimeOut")] Visit visit)
        {

            var lSchoolId = "000";
            var lSchoolYear = GetSchoolYear();
            var lIPAddress = GetIPAddress();
            visit.SchoolId = lSchoolId;


            try
            {
                if (ModelState.IsValid)
                {
                    db.Visits.Add(visit);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.AreaToVisit = new SelectList(db.Areas, "AreaID", "Description", visit.AreaToVisit);
                ViewBag.ReasonForVisit = new SelectList(db.Reasons, "ReasonID", "Description", visit.ReasonForVisit);
            }

            catch (InvalidCastException e)
            {
                log.Error("error message: " + e);
            }
            return View(visit);
        }

        // GET: Visits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visit visit = db.Visits.Find(id);
            if (visit == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaToVisit = new SelectList(db.Areas, "AreaID", "Description", visit.AreaToVisit);
            ViewBag.ReasonForVisit = new SelectList(db.Reasons, "ReasonID", "Description", visit.ReasonForVisit);
            return View(visit);
        }

        // POST: Visits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SchoolId,LastName,FirstName,ReasonForVisit,AreaToVisit,SchoolYear,VisitDate,TimeIn,TimeOut")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaToVisit = new SelectList(db.Areas, "AreaID", "Description", visit.AreaToVisit);
            ViewBag.ReasonForVisit = new SelectList(db.Reasons, "ReasonID", "Description", visit.ReasonForVisit);
            return View(visit);
        }

        // GET: Visits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visit visit = db.Visits.Find(id);
            if (visit == null)
            {
                return HttpNotFound();
            }
            return View(visit);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Visit visit = db.Visits.Find(id);
            db.Visits.Remove(visit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Visits/CisitorCheckOut
        public ActionResult VisitorCheckOut(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visit visit = db.Visits.Find(id);
            if (visit == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaToVisit = new SelectList(db.Areas, "AreaID", "Description", visit.AreaToVisit);
            ViewBag.ReasonForVisit = new SelectList(db.Reasons, "ReasonID", "Description", visit.ReasonForVisit);
            return View(visit);
        }

        // POST: Visits/VisitorCheckOut
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VisitorCheckOut([Bind(Include = "ID,SchoolId,LastName,FirstName,ReasonForVisit,AreaToVisit,SchoolYear,VisitDate,TimeIn,TimeOut")] Visit visit)
        {
            string lId = visit.ID.ToString();
            if (string.IsNullOrEmpty(lId) == false)
            {
                String StringTime = DateTime.Now.ToString("H:mm:ss");

                try
                {
                    if (ModelState.IsValid)
                    {
                        TimeSpan MyTime = TimeSpan.Parse(StringTime);
                        var ltime = TimeSpan.Parse(StringTime);

                        visit.TimeOut = MyTime;
                        db.Entry(visit).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {
                    var lTime = DateTime.Now.ToString("H:mm ss ");
                    log.Error("Error Message " + e.Message + lTime);
                }
            }
            ViewBag.AreaToVisit = new SelectList(db.Areas, "AreaID", "Description", visit.AreaToVisit);
            ViewBag.ReasonForVisit = new SelectList(db.Reasons, "ReasonID", "Description", visit.ReasonForVisit);
            return View(visit);
        }






        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public int GetSchoolYear()
        {

            DateTime currentDate = DateTime.Today;
            int lSchoolYear = currentDate.Year;
            int lSchoolMonth = currentDate.Month;

            if (lSchoolMonth >= 7 && lSchoolMonth <= 12)
            {
                lSchoolYear = lSchoolYear + 1;
            }

            return lSchoolYear;
        }

        public string GetIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }
    }
}
