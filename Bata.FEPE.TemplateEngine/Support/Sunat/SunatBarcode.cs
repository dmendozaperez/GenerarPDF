// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Sunat.SunatBarcode
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using Bata.FEPE.TemplateEngine.PDF417Lib;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Bata.FEPE.TemplateEngine.Support.Sunat
{
  public class SunatBarcode
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

    public SunatBarcode(string companyRuc, string paymentReceiptType, string paymentReceiptSerial, string paymentReceiptNumber, string igv, string total, string issueDate, string receiverDocumentType, string receiverDocumentNumber, string digestValue, string signatureValue)
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

    public override string ToString()
    {
      return new StringBuilder().Append(this.CompanyRuc).Append("|").Append(this.PaymentReceiptType).Append("|").Append(this.PaymentReceiptSerial).Append("|").Append(this.PaymentReceiptNumber).Append("|").Append(this.Igv).Append("|").Append(this.Total).Append("|").Append(this.IssueDate).Append("|").Append(this.ReceiverDocumentType).Append("|").Append(this.ReceiverDocumentNumber).Append("|").Append(this.DigestValue).Append("|").Append(this.SignatureValue).Append("|").ToString();
    }

    public string ToBase64()
    {
      string str = string.Empty;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        PDF417.GetPdf417(this.ToString()).Save((Stream) memoryStream, ImageFormat.Png);
        str = Convert.ToBase64String(memoryStream.ToArray());
      }
      return str;
    }
  }
}
