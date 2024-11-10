using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Schema;
using XmlValidator.SaveToDataTable;
using XmlValidator.XsdValidator;
using XmlValidator.XslValidator;

namespace XmlValidator
{
    public class ValidationHandlerAsync : ICommandHandlerAsync<ValidationCommand>
    {
        private readonly ICommandHandlerAsync<SaveToDataTablesCommand, DataSet> _saveToDataTable;
        private readonly ICommandHandlerAsync<CheckUsingXslCommand, string> _checkUsingXslHandlerAsync;
        private readonly ICommandHandler<UpdateNumberOfXmlFilesCommand> _updateNumberOfXmlFiles;
        private readonly ICommandHandler<UpdateNumberOfXslFilesCommand> _updateNumberOfXslFiles;
        private readonly ICommandHandlerAsync<UblCiiXsdValidatorCommand, UblCiiXsdValidatorResult>
            _ublCiiXsdValidatorHandler;

        public CancellationTokenSource CancellationTokenSource { get; set; }

        public ValidationHandlerAsync(ICommandHandlerAsync<CheckUsingXslCommand, string> checkUsingXslHandlerAsync
            , ICommandHandlerAsync<SaveToDataTablesCommand, DataSet> saveToDataTablesHandlerAsync
            , ICommandHandler<UpdateNumberOfXmlFilesCommand> updateNumberOfXmlFiles
            , ICommandHandler<UpdateNumberOfXslFilesCommand> updateNumberOfXslFiles
            , ICommandHandlerAsync<UblCiiXsdValidatorCommand, UblCiiXsdValidatorResult> ublCiiXsdValidatorHandler)
        {
            _saveToDataTable = saveToDataTablesHandlerAsync;
            _checkUsingXslHandlerAsync = checkUsingXslHandlerAsync;
            _updateNumberOfXmlFiles = updateNumberOfXmlFiles;
            _updateNumberOfXslFiles = updateNumberOfXslFiles;

            _ublCiiXsdValidatorHandler = ublCiiXsdValidatorHandler;
        }

        public async Task Execute(ValidationCommand command)
        {
            await DoValidation(command);
        }

        private async Task DoValidation(ValidationCommand command)
        {
            if (!(CancellationTokenSource is null))
            {
                CancellationToken cancellationToken = CancellationTokenSource.Token;
                string xmlFileExtension = Path.GetExtension(command.XmlFileOrFolderName);
                string xslFileExtension = Path.GetExtension(command.XslFileOrFolderName);

                bool isFileXmlSingleFileName =
                    !string.IsNullOrEmpty(xmlFileExtension) && xmlFileExtension == ".xml";
                bool isFileXslSingleFileName =
                    !string.IsNullOrEmpty(xslFileExtension) && xslFileExtension.Contains(".xsl");

                string[] xmlFiles = !isFileXmlSingleFileName
                    ? Directory.GetFiles(command.XmlFileOrFolderName, "*.xml", SearchOption.AllDirectories)
                    : new[] { command.XmlFileOrFolderName };
                string[] xslFiles = !isFileXslSingleFileName
                    ? Directory.GetFiles(command.XslFileOrFolderName, "*.xsl", SearchOption.AllDirectories)
                    : new[] { command.XslFileOrFolderName };

                CheckUsingXslCommand checkUsingXslCommand = new CheckUsingXslCommand();
                int intNumOfXmlCnt = xmlFiles.Length;
                UpdateNumberOfXmlFilesCommand updateNumberOfXmlFilesCommand = new UpdateNumberOfXmlFilesCommand();
                UpdateNumberOfXslFilesCommand updateNumberOfXslFilesCommand = new UpdateNumberOfXslFilesCommand();
                updateNumberOfXmlFilesCommand.NumOfXmlCnt = intNumOfXmlCnt;
                updateNumberOfXmlFilesCommand.NumOfXmlCntObject = command.NumOfXmlCntObject;
                _updateNumberOfXmlFiles.Execute(updateNumberOfXmlFilesCommand);

                bool doNotUpdateExistingAddNew = true;
                foreach (string xmlFile in xmlFiles)
                {
                    if (!command.TestOnlyCiiFiles || IsCiiInvoice(xmlFile))
                    {
                        int intNumOfXslCnt = xslFiles.Length;

                        updateNumberOfXslFilesCommand.NumOfXslCnt = intNumOfXslCnt;
                        updateNumberOfXslFilesCommand.NumOfXslCntObject = command.NumOfXslCntObject;
                        _updateNumberOfXslFiles.Execute(updateNumberOfXslFilesCommand);

                        await ValidateWithXsdAndSave(xmlFile
                            , command.ResultDataSet
                            , command.ToolStripCiiStatusLabel
                            , command.ToolStripUblStatusLabel
                            , command.CiiXsdFilesLocation
                            , command.UblXsdFilesLocation
                            , command.CiiXmlSchemaSet
                            , command.UblXmlSchemaSet);
                        command.IsXsdAlreadyValidatedForThisFile = false;
                        foreach (string xslFile in xslFiles)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            checkUsingXslCommand.InputFile = xmlFile;
                            checkUsingXslCommand.TransformFile = xslFile;
                            await ValidateWithXslAndSave(checkUsingXslCommand, command.ResultDataSet, doNotUpdateExistingAddNew);
                            command.IsXsdAlreadyValidatedForThisFile = true;
                            doNotUpdateExistingAddNew = false;

                            intNumOfXslCnt--;
                            updateNumberOfXslFilesCommand.NumOfXslCnt = intNumOfXslCnt;
                            _updateNumberOfXslFiles.Execute(updateNumberOfXslFilesCommand);
                        }
                    }
                    else
                    {
                        var saveToDataTablesCommand = new SaveToDataTablesCommand
                        {
                            XmlFile = xmlFile,
                            XslFile = string.Empty,
                            ValidationResult = "Not a CII Invoice",
                            ResultDataSet = command.ResultDataSet,
                            DoNotUpdateExistingAddNew = doNotUpdateExistingAddNew
                        };
                        await _saveToDataTable.Execute(saveToDataTablesCommand);
                    }

                    intNumOfXmlCnt--;
                    updateNumberOfXmlFilesCommand.NumOfXmlCnt = intNumOfXmlCnt;
                    _updateNumberOfXmlFiles.Execute(updateNumberOfXmlFilesCommand);
                }
            }
        }

