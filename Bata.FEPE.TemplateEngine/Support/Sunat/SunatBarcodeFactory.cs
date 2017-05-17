// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Sunat.SunatBarcodeFactory
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using System;
using System.Xml;

namespace Bata.FEPE.TemplateEngine.Support.Sunat
{
  public class SunatBarcodeFactory
  {
    private const char SerialNumberSeparator = '-';
    private readonly XmlNamespaceManager xmlNamespaceManager;

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

    public SunatBarcode Build(XmlDocument paymentReceiptXmlDocument)
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

    private SunatBarcode CreateSunatBarcode(XmlDocument paymentReceiptXmlDocument, string digestValue, string signatureValue)
    {
      string[] strArray = SunatBarcodeFactory.ParseSerialNumber(this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.PaymentReceiptSerialNumber));
      return new SunatBarcode(this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.CompanyRuc), this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.PaymentReceiptType), strArray.Length >= 1 ? strArray[0] : string.Empty, strArray.Length >= 2 ? strArray[1] : string.Empty, this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.Igv), this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.Total), this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.IssueDate), this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.ReceiverDocumentType), this.GetFieldValue(paymentReceiptXmlDocument, this.BarcodeXPaths.ReceiverDocumentNumber), digestValue, signatureValue);
    }

    private static string[] ParseSerialNumber(string serialNumber)
    {
      string str = serialNumber;
      char[] separator = new char[1];
      int index = 0;
      int num1 = 45;
      separator[index] = (char) num1;
      int num2 = 1;
      return str.Split(separator, (StringSplitOptions) num2);
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
