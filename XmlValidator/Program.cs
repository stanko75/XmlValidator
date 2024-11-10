using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Schema;
using XmlValidator.DownloadFiles;
using XmlValidator.SaveToDataTable;
using XmlValidator.XsdValidator;
using XmlValidator.XsdValidator.CII;
using XmlValidator.XsdValidator.UBL;
using XmlValidator.XslDownload;
using XmlValidator.XslValidator;

namespace XmlValidator
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ICommandHandlerAsync<CalculateDownloadSizeForAllFilesCommand> calculateDownloadSizeForAllFiles =
                new CalculateDownloadSizeForAllFilesHandler();
            CancellationDecorator<CalculateDownloadSizeForAllFilesCommand> calculateDownloadSizeForAllFilesCancel =
                new CancellationDecorator<CalculateDownloadSizeForAllFilesCommand>(calculateDownloadSizeForAllFiles);
            ICommandHandler<DisplayDownloadProgressCommand> displayDownloadProgress =
                new DisplayDownloadProgressHandler();
            ICommandHandlerAsync<DownloadFileCommand> downloadFile =
                new DownloadFileHandlerAsync(displayDownloadProgress);
            CancellationDecorator<DownloadFileCommand> downloadFileCancel =
                new CancellationDecorator<DownloadFileCommand>(downloadFile);
            ICommandHandlerAsync<DownloadListOfFilesCommand> downloadListOfFiles = new DownloadListOfFilesHandlerAsync(
                calculateDownloadSizeForAllFilesCancel
                , downloadFileCancel);

            CancellationDecorator<DownloadListOfFilesCommand> downloadListOfFilesCancellationDecorator = new CancellationDecorator<DownloadListOfFilesCommand>(downloadListOfFiles);

            ICommandHandlerAsync<CheckUsingXslCommand, string> checkUsingXslHandlerAsync =
                new CheckUsingXslHandlerAsync();
            ICommandHandlerAsync<SaveToDataTablesCommand, DataSet> validateAndSaveToDataTablesHandlerAsync =
                new SaveToDataTablesHandlerAsync();
            ICommandHandler<UpdateNumberOfXmlFilesCommand> updateNumberOfXmlFiles =
                new UpdateNumberOfXmlFilesHandlerAsync();
            ICommandHandler<UpdateNumberOfXslFilesCommand> updateNumberOfXslFiles = new UpdateNumberOfXslFilesHandler();
            ICommandHandlerAsync<CheckUsingCiiXsdCommand, XsdValidationResult> checkUsingCiiHandlerAsync =
                new CheckUsingCiiXsdHandlerAsync();
            ICommandHandlerAsync<CheckUsingUblXsdCommand, XsdValidationResult> checkUsingUblHandlerAsync =
                new CheckUsingUblXsdHandlerAsync();
            ICommandHandler<CiiXmlSchemaSetCommand, XmlSchemaSet> ciiXmlSchemaSet = new CiiXmlSchemaSetHandler();
            ICommandHandler<UblXmlSchemaSetCommand, XmlSchemaSet> ublXmlSchemaSet = new UblXmlSchemaSetHandler();

            ICommandHandlerAsync<UblCiiXsdValidatorCommand, UblCiiXsdValidatorResult> ublCiiXsdValidatorHandler =
                new UblCiiXsdValidatorHandler(checkUsingCiiHandlerAsync, checkUsingUblHandlerAsync);

            ICommandHandlerAsync<DownloadUnzipAndCopyXsdFilesCommand> downloadUnzipAndCopyXsdFilesHandlerAsync =
                new DownloadUnzipAndCopyXsdFilesHandlerAsync(downloadListOfFilesCancellationDecorator);
            CancellationDecorator<DownloadUnzipAndCopyXsdFilesCommand> downloadUnzipAndCopyXsdFilesHandlerAsyncCancellationDecorator =
                new CancellationDecorator<DownloadUnzipAndCopyXsdFilesCommand>(
                    downloadUnzipAndCopyXsdFilesHandlerAsync);

            ICommandHandlerAsync<DownloadXslFromGitHubCommand> downloadXslFromGitHubHandlerAsync =
                new DownloadXslFromGitHubHandlerAsync(downloadListOfFilesCancellationDecorator);
            CancellationDecorator<DownloadXslFromGitHubCommand>  downloadXslFromGitHubHandlerAsyncCancellationDecorator =
                new CancellationDecorator<DownloadXslFromGitHubCommand>(downloadXslFromGitHubHandlerAsync);

            CancellationDecorator<ValidationCommand> validationHandlerAsync = new CancellationDecorator<ValidationCommand>(new ValidationHandlerAsync(
                checkUsingXslHandlerAsync
                , validateAndSaveToDataTablesHandlerAsync
                , updateNumberOfXmlFiles
                , updateNumberOfXslFiles
                , ublCiiXsdValidatorHandler));


            Application.Run(new Form1(validationHandlerAsync,
                downloadUnzipAndCopyXsdFilesHandlerAsyncCancellationDecorator,
                downloadXslFromGitHubHandlerAsyncCancellationDecorator,
                ciiXmlSchemaSet, ublXmlSchemaSet));
        }
    }
}
