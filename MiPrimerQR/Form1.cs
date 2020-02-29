using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Aztec;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using System.IO;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;

namespace MiPrimerQR
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {

        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevice) {
                comboBox1.Items.Add(Device.Name);
            }
            comboBox1.SelectedIndex = 0;
            FinalFrame = new VideoCaptureDevice();

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if(txtNombre.Text!= "")
            {
                BarcodeWriter bw = new BarcodeWriter();
                bw.Format = BarcodeFormat.QR_CODE;
                Bitmap bm = new Bitmap(bw.Write(txtNombre.Text), 300, 300);
                pictureBoxGenerar.Image = bm;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(FinalFrame.IsRunning == true)
            {
                FinalFrame.Stop();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            FinalFrame = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();
        }

        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox2.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BarcodeReader Reader = new BarcodeReader();
            Result result = Reader.Decode((Bitmap)pictureBox2.Image);

            try
            {
                String decoded = result.ToString().Trim();
                if(decoded != null)
                {
                    timer1.Stop();
                    MessageBox.Show(decoded);
                }

            }catch (Exception ex)
            {

            }

        }
    }
}
