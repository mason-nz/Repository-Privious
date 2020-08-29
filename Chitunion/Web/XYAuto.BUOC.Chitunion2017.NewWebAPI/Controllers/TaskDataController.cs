using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [RoutePrefix("TaskData")]
    public class TaskDataController : Controller
    {
        // GET: TaskData
        public ActionResult Index(int? pageIndex=1)
        {
            var query = CartBll.Instance.TaskListInfo(new QueryTaskargs { PageIndex = Convert.ToInt32(pageIndex) });

            List<TaskListModel> list = new List<TaskListModel>();
            ViewData["TaskList"] = query.List;
            ViewData["TotalCount"] = query.TotalCount;
            ViewData["TaskPageIndex"] = pageIndex;
            return View();
        }

        // GET: TaskData/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TaskData/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaskData/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskData/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaskData/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskData/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaskData/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
