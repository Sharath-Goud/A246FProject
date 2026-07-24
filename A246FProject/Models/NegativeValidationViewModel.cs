using System.Data;

namespace A246FProject.Models
{
    public class NegativeValidationViewModel
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
        public NegativeValidationResult[] NegativeValidationResults { get; set; }
        public string CreatedBy { get; set; }
    }

    public class NegativeValidationResult
    {
        public int RiskId { get; set; }
        public int StatusId { get; set; }
        public string IdNumber { get; set; }
        public string Image { get; set; }
    }

    public class ValidationResult
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
    }

    public class NegativeValidationSingleRequest
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
        public string GoodSample { get; set; }
        public string FailSample { get; set; }
    }


    public class NegativeValidationResultForm
    {
        public int RiskId { get; set; }

        public string GoodSample { get; set; }

        public string FailSample { get; set; }
    }

    public class NegativeValidationBulkForm
    {
        public int LineId { get; set; }
        public int ProjectId { get; set; }
        public int ModelId { get; set; }
        public int PartId { get; set; }
        public string ProdLineLeader { get; set; }
        public string CheckedBy { get; set; }
        public string ApprovedBy { get; set; }
        public List<NegativeValidationResultForm> NegativeValidationResults { get; set; }
    }
}

