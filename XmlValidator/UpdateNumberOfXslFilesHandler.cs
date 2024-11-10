using System.Windows.Forms;

namespace XmlValidator
{
    public class UpdateNumberOfXslFilesHandler: ICommandHandler<UpdateNumberOfXslFilesCommand>
    {
        public void Execute(UpdateNumberOfXslFilesCommand command)
        {
            command.NumOfXslCntObject.GetCurrentParent().Invoke((MethodInvoker)(() =>
            {
                command.NumOfXslCntObject.Text = command.NumOfXslCnt.ToString();
            }));
        }
    }
}