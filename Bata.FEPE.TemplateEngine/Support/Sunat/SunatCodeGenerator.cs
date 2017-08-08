// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Sunat.SunatCodeGenerator
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using Carvajal.FEPE.TemplateEngine.PDF417Lib;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Carvajal.FEPE.TemplateEngine.Support.Sunat
{
  public class SunatCodeGenerator
  {
    private const string FieldSeparator = "|";

    public string CompanyRuc { get; private set; }

    public string PaymentReceiptType { get; private set; }

    public string PaymentReceiptSerial { get; private set; }

    public string PaymentReceiptNumber { get; private set; }

    public string Igv { get; private set; }

    public string Total { get; private set; }

    public string IssueDate { get; private set; }

    public string ReceiverDocumentType { get; private set; }

    public string ReceiverDocumentNumber { get; private set; }

    public string DigestValue { get; private set; }

    public string SignatureValue { get; private set; }

    public SunatCodeGenerator(string companyRuc, string paymentReceiptType, string paymentReceiptSerial, string paymentReceiptNumber, string igv, string total, string issueDate, string receiverDocumentType, string receiverDocumentNumber, string digestValue, string signatureValue)
    {
      this.CompanyRuc = companyRuc;
      this.PaymentReceiptType = paymentReceiptType;
      this.PaymentReceiptSerial = paymentReceiptSerial;
      this.PaymentReceiptNumber = paymentReceiptNumber;
      this.Igv = igv;
      this.Total = total;
      this.IssueDate = issueDate;
      this.ReceiverDocumentType = receiverDocumentType;
      this.ReceiverDocumentNumber = receiverDocumentNumber;
      this.DigestValue = digestValue;
      this.SignatureValue = signatureValue;
    }

    public string GetBarCodeString()
    {
      return new StringBuilder().Append(this.CompanyRuc).Append("|").Append(this.PaymentReceiptType).Append("|").Append(this.PaymentReceiptSerial).Append("|").Append(this.PaymentReceiptNumber).Append("|").Append(this.Igv).Append("|").Append(this.Total).Append("|").Append(this.IssueDate).Append("|").Append(this.ReceiverDocumentType).Append("|").Append(this.ReceiverDocumentNumber).Append("|").Append(this.DigestValue).Append("|").Append(this.SignatureValue).Append("|").ToString();
    }

    public string GetQrCodeString()
    {
      return new StringBuilder().Append(this.CompanyRuc).Append("|").Append(this.PaymentReceiptType).Append("|").Append(this.PaymentReceiptSerial).Append("|").Append(this.PaymentReceiptNumber).Append("|").Append(this.Igv).Append("|").Append(this.Total).Append("|").Append(this.IssueDate).Append("|").Append(this.ReceiverDocumentType).Append("|").Append(this.ReceiverDocumentNumber).Append("|").ToString();
    }

    public string BarCodeToBase64(string text)
    {
      string empty = string.Empty;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        PDF417.GetPdf417(text).Save((Stream) memoryStream, ImageFormat.Png);
        return Convert.ToBase64String(memoryStream.ToArray());
      }
    }

    public string QrCodeToBase64(string text)
    {
      string empty = string.Empty;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        this.GenerateQrCode(text).Save((Stream) memoryStream, ImageFormat.Png);
        return Convert.ToBase64String(memoryStream.ToArray());
      }
    }

    private Image GenerateQrCode(string text)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new GraphicsRenderer((ISizeCalculation) new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White).WriteToStream(new QrEncoder(ErrorCorrectionLevel.Q).Encode(text).Matrix, ImageFormat.Png, (Stream) memoryStream);
        return (Image) new Bitmap((Stream) memoryStream);
      }
    }
  }
}
