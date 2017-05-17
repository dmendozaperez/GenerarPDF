// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Services.CompiledTemplate
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using Bata.FEPE.TemplateEngine.ABCpdf;
using Bata.FEPE.TemplateEngine.Support;
using Bata.FEPE.TemplateEngine.Support.Preprocessor;
using Bata.FEPE.TemplateEngine.Support.Sunat;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Bata.FEPE.TemplateEngine.Services
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
      SunatBarcode sunatBarcode = new SunatBarcodeFactory(this.settings.PaymentReceiptType, this.xmlNamespaceManager).Build(paymentReceiptXmlDocument);
      XsltArgumentList arguments = new XsltArgumentList();
      arguments.AddParam("codigoBarras", string.Empty, (object) sunatBarcode.ToBase64());
      arguments.AddParam("hash", string.Empty, (object) sunatBarcode.DigestValue);
      StringBuilder output = new StringBuilder();
      XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
      int num1 = 0;
      xmlWriterSettings.ConformanceLevel = (ConformanceLevel) num1;
      int num2 = 1;
      xmlWriterSettings.Indent = num2 != 0;
      Encoding encoding = CompiledTemplate.OutputEncoding;
      xmlWriterSettings.Encoding = encoding;
      int num3 = 1;
      xmlWriterSettings.CloseOutput = num3 != 0;
      XmlWriterSettings settings = xmlWriterSettings;
      string str;
      using (XmlWriter results = XmlWriter.Create(output, settings))
      {
        this.preprocessor.Preprocess(paymentReceiptXmlDocument);
        this.xslCompiledTransform.Transform((IXPathNavigable) paymentReceiptXmlDocument, arguments, results);
        str = output.ToString();
      }
      return str;
    }

    public byte[] BuildPdfDocument(string htmlContent)
    {
      return PdfDocumentBuilder.BuildPdfDocument(htmlContent, this.settings.PageSize, this.settings.ScaleXFactor, this.settings.ScaleYFactor);
    }
  }
}
