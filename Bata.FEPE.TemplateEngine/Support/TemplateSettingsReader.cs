// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.TemplateSettingsReader
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using System.Xml;
using System.Xml.XPath;

namespace Bata.FEPE.TemplateEngine.Support
{
  public class TemplateSettingsReader
  {
    private const string DefaultAdjustmentType = "N";
    private const double DefaultScaleXFactor = 1.0;
    private const double DefaultScaleYFactor = 1.0;
    private const string DefaultPageSize = "A4";
    private readonly XmlNamespaceManager xmlNamespaceManager;

    public TemplateSettingsReader(XmlNamespaceManager xmlNamespaceManager)
    {
      this.xmlNamespaceManager = xmlNamespaceManager;
    }

    public string GetPageSize(XmlDocument templateXmlDocument)
    {
      string fieldValue = this.GetFieldValue(templateXmlDocument, "/xsl:stylesheet/xsl:variable[@name='tamanoPagina']");
      return !string.IsNullOrEmpty(fieldValue) ? fieldValue : "A4";
    }

    public double GetScaleXFactor(XmlDocument templateXmlDocument)
    {
      return this.GetFieldDoubleValue(templateXmlDocument, "/xsl:stylesheet/xsl:variable[@name='factorEscalaHorizontal']", 1.0);
    }

    public double GetScaleYFactor(XmlDocument templateXmlDocument)
    {
      return this.GetFieldDoubleValue(templateXmlDocument, "/xsl:stylesheet/xsl:variable[@name='factorEscalaVertical']", 1.0);
    }

    public string GetAdjustmentType(XmlDocument templateXmlDocument)
    {
      string fieldValue = this.GetFieldValue(templateXmlDocument, "/xsl:stylesheet/xsl:variable[@name='tipoAjuste']");
      return !string.IsNullOrEmpty(fieldValue) ? fieldValue : "N";
    }

    public double GetFieldDoubleValue(XmlDocument xmlDocument, string xpath, double defaultValue)
    {
      double result;
      return double.TryParse(this.GetFieldValue(xmlDocument, xpath), out result) ? result : defaultValue;
    }

    private string GetFieldValue(XmlDocument xmlDocument, string xpath)
    {
      try
      {
        XmlNode xmlNode = xmlDocument.SelectSingleNode(xpath, this.xmlNamespaceManager);
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
