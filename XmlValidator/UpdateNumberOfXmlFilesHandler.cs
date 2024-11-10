using System.Windows.Forms;

namespace XmlValidator
{
    public class UpdateNumberOfXmlFilesHandlerAsync: ICommandHandler<UpdateNumberOfXmlFilesCommand>
    {
        public void Execute(UpdateNumberOfXmlFilesCommand command)
        {
            command.NumOfXmlCntObject.GetCurrentParent().Invoke((MethodInvoker)(() =>
            {
                command.NumOfXmlCntObject.Text = command.NumOfXmlCnt.ToString();
            }));
        }
    }
}