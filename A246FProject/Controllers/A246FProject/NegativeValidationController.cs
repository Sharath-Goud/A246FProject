using A246FProject.BAL;
using A246FProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class NegativeValidationController : Controller
    {
        A246FCTPParameterBAL _commonBAL =
            new A246FCTPParameterBAL();

        NegativeValidationBAL _bal =
            new NegativeValidationBAL();

        [HttpGet]
        public IActionResult Index()
        {
            NegativeValidationViewModel model =
                new NegativeValidationViewModel();

            model.dtChecklist =
                new DataTable();

            model.Lines =
                _commonBAL.GetLine();

            model.Projects =
                _commonBAL.GetProject();

            model.ModelNos =
                new List<ModelNo>();

            model.PartNos =
                new List<PartNo>();

            model.Statuses =
                _bal.GetResult();

            return View(
                "~/Views/A246FProject/NegativeValidation/Index.cshtml",
                model);
        }

        [HttpGet]
        public JsonResult GetModels(
            int projectId)
        {
            return Json(
                _commonBAL
                .GetModelNoByProject(
                    projectId));
        }

        [HttpGet]
        public JsonResult GetParts(
            int modelId)
        {
            return Json(
                _commonBAL
                .GetPartNoByModel(
                    modelId));
        }

        [HttpPost]
        public IActionResult Index(NegativeValidationViewModel model)
        {
            model.Lines = _commonBAL.GetLine();
            model.Projects = _commonBAL.GetProject();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            model.Statuses = _bal.GetResult();

            model.dtChecklist =
                _bal.GetNegativeValidationData(model.LineId, model.ProjectId);

            return View("~/Views/A246FProject/NegativeValidation/Index.cshtml", model);
        }

        [HttpPost]
        public JsonResult SaveSingleNegativeValidation(
    [FromForm] int LineId,
    [FromForm] int ProjectId,
    [FromForm] int ModelId,
    [FromForm] int PartId,
    [FromForm] string ProdLineLeader,
    [FromForm] string CheckedBy,
    [FromForm] string ApprovedBy,
    [FromForm] int RiskId,
    [FromForm] string GoodSample,
    [FromForm] string FailSample)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("ValidId", typeof(int));
                dt.Columns.Add("GoodSample", typeof(string));
                dt.Columns.Add("FailSample", typeof(string));
                dt.Columns.Add("Result", typeof(string));

                dt.Rows.Add(
                    1,
                    RiskId,
                    GoodSample,
                    FailSample,
                    DBNull.Value
                );

                string createdBy = HttpContext.Session.GetString("User") ?? "Admin";

                int result = _bal.InsertBulkNegativeValidation(
                    dt, LineId, ProjectId, ProdLineLeader, CheckedBy, ApprovedBy,
                    ModelId, PartId, createdBy
                );

                return Json(result > 0 ? "Saved" : "Not Saved");
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex.Message);
            }
        }

        [HttpPost]
        public JsonResult SaveBulkNegativeValidation([FromForm] NegativeValidationBulkForm model)
        {
            try
            {
                if (model.NegativeValidationResults == null || model.NegativeValidationResults.Count == 0)
                    return Json("No Data Found");

                DataTable dt = new DataTable();
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("ValidId", typeof(int));
                dt.Columns.Add("GoodSample", typeof(string));
                dt.Columns.Add("FailSample", typeof(string));
                dt.Columns.Add("Result", typeof(string));

                int uid = 1;
                foreach (var r in model.NegativeValidationResults)
                {
                    dt.Rows.Add(
                        uid,
                        r.RiskId,
                        r.GoodSample,
                        r.FailSample,
                        DBNull.Value
                    );
                    uid++;
                }

                string createdBy = HttpContext.Session.GetString("User") ?? "Admin";

                int result = _bal.InsertBulkNegativeValidation(
                    dt, model.LineId, model.ProjectId, model.ProdLineLeader,
                    model.CheckedBy, model.ApprovedBy, model.ModelId, model.PartId, createdBy
                );

                return Json(result > 0 ? "Saved Successfully" : "Insert Failed");
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex.Message);
            }
        }

        // Private helper — not a separate endpoint, just shared logic used inline
        private string SaveUploadedFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "";

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return "/Images/" + fileName;
        }
    }
}
