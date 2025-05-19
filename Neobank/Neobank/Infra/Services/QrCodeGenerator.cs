using QRCoder;

namespace Neobank.Services;

public class QrCodeGenerator
{
    public static byte[] GenerateImage(string url)
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new BitmapByteQRCode(qrCodeData);
        var qrCodeImage = qrCode.GetGraphic(10);
        return qrCodeImage;
    }
    
}