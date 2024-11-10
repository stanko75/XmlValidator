using System;
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
        private readonly object _dataTableLock = new object();
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
            lock (_dataTableLock)
            {
                AddXmlFilesDataTableIfNotExist(dsResult, xmlFile, addNew);
                AddXslFilesDataTable(dsResult, xmlFile, xslFile);

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
                    AddToResultDataTable(dsResult, failedAssert, xmlFile, xslFile);
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
                    AddToResultDataTable(dsResult, failedAssert, xmlFile, xslFile);
                }
            }
        }

        private void AddXmlFilesDataTableIfNotExist(DataSet dsResult, string xmlFile, bool addNew)
        {
            lock (_dataTableLock)
            {
                DataTable dtXmlFiles = dsResult.Tables[DataTableNames.XmlFiles];

                string searchExpression = $"{XmlFilesColumns.FileName} = '{xmlFile}'";
                DataRow[] drXmlFilesFiltered = dtXmlFiles.Select(searchExpression);
                if (drXmlFilesFiltered.Length == 0 || addNew)
                {
                    DataRow drXmlFiles = dtXmlFiles.NewRow();
                    drXmlFiles[XmlFilesColumns.Id] = dtXmlFiles.Rows.Count;
                    drXmlFiles[XmlFilesColumns.FileName] = xmlFile;
                    dtXmlFiles.Rows.Add(drXmlFiles);
                }
            }
        }

        private void AddXslFilesDataTable(DataSet dsResult, string xmlFile, string xslFile)
        {
            lock (_dataTableLock)
            {
                DataTable dtXmlFiles = dsResult.Tables[DataTableNames.XmlFiles];
                string searchXmlFileInXmlFiles = $"{XmlFilesColumns.FileName} = '{xmlFile}'";
                DataRow[] drXmlFilesFiltered = dtXmlFiles.Select(searchXmlFileInXmlFiles);
                if (drXmlFilesFiltered.Length == 1)
                {
                    int xmlFilesId = (int)drXmlFilesFiltered[0][XmlFilesColumns.Id];
                    DataTable dtXslFiles = dsResult.Tables[DataTableNames.XslFiles];
                    string searchXslFileInXslFiles = $"{XslFilesColumns.FileName} = '{xslFile}' AND {XslFilesColumns.FkXmlFilesId} = {xmlFilesId}";
                    DataRow[] drXslFilesFiltered = dtXslFiles.Select(searchXslFileInXslFiles);
                    if (drXslFilesFiltered.Length == 0)
                    {

                        DataRow drXslFiles = dtXslFiles.NewRow();
                        drXslFiles[XslFilesColumns.Id] = dtXslFiles.Rows.Count;
                        drXslFiles[XslFilesColumns.FkXmlFilesId] = xmlFilesId;
                        drXslFiles[XslFilesColumns.FileName] = xslFile;
                        dtXslFiles.Rows.Add(drXslFiles);
                    }
                }
                else
                {
                    throw new Exception($"There are {drXmlFilesFiltered.Length} rows in XML files datatable!");
                }
            }
        }

        private void AddToResultDataTable(DataSet dsResult, FailedAssert failedAssert, string xmlFile, string xslFile)
        {
            lock (_dataTableLock)
            {
                DataTable dtXmlFiles = dsResult.Tables[DataTableNames.XmlFiles];
                string searchXmlFileInXmlFiles = $"{XmlFilesColumns.FileName} = '{xmlFile}'";
                DataRow[] drXmlFilesFiltered = dtXmlFiles.Select(searchXmlFileInXmlFiles);

                if (drXmlFilesFiltered.Length == 1)
                {
                    int xmlFilesId = (int)drXmlFilesFiltered[0][XmlFilesColumns.Id];
                    string searchXslFileInXslFiles = $"{XslFilesColumns.FileName} = '{xslFile}' AND {XslFilesColumns.FkXmlFilesId} = {xmlFilesId}";
                    DataTable dtXslFiles = dsResult.Tables[DataTableNames.XslFiles];
                    DataRow[] drXslFilesFiltered = dtXslFiles.Select(searchXslFileInXslFiles);
                    if (drXslFilesFiltered.Length == 1)
                    {
                        int xslFilesId = (int)drXslFilesFiltered[0][XslFilesColumns.Id];
                        DataTable dtResultValidator = dsResult.Tables[DataTableNames.ResultValidator];

                        DataRow drResultValidator = dtResultValidator.NewRow();
                        drResultValidator[ResultValidatorColumns.Id] = dtResultValidator.Rows.Count;
                        drResultValidator[ResultValidatorColumns.FkXslFilesId] = xslFilesId;

                        drResultValidator[ResultValidatorColumns.Flag] = failedAssert.Flag;
                        drResultValidator[ResultValidatorColumns.XRechnungId] = failedAssert.Id;
                        drResultValidator[ResultValidatorColumns.Test] = failedAssert.Test;
                        drResultValidator[ResultValidatorColumns.Location] = failedAssert.Location;
                        drResultValidator[ResultValidatorColumns.ExceptionMessage] = failedAssert.ExceptionMessage;
                        drResultValidator[ResultValidatorColumns.Text] = failedAssert.Text;
                        dtResultValidator.Rows.Add(drResultValidator);

                        string xslColumnName = null;
                        string xmlColumnName = null;
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

                        //UpdateWarningAndErrorCount(dtXslFiles, XslFilesColumns.Id, xslColumnName);
                        //UpdateWarningAndErrorCount(dtXmlFiles, XmlFilesColumns.Id, xmlColumnName);
                        UpdateWarningAndErrorCountInXml(drXmlFilesFiltered, xmlColumnName);
                        UpdateWarningAndErrorCountinXsl(drXslFilesFiltered, xslColumnName);
                    }
                    else
                    {
                        throw new Exception($"There are {drXslFilesFiltered.Length} rows in XSL files datatable!");
                    }
                }
                else
                {
                    throw new Exception($"There are {drXmlFilesFiltered.Length} rows in XML files datatable!");
                }
            }
        }

        private void UpdateWarningAndErrorCountinXsl(DataRow[] dt, string columnName)
        {
            if (dt[0].IsNull(columnName))
            {
                dt[0][columnName] = 1;
            }
            else
            {
                dt[0][columnName] = (int)dt[0][columnName] + 1;
            }
        }

        private void UpdateWarningAndErrorCountInXml(DataRow[] dt, string columnName)
        {
            if (dt[0].IsNull(columnName))
            {
                dt[0][columnName] = 1;
            }
            else
            {
                dt[0][columnName] = (int)dt[0][columnName] + 1;
            }
        }
    }
}