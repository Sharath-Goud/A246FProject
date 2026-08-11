//HotBarController

using Microsoft.AspNetCore.Mvc;
using A246FProject.BAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class HotBarController : Controller
    {
        HotBarBAL _bal =
            new HotBarBAL();

        [HttpGet]
        public IActionResult Index()
        {
            HotBarViewModel model =
                new HotBarViewModel();

            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.ModelNos = _bal.GetModelNo();
            model.Machines = _bal.GetMachines();
            model.PartNos = new List<PartNo>();

            return View(
                "~/Views/A246FProject/HotBar/Index.cshtml",
                model);
        }

        [HttpPost]
        public IActionResult Index(
    HotBarViewModel model)
        {
 
            model.Lines = _bal.GetLine();

            model.Projects = _bal.GetProject();

            model.ModelNos = _bal.GetModelNo();

            model.Machines = _bal.GetMachines();

            if (model.ModelId > 0)
            {
                model.PartNos =
                    _bal.GetPartNoByModel(model.ModelId);
            }
            else
            {
                model.PartNos =
                    new List<PartNo>();
            }

            if (model.LineId <= 0 ||
                model.ProjectId <= 0 ||
                model.ModelId <= 0 ||
                model.PartId <= 0 ||
                string.IsNullOrWhiteSpace(model.ProdLineLeader) ||
                string.IsNullOrWhiteSpace(model.CheckedBy) ||
                string.IsNullOrWhiteSpace(model.ApprovedBy))
            {
                ViewBag.Message =
                    "Please enter Production Line Leader, Checked By and Approved By before Search.";

                model.dtChecklist = new DataTable();

                return View(
                    "~/Views/A246FProject/HotBar/Index.cshtml",
                    model);
            }

            model.dtChecklist =
                _bal.GetHotBarData(
                    model.ProjectId,
                    model.LineId);


            return View(
                "~/Views/A246FProject/HotBar/Index.cshtml",
                model);
        }

        [HttpPost]
        public JsonResult GetModelNoByProject(
            int projectId)
        {
            return Json(
                _bal.GetModelNoByProject(projectId));
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(
            int modelId)
        {
            return Json(
                _bal.GetPartNoByModel(modelId));
        }

        [HttpPost]
        public IActionResult SaveA246FMHB1CheckList([FromBody] HotBarViewModel model)
        {
            try
            {
                if (model?.HotBarResult == null)
                    return Json("No data received");

                string userId = HttpContext.Session.GetString("User");

                if (string.IsNullOrEmpty(userId))
                    return Json("Session expired - login again");

                DataTable dt = new DataTable();

                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("SectionId", typeof(int));
                dt.Columns.Add("Value1", typeof(decimal));
                dt.Columns.Add("Value2", typeof(decimal));
                dt.Columns.Add("Value3", typeof(decimal));
                dt.Columns.Add("Value4", typeof(decimal));
                dt.Columns.Add("Value5", typeof(decimal));
                dt.Columns.Add("InspectionResults", typeof(string));
                dt.Columns.Add("CablesSerialNumber", typeof(string));

                int i = 1;

                foreach (var row in model.HotBarResult)
                {
                    dt.Rows.Add(
                        i++,
                        row.SectionId,
                        row.Value1 ?? 0,
                        row.Value2 ?? 0,
                        row.Value3 ?? 0,
                        row.Value4 ?? 0,
                        row.Value5 ?? 0,
                        "",
                        row.CablesSerialNumber
                    );
                }

                int result = _bal.InsertBulkA246FMHB1CheckList(
                    dt,
                    model.LineId,
                    model.ProjectId,
                    model.ModelId,
                    model.PartId,
                    model.ProdLineLeader,
                    model.CheckedBy,
                    model.ApprovedBy,
                    userId
                );

                return Json(new { success = true, message = "Saved Successfully", result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
