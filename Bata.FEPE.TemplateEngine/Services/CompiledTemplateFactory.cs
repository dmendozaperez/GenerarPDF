// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Services.CompiledTemplateFactory
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using Carvajal.FEPE.TemplateEngine.Support;
using Carvajal.FEPE.TemplateEngine.Support.Preprocessor;
using Common.Entities.UBL;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace Carvajal.FEPE.TemplateEngine.Services
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
        XmlReader xmlReader = XmlReader.Create((Stream) fileStream);
        TemplateXmlUrlResolver templateXmlUrlResolver1 = new TemplateXmlUrlResolver(templateFilePath);
        XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
        XslCompiledTransform compiledTransform = xslCompiledTransform;
        XmlReader stylesheet = xmlReader;
        XsltSettings settings2 = new XsltSettings();
        settings2.EnableScript = true;
        settings2.EnableDocumentFunction = true;
        TemplateXmlUrlResolver templateXmlUrlResolver2 = templateXmlUrlResolver1;
        compiledTransform.Load(stylesheet, settings2, (XmlResolver) templateXmlUrlResolver2);
        xmlReader.Close();
        return new CompiledTemplate(settings1, namespaceManager1, preprocessor, xslCompiledTransform);

      }
    }
  }
}
