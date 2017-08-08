// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.XmlDocumentUtils
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using System.Xml;
using System.Xml.Linq;

namespace Carvajal.FEPE.TemplateEngine.Support
{
  public static class XmlDocumentUtils
  {
    public static XDocument ToXDocument(this XmlDocument xmlDocument)
    {
      XDocument xdocument;
      using (XmlNodeReader xmlNodeReader = new XmlNodeReader((XmlNode) xmlDocument))
      {
        xdocument = XDocument.Load((XmlReader) xmlNodeReader);
        xmlNodeReader.Close();
      }
      return xdocument;
    }

    public static XmlDocument ToXmlDocument(this XDocument xDocument)
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (XmlReader reader = xDocument.CreateReader())
        xmlDocument.Load(reader);
      return xmlDocument;
    }
  }
}
