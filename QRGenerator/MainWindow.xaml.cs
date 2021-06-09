using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QRGenerator
{
    public partial class MainWindow : Window
    {
        private const string _dealText = "Գործարքի համար՝ ";
        private const string _sumText = "Գումար՝ ";
        private double _sum = 0;
        private int _dealNumber = 0;
        private string _qrText = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateQR_Click(object sender, RoutedEventArgs e)
        {
            GetQRText(ref _qrText);
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(_qrText, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);
            Bitmap qrCodeImage = qRCode.GetGraphic(20);
            image.Source = BitmapToImage(qrCodeImage);
        }

        private void GetQRText(ref string qrText)
        {
            try
            {
                _sum = Convert.ToDouble(Sum.Text);
                _dealNumber = Convert.ToInt32(DealNumber.Text);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Մուտքագրված թիվը սխալ է");
                qrText = "Խնդրում ենք մուտքագրել համապատասխան արժեքները";
                return;
            }
            catch(Exception ex)
            {
                return;
            }

            qrText = _dealText + _dealNumber + ", " + _sumText + _sum;
        }

        private BitmapSource BitmapToImage(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Bmp);
                memoryStream.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void Sum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DealNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DragGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

    }
}
