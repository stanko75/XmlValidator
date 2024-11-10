using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlValidator.DataTables.Models;

namespace XmlValidator.SaveToDataTable
{
    public class SaveToDataTablesHandlerAsync : ICommandHandlerAsync<SaveToDataTablesCommand, DataSet>
    {
        public async Task<DataSet> Execute(SaveToDataTablesCommand command)
        {
            DataSet dsResult = command.ResultDataSet;
            await Task.Run(() =>
            {
                SaveToDataSet(dsResult, command.XmlFile, command.XslFile, command.ValidationResult, command.DoNotUpdateExistingAddNew, command.Flag);
                return dsResult;
            });

            return null;
        }

        private void SaveToDataSet(DataSet dsResult, string xmlFile, string xslFile, string validationResult, bool addNew, string flag)
        {
            AddXmlFilesDataTableIfNotExist(dsResult, xmlFile, addNew);
            AddXslFilesDataTable(dsResult, xslFile);

            if (string.IsNullOrEmpty(validationResult)) return;

            XDocument doc = null;
            try
            {
                doc = XDocument.Parse(validationResult);
            }
            catch
            {
                FailedAssert failedAssert = new FailedAssert
                {
                    Flag = flag ?? Flags.Exception,
                    ExceptionMessage = validationResult
                };
                AddToResultDataTable(dsResult, failedAssert);
            }
            if (doc is null) return;

            XNamespace svrl = "http://purl.oclc.org/dsdl/svrl";
            IEnumerable<FailedAssert> failedAsserts = doc.Descendants(svrl + "failed-assert")
                .Select(fa => new FailedAssert
                {
                    Flag = fa.Attribute("flag")?.Value,
                    Id = fa.Attribute("id")?.Value,
                    Test = fa.Attribute("test")?.Value,
                    Location = fa.Attribute("location")?.Value,
                    Text = fa.Element(svrl + "text")?.Value
                });

            IEnumerable<FailedAssert> asserts = failedAsserts as FailedAssert[] ?? failedAsserts.ToArray();
            foreach (FailedAssert failedAssert in asserts)
            {
                AddToResultDataTable(dsResult, failedAssert);
            }
        }

        private void AddXmlFilesDataTableIfNotExist(DataSet dsResult, string xmlFile, bool addNew)
        {
            DataTable dtXmlFiles = dsResult.Tables[DataTableNames.XmlFiles];

            string searchExpression = $"{XmlFilesColumns.Id} = {dtXmlFiles.Rows.Count - 1} AND {XmlFilesColumns.FileName} = '{xmlFile}'";
            DataRow[] drXmlFilesFiltered = dtXmlFiles.Select(searchExpression);
            if (drXmlFilesFiltered.Length == 0 || addNew)
            {
                DataRow drXmlFiles = dtXmlFiles.NewRow();
                drXmlFiles[XmlFilesColumns.Id] = dtXmlFiles.Rows.Count;
                drXmlFiles[XmlFilesColumns.FileName] = xmlFile;
                dtXmlFiles.Rows.Add(drXmlFiles);
            }
        }

        private void AddXslFilesDataTable(DataSet dsResult, string xslFile)
        {
            DataTable dtXmlFiles = dsResult.Tables[DataTableNames.XmlFiles];
            DataTable dtXslFiles = dsResult.Tables[DataTableNames.XslFiles];
            DataRow drXslFiles = dtXslFiles.NewRow();
            drXslFiles[XslFilesColumns.Id] = dtXslFiles.Rows.Count;
            drXslFiles[XslFilesColumns.FkXmlFilesId] = dtXmlFiles.Rows.Count - 1;
            drXslFiles[XslFilesColumns.FileName] = xslFile;
            dtXslFiles.Rows.Add(drXslFiles);
        }

        private void AddToResultDataTable(DataSet dsResult, FailedAssert failedAssert)
        {
            DataTable dtXslFiles = dsResult.Tables[DataTableNames.XslFiles];
            DataTable dtResultValidator = dsResult.Tables[DataTableNames.ResultValidator];

            DataRow drResultValidator = dtResultValidator.NewRow();
            drResultValidator[ResultValidatorColumns.Id] = dtResultValidator.Rows.Count;
            drResultValidator[ResultValidatorColumns.FkXslFilesId] = dtXslFiles.Rows.Count - 1;

            drResultValidator[ResultValidatorColumns.Flag] = failedAssert.Flag;
            drResultValidator[ResultValidatorColumns.XRechnungId] = failedAssert.Id;
            drResultValidator[ResultValidatorColumns.Test] = failedAssert.Test;
            drResultValidator[ResultValidatorColumns.Location] = failedAssert.Location;
            drResultValidator[ResultValidatorColumns.ExceptionMessage] = failedAssert.ExceptionMessage;
            drResultValidator[ResultValidatorColumns.Text] = failedAssert.Text;
            dtResultValidator.Rows.Add(drResultValidator);

            string xslColumnName = null;
            string xmlColumnName = null;
            DataTable dtXmlFiles = dsResult.Tables[DataTableNames.XmlFiles];
            switch (failedAssert.Flag)
            {
                case Flags.Fatal:
                    xslColumnName = XslFilesColumns.ErrorCount;
                    xmlColumnName = XmlFilesColumns.ErrorCount;
                    break;
                case Flags.Warning:
                    xslColumnName = XslFilesColumns.WarningCount;
                    xmlColumnName = XmlFilesColumns.WarningCount;
                    break;
                case Flags.Exception:
                    xslColumnName = XslFilesColumns.ExceptionsCount;
                    xmlColumnName = XmlFilesColumns.ExceptionsCount;
                    break;
            }

            UpdateWarningAndErrorCount(dtXslFiles, XslFilesColumns.Id, xslColumnName);
            UpdateWarningAndErrorCount(dtXmlFiles, XmlFilesColumns.Id, xmlColumnName);

        }

        private void UpdateWarningAndErrorCount(DataTable dt, string idColumn, string columnName)
        {
            string searchExpression = $"{idColumn} = {dt.Rows.Count - 1}";
            DataRow[] foundRows = dt.Select(searchExpression);

            foreach (DataRow foundRow in foundRows)
            {
                if (foundRow.IsNull(columnName))
                {
                    foundRow[columnName] = 1;
                }
                else
                {
                    foundRow[columnName] = (int)foundRow[columnName] + 1;
                }
            }
        }
    }
}