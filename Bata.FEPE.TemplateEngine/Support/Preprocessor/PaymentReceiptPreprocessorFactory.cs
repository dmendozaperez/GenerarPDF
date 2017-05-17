// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Preprocessor.PaymentReceiptPreprocessorFactory
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using System.Xml;
using System.Xml.XPath;

namespace Bata.FEPE.TemplateEngine.Support.Preprocessor
{
  public class PaymentReceiptPreprocessorFactory
  {
    private const int DefaultDetailLineWidth = 250;
    private readonly XmlNamespaceManager templateXmlNamespaceManager;

    public string PaymentReceiptType { get; private set; }

    public PaymentReceiptAdjustmentXPaths.Value AdjustmentXPaths { get; private set; }

    public PaymentReceiptPreprocessorFactory(string paymentReceiptType, XmlNamespaceManager templateXmlNamespaceManager)
    {
      this.PaymentReceiptType = paymentReceiptType;
      this.templateXmlNamespaceManager = templateXmlNamespaceManager;
      this.SetAdjustmentXPaths();
    }

    private void SetAdjustmentXPaths()
    {
      string paymentReceiptType = this.PaymentReceiptType;
      if (!(paymentReceiptType == "01") && !(paymentReceiptType == "03"))
      {
        if (!(paymentReceiptType == "07"))
        {
          if (paymentReceiptType == "08")
            this.AdjustmentXPaths = PaymentReceiptAdjustmentXPaths.DebitNote;
          else
            this.AdjustmentXPaths = PaymentReceiptAdjustmentXPaths.InvoiceOrBallot;
        }
        else
          this.AdjustmentXPaths = PaymentReceiptAdjustmentXPaths.CreditNote;
      }
      else
        this.AdjustmentXPaths = PaymentReceiptAdjustmentXPaths.InvoiceOrBallot;
    }

    public IPaymentReceiptPreprocessor Build(XmlDocument templateXmlDocument, XmlNamespaceManager paymentReceiptXmlNamespaceManager, string adjustmentType)
    {
      string str = adjustmentType;
      if (str == "N")
        return (IPaymentReceiptPreprocessor) NoPreprocessing.Instance;
      if (str == "T")
        return (IPaymentReceiptPreprocessor) new DescriptionTextPreprocessor(this.GetDetailLineWidth(templateXmlDocument), this.AdjustmentXPaths, paymentReceiptXmlNamespaceManager);
      if (str == "W")
        return (IPaymentReceiptPreprocessor) new DescriptionWordsPreprocessor(this.GetDetailLineWidth(templateXmlDocument), this.AdjustmentXPaths, paymentReceiptXmlNamespaceManager);
      return (IPaymentReceiptPreprocessor) NoPreprocessing.Instance;
    }

    private int GetDetailLineWidth(XmlDocument templateXmlDocument)
    {
      int result;
      return int.TryParse(this.GetFieldValue(templateXmlDocument, "/xsl:stylesheet/xsl:variable[@name='anchoLinea']"), out result) ? result : 250;
    }

    private string GetFieldValue(XmlDocument xmlDocument, string xpath)
    {
      try
      {
        XmlNode xmlNode = xmlDocument.SelectSingleNode(xpath, this.templateXmlNamespaceManager);
        if (xmlNode != null && xmlNode.ChildNodes.Count > 0)
          return xmlNode.FirstChild.Value;
        return string.Empty;
      }
      catch (XPathException ex)
      {
        return string.Empty;
      }
    }
  }
}
