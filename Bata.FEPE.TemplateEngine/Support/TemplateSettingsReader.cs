// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.TemplateSettingsReader
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using System.Xml;
using System.Xml.XPath;

namespace Carvajal.FEPE.TemplateEngine.Support
{
  public class TemplateSettingsReader
  {
    private readonly XmlNamespaceManager xmlNamespaceManager;
    private const string DefaultAdjustmentType = "N";
    private const double DefaultScaleXFactor = 1.0;
    private const double DefaultScaleYFactor = 1.0;
    private const string DefaultPageSize = "A4";

    public TemplateSettingsReader(XmlNamespaceManager xmlNamespaceManager)
    {
      this.xmlNamespaceManager = xmlNamespaceManager;
    }

    public string GetPageSize(XmlDocument templateXmlDocument)
    {
      string fieldValue = this.GetFieldValue(templateXmlDocument, "/xsl:stylesheet/xsl:variable[@name='tamanoPagina']");
      if (string.IsNullOrEmpty(fieldValue))
        return "A4";
      return fieldValue;
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
      if (string.IsNullOrEmpty(fieldValue))
        return "N";
      return fieldValue;
    }

    public double GetFieldDoubleValue(XmlDocument xmlDocument, string xpath, double defaultValue)
    {
      double result;
      if (!double.TryParse(this.GetFieldValue(xmlDocument, xpath), out result))
        return defaultValue;
      return result;
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
