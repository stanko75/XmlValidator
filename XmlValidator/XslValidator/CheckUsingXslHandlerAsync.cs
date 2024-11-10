using System;
using System.IO;
using System.Threading.Tasks;
using Saxon.Api;

namespace XmlValidator.XslValidator
{
    public class CheckUsingXslHandlerAsync : ICommandHandlerAsync<CheckUsingXslCommand, string>
    {
        public async Task<string> Execute(CheckUsingXslCommand command)
        {
            return await ValidateXmlFileAsync(command.InputFile, command.TransformFile);
        }

        private Task<string> ValidateXmlFileAsync(string inputFile, string transformFile)
        {
            return Task.Run(() => ValidateXmlFile(inputFile, transformFile));
        }

        private string ValidateXmlFile(string inputFile, string transformFile)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputFile))
                {
                    throw new FileNotFoundException("Filename is empty");
                }

                Processor processor = new Processor();

                DocumentBuilder builder = processor.NewDocumentBuilder();
                builder.BaseUri = new Uri(inputFile);

                XdmNode inputNode;
                using (var inputStream = File.OpenRead(inputFile))
                {
                    inputNode = builder.Build(inputStream);
                }

                XsltCompiler compiler = processor.NewXsltCompiler();
                XsltExecutable executable;
                using (var xsltStream = File.OpenRead(transformFile))
                {
                    executable = compiler.Compile(xsltStream);
                    if (compiler.GetErrorList().Count != 0)
                        throw new Exception("Exception loading xsl!");
                }

                XsltTransformer transformer = executable.Load();

                transformer.InitialContextNode = inputNode;

                Serializer serializer = processor.NewSerializer();
                using (var stringWriter = new StringWriter())
                {
                    serializer.SetOutputWriter(stringWriter);
                    transformer.Run(serializer);
                    string result = stringWriter.ToString();
                    return result;
                }
            }
            catch (Exception e)
            {
               return e.Message;
            }
        }
    }
}