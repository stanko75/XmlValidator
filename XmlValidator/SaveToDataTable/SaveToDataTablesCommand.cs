using System.Data;

namespace XmlValidator.SaveToDataTable
{
    public class SaveToDataTablesCommand
    {
        public string XmlFile { get; set; }
        public string XslFile { get; set; }
        public DataSet ResultDataSet { get; set; }
        public string ValidationResult { get; set; }
        public bool DoNotUpdateExistingAddNew { get; set; }
        public string Flag { get; set; } = null;
    }
}