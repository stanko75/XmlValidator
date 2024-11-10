using System.Data;
using XmlValidator.DataTables.Models;

namespace XmlValidator.DataTables
{
    public class ValidatorDataTables
    {
        private DataTable CreateXmlFilesDataTable()
        {
            DataTable dtXmlFiles = new DataTable(DataTableNames.XmlFiles);
            dtXmlFiles.Columns.Add(XmlFilesColumns.Id, typeof(int));
            dtXmlFiles.Columns.Add(XmlFilesColumns.FileName, typeof(string));
            dtXmlFiles.Columns.Add(XmlFilesColumns.ErrorCount, typeof(int));
            dtXmlFiles.Columns.Add(XmlFilesColumns.WarningCount, typeof(int));
            dtXmlFiles.Columns.Add(XmlFilesColumns.ExceptionsCount, typeof(int));
            dtXmlFiles.PrimaryKey = new[]
            {
                dtXmlFiles.Columns[XmlFilesColumns.Id],
                dtXmlFiles.Columns[XmlFilesColumns.FileName]
            };
            return dtXmlFiles;
        }

        private DataTable CreateXslFilesDataTable()
        {
            DataTable dtXslFiles = new DataTable(DataTableNames.XslFiles);
            dtXslFiles.Columns.Add(XslFilesColumns.Id, typeof(int));
            dtXslFiles.Columns.Add(XslFilesColumns.FileName, typeof(string));
            dtXslFiles.Columns.Add(XslFilesColumns.FkXmlFilesId, typeof(int));
            dtXslFiles.Columns.Add(XslFilesColumns.ErrorCount, typeof(int));
            dtXslFiles.Columns.Add(XslFilesColumns.WarningCount, typeof(int));
            dtXslFiles.Columns.Add(XslFilesColumns.ExceptionsCount, typeof(int));
            dtXslFiles.PrimaryKey = new[]
            {
                dtXslFiles.Columns[XslFilesColumns.Id],
                dtXslFiles.Columns[XslFilesColumns.FkXmlFilesId]
            };
            return dtXslFiles;
        }

        private DataTable CreateResultValidator()
        {
            DataTable dtResultValidator = new DataTable(DataTableNames.ResultValidator);
            dtResultValidator.Columns.Add(ResultValidatorColumns.Text, typeof(string));
            dtResultValidator.Columns.Add(ResultValidatorColumns.Id, typeof(int));
            dtResultValidator.Columns.Add(ResultValidatorColumns.FkXslFilesId, typeof(int));
            dtResultValidator.Columns.Add(ResultValidatorColumns.Flag, typeof(string));
            dtResultValidator.Columns.Add(ResultValidatorColumns.XRechnungId, typeof(string));
            dtResultValidator.Columns.Add(ResultValidatorColumns.Test, typeof(string));
            dtResultValidator.Columns.Add(ResultValidatorColumns.Location, typeof(string));
            dtResultValidator.Columns.Add(ResultValidatorColumns.ExceptionMessage, typeof(string));
            dtResultValidator.PrimaryKey = new[]
            {
                dtResultValidator.Columns[ResultValidatorColumns.Id],
                dtResultValidator.Columns[ResultValidatorColumns.FkXslFilesId]
            };
            return dtResultValidator;
        }

        public DataSet CreateValidatorDataSet()
        {
            DataSet dsValidator = new DataSet("Validator");
            dsValidator.Tables.Add(CreateXmlFilesDataTable());
            dsValidator.Tables.Add(CreateXslFilesDataTable());
            dsValidator.Tables.Add(CreateResultValidator());
            DataRelation relXmlFilesXslFilesRelation = new DataRelation(RelationNames.XmlFilesXslFilesRelation
                , dsValidator.Tables[DataTableNames.XmlFiles].Columns[XmlFilesColumns.Id]
                , dsValidator.Tables[DataTableNames.XslFiles].Columns[XslFilesColumns.FkXmlFilesId]
            );
            DataRelation relXslFilesResultRelation = new DataRelation(RelationNames.XslFilesResultRelation
                , dsValidator.Tables[DataTableNames.XslFiles].Columns[XslFilesColumns.Id]
                , dsValidator.Tables[DataTableNames.ResultValidator].Columns[ResultValidatorColumns.FkXslFilesId]
            );
            dsValidator.Relations.Add(relXmlFilesXslFilesRelation);
            dsValidator.Relations.Add(relXslFilesResultRelation);
            return dsValidator;
        }
    }
}