using Ionic.Zip;
using LauncherTester;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PBLauncher
{
    public partial class Update : MetroFramework.Forms.MetroForm
    {
        public string sessao = "Project Eros";
        public int newersion;
        public string IP = "127.0.0.1";
        public WebClient gameUpdate = new WebClient(), web = new WebClient();
        public Update()
        {
            InitializeComponent();
            gameUpdate.DownloadFileCompleted += new AsyncCompletedEventHandler(gameUpdate_DownloadCompleted);
            gameUpdate.DownloadProgressChanged += new DownloadProgressChangedEventHandler(gameUpdate_DownloadProgressChanged);
            gameUpdate.DownloadProgressChanged += new DownloadProgressChangedEventHandler(gameUpdate_DownloadProgressChanged1);
            if (Process.GetProcessesByName("PBLauncher").Length > 1)
            {
                MessageBox.Show("Você não pode abrir esses arquivos 2 vezes!", "PBLauncher", MessageBoxButtons.OK);
                Environment.Exit(0);
            }
            started();
        }
        public void isHack()
        {
            try
            {
                string str = null;
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    str = "system32\\drivers\\etc\\hosts";
                }
                string str1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), str);
                string[] str2 = File.ReadAllLines(str1);
                for (int i = 0; i < str2.Length; i++)
                {
                    string line = str2[i];
                    if (line.Contains("0.0.0.0 brcheats.net.br www.brcheats.net.br"))
                    {
                        Email($"www.brcheats.net.br Usuario tentou usar hacker ou baixou algum comonente remente a esse URl de site. Mac: {GetEnderecoMAC1()}, IP: {GetLocalIPAddress()}, UserName: {Environment.MachineName}");
                        break;
                    }
                    else if (line.Contains("0.0.0.0 www.foxcheats.net"))
                    {
                        Email($"www.foxcheats.net Usuario tentou usar hacker ou baixou algum comonente remente a esse URl de site. Mac: {GetEnderecoMAC1()}, IP: {GetLocalIPAddress()}, UserName: {Environment.MachineName}");
                        break;
                    }
                    else if (line.Contains("0.0.0.0 www.webfirebr.com"))
                    {
                        Email($"www.webfirebr.com Usuario tentou usar hacker ou baixou algum comonente remente a esse URl de site. Mac: {GetEnderecoMAC1()}, IP: {GetLocalIPAddress()}, UserName: {Environment.MachineName}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void Email(string email)
        {
            try
            {
                MailMessage mensagemEmail = new MailMessage("azurepb2018@gmail.com", "contato@troiagamez.com.br", email, "Detect Hack");
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true
                };
                NetworkCredential cred = new NetworkCredential("brasilhistorygaming@gmail.com", "91729499");
                client.Credentials = cred;
                client.UseDefaultCredentials = true;
                client.Send(mensagemEmail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            for (int i = 0; i < host.AddressList.Length; i++)
            {
                IPAddress ip = host.AddressList[i];
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public static string GetEnderecoMAC1()
        {
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String enderecoMAC = string.Empty;
                for (int i = 0; i < nics.Length; i++)
                {
                    NetworkInterface adapter = nics[i];
                    if (enderecoMAC == string.Empty)
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        enderecoMAC = adapter.GetPhysicalAddress().ToString();
                    }
                }
                return enderecoMAC;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void started()
        {
            try
            {
                metroProgressBar1.ForeColor = Color.RoyalBlue;
                newersion = int.Parse(web.DownloadString("http://" + IP + "/launcher/versions/versao_atual.txt"));
                int num = int.Parse(LerArquivo(Application.StartupPath + "\\config.zpt", sessao, "lancador", "0"));
                if (num > newersion)
                {
                    MessageBox.Show("Error a versão config não pode ser maior que a necessaria!");
                    Close();
                }
                else if(newersion == 0)
                {
                    if (MessageBox.Show(";C Desculpa o jogo está em manunteção.", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning)== DialogResult.OK)
                        Close();
                }
                else if (newersion > num)
                {
                    if (Directory.Exists(Application.StartupPath + "\\_DownloadPatchFiles"))
                        Directory.Delete(Application.StartupPath + "\\_DownloadPatchFiles");
                    if (MessageBox.Show("Ops! ;D uma atualização foi encontrada deseja continuar?", "PBLauncher", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        AutorizedDownload();
                    else
                        Close();
                }
                else
                {
                    Close();
                    Application.EnableVisualStyles();
                    Application.Run(new Form1());
                }
            }
            catch
            {
                MessageBox.Show("Verifique sua conexão a internet ou contate o desenvolvedor");
                Close();
            }
        }
        public void AutorizedDownload()
        {
            try
            {
                int num = int.Parse(LerArquivo(Application.StartupPath + "\\config.zpt", sessao, "lancador", "0")) + 1;
                Directory.CreateDirectory(Application.StartupPath + "\\_DownloadPatchFiles");
                Uri address = new Uri(string.Concat("http://" + IP + "/launcher/versions/updates/Update_", num,".zip"));
                gameUpdate.DownloadFileAsync(address, string.Concat(Application.StartupPath,"\\_DownloadPatchFiles\\Update_", num,".zip"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ao atualizar o jogo! " + ex.ToString());
                Close();
            }
        }
        public void Bar1SetProgress(long received, long maximum)
        {
            metroProgressBar1.Width = (int)(received * 405 / maximum);
        }

        private void gameUpdate_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    int num = int.Parse(LerArquivo(string.Concat(Application.StartupPath, "\\config.zpt"), sessao, "lancador", "0"));
                    int num1 = num + 1;
                    unzip(Application.StartupPath, string.Concat(Application.StartupPath, "\\_DownloadPatchFiles\\Update_", num1, ".zip"));
                    Directory.Delete(string.Concat(Application.StartupPath, "\\_DownloadPatchFiles"), true);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void gameUpdate_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Bar1SetProgress(e.BytesReceived, e.TotalBytesToReceive);
        }

        private void gameUpdate_DownloadProgressChanged1(object sender, DownloadProgressChangedEventArgs e)
        {
            Application.DoEvents();
            new Action(() => label3.Text = "Aguarde o jogo está sendo atualizado [" + e.ProgressPercentage + "]%").Invoke();

        }
        public void unzip(string TargetDir, string ZipToUnpack)
        {
            try
            {
                ZipFile zipFile = ZipFile.Read(ZipToUnpack);
                try
                {
                    zipFile.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(unzip_ExtractProgressChanged);
                    foreach (ZipEntry zipEntry1 in zipFile)
                    {
                        string fileName = zipEntry1.FileName;
                        if (fileName.Contains("/"))
                        {
                            int num2 = fileName.LastIndexOf("/");
                            fileName = fileName.Substring(num2 + 1);
                        }
                        zipEntry1.Extract(TargetDir, ExtractExistingFileAction.OverwriteSilently);
                    }
                    Write();
                }
                finally
                {
                    if (zipFile != null)
                    {
                        zipFile.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
        public void Write()
        {
            StreamWriter stream = new StreamWriter(Application.StartupPath + "\\config.zpt");
            stream.WriteLine("[Project Eros]");
            stream.WriteLine($"lancador={newersion}");
            stream.Dispose();
            stream.Close();
            Close();
            new Thread(new ThreadStart(InitialNewForm)).Start();
        }
        public void InitialNewForm()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
        private void unzip_ExtractProgressChanged(object sender, ExtractProgressEventArgs e)
        {
            try
            {
                if (e.TotalBytesToTransfer != 0)
                {
                    label3.Text = "Aguarde o jogo está sendo extraido [" + e.BytesTransferred + " // " + e.TotalBytesToTransfer + " ]%";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
        public static extern bool GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int size, string lpFileName);
        private string LerArquivo(string arquivo, string secao, string chave, string valorPadrao)
        {
            StringBuilder stringBuilder = new StringBuilder(500);
            GetPrivateProfileString(secao, chave, valorPadrao, stringBuilder, 500, arquivo);
            return stringBuilder.ToString();
        }
    }
}
