using System.Drawing;
using ZXing;
using ZXing.Common;

namespace MerchantService.QR
{
    public class QRService
    {
        public Bitmap GenerateQRCode(string content)
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
            return qrCodeImage;
        }
    }

}
