using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;

namespace XmlValidator.XsdValidator.CII
{
    public class CiiXmlSchemaSetHandler : ICommandHandler<CiiXmlSchemaSetCommand, XmlSchemaSet>
    {
        public XmlSchemaSet Execute(CiiXmlSchemaSetCommand command)
        {
            string rootFolder = command.RootFolderWhereAreXsds;
            Dictionary<string, string> xsdFiles = new Dictionary<string, string>
            {
                {
                    "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100",
                    Path.Combine(rootFolder, "CrossIndustryInvoice_100pD16B.xsd")
                },
                {
                    "urn:un:unece:uncefact:data:standard:QualifiedDataType:100",
                    Path.Combine(rootFolder, "CrossIndustryInvoice_QualifiedDataType_100pD16B.xsd")
                },
                {
                    "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100",
                    Path.Combine(rootFolder,
                        "CrossIndustryInvoice_ReusableAggregateBusinessInformationEntity_100pD16B.xsd")
                },
                {
                    "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100",
                    Path.Combine(rootFolder, "CrossIndustryInvoice_UnqualifiedDataType_100pD16B.xsd")
                }
            };

            XmlSchemaSet schemaSet = new XmlSchemaSet();

            foreach (KeyValuePair<string, string> xsdFile in xsdFiles)
            {
                if (File.Exists(xsdFile.Value))
                {
                    schemaSet.Add(xsdFile.Key, xsdFile.Value);
                }
            }

            schemaSet.Compile();

            return schemaSet;

        }
    }
}