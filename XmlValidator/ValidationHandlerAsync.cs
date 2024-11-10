using System.Collections.Concurrent;
using System;
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

                UpdateStatus("Getting XML and XSL files, please wait...", command.ToolStripValidationStatusLabel);
                try
                {
                    await Task.Run(() =>
                    {
                        string[] xmlFiles = !isFileXmlSingleFileName
                            ? Directory.GetFiles(command.XmlFileOrFolderName, "*.xml", SearchOption.AllDirectories)
                            : new[] { command.XmlFileOrFolderName };
                        string[] xslFiles = !isFileXslSingleFileName
                            ? Directory.GetFiles(command.XslFileOrFolderName, "*.xsl", SearchOption.AllDirectories)
                            : new[] { command.XslFileOrFolderName };

                        CheckUsingXslCommand checkUsingXslCommand = new CheckUsingXslCommand();
                        UpdateNumberOfXmlFilesCommand updateNumberOfXmlFilesCommand = new UpdateNumberOfXmlFilesCommand();

                        bool doNotUpdateExistingAddNew = true;

                        int totalXmlFiles = xmlFiles.Length;
                        int totalXslFiles = xslFiles.Length;
                        int totalNumberOfProcessedFiles = totalXmlFiles * totalXslFiles;

                        updateNumberOfXmlFilesCommand.NumOfXmlCnt = totalNumberOfProcessedFiles;
                        updateNumberOfXmlFilesCommand.NumOfXmlCntObject = command.NumOfXmlCntObject;
                        _updateNumberOfXmlFiles.Execute(updateNumberOfXmlFilesCommand);

                        ThreadPool.SetMinThreads(Environment.ProcessorCount * 2, Environment.ProcessorCount * 2);
                        var parallelOptions = new ParallelOptions
                        {
                            MaxDegreeOfParallelism = Environment.ProcessorCount * 2, CancellationToken = cancellationToken
                        };

                        var xslPartitioner = Partitioner.Create(0, xslFiles.Length, 1);
                        var xmlPartitioner = Partitioner.Create(0, xmlFiles.Length, 1);
                        try
                        {
                            Parallel.ForEach(xslPartitioner, parallelOptions, (xslRange, loopMain) =>
                            {
                                if (parallelOptions.CancellationToken.IsCancellationRequested)
                                {
                                    loopMain.Stop();
                                }

                                for (int xslIndex = xslRange.Item1; xslIndex < xslRange.Item2; xslIndex++)
                                {
                                    try
                                    {
                                        string xslFile = xslFiles[xslIndex];

                                        async void XmlBody(Tuple<int, int> xmlRange, ParallelLoopState loopChild)
                                        {
                                            if (parallelOptions.CancellationToken.IsCancellationRequested)
                                            {
                                                loopChild.Stop();
                                            }

                                            for (int xmlIndex = xmlRange.Item1; xmlIndex < xmlRange.Item2; xmlIndex++)
                                            {
                                                string xmlFile = xmlFiles[xmlIndex];
                                                if (!command.TestOnlyCiiFiles || IsCiiInvoice(xmlFile))
                                                {
                                                    if (parallelOptions.CancellationToken.IsCancellationRequested)
                                                    {
                                                        return;
                                                    }

                                                    await ValidateWithXsdAndSave(xmlFile, command.ResultDataSet, command.ToolStripCiiStatusLabel, command.ToolStripUblStatusLabel,
                                                        command.CiiXsdFilesLocation,
                                                        command.UblXsdFilesLocation, command.CiiXmlSchemaSet, command.UblXmlSchemaSet, cancellationToken);
                                                    command.IsXsdAlreadyValidatedForThisFile = false;
                                                    {
                                                        checkUsingXslCommand.InputFile = xmlFile;
                                                        checkUsingXslCommand.TransformFile = xslFile;
                                                        await ValidateWithXslAndSave(checkUsingXslCommand, command.ResultDataSet, cancellationToken);
                                                        command.IsXsdAlreadyValidatedForThisFile = true;
                                                        doNotUpdateExistingAddNew = false;
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

                                                if (parallelOptions.CancellationToken.IsCancellationRequested)
                                                {
                                                    UpdateStatus("Cancel requested, please wait", command.ToolStripValidationStatusLabel);
                                                }
                                                else if (command.ToolStripValidationStatusLabel.Text != "Validation started")
                                                {
                                                    UpdateStatus("Validation started", command.ToolStripValidationStatusLabel);
                                                }

                                                Interlocked.Decrement(ref totalNumberOfProcessedFiles);

                                                if (totalNumberOfProcessedFiles == 0)
                                                {
                                                    UpdateStatus("Validation ended", command.ToolStripValidationStatusLabel);
                                                    command.ExecuteDisplayInGrid();
                                                }

                                                updateNumberOfXmlFilesCommand.NumOfXmlCnt = totalNumberOfProcessedFiles;
                                                _updateNumberOfXmlFiles.Execute(updateNumberOfXmlFilesCommand);
                                                if (parallelOptions.CancellationToken.IsCancellationRequested)
                                                {
                                                    loopChild.Stop();
                                                    break;
                                                }
                                            }
                                        }

                                        Parallel.ForEach(xmlPartitioner, parallelOptions, XmlBody);
                                        if (parallelOptions.CancellationToken.IsCancellationRequested)
                                        {
                                            loopMain.Stop();
                                            break;
                                        }
                                    }
                                    catch (OperationCanceledException)
                                    {
                                        UpdateStatus("Validation canceled", command.ToolStripValidationStatusLabel);
                                        command.ExecuteDisplayInGrid();
                                    }
                                }
                            });
                        }
                        catch (OperationCanceledException)
                        {
                            UpdateStatus("Validation canceled", command.ToolStripValidationStatusLabel);
                            command.ExecuteDisplayInGrid();
                        }
                    }, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    UpdateStatus("Validation canceled", command.ToolStripValidationStatusLabel);
                    command.ExecuteDisplayInGrid();
                }
            }
        }

        private void UpdateStatus(string text, ToolStripStatusLabel toolStripValidationStatusLabel)
        {
            if (toolStripValidationStatusLabel.GetCurrentParent().InvokeRequired)
            {
                toolStripValidationStatusLabel.GetCurrentParent().BeginInvoke(new Action(() => { toolStripValidationStatusLabel.Text = text; }));
            }
            else
            {
                toolStripValidationStatusLabel.Text = text;
            }
        }

        private async Task ValidateWithXslAndSave(CheckUsingXslCommand checkUsingXslCommand, DataSet dsResult, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var validateAndSaveToDataTablesCommand = new SaveToDataTablesCommand
            {
                XmlFile = checkUsingXslCommand.InputFile,
                XslFile = checkUsingXslCommand.TransformFile,
                ResultDataSet = dsResult,
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
            , XmlSchemaSet ublXmlSchemaSet, CancellationToken cancellationToken)
        {
            UblCiiXsdValidatorCommand ublCiiXsdValidatorCommand = new UblCiiXsdValidatorCommand
            {
                InputFile = inputFile
                , CiiXsdFilesLocation = ciiXsdFilesLocation
                , UblXsdFilesLocation = ublXsdFilesLocation
                , CiiXmlSchemaSet = ciiXmlSchemaSet
                , UblXmlSchemaSet = ublXmlSchemaSet
            };

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            UblCiiXsdValidatorResult ublCiiXsdValidationResult = await _ublCiiXsdValidatorHandler.Execute(ublCiiXsdValidatorCommand);

            if (!ublCiiXsdValidationResult.CiiValid)
            {
                await SaveXsdValidationToDataTable(inputFile, dsResult,ublCiiXsdValidationResult.CiiResultMessage, cancellationToken);
            }

            if (!ublCiiXsdValidationResult.UblValid)
            {
                await SaveXsdValidationToDataTable(inputFile, dsResult, ublCiiXsdValidationResult.UblResultMessage, cancellationToken);
            }

            if (ublCiiXsdValidationResult.NoUblXsd)
            {
                if (toolStripUblStatusLabel.GetCurrentParent().IsHandleCreated)
                {
                    toolStripUblStatusLabel.GetCurrentParent()
                        .Invoke((MethodInvoker)(() => toolStripUblStatusLabel.Text = "No UBL XSD"));
                }
            }
            else
            {
                if (toolStripUblStatusLabel.GetCurrentParent().IsHandleCreated)
                {
                    toolStripUblStatusLabel.GetCurrentParent()
                        .Invoke((MethodInvoker)(() => toolStripUblStatusLabel.Text = "UBL XSD is OK"));
                }
            }

            if (ublCiiXsdValidationResult.NoCiiXsd)
            {
                if (toolStripUblStatusLabel.GetCurrentParent().IsHandleCreated)
                {
                    toolStripCiiStatusLabel?.GetCurrentParent()
                        .Invoke((MethodInvoker)(() => toolStripCiiStatusLabel.Text = "No CII XSD"));
                }
            }
            else
            {
                if (toolStripUblStatusLabel.GetCurrentParent().IsHandleCreated)
                {
                    toolStripCiiStatusLabel?.GetCurrentParent()
                        .Invoke((MethodInvoker)(() => toolStripCiiStatusLabel.Text = "CII XSD is OK"));
                }
            }
        }

        private async Task SaveXsdValidationToDataTable(string xmlFile, DataSet resultDataSet, string message, CancellationToken cancellationToken)
        {
            var validateAndSaveToDataTablesCommand = new SaveToDataTablesCommand
            {
                XmlFile = xmlFile,
                XslFile = "XSD validation",
                ResultDataSet = resultDataSet,
                ValidationResult = message,
                Flag = Flags.Fatal
            };

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

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