using System.Xml.Schema;

namespace XmlValidator.XsdValidator
{
    public class XsdCommand
    {
        public string XmlFile { get; set; }
        public XmlSchemaSet XmlSchemaSet { get; set; }
    }
}