using A246FProject.BAL;
using A246FProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class SOPPORController : Controller
    {
        A246FCTPParameterBAL _commonBAL =
            new A246FCTPParameterBAL();

        SOPPORBAL _bal =
            new SOPPORBAL();

        [HttpGet]
        public IActionResult Index()
        {
            SOPPORViewModel model =
                new SOPPORViewModel();

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
                "~/Views/A246FProject/SOPPOR/Index.cshtml",
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
        public IActionResult Index(SOPPORViewModel model)
        {
            model.Lines = _commonBAL.GetLine();
            model.Projects = _commonBAL.GetProject();

            model.ModelNos = new List<ModelNo>();
            model.PartNos = new List<PartNo>();

            model.Statuses = _bal.GetResult();

            model.dtChecklist =
                _bal.GetSOPPORData(model.LineId, model.ProjectId);

            return View("~/Views/A246FProject/SOPPOR/Index.cshtml", model);
        }

        [HttpPost]
        public JsonResult SaveSingleSOPPOR(
    [FromForm] int LineId,
    [FromForm] int ProjectId,
    [FromForm] int ModelId,
    [FromForm] int PartId,
    [FromForm] string ProdLineLeader,
    [FromForm] string CheckedBy,
    [FromForm] string ApprovedBy,
    [FromForm] int RiskId,
    [FromForm] int StatusId,
    [FromForm] string IdNumber,
    IFormFile ImageFile)
        {
            try
            {
                string imagePath = SaveUploadedFile(ImageFile);

                DataTable dt = new DataTable("A246FQualityAuditNew");
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("RiskId", typeof(int));
                dt.Columns.Add("StatusId", typeof(int));
                dt.Columns.Add("Namee", typeof(string));
                dt.Columns.Add("Image", typeof(string));

                dt.Rows.Add(
                    1, RiskId, StatusId, IdNumber,
                    string.IsNullOrEmpty(imagePath) ? (object)DBNull.Value : imagePath
                );

                string createdBy = HttpContext.Session.GetString("User") ?? "Admin";

                int result = _bal.InsertBulkSOPPOR(
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
        public JsonResult SaveBulkSOPPOR([FromForm] SOPPORBulkForm model)
        {
            try
            {
                if (model.SOPPORResults == null || model.SOPPORResults.Count == 0)
                    return Json("No Data Found");

                DataTable dt = new DataTable("A246FQualityAuditNew");
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("RiskId", typeof(int));
                dt.Columns.Add("StatusId", typeof(int));
                dt.Columns.Add("Namee", typeof(string));
                dt.Columns.Add("Image", typeof(string));

                int uid = 1;
                foreach (var r in model.SOPPORResults)
                {
                    string imagePath = SaveUploadedFile(r.ImageFile);

                    dt.Rows.Add(
                        uid, r.RiskId, r.StatusId, r.IdNumber,
                        string.IsNullOrEmpty(imagePath) ? (object)DBNull.Value : imagePath
                    );
                    uid++;
                }

                string createdBy = HttpContext.Session.GetString("User") ?? "Admin";

                int result = _bal.InsertBulkSOPPOR(
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