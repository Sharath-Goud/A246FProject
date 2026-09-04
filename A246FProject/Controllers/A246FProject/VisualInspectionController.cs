using A246FProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

public class VisualInspectionController : Controller
{
    private readonly VisualInspectionBAL _bal;

    public VisualInspectionController()
    {
        _bal = new VisualInspectionBAL();
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
    public JsonResult GetVisuals(int projectId)
    {
        return Json(_bal.GetVisuals(projectId));
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Role =
            HttpContext.Session.GetString("Role");

        VisualInspectionViewModel model = new();

        model.dtChecklist = new DataTable();

        model.Lines = _bal.GetLine();
        model.Shifts = _bal.GetShift();
        model.Projects = _bal.GetProject();

        model.ModelNos = new List<ModelNo>();
        model.PartNos = new List<PartNo>();
        model.Visualss = new List<Visuals>();

        return View(
            "~/Views/A246FProject/VisualInspection/Index.cshtml",
            model);
    }

    [HttpPost]
    public IActionResult Index(VisualInspectionViewModel model)
    {
        ViewBag.Role =
            HttpContext.Session.GetString("Role");

        model.Lines = _bal.GetLine();

        model.Shifts = _bal.GetShift();

        model.Projects = _bal.GetProject();

        model.ModelNos =
            model.ProjectId > 0
            ? _bal.GetModelNoByProject(model.ProjectId)
            : new List<ModelNo>();

        model.PartNos =
            model.ModelId > 0
            ? _bal.GetPartNoByModel(model.ModelId)
            : new List<PartNo>();

        model.Visualss =
            model.ProjectId > 0
            ? _bal.GetVisuals(model.ProjectId)
            : new List<Visuals>();

        if (model.LineId <= 0 ||
           model.ProjectId <= 0 ||
           model.ModelId <= 0 ||
           model.VisualsId <= 0 ||
           model.PartId <= 0 ||
           string.IsNullOrWhiteSpace(model.ProdLineLeader) ||
           string.IsNullOrWhiteSpace(model.CheckedBy) ||
           string.IsNullOrWhiteSpace(model.ApprovedBy))
        {

            ViewBag.Message =
            "Please enter Production Line Leader, Checked By and Approved By before Search";

            model.dtChecklist = new DataTable();

            return View(
            "~/Views/A246FProject/VisualInspection/Index.cshtml",
            model);

        }

        model.dtChecklist =
            _bal.GetVisualInspectionData(
                model.LineId,
                model.ProjectId,
                model.VisualsId);

        return View(
        "~/Views/A246FProject/VisualInspection/Index.cshtml",
        model);
    }

    [HttpPost]
    public IActionResult SaveSingle([FromBody] VisualInspectionViewModel model)
    {
        try
        {
            if (model == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid request data."
                });
            }

            if (model.VisualInspectionResults == null ||
                !model.VisualInspectionResults.Any())
            {
                return Json(new
                {
                    success = false,
                    message = "No inspection data found."
                });
            }

            string userId =
                HttpContext.Session.GetString("User") ?? "";

            DataTable dt = new DataTable();

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("DataId", typeof(int));
            dt.Columns.Add("Section1", typeof(string));
            dt.Columns.Add("DefectiveNumber", typeof(string));

            var row = model.VisualInspectionResults.First();

            dt.Rows.Add(
                1,
                row.DataId,
                row.Section1 ?? "",
                row.DefectiveNumber ?? "0"
            );

            _bal.InsertBulkVisualInspection(
                dt,
                userId,
                model.LineId,
                model.ProjectId,
                model.Model,
                model.ProdLineLeader ?? "",
                model.CheckedBy ?? "",
                model.ApprovedBy ?? "",
                model.ModelId,
                model.PartId
            );

            return Json(new
            {
                success = true,
                message = "Inspection data saved successfully."
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

    [HttpPost]
    public IActionResult Submit([FromBody] VisualInspectionViewModel model)
    {
        try
        {
            if (model == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid request data."
                });
            }

            if (model.VisualInspectionResults == null ||
                model.VisualInspectionResults.Count == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "No inspection data found."
                });
            }

            string userId =
                HttpContext.Session.GetString("User") ?? "";

            DataTable dt = new DataTable();

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("DataId", typeof(int));
            dt.Columns.Add("Section1", typeof(string));
            dt.Columns.Add("DefectiveNumber", typeof(string));

            int i = 1;

            foreach (var row in model.VisualInspectionResults)
            {
                dt.Rows.Add(
                    i++,
                    row.DataId,
                    row.Section1 ?? "",
                    row.DefectiveNumber ?? "0"
                );
            }

            _bal.InsertBulkVisualInspection(
                dt,
                userId,
                model.LineId,
                model.ProjectId,
                model.Model,
                model.ProdLineLeader ?? "",
                model.CheckedBy ?? "",
                model.ApprovedBy ?? "",
                model.ModelId,
                model.PartId
            );

            return Json(new
            {
                success = true,
                message = "Inspection data saved successfully."
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