using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Lime.Utils;
using System.Drawing;
using System.Threading;
using Microsoft.Win32;
using System.Drawing.Imaging;

namespace MainWpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<DecodeResult> _res = null;
        private int tag = 0;

        public MainWindow() {
            InitializeComponent();
            previousQRBtn.IsEnabled = false;
            nextQRBtn.IsEnabled = false;
            saveQRBtn.IsEnabled = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            this.DragMove();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void minSizeBtn_Click(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        private void scanQRBtn_Click(object sender, RoutedEventArgs e) {
            //this.WindowState = WindowState.Minimized;
            this.Opacity = 0;

            Task task = Task.Factory.StartNew(() => {
                var img = ScreenshotUtil.Screenshot;
                List<DecodeResult> res = QrDecode.MultipleDecoderCustom((Bitmap)img);
                updateDataInvoke(res);
            });
            //Debug Code
            //var img = ScreenshoUtil.Screenshot;
            //List<DecodeResult> res = QrDecode.MultipleDecoderCustom((Bitmap)img);
            //updateData(res);

        }

        /// <summary>
        /// 跨线程刷新控件
        /// </summary>
        /// <param name="res"></param>
        private void updateDataInvoke(List<DecodeResult> res) {
            if (Dispatcher.Thread != Thread.CurrentThread) {
                Action<List<DecodeResult>> actionDelegate = updateData;
                this.Dispatcher.Invoke(actionDelegate, res);
            }
            else
                updateData(res);
        }

        private void updateData(List<DecodeResult> res) {
            //this.WindowState = WindowState.Normal;
            this.Opacity = 100;
            _res = res;
            tag = 0;
            if (res == null) {
                infolabel.Content = "未扫描到任何二维码";
                previousQRBtn.IsEnabled = false;
                nextQRBtn.IsEnabled = false;
                saveQRBtn.IsEnabled = false;
                return;
            }
            infolabel.Content = "总共识别" + res.Count + "个二维码";
            if(res.Count <= 1) {
                previousQRBtn.IsEnabled = false;
                nextQRBtn.IsEnabled = false;
            }
            else {
                nextQRBtn.IsEnabled = true;
                previousQRBtn.IsEnabled = false;
            }
            saveQRBtn.IsEnabled = true;
            qrImage.Source = BitmapToBitmapSource((Bitmap)res[0].QrImage);
            qrLabel.Text = res[0].Text;
        }


        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        /// <summary>
        /// 把Bitmap数据转换为WPF所支持的数据格式
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <returns>BitmapImage</returns>
        private BitmapSource BitmapToBitmapSource(System.Drawing.Bitmap bitmap) {
            IntPtr ptr = bitmap.GetHbitmap();
            BitmapSource result =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            //release resource  
            DeleteObject(ptr);

            return result;
        }

        private void previousQRBtn_Click(object sender, RoutedEventArgs e) {
            if (tag <= 0) {
                previousQRBtn.IsEnabled = false;
                tag = 0;
            }
            else {
                tag--;
                display();
                if (tag == 0)
                    previousQRBtn.IsEnabled = false;
                else
                    previousQRBtn.IsEnabled = true;
                nextQRBtn.IsEnabled = true;
            }
        }

        private void nextQRBtn_Click(object sender, RoutedEventArgs e) {
            if (_res.Count - 1 <= tag) {
                nextQRBtn.IsEnabled = false;
                tag = _res.Count - 1;
            }
            else {
                tag++;
                display();
                if (_res.Count - 1 == tag)
                    nextQRBtn.IsEnabled = false;
                else
                    nextQRBtn.IsEnabled = true;
                previousQRBtn.IsEnabled = true;
            }
        }

        private void display() {
            qrImage.Source = BitmapToBitmapSource((Bitmap)_res[tag].QrImage);
            qrLabel.Text = _res[tag].Text;
        }

        private void saveQRBtn_Click(object sender, RoutedEventArgs e) {
            //创建一个保存文件式的对话框  
            SaveFileDialog sfd = new SaveFileDialog();
            //设置这个对话框的起始保存路径  
            //sfd.InitialDirectory = @"D:\";
            //设置保存的文件的类型，注意过滤器的语法  
            sfd.Filter = "PNG图片|*.png";
            //调用ShowDialog()方法显示该对话框，该方法的返回值代表用户是否点击了确定按钮  
            if (sfd.ShowDialog() == true) {
                _res[tag].QrImage.Save(sfd.FileName, ImageFormat.Png);
                MessageBox.Show("保存成功");
            }
            else {
                MessageBox.Show("取消保存");
            }
        }
    }
}
