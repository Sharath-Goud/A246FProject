using System.Data;

namespace A246FProject.Models
{
    public class GlueWeighingViewModel
    {
        public DataTable dtChecklist { get; set; } = new DataTable();

        public List<Line> Lines { get; set; } = new();

        public List<Project> Projects { get; set; } = new();

        public List<ModelNo> ModelNos { get; set; } = new();

        public List<PartNo> PartNos { get; set; } = new();

        public List<Adhesive> Adhesives { get; set; } = new();


        public int LineId { get; set; }

        public int ProjectId { get; set; }

        public int ModelId { get; set; }

        public int PartId { get; set; }


        public int AdhesiveId { get; set; }


        public string ProdLineLeader { get; set; }

        public string CheckedBy { get; set; }

        public string ApprovedBy { get; set; }

        public List<GlueWeighingResult> GlueWeighingResults { get; set; } = new();
    }

    public class GlueWeighingResult
    {
        public int AdhesiveId { get; set; }

        public decimal DataValue { get; set; }

        public string RootCause { get; set; }
    }


    public class Adhesive
    {
        public int AdhesiveId { get; set; }

        public string AdhesiveName { get; set; }
    }
}