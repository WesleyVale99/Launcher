using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace PBLauncher
{
    public partial class Loading : MetroFramework.Forms.MetroForm
    {
        public WebClient client = new WebClient();
        public List<string> arquivos = new List<string>();
        public Loading()
        {
            InitializeComponent();
            metroProgressBar1.ForeColor = Color.Blue;
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(process);
        }

        private void process(object sender, DownloadProgressChangedEventArgs e)
        {
            metroProgressBar1.Width = (int)(e.BytesReceived * 367 / e.TotalBytesToReceive);
        }

        public void OpenAPP()
        {
            Application.EnableVisualStyles();
            Application.Run(new Update());
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            try
            {
                new Thread(() =>
                {
                    lock (this)
                    {
                        Application.DoEvents();
                        string path = Application.StartupPath;
                        if (!File.Exists(path + "\\DotNetZip.dll"))
                            arquivos.Add("DotNetZip.dll");
                        if (!File.Exists(path + "\\config.zpt"))
                            arquivos.Add("config.zpt");
                        if (!File.Exists(path + "\\Ionic.Zip.dll"))
                            arquivos.Add("Ionic.Zip.dll");
                        int itens = arquivos.Count;
                        if (itens == 0)
                        {
                            new Thread(new ThreadStart(OpenAPP)).Start();
                            Close();
                        }
                        else
                        {
                            for (int i = 0; i < arquivos.Count; i++)
                            {
                                string name = arquivos[i];
                                using (client)
                                {
                                    client.DownloadFile(new Uri("http://127.0.0.1/launcher/versions/arquivos/" + name + ""), name);
                                    client.Dispose();
                                }
                             //   new Action(() => { label1.Text = ("Baixados: " + index + " de " + itens + " [" + name + "]"); }).Invoke();
                            }
                            new Thread(new ThreadStart(OpenAPP)).Start();
                            Close();
                        }
                    }
                }).Start();
            }
            catch
            {
                MessageBox.Show("Error ao baixar pendência, verifique sua conexão a internet ou contate o desenvolvedor.", "PBLauncher", MessageBoxButtons.OK);
            }
        }
    }
}
