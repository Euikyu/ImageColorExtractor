using Cognex.VisionPro;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Implementation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageSpoid_Example
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private CogRecordDisplay m_Display;
        private CogAffineTransformTool m_Tool0;
        private CogAffineTransformTool m_Tool1;
        private CogAffineTransformTool m_Tool2;
        private CogAffineTransformTool m_Tool3;
        private CogAffineTransformTool m_Tool4;
        private CogAffineTransformTool m_Tool5;
        private bool m_IsRun;
        private int m_Progress;

        private Thread m_ProcThread;
        private int _maxCount;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string pName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pName));
        }

        public bool IsRun { get => m_IsRun; set { m_IsRun = value; RaisePropertyChanged(nameof(IsRun)); } }
        public int Progress { get => m_Progress; set { m_Progress = value; RaisePropertyChanged(nameof(Progress)); } }
        public int MaxCount { get => _maxCount; set { _maxCount = value; RaisePropertyChanged(nameof(MaxCount)); } }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Host_Loaded(object sender, RoutedEventArgs e)
        {
            m_Tool0 = new CogAffineTransformTool();
            m_Tool1 = new CogAffineTransformTool();
            m_Tool2 = new CogAffineTransformTool();
            m_Tool3 = new CogAffineTransformTool();
            m_Tool4 = new CogAffineTransformTool();
            m_Tool5 = new CogAffineTransformTool();
            m_Display = new CogRecordDisplay();
            host.Child = m_Display;

            if (!m_Display.Created) MessageBox.Show("Unapplied controls.");
        }

        private void Image_Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsRun) return;

            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Image files. (*.bmp; *.png; *.jpg; *.jpeg)|*.bmp;*.png;*.jpg;*.jpeg"
            };
            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    m_Display.Image = new CogImage24PlanarColor(new System.Drawing.Bitmap(dialog.FileName));
                    m_Tool0.InputImage = m_Display.Image;
                    m_Tool1.InputImage = m_Display.Image;
                    m_Tool2.InputImage = m_Display.Image;
                    m_Tool3.InputImage = m_Display.Image;
                    m_Tool4.InputImage = m_Display.Image;
                    m_Tool5.InputImage = m_Display.Image;

                    var rec = new CogRecord
                    {
                        Content = m_Display.Image
                    };
                    rec.SubRecords.Add(m_Tool0.CreateCurrentRecord().SubRecords[0]);
                    rec.SubRecords.Add(m_Tool1.CreateCurrentRecord().SubRecords[0]);
                    rec.SubRecords.Add(m_Tool2.CreateCurrentRecord().SubRecords[0]);
                    rec.SubRecords.Add(m_Tool3.CreateCurrentRecord().SubRecords[0]);
                    rec.SubRecords.Add(m_Tool4.CreateCurrentRecord().SubRecords[0]);
                    rec.SubRecords.Add(m_Tool5.CreateCurrentRecord().SubRecords[0]);

                    m_Display.Record = rec;
                }
                catch (Exception err)
                {
                    MessageBox.Show("Image open err :" + err.Message);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsRun) return;
                Progress = 0;
                IsRun = true;

                m_Tool0.Run();
                if (m_Tool0.RunStatus.Exception != null)
                {
                    MessageBox.Show("Execute failed : " + m_Tool0.RunStatus.Exception.Message);
                    return;
                }
                m_Tool1.Run();
                if (m_Tool1.RunStatus.Exception != null)
                {
                    MessageBox.Show("Execute failed : " + m_Tool1.RunStatus.Exception.Message);
                    return;
                }
                m_Tool2.Run();
                if (m_Tool2.RunStatus.Exception != null)
                {
                    MessageBox.Show("Execute failed : " + m_Tool2.RunStatus.Exception.Message);
                    return;
                }
                m_Tool3.Run();
                if (m_Tool3.RunStatus.Exception != null)
                {
                    MessageBox.Show("Execute failed : " + m_Tool3.RunStatus.Exception.Message);
                    return;
                }
                m_Tool4.Run();
                if (m_Tool4.RunStatus.Exception != null)
                {
                    MessageBox.Show("Execute failed : " + m_Tool4.RunStatus.Exception.Message);
                    return;
                }
                m_Tool5.Run();
                if (m_Tool5.RunStatus.Exception != null)
                {
                    MessageBox.Show("Execute failed : " + m_Tool5.RunStatus.Exception.Message);
                    return;
                }

                m_ProcThread = new Thread(new ThreadStart(() =>
                {
                    var bmp0 = m_Tool0.OutputImage.ToBitmap();

                    byte[] img0Arr;

                    var bData0 = bmp0.LockBits(new System.Drawing.Rectangle(0, 0, bmp0.Width, bmp0.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp0.PixelFormat);
                    img0Arr = new byte[bData0.Stride * bData0.Height];
                    Marshal.Copy(bData0.Scan0, img0Arr, 0, img0Arr.Length);
                    bmp0.UnlockBits(bData0);

                    var bmp1 = m_Tool1.OutputImage.ToBitmap();

                    byte[] img1Arr;

                    var bData1 = bmp1.LockBits(new System.Drawing.Rectangle(0, 0, bmp1.Width, bmp1.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp1.PixelFormat);
                    img1Arr = new byte[bData1.Stride * bData1.Height];
                    Marshal.Copy(bData1.Scan0, img1Arr, 0, img1Arr.Length);
                    bmp1.UnlockBits(bData1);

                    var bmp2 = m_Tool2.OutputImage.ToBitmap();

                    byte[] img2Arr;

                    var bData2 = bmp2.LockBits(new System.Drawing.Rectangle(0, 0, bmp2.Width, bmp2.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp2.PixelFormat);
                    img2Arr = new byte[bData2.Stride * bData2.Height];
                    Marshal.Copy(bData2.Scan0, img2Arr, 0, img2Arr.Length);
                    bmp2.UnlockBits(bData2);

                    var bmp3 = m_Tool3.OutputImage.ToBitmap();

                    byte[] img3Arr;

                    var bData3 = bmp3.LockBits(new System.Drawing.Rectangle(0, 0, bmp3.Width, bmp3.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp3.PixelFormat);
                    img3Arr = new byte[bData3.Stride * bData3.Height];
                    Marshal.Copy(bData3.Scan0, img3Arr, 0, img3Arr.Length);
                    bmp3.UnlockBits(bData3);

                    var bmp4 = m_Tool4.OutputImage.ToBitmap();

                    byte[] img4Arr;

                    var bData4 = bmp4.LockBits(new System.Drawing.Rectangle(0, 0, bmp4.Width, bmp4.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp4.PixelFormat);
                    img4Arr = new byte[bData4.Stride * bData4.Height];
                    Marshal.Copy(bData4.Scan0, img4Arr, 0, img4Arr.Length);
                    bmp4.UnlockBits(bData4);

                    var bmp5 = m_Tool5.OutputImage.ToBitmap();

                    byte[] img5Arr;

                    var bData5 = bmp5.LockBits(new System.Drawing.Rectangle(0, 0, bmp5.Width, bmp5.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp5.PixelFormat);
                    img5Arr = new byte[bData5.Stride * bData5.Height];
                    Marshal.Copy(bData5.Scan0, img5Arr, 0, img5Arr.Length);
                    bmp5.UnlockBits(bData5);

                    SaveFileDialog dialog = new SaveFileDialog
                    {
                        Filter = "CSV file. (*.csv)|*.csv"
                    };

                    if ((bool)dialog.ShowDialog())
                    {
                        string csv = "T1_R,T1_G,T1_B,T2_R,T2_G,T2_B,T3_R,T3_G,T3_B,T4_R,T4_G,T4_B,T5_R,T5_G,T5_B,T6_R,T6_G,T6_B" + Environment.NewLine;

                        var maxSize = (new int[] { img0Arr.Length, img1Arr.Length, img2Arr.Length, img3Arr.Length, img4Arr.Length, img5Arr.Length }).Max();

                        MaxCount = maxSize;

                        for (int i = 0; i < maxSize; i += 3)
                        {
                            if (i + 2 < img0Arr.Length)
                            {
                                csv += img0Arr[i + 0] + ",";
                                csv += img0Arr[i + 1] + ",";
                                csv += img0Arr[i + 2] + ",";
                            }
                            else
                            {
                                csv += ",,,";
                            }

                            if (i + 2 < img1Arr.Length)
                            {
                                csv += img1Arr[i + 0] + ",";
                                csv += img1Arr[i + 1] + ",";
                                csv += img1Arr[i + 2] + ",";
                            }
                            else
                            {
                                csv += ",,,";
                            }

                            if (i + 2 < img2Arr.Length)
                            {
                                csv += img2Arr[i + 0] + ",";
                                csv += img2Arr[i + 1] + ",";
                                csv += img2Arr[i + 2] + ",";
                            }
                            else
                            {
                                csv += ",,,";
                            }

                            if (i + 2 < img3Arr.Length)
                            {
                                csv += img3Arr[i + 0] + ",";
                                csv += img3Arr[i + 1] + ",";
                                csv += img3Arr[i + 2] + ",";
                            }
                            else
                            {
                                csv += ",,,";
                            }

                            if (i + 2 < img4Arr.Length)
                            {
                                csv += img4Arr[i + 0] + ",";
                                csv += img4Arr[i + 1] + ",";
                                csv += img4Arr[i + 2] + ",";
                            }
                            else
                            {
                                csv += ",,,";
                            }

                            if (i + 2 < img5Arr.Length)
                            {
                                csv += img5Arr[i + 0] + ",";
                                csv += img5Arr[i + 1] + ",";
                                csv += img5Arr[i + 2] + ",";
                            }
                            else
                            {
                                csv += ",,,";
                            }
                            csv += Environment.NewLine;

                            Progress += 3;
                        }

                        using (var sw = new StreamWriter(dialog.FileName))
                        {
                            sw.Write(csv);
                        }

                        IsRun = false;
                    }
                }));

                m_ProcThread.Start();



            }
            catch (Exception err)
            {
                MessageBox.Show("Image open err :" + err.Message);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (m_ProcThread != null && m_ProcThread.IsAlive)
            {
                m_ProcThread.Abort();
                m_ProcThread.Join(1000);
            }
        }
    }
}
