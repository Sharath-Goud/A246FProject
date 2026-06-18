using Microsoft.AspNetCore.Mvc;
using A246FProject.BAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class CCMCHI4SCController : Controller
    {
        CCMCHI4SCBAL _bal =
            new CCMCHI4SCBAL();

        [HttpGet]
        public IActionResult Index()
        {
            CCMCHI4SCViewModel model =
                new CCMCHI4SCViewModel();

            model.Lines = _bal.GetLine();

            model.Projects = _bal.GetProject();

            model.ModelNos = _bal.GetModelNo();

            model.Machines = _bal.GetMachines();

            model.PartNos =
                new List<PartNo>();

            return View(
                "~/Views/A246FProject/CCMCHI4SC/Index.cshtml",
                model);
        }

        [HttpPost]
        public IActionResult Index(
            CCMCHI4SCViewModel model)
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
                _bal.GetCCMCHI4SCData(
                    model.LineId,
                    model.ProjectId,
                    model.MachineId);

            return View(
                "~/Views/A246FProject/CCMCHI4SC/Index.cshtml",
                model);
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(
            int modelId)
        {
            return Json(
                _bal.GetPartNoByModel(modelId));
        }

        [HttpPost]
        public JsonResult GetModelNoByProject(
            int projectId)
        {
            return Json(
                _bal.GetModelNoByProject(projectId));
        }

        [HttpPost]
        public JsonResult SaveA246FCCMMC1CheckList(
            [FromBody] CCMCHI4SCViewModel model)
        {
            try
            {
                DataTable dtChecklist = new DataTable();

                dtChecklist.Columns.Add("Id", typeof(int));
                dtChecklist.Columns.Add("ProjectId", typeof(int));
                dtChecklist.Columns.Add("InspectionId", typeof(int));
                dtChecklist.Columns.Add("SectionId", typeof(int));
                dtChecklist.Columns.Add("LineId", typeof(int));
                dtChecklist.Columns.Add("ShiftId", typeof(int));
                dtChecklist.Columns.Add("ApprovalId", typeof(int));

                dtChecklist.Columns.Add("Value1", typeof(decimal));
                dtChecklist.Columns.Add("Value2", typeof(decimal));
                dtChecklist.Columns.Add("Value3", typeof(decimal));
                dtChecklist.Columns.Add("Value4", typeof(decimal));
                dtChecklist.Columns.Add("Value5", typeof(decimal));

                dtChecklist.Columns.Add("InspectionResults", typeof(string));
                dtChecklist.Columns.Add("CreatedBy", typeof(string));
                dtChecklist.Columns.Add("ModelId", typeof(int));
                dtChecklist.Columns.Add("PartId", typeof(int));
                dtChecklist.Columns.Add("MachineId", typeof(int));

                int uid = 1;

                // ✔ SHIFT LOGIC (A/B SHIFT)
                int shiftId =
                    (DateTime.Now.TimeOfDay >= TimeSpan.Parse("08:00:00") &&
                     DateTime.Now.TimeOfDay <= TimeSpan.Parse("18:00:00"))
                    ? 0 : 1;

                foreach (var row in model.CCMCHI4SCResults)
                {
                    // ✔ IMPORTANT FIX: get InspectionId per SectionId
                    int inspectionId = _bal.GetInspectionIdBySection(row.SectionId);

                    dtChecklist.Rows.Add(
                        uid,
                        model.ProjectId,
                        inspectionId,
                        row.SectionId,
                        model.LineId,
                        shiftId,
                        0,

                        row.Value1 ?? 0,
                        row.Value2 ?? 0,
                        row.Value3 ?? 0,
                        row.Value4 ?? 0,
                        row.Value5 ?? 0,

                        "",
                        HttpContext.Session.GetString("User"),
                        model.ModelId,
                        model.PartId,
                        model.MachineId
                    );

                    uid++;
                }

                string userId = HttpContext.Session.GetString("User");

                if (string.IsNullOrEmpty(userId))
                {
                    return Json("Session User is NULL");
                }

                int result = _bal.InsertBulkA246FCMMC1CheckList(
                    dtChecklist,
                    userId,
                    model.LineId,
                    model.ProdLineLeader,
                    model.CheckedBy,
                    model.ApprovedBy,
                    model.ModelId,
                    model.PartId
                );

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}