        private async Task ValidateWithXslAndSave(CheckUsingXslCommand checkUsingXslCommand, DataSet dsResult, bool doNotUpdateExistingAddNew)
        {
            var validateAndSaveToDataTablesCommand = new SaveToDataTablesCommand
            {
                XmlFile = checkUsingXslCommand.InputFile,
                XslFile = checkUsingXslCommand.TransformFile,
                ResultDataSet = dsResult,
                DoNotUpdateExistingAddNew = doNotUpdateExistingAddNew,
                ValidationResult = await _checkUsingXslHandlerAsync.Execute(checkUsingXslCommand)
            };

            await _saveToDataTable.Execute(validateAndSaveToDataTablesCommand);
        }

        private async Task ValidateWithXsdAndSave(string inputFile
            , DataSet dsResult
            , ToolStripStatusLabel toolStripCiiStatusLabel
            , ToolStripStatusLabel toolStripUblStatusLabel
            , string ciiXsdFilesLocation
            , string ublXsdFilesLocation
            , XmlSchemaSet ciiXmlSchemaSet
            , XmlSchemaSet ublXmlSchemaSet)
        {
            UblCiiXsdValidatorCommand ublCiiXsdValidatorCommand = new UblCiiXsdValidatorCommand
            {
                InputFile = inputFile
                , CiiXsdFilesLocation = ciiXsdFilesLocation
                , UblXsdFilesLocation = ublXsdFilesLocation
                , CiiXmlSchemaSet = ciiXmlSchemaSet
                , UblXmlSchemaSet = ublXmlSchemaSet
            };

            UblCiiXsdValidatorResult ublCiiXsdValidationResult = await _ublCiiXsdValidatorHandler.Execute(ublCiiXsdValidatorCommand);

            if (!ublCiiXsdValidationResult.CiiValid)
            {
                await SaveXsdValidationToDataTable(inputFile, dsResult,ublCiiXsdValidationResult.CiiResultMessage);
            }

            if (!ublCiiXsdValidationResult.UblValid)
            {
                await SaveXsdValidationToDataTable(inputFile, dsResult, ublCiiXsdValidationResult.UblResultMessage);
            }

            if (ublCiiXsdValidationResult.NoUblXsd)
            {
                toolStripUblStatusLabel?.GetCurrentParent()
                    .Invoke((MethodInvoker)(() => toolStripUblStatusLabel.Text = "No UBL XSD"));
            }
            else
            {
                toolStripUblStatusLabel?.GetCurrentParent()
                    .Invoke((MethodInvoker)(() => toolStripUblStatusLabel.Text = "UBL XSD is OK"));
            }

            if (ublCiiXsdValidationResult.NoCiiXsd)
            {
                toolStripCiiStatusLabel?.GetCurrentParent()
                    .Invoke((MethodInvoker)(() => toolStripCiiStatusLabel.Text = "No CII XSD"));
            }
            else
            {
                toolStripCiiStatusLabel?.GetCurrentParent()
                    .Invoke((MethodInvoker)(() => toolStripCiiStatusLabel.Text = "CII XSD is OK"));
            }
        }

        private async Task SaveXsdValidationToDataTable(string xmlFile, DataSet resultDataSet, string message)
        {
            var validateAndSaveToDataTablesCommand = new SaveToDataTablesCommand
            {
                XmlFile = xmlFile,
                XslFile = "XSD validation",
                ResultDataSet = resultDataSet,
                ValidationResult = message,
                Flag = Flags.Fatal
            };

            await _saveToDataTable.Execute(validateAndSaveToDataTablesCommand);
        }

        private bool IsCiiInvoice(string xmlFile)
        {
            XDocument document = XDocument.Load(xmlFile);
            XNamespace ciiNamespace = "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100";
            var invoices = document.Descendants(ciiNamespace + "CrossIndustryInvoice");

            IEnumerable<XElement> xElements = invoices as XElement[] ?? invoices.ToArray();
            return xElements.Any();
        }
    }
}