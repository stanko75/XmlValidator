using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using XmlValidator.DataTables;
using XmlValidator.DataTables.Models;
using XmlValidator.SaveToDataTable;
using XmlValidator.XsdValidator;
using XmlValidator.XsdValidator.CII;
using XmlValidator.XsdValidator.UBL;
using XmlValidator.XslDownload;
using Exception = System.Exception;
using Path = System.IO.Path;

namespace XmlValidator
{
    public partial class Form1 : Form
    {
        private readonly DataSet _dsValidator = new ValidatorDataTables().CreateValidatorDataSet();
        private readonly CancellationDecorator<ValidationCommand> _validationHandlerAsync;

        private readonly CancellationDecorator<DownloadUnzipAndCopyXsdFilesCommand>
            _downloadUnzipAndCopyXsdFilesHandlerAsync;

        private readonly CancellationDecorator<DownloadXslFromGitHubCommand> _downloadXslFromGitHubHandlerAsync;

        private readonly ICommandHandler<UblXmlSchemaSetCommand, XmlSchemaSet> _ublXmlSchemaSet;
        private readonly ICommandHandler<CiiXmlSchemaSetCommand, XmlSchemaSet> _ciiXmlSchemaSet;
        //private readonly List<CancellationDecorator<DownloadXslFromGitHubCommand>> _listOfCancellationDecorators;

        public Form1(CancellationDecorator<ValidationCommand> validationHandlerAsync
            , CancellationDecorator<DownloadUnzipAndCopyXsdFilesCommand> downloadUnzipAndCopyXsdFilesHandlerAsyncCancellationDecorator
            , CancellationDecorator<DownloadXslFromGitHubCommand> downloadXslFromGitHubHandlerAsyncCancellationDecorator
            , ICommandHandler<CiiXmlSchemaSetCommand, XmlSchemaSet> ciiXmlSchemaSet
            , ICommandHandler<UblXmlSchemaSetCommand, XmlSchemaSet> ublXmlSchemaSet)
        {
            InitializeComponent();

            _validationHandlerAsync = validationHandlerAsync;
            _downloadUnzipAndCopyXsdFilesHandlerAsync = downloadUnzipAndCopyXsdFilesHandlerAsyncCancellationDecorator;
            _downloadXslFromGitHubHandlerAsync = downloadXslFromGitHubHandlerAsyncCancellationDecorator;
            _ciiXmlSchemaSet = ciiXmlSchemaSet;
            _ublXmlSchemaSet = ublXmlSchemaSet;

            //https://stackoverflow.com/questions/56606962/httpclient-with-responseheadersread-failstimeouts-at-2nd-getasync-try-without
            ServicePointManager.DefaultConnectionLimit = 4;

        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            var xmlFileOrFolderName = tbXmlFileOrFolderName.Text.Trim('\"');
            var xmlDirExists = Directory.Exists(xmlFileOrFolderName);
            var xmlFileExists = File.Exists(xmlFileOrFolderName);
            var xmlExists = xmlDirExists || xmlFileExists;

            var xslFileOrFolderName = tbXslFileOrFolderName.Text.Trim('\"');
            var xslDirExists = Directory.Exists(xslFileOrFolderName);
            var xslFileExists = File.Exists(xslFileOrFolderName);
            var xslExists = xslDirExists || xslFileExists;

            if (string.IsNullOrWhiteSpace(xmlFileOrFolderName))
            {
                MessageBox.Show("XML file or folder name is empty!");
            }
            else if (string.IsNullOrWhiteSpace(xslFileOrFolderName))
            {
                MessageBox.Show("XSL file or folder name is empty!");
            }
            else if (!xmlExists)
            {
                MessageBox.Show("XML file or folder does not exists!");
            }
            else if (!xslExists)
            {
                MessageBox.Show("XSL file or folder does not exists!");
            }
            else
            {
                btnStart.Enabled = false;
                DisconnectDataSources();

                await DoValidation(xmlFileOrFolderName
                    , xslFileOrFolderName
                    , tsslNumOfXmlCnt
                    , tssValidationStatus
                    , _dsValidator
                    , cbOnlyCII.Checked
                    , tbUblXsdFilesLocation.Text
                    , tbCiiXsdFilesLocation.Text);
            }
        }

