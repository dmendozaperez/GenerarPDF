// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Services.CompiledTemplateFactory
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using Bata.FEPE.TemplateEngine.Support;
using Bata.FEPE.TemplateEngine.Support.Preprocessor;
using Common.Entities.UBL;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace Bata.FEPE.TemplateEngine.Services
{
  public static class CompiledTemplateFactory
  {
    public static CompiledTemplate Build(string templateFilePath, string paymentReceiptType)
    {
      using (FileStream fileStream = new FileStream(templateFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        XmlDocument templateXmlDocument = new XmlDocument();
        templateXmlDocument.Load((Stream) fileStream);
        XmlNamespaceManager namespaceManager1 = XmlNamespaceManagerFactory.ForPaymentReceiptFile(paymentReceiptType, templateXmlDocument.NameTable);
        XmlNamespaceManager namespaceManager2 = XmlNamespaceManagerFactory.ForTemplateFile(templateXmlDocument.NameTable);
        TemplateSettingsReader templateSettingsReader = new TemplateSettingsReader(namespaceManager2);
        TemplateSettings settings1 = new TemplateSettings(paymentReceiptType, templateSettingsReader.GetPageSize(templateXmlDocument), templateSettingsReader.GetScaleXFactor(templateXmlDocument), templateSettingsReader.GetScaleYFactor(templateXmlDocument), templateSettingsReader.GetAdjustmentType(templateXmlDocument));
        IPaymentReceiptPreprocessor preprocessor = new PaymentReceiptPreprocessorFactory(paymentReceiptType, namespaceManager2).Build(templateXmlDocument, namespaceManager1, settings1.AdjustmentType);
        fileStream.Position = 0L;
        XmlReader xmlReader1 = XmlReader.Create((Stream) fileStream);
        TemplateXmlUrlResolver templateXmlUrlResolver = new TemplateXmlUrlResolver(templateFilePath);
        XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
        XslCompiledTransform compiledTransform = xslCompiledTransform;
        XmlReader xmlReader2 = xmlReader1;
        XsltSettings xsltSettings1 = new XsltSettings();
        int num1 = 1;
        xsltSettings1.EnableScript = num1 != 0;
        int num2 = 1;
        xsltSettings1.EnableDocumentFunction = num2 != 0;
        XsltSettings xsltSettings2 = xsltSettings1;
        XmlResolver xmlResolver = (XmlResolver) templateXmlUrlResolver;
        XmlReader stylesheet = xmlReader2;
        XsltSettings settings2 = xsltSettings2;
        XmlResolver stylesheetResolver = xmlResolver;
        compiledTransform.Load(stylesheet, settings2, stylesheetResolver);
        xmlReader1.Close();
        return new CompiledTemplate(settings1, namespaceManager1, preprocessor, xslCompiledTransform);
      }
    }
  }
}
