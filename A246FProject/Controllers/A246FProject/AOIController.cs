using Microsoft.AspNetCore.Mvc;
using A246FProject.BAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class AOIController : Controller
    {
        private readonly AOIBAL _bal = new AOIBAL();

        [HttpGet]
        public IActionResult Index()
        {
            AOIViewModel model = new AOIViewModel();

            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.ModelNos = _bal.GetModelNo();
            model.Machines = _bal.GetMachines();
            model.PartNos = new List<PartNo>();
            model.dtChecklist = new DataTable();

            return View("~/Views/A246FProject/AOI/Index.cshtml", model);
        }

        [HttpPost]
        public IActionResult Index(AOIViewModel model)
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
                model.MachineId <= 0 ||
                string.IsNullOrWhiteSpace(model.ProdLineLeader) ||
                string.IsNullOrWhiteSpace(model.CheckedBy) ||
                string.IsNullOrWhiteSpace(model.ApprovedBy))
            {

                ViewBag.Message =
                "Please enter Production Line Leader, Checked By and Approved By before Search.";

                model.dtChecklist = new DataTable();

                return View(
                    "~/Views/A246FProject/AOI/Index.cshtml",
                    model);
            }

            model.dtChecklist =
                _bal.GetAOIData(
                    model.ProjectId,
                    model.LineId,
                    model.MachineId);

            return View(
                "~/Views/A246FProject/AOI/Index.cshtml",
                model);
        }


        [HttpPost]
        public JsonResult GetModelNoByProject(int projectId)
        {
            return Json(_bal.GetModelNoByProject(projectId));
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(int modelId)
        {
            return Json(_bal.GetPartNoByModel(modelId));
        }


        [HttpPost]
        public JsonResult Save([FromBody] AOIViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Invalid Request."
                    });
                }

                if (model.AOIResults == null ||
                    model.AOIResults.Length == 0)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "No Data Found."
                    });
                }

                DataTable dtChecklist = new DataTable();

                dtChecklist.Columns.Add("Id", typeof(int));
                dtChecklist.Columns.Add("SectionId", typeof(int));
                dtChecklist.Columns.Add("Value1", typeof(decimal));
                dtChecklist.Columns.Add("Value2", typeof(decimal));
                dtChecklist.Columns.Add("Value3", typeof(decimal));
                dtChecklist.Columns.Add("Value4", typeof(decimal));
                dtChecklist.Columns.Add("Value5", typeof(decimal));
                dtChecklist.Columns.Add("InspectionResults", typeof(string));

                int id = 1;

                foreach (var row in model.AOIResults)
                {
                    dtChecklist.Rows.Add(
                        id++,
                        row.SectionId,
                        row.Value1 ?? 0,
                        row.Value2 ?? 0,
                        row.Value3 ?? 0,
                        row.Value4 ?? 0,
                        row.Value5 ?? 0,
                        row.InspectionResults ?? ""
                    );
                }

                string user = HttpContext.Session.GetString("User");
                
                if (string.IsNullOrWhiteSpace(user))
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Session Expired."
                    });
                }

                int result = _bal.InsertBulkAOI(
                    dtChecklist,
                    user,
                    model.LineId,
                    model.ProjectId,
                    model.ProdLineLeader,
                    model.CheckedBy,
                    model.ApprovedBy,
                    model.ModelId,
                    model.PartId);

                if (result > 0)
                {
                    return Json(new
                    {
                        Success = true,
                        Message = "Saved Successfully."
                    });
                }

                return Json(new
                {
                    Success = false,
                    Message = "Already Exists."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

    }
}