        private async Task DoValidation(string xmlFileOrFolderName
            , string xslFileOrFolderName
            , ToolStripStatusLabel numOfXmlCnt
            , ToolStripStatusLabel tssValidationStatus
            , DataSet dsValidation
            , bool bolTestOnlyCiiFiles
            , string ublXsdFilesLocation
            , string ciiXsdFilesLocation)
        {
            CiiXmlSchemaSetCommand ciiXmlSchemaSetCommand = new CiiXmlSchemaSetCommand
            {
                RootFolderWhereAreXsds = ciiXsdFilesLocation
            };

            UblXmlSchemaSetCommand ublXmlSchemaSetCommand = new UblXmlSchemaSetCommand
            {
                RootFolderWhereAreXsds = ublXsdFilesLocation
            };

            ValidationCommand validationCommand = new ValidationCommand
            {
                XmlFileOrFolderName = xmlFileOrFolderName,
                XslFileOrFolderName = xslFileOrFolderName,
                ResultDataSet = dsValidation,
                NumOfXmlCntObject = numOfXmlCnt,
                TestOnlyCiiFiles = bolTestOnlyCiiFiles,
                ToolStripCiiStatusLabel = tssCiiStatus,
                ToolStripUblStatusLabel = tssUblStatus,
                ToolStripValidationStatusLabel = tssValidationStatus,
                UblXsdFilesLocation = ublXsdFilesLocation,
                CiiXsdFilesLocation = ciiXsdFilesLocation,
                CiiXmlSchemaSet = _ciiXmlSchemaSet.Execute(ciiXmlSchemaSetCommand),
                UblXmlSchemaSet = _ublXmlSchemaSet.Execute(ublXmlSchemaSetCommand),
                ExecuteDisplayInGrid = ConnectDataGridViews
            };
            try
            {
                await _validationHandlerAsync.Execute(validationCommand);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DgvResultFilesOnRowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvResult.Rows[e.RowIndex];

            if (!(row.Cells[ResultValidatorColumns.Flag].Value is null))
            {
                if (row.Cells[ResultValidatorColumns.Flag].Value.ToString()
                    .Equals(Flags.Fatal, StringComparison.CurrentCultureIgnoreCase))
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (row.Cells[ResultValidatorColumns.Flag].Value.ToString()
                         .Equals(Flags.Exception, StringComparison.CurrentCultureIgnoreCase))
                {
                    row.DefaultCellStyle.BackColor = Color.Tomato;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else
                {
                    if (row.Cells[ResultValidatorColumns.Flag].Value.ToString()
                        .Equals(Flags.Warning, StringComparison.CurrentCultureIgnoreCase))
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void DgvXmlFilesFilesOnRowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvXmlFiles.Rows[e.RowIndex];

            if (!(row.Cells[XmlFilesColumns.ErrorCount].Value is null))
            {
                if (int.TryParse(row.Cells[XmlFilesColumns.ErrorCount].Value.ToString(), out int errorCount) ||
                    errorCount > 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (int.TryParse(row.Cells[XmlFilesColumns.WarningCount].Value.ToString(), out int warningCount) ||
                         warningCount > 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (int.TryParse(row.Cells[XslFilesColumns.ExceptionsCount].Value.ToString(),
                             out int exceptionsCount) || exceptionsCount > 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Tomato;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }

        private void DgvXslFilesOnRowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvXslFiles.Rows[e.RowIndex];

            if (!(row.Cells[XslFilesColumns.ErrorCount].Value is null))
            {
                if (int.TryParse(row.Cells[XslFilesColumns.ErrorCount].Value.ToString(), out int errorCount) ||
                    errorCount > 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (int.TryParse(row.Cells[XslFilesColumns.WarningCount].Value.ToString(), out int warningCount) ||
                         warningCount > 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (int.TryParse(row.Cells[XslFilesColumns.ExceptionsCount].Value.ToString(),
                             out int exceptionsCount) || exceptionsCount > 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Tomato;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }

        private void tbXmlFileOrFolderName_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.XmlFileOrFolderName = tbXmlFileOrFolderName.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void tbXslFileOrFolderName_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.XslFileOrFolderName = tbXslFileOrFolderName.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbXmlFileOrFolderName.Text = Properties.Settings.Default.XmlFileOrFolderName;
            tbXslFileOrFolderName.Text = Properties.Settings.Default.XslFileOrFolderName;

            tbCiiXsdFilesLocation.Text = Properties.Settings.Default.CiiXsdFilesLocation;
            tbUblXsdFilesLocation.Text = Properties.Settings.Default.UblXsdFilesLocation;

            tbLocationOfDownload.Text = string.IsNullOrWhiteSpace(Properties.Settings.Default.LocationOfDownload)
                ? @"c:\tmp"
                : Properties.Settings.Default.LocationOfDownload;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                if (!(control is Button button)) continue;
                if (!button.Enabled)
                {
                    tssValidationStatus.Text = "Cancel requested, please wait";
                }
            }

            _validationHandlerAsync.CancelOperation();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearDataTables();
        }

        private void ClearDataTables()
        {
            _dsValidator.Tables[DataTableNames.ResultValidator].Clear();
            _dsValidator.Tables[DataTableNames.XslFiles].Clear();
            _dsValidator.Tables[DataTableNames.XmlFiles].Clear();

            dgvXmlFiles.Refresh();
            dgvXslFiles.Refresh();
            dgvResult.Refresh();

            DisconnectDataSources();
        }

        private void DisconnectDataSources()
        {
            dgvXmlFiles.DataSource = null;
            dgvXslFiles.DataSource = null;
            dgvResult.DataSource = null;
        }

        private void btnSaveToXml_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "XML files (*.xml)|*.xml";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _dsValidator.WriteXml(saveFileDialog1.FileName);
            }
        }

        private void btnLoadFromXml_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ClearDataTables();
                _dsValidator.ReadXml(openFileDialog1.FileName);
                ConnectDataGridViews();
            }
        }

        private readonly object _dataSetLock = new object();
        private readonly ReaderWriterLockSlim _dataSetLockSlim = new ReaderWriterLockSlim();

        private void ConnectDataGridViews()
        {
            try
            {
                DataSet tempDataSet;
                lock (_dataSetLock)
                {
                    _dataSetLockSlim.EnterReadLock();
                    try
                    {
                        tempDataSet = _dsValidator.Copy();
                    }
                    finally
                    {
                        _dataSetLockSlim.ExitReadLock();
                    }
                }

                BindingSource xmlBindingSource = new BindingSource();
                xmlBindingSource.SuspendBinding();
                xmlBindingSource.DataSource = tempDataSet;
                xmlBindingSource.DataMember = DataTableNames.XmlFiles;
                xmlBindingSource.ResumeBinding();

                dgvXslFiles.Invoke((MethodInvoker)delegate
                {
                    dgvXmlFiles.DataSource = xmlBindingSource;
                    dgvXmlFiles.RowPrePaint += DgvXmlFilesFilesOnRowPrePaint;
                });

                BindingSource xslBindingSource = new BindingSource();
                xslBindingSource.SuspendBinding();
                xslBindingSource.DataSource = xmlBindingSource;
                xslBindingSource.DataMember = RelationNames.XmlFilesXslFilesRelation;
                xslBindingSource.ResumeBinding();

                dgvXslFiles.Invoke((MethodInvoker)delegate
                {
                    dgvXslFiles.RowPrePaint += DgvXslFilesOnRowPrePaint;
                    dgvXslFiles.DataSource = xslBindingSource;
                });

                BindingSource resultValidatorBindingSource = new BindingSource();
                resultValidatorBindingSource.SuspendBinding();
                resultValidatorBindingSource.DataSource = xslBindingSource;
                resultValidatorBindingSource.DataMember = RelationNames.XslFilesResultRelation;
                resultValidatorBindingSource.ResumeBinding();
                dgvResult.Invoke((MethodInvoker)delegate
                {
                    dgvResult.DataSource = resultValidatorBindingSource;
                    dgvResult.RowPrePaint += DgvResultFilesOnRowPrePaint;
                });

                btnStart.Invoke((MethodInvoker)delegate { btnStart.Enabled = true; });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void btnCopyGoodFiles_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Properties.Settings.Default.SelectedPath;

            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;

            Properties.Settings.Default.SelectedPath = folderBrowserDialog1.SelectedPath;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            DataTable dtXmlFiles = _dsValidator.Tables[DataTableNames.XmlFiles];

            string searchExpression = $"({XmlFilesColumns.ErrorCount} IS NULL OR {XmlFilesColumns.ErrorCount} = 0)";
            DataRow[] drXmlFilesFiltered = dtXmlFiles.Select(searchExpression);
            foreach (DataRow dataRow in drXmlFilesFiltered)
            {
                string strOrigFullFileName = dataRow[XmlFilesColumns.FileName].ToString();
                string strOrigFileNameOnly = Path.GetFileName(strOrigFullFileName);
                string strFullCopiedFileName = Path.Combine(folderBrowserDialog1.SelectedPath, strOrigFileNameOnly);
                File.Copy(strOrigFullFileName, strFullCopiedFileName, true);
            }
        }

        private async void btnDownloadCiiXsdFiles_Click(object sender, EventArgs e)
        {
            ToolStripProgressBar tsslCiiDownloadProgress = new ToolStripProgressBar();
            statusStrip1.Items.Add(tsslCiiDownloadProgress);

            string uneceFolder = Path.Combine(tbLocationOfDownload.Text, @"unece");
            btnDownloadCiiXsdFiles.Enabled = false;
            DownloadUnzipAndCopyXsdFilesCommand downloadUnzipAndCopyXsdFilesCommand =
                new DownloadUnzipAndCopyXsdFilesCommand
                {
                    SecondZipFileNameWithPath =
                        @"D16B SCRDM (Subset) CII\D16B SCRDM (Subset) CII uncoupled.zip", //ToDo uncoupled should be used
                    FileName = "D16B_SCRDM__Subset__CII.zip",
                    RootFolder = uneceFolder,
                    CopyFromFolder = @"D16B SCRDM (Subset) CII uncoupled\uncoupled clm\CII\uncefact\",
                    CopyToFolder = uneceFolder,
                    ToolStripProgressBar = tsslCiiDownloadProgress,
                    ToolStripStatusLabel = tssCiiStatus
                };

            string saveDownloadedFileTo = Path.Combine(downloadUnzipAndCopyXsdFilesCommand.RootFolder,
                downloadUnzipAndCopyXsdFilesCommand.FileName);

            Dictionary<string, string> saveUrlsToFile = new Dictionary<string, string>
            {
                {
                    "https://unece.org/DAM/cefact/xml_schemas/D16B_SCRDM__Subset__CII.zip",
                    saveDownloadedFileTo
                }
            };
            downloadUnzipAndCopyXsdFilesCommand.SaveUrlsToFile = saveUrlsToFile;
            try
            {
                await _downloadUnzipAndCopyXsdFilesHandlerAsync.Execute(downloadUnzipAndCopyXsdFilesCommand);

                tbCiiXsdFilesLocation.Invoke(new Action(() =>
                {
                    tbCiiXsdFilesLocation.Text = Path.Combine(downloadUnzipAndCopyXsdFilesCommand.CopyToFolder
                        , @"D16B SCRDM (Subset) CII uncoupled\uncoupled clm\CII\uncefact\data\standard");
                    Properties.Settings.Default.CiiXsdFilesLocation = tbCiiXsdFilesLocation.Text;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                }));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                btnDownloadCiiXsdFiles.Enabled = true;
                statusStrip1.Items.Remove(tsslCiiDownloadProgress);
            }
        }

        private async void btnDownloadUblXsdFiles_Click(object sender, EventArgs e)
        {
            ToolStripProgressBar tssUblDownloadProgress = new ToolStripProgressBar();
            statusStrip1.Items.Add(tssUblDownloadProgress);

            string rootFolder = Path.Combine(tbLocationOfDownload.Text, @"oasis\");
            btnDownloadUblXsdFiles.Enabled = false;
            DownloadUnzipAndCopyXsdFilesCommand downloadUnzipAndCopyXsdFilesCommand =
                new DownloadUnzipAndCopyXsdFilesCommand
                {
                    FileName = "UBL-2.1.zip",
                    RootFolder = rootFolder,
                    CopyFromFolder = @"xsd\",
                    CopyToFolder = rootFolder,
                    ToolStripProgressBar = tssUblDownloadProgress,
                    ToolStripStatusLabel = tssUblStatus
                };

            string saveDownloadedFileTo = Path.Combine(downloadUnzipAndCopyXsdFilesCommand.RootFolder,
                downloadUnzipAndCopyXsdFilesCommand.FileName);
            Dictionary<string, string> saveUrlsToFile = new Dictionary<string, string>
            {
                {
                    "https://docs.oasis-open.org/ubl/os-UBL-2.1/UBL-2.1.zip",
                    saveDownloadedFileTo
                }
            };
            downloadUnzipAndCopyXsdFilesCommand.SaveUrlsToFile = saveUrlsToFile;
            try
            {
                await _downloadUnzipAndCopyXsdFilesHandlerAsync.Execute(downloadUnzipAndCopyXsdFilesCommand);
                tbUblXsdFilesLocation.Invoke(new Action(() =>
                {
                    tbUblXsdFilesLocation.Text = Path.Combine(downloadUnzipAndCopyXsdFilesCommand.CopyToFolder, "xsd");
                    Properties.Settings.Default.UblXsdFilesLocation = tbUblXsdFilesLocation.Text;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                }));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                btnDownloadUblXsdFiles.Enabled = true;
                statusStrip1.Items.Remove(tssUblDownloadProgress);
            }
        }

        private async void btnDownloadXslFiles_Click(object sender, EventArgs e)
        {
            ToolStripProgressBar tssDownloadXslFilesProgress = new ToolStripProgressBar();
            statusStrip1.Items.Add(tssDownloadXslFilesProgress);

            List<string> repositories = new List<string>
            {
                "https://api.github.com/repos/itplr-kosit/validator-configuration-xrechnung/releases",
                "https://api.github.com/repos/itplr-kosit/xrechnung-schematron/releases",
                "https://api.github.com/repos/ConnectingEurope/eInvoicing-EN16931/releases"
            };

            DownloadXslFromGitHubCommand downloadXslFromGitHubCommand = new DownloadXslFromGitHubCommand
            {
                Repositories = repositories,
                TssUblStatus = tssUblStatus,
                TsslUblDownloadProgress = tssDownloadXslFilesProgress,
                RooFolderWhereToSaveDownloadedFiles = Path.Combine(tbLocationOfDownload.Text, "xslFiles")
            };

            btnDownloadXslFiles.Enabled = false;
            try
            {
                await _downloadXslFromGitHubHandlerAsync.Execute(downloadXslFromGitHubCommand);
                tbXslFileOrFolderName.Invoke(new Action(() =>
                {
                    tbXslFileOrFolderName.Text = Path.Combine(tbLocationOfDownload.Text, "xslFiles");
                    tbXslFileOrFolderName_Leave(null, null);
                }));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                btnDownloadXslFiles.Enabled = true;
                statusStrip1.Items.Remove(tssDownloadXslFilesProgress);
            }
        }

        private async void btnDownloadKositValidatorConfiguration_Click(object sender, EventArgs e)
        {
            ToolStripProgressBar tssDownloadKositFilesProgress = new ToolStripProgressBar();
            statusStrip1.Items.Add(tssDownloadKositFilesProgress);

            List<string> repositories = new List<string>
            {
                "https://api.github.com/repos/itplr-kosit/validator-configuration-xrechnung/releases"
            };

            DownloadXslFromGitHubCommand downloadKositFromGitHubCommand = new DownloadXslFromGitHubCommand
            {
                Repositories = repositories,
                TssUblStatus = tssUblStatus,
                TsslUblDownloadProgress = tssDownloadKositFilesProgress,
                RooFolderWhereToSaveDownloadedFiles = Path.Combine(tbLocationOfDownload.Text, "kosit")
            };

            btnDownloadKositValidatorConfiguration.Enabled = false;
            try
            {
                await _downloadXslFromGitHubHandlerAsync.Execute(downloadKositFromGitHubCommand);
                tbXslFileOrFolderName.Invoke(new Action(() => {
                    tbXslFileOrFolderName.Text = downloadKositFromGitHubCommand.RooFolderWhereToSaveDownloadedFiles;
                    tbXslFileOrFolderName_Leave(null, null);
                }));

                tbCiiXsdFilesLocation.Invoke(new Action(() =>
                {
                    tbCiiXsdFilesLocation.Text =
                        Path.Combine(downloadKositFromGitHubCommand.RooFolderWhereToSaveDownloadedFiles, @"validator-configuration-xrechnung\zip\resources\cii\16b\xsd");
                }));

                tbUblXsdFilesLocation.Invoke(new Action(() =>
                {
                    tbUblXsdFilesLocation.Text = Path.Combine(downloadKositFromGitHubCommand.RooFolderWhereToSaveDownloadedFiles, @"validator-configuration-xrechnung\zip\resources\ubl\2.1\xsd");
                }));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                btnDownloadKositValidatorConfiguration.Enabled = true;
                statusStrip1.Items.Remove(tssDownloadKositFilesProgress);
            }
        }

        private void tbCiiXsdFilesLocation_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.CiiXsdFilesLocation = tbCiiXsdFilesLocation.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void tbUblXsdFilesLocation_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.UblXsdFilesLocation = tbUblXsdFilesLocation.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }
    }
}