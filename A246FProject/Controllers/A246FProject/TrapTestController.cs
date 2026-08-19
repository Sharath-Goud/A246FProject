using A246FProject.BAL;
using A246FProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.A246FProject
{
    public class TrapTestController : Controller
    {
        private readonly A246FCTPParameterBAL _commonBAL;
        private readonly TrapTestBAL _bal;

        public TrapTestController()
        {
            _commonBAL = new A246FCTPParameterBAL();
            _bal = new TrapTestBAL();
        }


        [HttpGet]
        public IActionResult Index()
        {
            TrapTestViewModel model = new TrapTestViewModel();

            model.dtChecklist = new DataTable();

            model.Lines = _commonBAL.GetLine();

            model.Projects = _commonBAL.GetProject();

            model.ModelNos = new List<ModelNo>();

            model.PartNos = new List<PartNo>();

            model.InspectorSS = new List<Inspector>
    {
        new Inspector
        {
            InspecId = 1,
            InspectorName = "Primary"
        },
        new Inspector
        {
            InspecId = 2,
            InspectorName = "Secondary 1"
        },
        new Inspector
        {
            InspecId = 3,
            InspectorName = "Secondary 2"
        },
        new Inspector
        {
            InspecId = 4,
            InspectorName = "Secondary 3"
        }
    };

            return View(
                "~/Views/A246FProject/TrapTest/Index.cshtml",
                model);
        }


        [HttpGet]
        public JsonResult GetModels(int projectId)
        {
            var models =
                _commonBAL.GetModelNoByProject(projectId);

            return Json(models);
        }


        [HttpGet]
        public JsonResult GetParts(int modelId)
        {
            var parts =
                _commonBAL.GetPartNoByModel(modelId);

            return Json(parts);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(TrapTestViewModel model)
        {
            model.Lines =
                _commonBAL.GetLine();

            model.Projects =
                _commonBAL.GetProject();

            model.ModelNos =
                new List<ModelNo>();

            model.PartNos =
                new List<PartNo>();

            model.InspectorSS = new List<Inspector>
    {
        new Inspector
        {
            InspecId = 1,
            InspectorName = "Primary"
        },
        new Inspector
        {
            InspecId = 2,
            InspectorName = "Secondary 1"
        },
        new Inspector
        {
            InspecId = 3,
            InspectorName = "Secondary 2"
        },
        new Inspector
        {
            InspecId = 4,
            InspectorName = "Secondary 3"
        }
    };

            if (model.ProjectId > 0)
            {
                model.ModelNos =
                    _commonBAL.GetModelNoByProject(
                        model.ProjectId);
            }

            if (model.ModelId > 0)
            {
                model.PartNos =
                    _commonBAL.GetPartNoByModel(
                        model.ModelId);
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
                    "Please enter Production Line Leader, Checked By and Approved By.";

                model.dtChecklist = new DataTable();

                return View(
                    "~/Views/A246FProject/TrapTest/Index.cshtml",
                    model);
            }

            model.dtChecklist =
                _bal.GetTrapTestData(
                    model.LineId,
                    model.ProjectId);

            return View(
                "~/Views/A246FProject/TrapTest/Index.cshtml",
                model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveBulkTrapTest([FromForm] TrapTestBulkForm model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid request."
                    });
                }

                if (model.LineId <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please select Line."
                    });
                }

                if (model.ProjectId <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please select Project."
                    });
                }

                if (model.ModelId <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please select Model."
                    });
                }

                if (model.PartId <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please select Part."
                    });
                }

                if (string.IsNullOrWhiteSpace(model.ProdLineLeader))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please enter Production Line Leader."
                    });
                }

                if (string.IsNullOrWhiteSpace(model.CheckedBy))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please enter Checked By."
                    });
                }

                if (string.IsNullOrWhiteSpace(model.ApprovedBy))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please enter Approved By."
                    });
                }

                if (model.TrapTestResults == null ||
                    model.TrapTestResults.Count == 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "No checklist data found."
                    });
                }

                DataTable dt = new DataTable();

                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("InspectId", typeof(int));
                dt.Columns.Add("InspecId", typeof(int));
                dt.Columns.Add("InspectorName", typeof(string));
                dt.Columns.Add("InspectorId", typeof(string));
                dt.Columns.Add("NoOfCables", typeof(int));
                dt.Columns.Add("CheckResult", typeof(bool));
                dt.Columns.Add("SkippedQty", typeof(int));
                dt.Columns.Add("CheckedQty", typeof(int));

                int uid = 1;

                foreach (var row in model.TrapTestResults)
                {
                    int noOfCables = 0;
                    int skippedQty = 0;
                    int checkedQty = 0;

                    if (!string.IsNullOrWhiteSpace(row.NoOfCables))
                    {
                        int.TryParse(row.NoOfCables, out noOfCables);
                    }

                    if (!string.IsNullOrWhiteSpace(row.SkippedQty))
                    {
                        int.TryParse(row.SkippedQty, out skippedQty);
                    }

                    if (!string.IsNullOrWhiteSpace(row.CheckedQty))
                    {
                        int.TryParse(row.CheckedQty, out checkedQty);
                    }

                    bool checkResult = false;

                    if (!string.IsNullOrWhiteSpace(row.CheckResult))
                    {
                        if (row.CheckResult == "1" ||
                            row.CheckResult.Equals("true",
                                StringComparison.OrdinalIgnoreCase) ||
                            row.CheckResult.Equals("pass",
                                StringComparison.OrdinalIgnoreCase) ||
                            row.CheckResult.Equals("ok",
                                StringComparison.OrdinalIgnoreCase))
                        {
                            checkResult = true;
                        }
                    }

                    DataRow dr = dt.NewRow();

                    dr["Id"] = uid;
                    dr["InspectId"] = row.InspectId;
                    dr["InspecId"] = row.InspecId;
                    dr["InspectorName"] = row.InspectorName ?? "";
                    dr["InspectorId"] = row.InspectorId ?? "";
                    dr["NoOfCables"] = noOfCables;
                    dr["CheckResult"] = checkResult;
                    dr["SkippedQty"] = skippedQty;
                    dr["CheckedQty"] = checkedQty;

                    dt.Rows.Add(dr);

                    uid++;
                }

                string createdBy =
                    HttpContext.Session.GetString("User") ?? "Admin";

                int result = _bal.InsertBulkTrapTest(
                    dt,
                    model.LineId,
                    model.ProjectId,
                    model.ProdLineLeader,
                    model.CheckedBy,
                    model.ApprovedBy,
                    model.ModelId,
                    model.PartId,
                    createdBy
                );

                return Json(new
                {
                    success = true,
                    message = "Trap Test results saved successfully."
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