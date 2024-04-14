using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;

namespace MerchantService.QR
{
    public class QRService
    {
        public bool GenerateQRCode(string content, string productInfo)
        {
            try
            {
                BarcodeWriter barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 200,
                        Width = 200
                    }
                };

                Bitmap qrCodeImage = barcodeWriter.Write(content);
                SaveQRImage(qrCodeImage, content, productInfo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SaveQRImage(Bitmap qrImage, string imageId, string productInfo)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            string directoryPath = @"D:\ProductQR\" + currentDate + @"\";
            Directory.CreateDirectory(directoryPath);
            string imagePath = Path.Combine(directoryPath, imageId.ToString() + ".png");
            qrImage.Save(imagePath, ImageFormat.Png);

            string textFilePath = Path.Combine(directoryPath, "QRInfoReadMe.txt");
            File.WriteAllText(textFilePath, productInfo);

        }

    }

}
