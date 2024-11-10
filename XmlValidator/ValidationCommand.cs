using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Schema;

namespace XmlValidator
{
    public class ValidationCommand
    {
        public string XmlFileOrFolderName { get; set; }
        public string XslFileOrFolderName { get; set; }
        public DataSet ResultDataSet { get; set; }
        public ToolStripStatusLabel NumOfXmlCntObject { get; set; }
        public bool TestOnlyCiiFiles { get; set; }
        public ToolStripStatusLabel ToolStripCiiStatusLabel { get; set; }
        public ToolStripStatusLabel ToolStripUblStatusLabel { get; set; }
        public bool IsXsdAlreadyValidatedForThisFile { get; set; }
        public string UblXsdFilesLocation { get; set; }
        public string CiiXsdFilesLocation { get; set; }
        public XmlSchemaSet CiiXmlSchemaSet { get; set; }
        public XmlSchemaSet UblXmlSchemaSet { get; set; }
        public ToolStripStatusLabel ToolStripValidationStatusLabel { get; set; }
        public Action ExecuteDisplayInGrid { get; set; }
    }
}