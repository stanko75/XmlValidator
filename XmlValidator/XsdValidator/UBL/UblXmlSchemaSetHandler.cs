using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace XmlValidator.XsdValidator.UBL
{
    public class UblXmlSchemaSetHandler: ICommandHandler<UblXmlSchemaSetCommand, XmlSchemaSet>
    {
        public XmlSchemaSet Execute(UblXmlSchemaSetCommand command)
        {
            XmlSchemaSet schemaSet = new XmlSchemaSet();

            string relativeXsdFolder = command.RootFolderWhereAreXsds;
            if (Directory.Exists(relativeXsdFolder))
            {
                string[] xsdFiles = Directory.GetFiles(relativeXsdFolder, "*.xsd",
                    SearchOption.AllDirectories);
                foreach (string xsdFile in xsdFiles)
                {
                    XmlReaderSettings settings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse
                    };

                    using (XmlReader reader = XmlReader.Create(xsdFile, settings))
                    {
                        schemaSet.Add(null, reader);
                    }
                }

                schemaSet.Compile();
                return schemaSet;
            }

            return null;
        }
    }
}