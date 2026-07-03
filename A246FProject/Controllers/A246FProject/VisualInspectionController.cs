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

        model.dtChecklist =
            _bal.GetVisualInspectionData(
                model.LineId,
                model.ProjectId,
                model.ModelId,
                model.PartId,
                model.VisualsId);

        return View(
            "~/Views/A246FProject/VisualInspection/Index.cshtml",
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
    public JsonResult GetVisuals(int projectId)
    {
        return Json(_bal.GetVisuals(projectId));
    }
}