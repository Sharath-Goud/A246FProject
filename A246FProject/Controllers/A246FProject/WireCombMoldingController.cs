using Microsoft.AspNetCore.Mvc;
using A246FProject.BAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class WireCombMoldingController : Controller
    {
        WireCombMoldingBAL _bal =
            new WireCombMoldingBAL();

        [HttpGet]
        public IActionResult Index()
        {
            WireCombMoldingViewModel model =
                new WireCombMoldingViewModel();

            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.ModelNos = _bal.GetModelNo();
            model.Machines = _bal.GetMachines();
            model.PartNos = new List<PartNo>();

            return View(
                "~/Views/A246FProject/WireCombMolding/Index.cshtml",
                model);
        }

        [HttpPost]
        public IActionResult Index(
            WireCombMoldingViewModel model)
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

            model.dtChecklist =
                _bal.GetWireCombMoldingData(
                    model.ProjectId,
                    model.LineId,
                    model.MachineId);

            return View(
                "~/Views/A246FProject/WireCombMolding/Index.cshtml",
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
        public IActionResult SaveA246FWCMMCCheckList([FromBody] WireCombMoldingViewModel model)
        {
            try
            {
                if (model?.WireCombMoldingResults == null)
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

                foreach (var row in model.WireCombMoldingResults)
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

                int result = _bal.InsertBulkA246FWCMMC1CheckList(
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

                return Json(new { success = true, result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}