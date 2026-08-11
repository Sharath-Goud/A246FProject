using A246FProject.BAL;
using A246FProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class GlueWeighingController : Controller
    {
        private readonly MasterBAL _master;

        public GlueWeighingController()
        {
            _master = new MasterBAL();
        }


        [HttpPost]
        public JsonResult GetModelNoByProject(int projectId)
        {
            return Json(_master.GetModelNoByProject(projectId));
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(int modelId)
        {
            return Json(_master.GetPartNoByModel(modelId));
        }

        [HttpPost]
        public JsonResult GetAdhesiveByProject(int projectId)
        {
            var adhesive = _master.GetAdhesive(projectId);

            return Json(adhesive);
        }

        [HttpGet]
        public IActionResult Index()
        {
            GlueWeighingViewModel model = new GlueWeighingViewModel();

            model.Lines = _master.GetLine();

            model.Projects = _master.GetProject();

            model.Adhesives = new List<Adhesive>();

            model.ModelNos = new List<ModelNo>();

            model.PartNos = new List<PartNo>();

            return View(
                "~/Views/A246FProject/GlueWeighing/Index.cshtml",
                model);
        }

        [HttpPost]
        public IActionResult GetGlueWeighingData(
            GlueWeighingViewModel model)
        {

            model.Lines = _master.GetLine();

            model.Projects = _master.GetProject();

            model.ModelNos = model.ProjectId > 0
                ? _master.GetModelNoByProject(model.ProjectId)
                : new List<ModelNo>();

            model.PartNos = model.ModelId > 0
                ? _master.GetPartNoByModel(model.ModelId)
                : new List<PartNo>();

            model.Adhesives = model.ProjectId > 0
                ? _master.GetAdhesive(model.ProjectId)
                : new List<Adhesive>();

            if (model.LineId <= 0 ||
               model.ProjectId <= 0 ||
               model.ModelId <= 0 ||
               model.PartId <= 0 ||
               model.AdhesiveId <= 0 ||
               string.IsNullOrWhiteSpace(model.ProdLineLeader) ||
               string.IsNullOrWhiteSpace(model.CheckedBy) ||
               string.IsNullOrWhiteSpace(model.ApprovedBy))
            {

                ViewBag.Message =
                "Please enter Production Line Leader, Checked By and Approved By";

                model.dtChecklist = null;

                return View(
                "~/Views/A246FProject/GlueWeighing/Index.cshtml",
                model);

            }

            model.dtChecklist =
                _master.GetFormByA246FAdhesive(
                    model.LineId,
                    model.ProjectId,
                    model.AdhesiveId);

            return View(
                "~/Views/A246FProject/GlueWeighing/Index.cshtml",
                model);
        }

        [HttpPost]
        public IActionResult SaveGlueWeighing(GlueWeighingViewModel model)
        {
            try
            {
                DataTable dtChecklist = new DataTable("GlueWeighingChecklist");

                dtChecklist.Columns.Add("Id");
                dtChecklist.Columns.Add("AdhesiveId");
                dtChecklist.Columns.Add("DataValue");
                dtChecklist.Columns.Add("RootCause");

                int id = 1;

                foreach (var item in model.GlueWeighingResults)
                {
                    dtChecklist.Rows.Add(
                        id,
                        item.AdhesiveId,
                        item.DataValue,
                        item.RootCause);

                    id++;
                }

                int result = _master.InsertBulkGlueWeighingData(
                    dtChecklist,
                    "2063907",
                    model.LineId,
                    model.ProjectId,
                    model.ProdLineLeader,
                    model.CheckedBy,
                    model.ApprovedBy,
                    model.ModelId,
                    model.PartId);

                if (result > 0)
                {
                    TempData["msg"] = "Glue Weighing Saved Successfully";
                }
                else
                {
                    TempData["msg"] = "Data not saved";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}