// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Preprocessor.PaymentReceiptPreprocessorFactory
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using System.Xml;
using System.Xml.XPath;

namespace Carvajal.FEPE.TemplateEngine.Support.Preprocessor
{
  public class PaymentReceiptPreprocessorFactory
  {
    private readonly XmlNamespaceManager templateXmlNamespaceManager;
    private const int DefaultDetailLineWidth = 250;

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
      if (adjustmentType == "N")
        return (IPaymentReceiptPreprocessor) NoPreprocessing.Instance;
      if (adjustmentType == "T")
        return (IPaymentReceiptPreprocessor) new DescriptionTextPreprocessor(this.GetDetailLineWidth(templateXmlDocument), this.AdjustmentXPaths, paymentReceiptXmlNamespaceManager);
      if (adjustmentType == "W")
        return (IPaymentReceiptPreprocessor) new DescriptionWordsPreprocessor(this.GetDetailLineWidth(templateXmlDocument), this.AdjustmentXPaths, paymentReceiptXmlNamespaceManager);
      return (IPaymentReceiptPreprocessor) NoPreprocessing.Instance;
    }

    private int GetDetailLineWidth(XmlDocument templateXmlDocument)
    {
      int result;
      if (!int.TryParse(this.GetFieldValue(templateXmlDocument, "/xsl:stylesheet/xsl:variable[@name='anchoLinea']"), out result))
        return 250;
      return result;
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
