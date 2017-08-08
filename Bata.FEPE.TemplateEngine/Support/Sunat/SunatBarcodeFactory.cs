// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Sunat.SunatBarcodeFactory
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using System;
using System.Xml;

namespace Carvajal.FEPE.TemplateEngine.Support.Sunat
{
  public class SunatBarcodeFactory
  {
    private readonly XmlNamespaceManager xmlNamespaceManager;
    private const char SerialNumberSeparator = '-';

    public string PaymentReceiptType { get; private set; }

    public SunatBarcodeFieldsXPaths.Value BarcodeXPaths { get; private set; }

    public SunatBarcodeFactory(string paymentReceiptType, XmlNamespaceManager xmlNamespaceManager)
    {
      this.PaymentReceiptType = paymentReceiptType;
      this.xmlNamespaceManager = xmlNamespaceManager;
      this.SetBarcodeXPaths();
    }

    private void SetBarcodeXPaths()
    {
      string paymentReceiptType = this.PaymentReceiptType;
      if (!(paymentReceiptType == "01") && !(paymentReceiptType == "03"))
      {
        if (!(paymentReceiptType == "07"))
        {
          if (paymentReceiptType == "08")
            this.BarcodeXPaths = SunatBarcodeFieldsXPaths.DebitNote;
          else
            this.BarcodeXPaths = SunatBarcodeFieldsXPaths.InvoiceOrBallot;
        }
        else
          this.BarcodeXPaths = SunatBarcodeFieldsXPaths.CreditNote;
      }
      else
        this.BarcodeXPaths = SunatBarcodeFieldsXPaths.InvoiceOrBallot;
    }

    public SunatCodeGenerator Build(XmlDocument paymentReceiptXmlDocument)
    {
      string digestValue = this.GetDigestValue(paymentReceiptXmlDocument);
      string signatureValue = this.GetSignatureValue(paymentReceiptXmlDocument);
      return this.CreateSunatBarcode(paymentReceiptXmlDocument, digestValue, signatureValue);
    }

    private string GetDigestValue(XmlDocument paymentReceiptXmlDocument)
    {
      return this.GetFieldValue(paymentReceiptXmlDocument, "//ds:DigestValue");
    }

    private string GetSignatureValue(XmlDocument paymentReceiptXmlDocument)
    {
      return this.GetFieldValue(paymentReceiptXmlDocument, "//ds:SignatureValue");
    }

    private SunatCodeGenerator CreateSunatBarcode(XmlDocument paymentReceiptXmlDocument, string digestValue, string signatureValue)
    {
      string[] serialNumber = SunatBarcodeFactory.ParseSerialNumber(this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.PaymentReceiptSerialNumber));
      return new SunatCodeGenerator(this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.CompanyRuc), this.PaymentReceiptType, serialNumber.Length >= 1 ? serialNumber[0] : string.Empty, serialNumber.Length >= 2 ? serialNumber[1] : string.Empty, this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.Igv), this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.Total), this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.IssueDate), this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.ReceiverDocumentType), this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.ReceiverDocumentNumber), digestValue, signatureValue);
    }

    private static string[] ParseSerialNumber(string serialNumber)
    {
      return serialNumber.Split(new char[1]{ '-' }, StringSplitOptions.RemoveEmptyEntries);
    }

    private string GetFieldValue(XmlDocument xmlDocument, string xpath)
    {
      XmlNode xmlNode = xmlDocument.SelectSingleNode(xpath, this.xmlNamespaceManager);
      if (xmlNode != null && xmlNode.ChildNodes.Count > 0)
        return xmlNode.FirstChild.Value;
      return string.Empty;
    }
  }
}
