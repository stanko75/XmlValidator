using System.Threading.Tasks;
using System.Xml.Schema;
using XmlValidator.XsdValidator.CII;
using XmlValidator.XsdValidator.UBL;

namespace XmlValidator.XsdValidator
{
    public class UblCiiXsdValidatorHandler: ICommandHandlerAsync<UblCiiXsdValidatorCommand, UblCiiXsdValidatorResult>
    {
        private readonly ICommandHandlerAsync<CheckUsingCiiXsdCommand, XsdValidationResult> _checkUsingCiiXsdHandlerAsync;
        private readonly ICommandHandlerAsync<CheckUsingUblXsdCommand, XsdValidationResult> _checkUsingUblXsdHandlerAsync;

        public UblCiiXsdValidatorHandler(ICommandHandlerAsync<CheckUsingCiiXsdCommand, XsdValidationResult> checkUsingCiiXsdHandlerAsync
            , ICommandHandlerAsync<CheckUsingUblXsdCommand, XsdValidationResult> checkUsingUblXsdHandlerAsync)
        {

            _checkUsingCiiXsdHandlerAsync = checkUsingCiiXsdHandlerAsync;
            _checkUsingUblXsdHandlerAsync = checkUsingUblXsdHandlerAsync;
        }

        public async Task<UblCiiXsdValidatorResult> Execute(UblCiiXsdValidatorCommand command)
        {
            CheckUsingCiiXsdCommand checkUsingCiiXsdCommand = new CheckUsingCiiXsdCommand
            {
                XmlFile = command.InputFile
                , XmlSchemaSet = command.CiiXmlSchemaSet
            };
            XsdValidationResult ciiXsdValidationResult = await _checkUsingCiiXsdHandlerAsync.Execute(checkUsingCiiXsdCommand);

            UblCiiXsdValidatorResult ublCiiXsdValidatorResult = new UblCiiXsdValidatorResult
            {
                CiiResultMessage = ciiXsdValidationResult.Message,
                NoCiiXsd = ciiXsdValidationResult.NoXsd,
                CiiValid = ciiXsdValidationResult.Valid
            };

            /*
            if (ciiXsdValidationResult.NoXsd)
            {
                toolStripCiiStatusLabel?.GetCurrentParent()
                    .Invoke((MethodInvoker)(() => toolStripCiiStatusLabel.Text = "No CII XSD"));
            }
            else
            {
                toolStripCiiStatusLabel?.GetCurrentParent()
                    .Invoke((MethodInvoker)(() => toolStripCiiStatusLabel.Text = "CII XSD is OK"));
            }
            */


            CheckUsingUblXsdCommand checkUsingUblXsdCommand = new CheckUsingUblXsdCommand
            {
                XmlFile = command.InputFile
                , XmlSchemaSet = command.UblXmlSchemaSet
            };
            XsdValidationResult ublXsdValidationResult = await _checkUsingUblXsdHandlerAsync.Execute(checkUsingUblXsdCommand);

            ublCiiXsdValidatorResult.UblResultMessage = ublXsdValidationResult.Message;
            ublCiiXsdValidatorResult.NoUblXsd = ublXsdValidationResult.NoXsd;
            ublCiiXsdValidatorResult.UblValid = ublXsdValidationResult.Valid;

            /*
            if (ublXsdValidationResult.NoXsd)
            {
                toolStripUblStatusLabel?.GetCurrentParent()
                    .Invoke((MethodInvoker)(() => toolStripUblStatusLabel.Text = "No UBL XSD"));
            }
            else
            {
                toolStripUblStatusLabel?.GetCurrentParent()
                    .Invoke((MethodInvoker)(() => toolStripUblStatusLabel.Text = "UBL XSD is OK"));
            }
            */

            return ublCiiXsdValidatorResult;
        }
    }

    public class UblCiiXsdValidatorResult
    {
        public string UblResultMessage { get; set; }
        public bool UblValid { get; set; }
        public bool NoUblXsd { get; set; }


        public string CiiResultMessage { get; set; }
        public bool CiiValid { get; set; }
        public bool NoCiiXsd { get; set; }
    }

    public class UblCiiXsdValidatorCommand
    {
        public string InputFile { get; set; }
        public string CiiXsdFilesLocation { get; set; }
        public string UblXsdFilesLocation { get; set; }
        public XmlSchemaSet CiiXmlSchemaSet { get; set; }
        public XmlSchemaSet UblXmlSchemaSet { get; set; }
    }
}