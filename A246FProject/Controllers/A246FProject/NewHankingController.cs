using Microsoft.AspNetCore.Mvc;
using A246FProject.BAL;
using A246FProject.Models;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class NewHankingController : Controller
    {
        NewHankingBAL _bal =
            new NewHankingBAL();

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Role =
       HttpContext.Session.GetString("Role");
            NewHankingViewModel model =
                new NewHankingViewModel();

            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.Machines = _bal.GetMachines();
            model.ModelNos = _bal.GetModelNo();

            model.PartNos = new List<PartNo>();

            return View(
                "~/Views/A246FProject/NewHanking/Index.cshtml",
                model);
        }

        [HttpPost]
        public IActionResult Index(
            NewHankingViewModel model)
        {
            ViewBag.Role =
       HttpContext.Session.GetString("Role");
            model.Lines = _bal.GetLine();
            model.Projects = _bal.GetProject();
            model.Machines = _bal.GetMachines();
            model.ModelNos = _bal.GetModelNo();
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
                    _bal.GetNewHankingData(
                        model.LineId,
                        model.ProjectId,
                        model.MachineId,
                        model.PartId);

            return View(
                "~/Views/A246FProject/NewHanking/Index.cshtml",
                model);
        }

        [HttpPost]
        public JsonResult GetPartNoByModel(int modelId)
        {
            var partNos = _bal.GetPartNoByModel(modelId);

            return Json(partNos);
        }


        [HttpPost]
        public JsonResult GetModelNoByProject(int projectId)
        {
            var models = _bal.GetModelNoByProject(projectId);

            return Json(models);
        }

        [HttpPost]
        public JsonResult SubmitChecklist([FromBody] SubmitChecklistRequest request)
        {
            try
            {
                if (request == null || request.Items == null || request.Items.Count == 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "No rows to save."
                    });
                }

                if (request.LineId == 0 ||
                    request.ProjectId == 0 ||
                    request.ModelId == 0 ||
                    request.PartId == 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please select Line, Project, Model and Part."
                    });
                }

                string createdBy =
                    HttpContext.Session.GetString("User") ??
                    HttpContext.Session.GetString("EmployeeId") ??
                    "System";

                string modelName = _bal
                    .GetModelNoByProject(request.ProjectId)
                    .FirstOrDefault(x => x.ModelId == request.ModelId)?.Model;

                int result = _bal.InsertBulkA246FNewHanking(
                    request.Items,
                    createdBy,
                    request.LineId,
                    request.ProjectId,
                    modelName,
                    request.ProdLineLeader,
                    request.CheckedBy,
                    request.ApprovedBy,
                    request.ModelId,
                    request.PartId);

                if (result > 0)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Saved successfully."
                    });
                }

                return Json(new
                {
                    success = false,
                    message = "Unable to save data."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}