using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XmlValidator.XsdValidator
{
    public class XsdHandlerAsync: ICommandHandlerAsync<XsdCommand, XsdValidationResult>
    {
        protected int ExpectedNumberOfXsdFiles { get; set; }

        public virtual async Task<XsdValidationResult> Execute(XsdCommand command)
        {
            XmlSchemaSet xmlSchemaSet = command.XmlSchemaSet;
            XsdValidationResult xsdValidationResult = await Task.Run(() => ValidateAgainstSchemas(command.XmlFile, command.XmlSchemaSet));
            xsdValidationResult.NoXsd = xmlSchemaSet is null || xmlSchemaSet.Count < ExpectedNumberOfXsdFiles;
            return xsdValidationResult;
        }

        private XsdValidationResult ValidateAgainstSchemas(string xmlFile, XmlSchemaSet schemaSet)
        {
            if (schemaSet is null)
            {
                return new XsdValidationResult { Message = "XSD files not found", Valid = true };
            }
            XDocument xDocument = XDocument.Load(xmlFile);
            XsdValidationResult xsdValidationResult = new XsdValidationResult { Message = string.Empty, Valid = true };
            try
            {
                xDocument.Validate(schemaSet, (sender, args) =>
                {
                    if (args.Severity == XmlSeverityType.Error)
                    {
                        xsdValidationResult.Message = args.Message;
                        xsdValidationResult.Valid = false;
                    }
                }, true);
                return xsdValidationResult;
            }
            catch (Exception ex)
            {
                return new XsdValidationResult { Message = ex.Message, Valid = false };
            }
        }
    }
}