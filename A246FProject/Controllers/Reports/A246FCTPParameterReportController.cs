using A246FProject.Models.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace A246FProject.Controllers.Reports
{
    public class A246FCTPParameterReportController
    {
        [HttpGet]
        public IActionResult Report()
        {
            A246FCTPParameterReportViewModel model =
                new A246FCTPParameterReportViewModel();

            model.Lines = _bal.GetLine();

            model.Projects = _bal.GetProject();

            model.Machines = _bal.GetMachines();

            model.ModelNos = _bal.GetModelNo();

            model.Shifts = _bal.GetShift();

            model.PartNos = new List<PartNo>();

            model.dtReports = new DataTable();

            return View(model);
        }

        [HttpPost]
        public IActionResult Report(
A246FCTPParameterReportViewModel model,
string Command)
        {
            model.Lines = _bal.GetLine();

            model.Projects = _bal.GetProject();

            model.Machines = _bal.GetMachines();

            model.ModelNos = _bal.GetModelNo();

            model.Shifts = _bal.GetShift();

            model.PartNos =
                _bal.GetPartNoByModel(model.ModelId);

            if (Command == "Search")
            {
                model.dtReports =
                    _bal.GetCTPParameterReport(
                        model.FromDate,
                        model.LineId,
                        model.ShiftId,
                        model.ProjectId,
                        model.MachineId,
                        model.ModelId,
                        model.PartId);

                HttpContext.Session.SetString(
                    "Report",
                    JsonConvert.SerializeObject(model.dtReports));
            }

            return View(model);
        }
    }
}
