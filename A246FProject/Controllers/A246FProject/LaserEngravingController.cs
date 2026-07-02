using Microsoft.AspNetCore.Mvc;
using A246FProject.BAL.Reports;
using A246FProject.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace A246FProject.Controllers.A246FProject
{
    public class LaserEngravingController : Controller
    {
        LaserEngravingBAL _bal = new LaserEngravingBAL();

        [HttpPost]
        public JsonResult GetModelNoByProject(int projectId)
        {
            var models = _bal.GetModelNoByProject(projectId);
            return Json(models);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new LaserEngravingViewModel();

            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.ModelNos = _bal.GetModelNo();
            model.Machines = _bal.GetMachines();
            model.PartNos = new List<PartNo>();

            return View("~/Views/A246FProject/LaserEngraving/Index.cshtml", model);
        }

        [HttpPost]
        public IActionResult Index(LaserEngravingViewModel model)
        {
            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.ModelNos = _bal.GetModelNo();
            model.Machines = _bal.GetMachines();

            if (model.ModelId > 0)
                model.PartNos = _bal.GetPartNoByModel(model.ModelId);
            else
                model.PartNos = new List<PartNo>();

            model.dtChecklist = _bal.GetChecklist(model.LineId, model.ProjectId);

            return View("~/Views/A246FProject/LaserEngraving/Index.cshtml", model);
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(int modelId)
        {
            return Json(_bal.GetPartNoByModel(modelId));
        }

        [HttpPost]
        public JsonResult SaveSingleRow([FromBody] SaveSingleRowRequest request)
        {
            var r = request.Row;

            var employeeId = HttpContext.Session.GetString("User");

            DataTable dt = new DataTable();

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("LocationId", typeof(int));
            dt.Columns.Add("Value1");
            dt.Columns.Add("SerialNumber1");
            dt.Columns.Add("Value2");
            dt.Columns.Add("SerialNumber2");
            dt.Columns.Add("Value3");
            dt.Columns.Add("SerialNumber3");
            dt.Columns.Add("Value4");
            dt.Columns.Add("SerialNumber4");
            dt.Columns.Add("Value5");
            dt.Columns.Add("SerialNumber5");

            int rowId = 1;

            dt.Rows.Add(
                rowId++,
                r.LocationId,
                r.Value1,
                r.SerialNumber1,
                r.Value2,
                r.SerialNumber2,
                r.Value3,
                r.SerialNumber3,
                r.Value4,
                r.SerialNumber4,
                r.Value5,
                r.SerialNumber5
            );

            _bal.SaveLaserChecklist(request.Header, dt, employeeId);

            return Json(new { message = "Row saved successfully" });
        }

        [HttpPost]
        public JsonResult SaveLaserEngraving([FromBody] LaserEngravingViewModel model)
        {
            var employeeId = HttpContext.Session.GetString("User");

            if (model.LaserResults == null || model.LaserResults.Count == 0)
            {
                return Json(new { message = "No rows to save (LaserResults empty)" });
            }

            DataTable dt = new DataTable();

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("LocationId", typeof(int));
            dt.Columns.Add("Value1", typeof(decimal));
            dt.Columns.Add("SerialNumber1", typeof(string));
            dt.Columns.Add("Value2", typeof(decimal));
            dt.Columns.Add("SerialNumber2", typeof(string));
            dt.Columns.Add("Value3", typeof(decimal));
            dt.Columns.Add("SerialNumber3", typeof(string));
            dt.Columns.Add("Value4", typeof(decimal));
            dt.Columns.Add("SerialNumber4", typeof(string));
            dt.Columns.Add("Value5", typeof(decimal));
            dt.Columns.Add("SerialNumber5", typeof(string));

            int rowId = 1;

            foreach (var r in model.LaserResults)
            {
                dt.Rows.Add(
                    rowId++,
                    r.LocationId,
                    r.Value1,
                    r.SerialNumber1,
                    r.Value2,
                    r.SerialNumber2,
                    r.Value3,
                    r.SerialNumber3,
                    r.Value4,
                    r.SerialNumber4,
                    r.Value5,
                    r.SerialNumber5
                );
            }

            _bal.SaveLaserChecklist(model, dt, employeeId);

            return Json(new { message = "Saved successfully (Bulk)" });
        }

    }
}