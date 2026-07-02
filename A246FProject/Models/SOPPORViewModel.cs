using System.Data;
using Microsoft.AspNetCore.Http;   // ADD THIS — needed for IFormFile

namespace A246FProject.Models
{
    public class SOPPORViewModel
    {
        public DataTable dtChecklist { get; set; }
        public List<Line> Lines { get; set; }
        public List<Project> Projects { get; set; }
        public List<ModelNo> ModelNos { get; set; }
        public List<PartNo> PartNos { get; set; }
        public List<Result> Statuses { get; set; }
        public int LineId { get; set; }
        public int ProjectId { get; set; }
        public int ModelId { get; set; }
        public int PartId { get; set; }
        public int StatusId { get; set; }
        public string ProdLineLeader { get; set; }
        public string CheckedBy { get; set; }
        public string ApprovedBy { get; set; }
        public SOPPORResult[] SOPPORResults { get; set; }
        public string CreatedBy { get; set; }
    }

    public class SOPPORResult
    {
        public int RiskId { get; set; }
        public int StatusId { get; set; }
        public string IdNumber { get; set; }
        public string Image { get; set; }
    }

    public class Result
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
    }

    public class SOPPORSingleRequest
    {
        public int LineId { get; set; }
        public int ProjectId { get; set; }
        public int ModelId { get; set; }
        public int PartId { get; set; }
        public string ProdLineLeader { get; set; }
        public string CheckedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string CreatedBy { get; set; }
        public int RiskId { get; set; }
        public int StatusId { get; set; }
        public string IdNumber { get; set; }
    }


    public class SOPPORResultForm
    {
        public int RiskId { get; set; }
        public int StatusId { get; set; }
        public string IdNumber { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class SOPPORBulkForm
    {
        public int LineId { get; set; }
        public int ProjectId { get; set; }
        public int ModelId { get; set; }
        public int PartId { get; set; }
        public string ProdLineLeader { get; set; }
        public string CheckedBy { get; set; }
        public string ApprovedBy { get; set; }
        public List<SOPPORResultForm> SOPPORResults { get; set; }
    }
}