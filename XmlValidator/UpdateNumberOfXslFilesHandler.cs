using System;

namespace XmlValidator
{
    public class UpdateNumberOfXslFilesHandler: ICommandHandler<UpdateNumberOfXslFilesCommand>
    {
        public void Execute(UpdateNumberOfXslFilesCommand command)
        {
            if (command.NumOfXslCntObject.GetCurrentParent().InvokeRequired)
            {
                command.NumOfXslCntObject.GetCurrentParent().Invoke(new Action(() => { command.NumOfXslCntObject.Text = command.NumOfXslCnt.ToString(); }));
            }
            else
            {
                command.NumOfXslCntObject.Text = command.NumOfXslCnt.ToString();
            }
        }
    }
}