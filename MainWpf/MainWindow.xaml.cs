using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lime.Utils;
using System.Drawing;
using Microsoft.Win32;
using System.Drawing.Imaging;

namespace MainWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PrevQRButton.IsEnabled = false;
            NextQRButton.IsEnabled = false;
            SaveQRButton.IsEnabled = false;
        }

        #region Event
        private void WindowMouseLeftButtonOnDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ExitButtonOnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinSizeButtonOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ScanQRButtonOnClick(object sender, RoutedEventArgs e)
        {
            Scan();
        }

        private void PrevQRButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (Position <= 0)
            {
                PrevQRButton.IsEnabled = false;
                Position = 0;
            }
            else
            {
                Position--;
                Display();
                PrevQRButton.IsEnabled = Position != 0;
                NextQRButton.IsEnabled = true;
            }
        }

        private void NextQRButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (Result.Count - 1 <= Position)
            {
                NextQRButton.IsEnabled = false;
                Position = Result.Count - 1;
            }
            else
            {
                Position++;
                Display();
                NextQRButton.IsEnabled = Result.Count - 1 != Position;
                PrevQRButton.IsEnabled = true;
            }
        }

        private void SaveQRButtonOnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "PNG图片|*.png"
            };
            if (dialog.ShowDialog() == true)
            {
                Result[Position].QRImage.Save(dialog.FileName, ImageFormat.Png);
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("取消保存");
            }
        }

        #endregion

        #region Function
        private void Display()
        {
            QRImage.Source = Utils.Convert.BitmapToBitmapSource((Bitmap)Result[Position].QRImage);
            QRLabel.Text = Result[Position].Text;
        }

        private async void Scan()
        {
            Opacity = 0;
            var result = await Task.Factory.StartNew(() =>
            {
                var image = ScreenshotUtil.Screenshot();
                return QRDecode.MultipleDecoderCustom((Bitmap)image);
            });
            Opacity = 100;
            Result = result;
            Position = 0;
            if (result == null)
            {
                Infolabel.Content = "未扫描到任何二维码";
                PrevQRButton.IsEnabled = false;
                NextQRButton.IsEnabled = false;
                SaveQRButton.IsEnabled = false;
                return;
            }
            Infolabel.Content = "总共识别" + result.Count + "个二维码";
            PrevQRButton.IsEnabled = false;
            NextQRButton.IsEnabled = result.Count > 1;
            SaveQRButton.IsEnabled = true;
            QRImage.Source = Utils.Convert.BitmapToBitmapSource((Bitmap)result[0].QRImage);
            QRLabel.Text = result[0].Text;
        }
        #endregion

        #region Property

        private List<DecodeResult> Result = null;
        private int Position = 0;

        #endregion
    }
}
