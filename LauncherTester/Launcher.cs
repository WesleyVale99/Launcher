using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace LauncherTester
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public SortedList<string, string> strs;
        public bool initial = true;
        public WebClient web = new WebClient(), webcontas = new WebClient();
        public Form1()
        {
            InitializeComponent();
            Application.DoEvents();
            //new Thread(new ThreadStart(ArredondaCantosdoForm)).Start();
            //   strs = loadXML(string.Concat(Application.StartupPath, "\\UserFileList.dat"));
            //   MessageBox.Show(strs.Count.ToString());
            new Thread(new ThreadStart(TooTipAll)).Start();
            label3.Text = "Horario Local: " + DateTime.Now.ToString("dd/MM/yyyy HH:MM");
        }
        public void TooTipAll()
        {
            toolTip1.SetToolTip(button1, "Inicie o jogo aqui.");
            toolTip1.SetToolTip(button4, "Site do jogo para a diversão ficar melhor.");
            toolTip1.SetToolTip(pictureBox3, "Point Blank é um jogo eletrônico free-to-play de tiro em primeira pessoa on-line, no formato Massively Multiplayer Online Game");
            toolTip1.SetToolTip(pictureBox6, "Configuração do Point Blank");
            toolTip1.SetToolTip(label4, "Todos os cretidos reservados. - Wesley Vale");
        }
        public void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //int index = 0;
                //IEnumerable<FileSystemInfo> resultado = GetArquivosDiretorios(Application.StartupPath);
                //foreach (FileSystemInfo res in resultado)
                //{
                //    Application.DoEvents();
                //    if (res.FullName == strs.Keys[index])
                //    {
                //        label3.Text = "Check de arquivos: " + strs.Keys[index];
                //    }
                //    else
                //    {
                //        MessageBox.Show(res.FullName.ToString() + " || " + strs.Keys[index]);
                //    }
                //    ++index;
                //}
                if (!VerficarPB())
                    button1_KeyPress(sender, new KeyPressEventArgs((char)13));
                else
                    MessageBox.Show("O Jogo já está aberto.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        public static IEnumerable<FileSystemInfo> GetArquivosDiretorios(string dir)
        {
            if (string.IsNullOrWhiteSpace(dir))
            {
                throw new ArgumentNullException(nameof(dir));
            }

            //define uma instância da classe DirectoryInfo
            DirectoryInfo dirInfo = new DirectoryInfo(dir);

            //define uma pilha de objetos FileSystemInfo e insere o objeto no topo da pilha
            Stack<FileSystemInfo> stack = new Stack<FileSystemInfo>();
            stack.Push(dirInfo);

            //itera enquanto for diferente de null e o contador da pilha for maior que zero
            while (dirInfo != null || stack.Count > 0)
            {
                //retorna o objeto do topo da pilha e remove
                FileSystemInfo fileSystemInfo = stack.Pop();

                if (fileSystemInfo is DirectoryInfo subDiretorioInfo)
                {
                    //retorna cada elemento individualmente
                    yield return subDiretorioInfo;
                    FileSystemInfo[] array = subDiretorioInfo.GetFileSystemInfos();
                    for (int i = 0; i < array.Length; i++)
                    {
                        FileSystemInfo fsi = array[i];
                        //insere o objeto no topo da pilha
                        stack.Push(fsi);
                    }
                    dirInfo = subDiretorioInfo;
                }
                else
                {
                    //retorna cada elemento individualmente
                    yield return fileSystemInfo;
                    dirInfo = null;
                }
            }
        }
        public void ArredondaCantosdoForm()
        {
            using (GraphicsPath PastaGrafica = new GraphicsPath())
            {
                PastaGrafica.AddRectangle(new Rectangle(1, 1, Size.Width, Size.Height));

                //Arredondar canto superior esquerdo        
                PastaGrafica.AddRectangle(new Rectangle(1, 1, 10, 10));
                PastaGrafica.AddPie(1, 1, 20, 20, 180, 90);

                //Arredondar canto superior direito
                PastaGrafica.AddRectangle(new Rectangle(Width - 12, 1, 12, 13));
                PastaGrafica.AddPie(Width - 24, 1, 24, 26, 270, 90);

                //Arredondar canto inferior esquerdo
                PastaGrafica.AddRectangle(new Rectangle(1, Height - 10, 10, 10));
                PastaGrafica.AddPie(1, Height - 20, 20, 20, 90, 90);

                //Arredondar canto inferior direito
                PastaGrafica.AddRectangle(new Rectangle(Width - 12, Height - 13, 13, 13));
                PastaGrafica.AddPie(Width - 24, Height - 26, 24, 26, 0, 90);

                PastaGrafica.SetMarkers();
                Region = new Region(PastaGrafica);
            };

        }
        private void button4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Estamos usando a função auto create, pois estamos desenvolvendo nosso site. -Project Eros");
            Process.Start("https://projecteros.nloja.com/planos-vip/");
        }

        public void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Process.Start(Application.StartupPath + $"//PointBlank.exe", "289289289");
                // Process.Start(Application.StartupPath + $"//PointBlank.exe", $"/GameID:GarenaPB /Token:wsly");
                initial = false;
                Environment.Exit(0);

            }
            catch (Exception)
            {
                MessageBox.Show("O sistema não pode encontrar o jogo!", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/wesley.vale.3192");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            try
            {
                if (!VerficarPBconf())
                    Process.Start(Application.StartupPath + $"//PBConfig", "0");
                else
                    MessageBox.Show("Point Blank Config já está aberto.");
            }
            catch
            {
                MessageBox.Show("Erro ao encontrar o arquivo PBconfig.exe");
            }
        }
        public bool VerficarPB()
        {
            if (Process.GetProcessesByName("PointBlank").Length > 0)
                return true;
            return false;
        }
        public bool VerficarPBconf()
        {
            if (Process.GetProcessesByName("PBConfig").Length > 1)
                return true;
            return false;
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBox1.SelectedIndex == 0)
            {
                pictureBox7.Visible = true;
                pictureBox8.Visible = false;
                pictureBox9.Visible = false;
            }
            else if (metroComboBox1.SelectedIndex == 1)
            {
                pictureBox7.Visible = false;
                pictureBox8.Visible = false;
                pictureBox9.Visible = true;
            }
            else if (metroComboBox1.SelectedIndex == 2)
            {
                pictureBox7.Visible = false;
                pictureBox8.Visible = true;
                pictureBox9.Visible = false;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            pictureBox4.Visible = true;
            pictureBox5.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = true;
        }
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/groups/ProjectEROS/");

        }
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Process.Start("https://chat.whatsapp.com/LQjDKLiWsPkJVpbSLYF5ye");
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Process.Start("https://projecteros.nloja.com/");
        }
        public static SortedList<string, string> loadXML(string path)
        {
            SortedList<string, string> strs = new SortedList<string, string>();
            XmlDocument xmlDocument = new XmlDocument();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            if (fileStream.Length != 0)
            {
                try
                {
                    xmlDocument.Load(fileStream);
                    for (XmlNode i = xmlDocument.FirstChild; i != null; i = i.NextSibling)
                    {
                        if ("list".Equals(i.Name))
                        {
                            for (XmlNode j = i.FirstChild; j != null; j = j.NextSibling)
                            {
                                if ("f".Equals(j.Name))
                                {
                                    XmlNamedNodeMap attributes = j.Attributes;
                                    string value = attributes.GetNamedItem("n").Value;
                                    if (!strs.ContainsKey(value))
                                    {
                                        strs.Add(value, attributes.GetNamedItem("m").Value);
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            fileStream.Dispose();
            fileStream.Close();
            return strs;
        }
    }
}

