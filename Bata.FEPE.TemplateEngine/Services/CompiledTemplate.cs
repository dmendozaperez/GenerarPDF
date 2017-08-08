// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Services.CompiledTemplate
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using Carvajal.FEPE.TemplateEngine.ABCpdf;
using Carvajal.FEPE.TemplateEngine.Support;
using Carvajal.FEPE.TemplateEngine.Support.Preprocessor;
using Carvajal.FEPE.TemplateEngine.Support.Sunat;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Carvajal.FEPE.TemplateEngine.Services
{
  public class CompiledTemplate
  {
    private static readonly Encoding OutputEncoding = Encoding.UTF8;
    private readonly XmlNamespaceManager xmlNamespaceManager;
    private readonly IPaymentReceiptPreprocessor preprocessor;
    private readonly TemplateSettings settings;
    private readonly XslCompiledTransform xslCompiledTransform;

    public CompiledTemplate(TemplateSettings settings, XmlNamespaceManager xmlNamespaceManager, IPaymentReceiptPreprocessor preprocessor, XslCompiledTransform xslCompiledTransform)
    {
      this.settings = settings;
      this.xmlNamespaceManager = xmlNamespaceManager;
      this.preprocessor = preprocessor;
      this.xslCompiledTransform = xslCompiledTransform;
    }

    public string BuildHtmlContent(XmlDocument paymentReceiptXmlDocument)
    {
      SunatCodeGenerator sunatCodeGenerator = new SunatBarcodeFactory(this.settings.PaymentReceiptType, this.xmlNamespaceManager).Build(paymentReceiptXmlDocument);
      XsltArgumentList arguments = new XsltArgumentList();
      arguments.AddParam("codigoBarras", string.Empty, (object) sunatCodeGenerator.BarCodeToBase64(sunatCodeGenerator.GetBarCodeString()));
      arguments.AddParam("codigoQr", string.Empty, (object) sunatCodeGenerator.QrCodeToBase64(sunatCodeGenerator.GetQrCodeString()));
      arguments.AddParam("hash", string.Empty, (object) sunatCodeGenerator.DigestValue);
      StringBuilder output = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings()
      {
        ConformanceLevel = ConformanceLevel.Document,
        Indent = true,
        Encoding = CompiledTemplate.OutputEncoding,
        CloseOutput = true
      };
      using (XmlWriter results = XmlWriter.Create(output, settings))
      {
        this.preprocessor.Preprocess(paymentReceiptXmlDocument);
        this.xslCompiledTransform.Transform((IXPathNavigable) paymentReceiptXmlDocument, arguments, results);

                string HTML= output.ToString();
                return output.ToString();
      }
    }

    public byte[] BuildPdfDocument(string htmlContent)
    {
      return PdfDocumentBuilder.BuildPdfDocument(htmlContent, this.settings.PageSize, this.settings.ScaleXFactor, this.settings.ScaleYFactor);
    }
  }
}
