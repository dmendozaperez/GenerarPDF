// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Mapper.GenericDocumentMapper
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using Carvajal.FEPE.TemplateEngine.Support;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Carvajal.FEPE.TemplateEngine.Mapper
{
  public class GenericDocumentMapper : IDocumentMapper
  {
    public XmlDocument Transform(XmlDocument inputXmlDocument)
    {
      XDocument xdocument = inputXmlDocument.ToXDocument();
      this.ProcessCarvajalExtension(xdocument);
      return xdocument.ToXmlDocument();
    }

    private void ProcessCarvajalExtension(XDocument inputXDocument)
    {
      IEnumerable<XElement> source = inputXDocument.Root.Elements(GenericDocumentMapper.XmlConstants.UblExtensions).Elements<XElement>(GenericDocumentMapper.XmlConstants.UblExtension).Elements<XElement>(GenericDocumentMapper.XmlConstants.UblExtensionContent).Elements<XElement>(GenericDocumentMapper.XmlConstants.CtsAdditionalDocumentInformation).Elements<XElement>(GenericDocumentMapper.XmlConstants.CtsHeader).Elements<XElement>(GenericDocumentMapper.XmlConstants.CtsAdditionalProperty);
      if (!source.Any<XElement>())
        return;
      List<XElement> newSacAdditionalPropertyElements = new List<XElement>();
      foreach (XElement xelement1 in source)
      {
        XName ctsId = GenericDocumentMapper.XmlConstants.CtsId;
        string str1 = xelement1.Elements(ctsId).FirstOrDefault<XElement>().Value;
        XName ctsValue = GenericDocumentMapper.XmlConstants.CtsValue;
        string str2 = xelement1.Elements(ctsValue).FirstOrDefault<XElement>().Value;
        XElement xelement2 = new XElement(GenericDocumentMapper.XmlConstants.SacAdditionalProperty, new object[2]
        {
          (object) new XElement(GenericDocumentMapper.XmlConstants.CbcId, (object) str1),
          (object) new XElement(GenericDocumentMapper.XmlConstants.CbcValue, (object) str2)
        });
        newSacAdditionalPropertyElements.Add(xelement2);
      }
      this.AppendAfterLastAdditionalPropertyElement(inputXDocument.Root.Elements(GenericDocumentMapper.XmlConstants.UblExtensions).Elements<XElement>(GenericDocumentMapper.XmlConstants.UblExtension).Elements<XElement>(GenericDocumentMapper.XmlConstants.UblExtensionContent).Elements<XElement>(GenericDocumentMapper.XmlConstants.SacAdditionalInformation).FirstOrDefault<XElement>(), newSacAdditionalPropertyElements);
    }

    private void AppendAfterLastAdditionalPropertyElement(XElement sacAdditionalInformationElement, List<XElement> newSacAdditionalPropertyElements)
    {
      IEnumerable<XElement> source = sacAdditionalInformationElement.Elements(GenericDocumentMapper.XmlConstants.SacAdditionalProperty);
      if (source.Any<XElement>())
        source.LastOrDefault<XElement>().AddAfterSelf((object) newSacAdditionalPropertyElements);
      else
        this.AppendAfterLastAdditionalMonetaryTotalElement(sacAdditionalInformationElement, newSacAdditionalPropertyElements);
    }

    private void AppendAfterLastAdditionalMonetaryTotalElement(XElement sacAdditionalInformationElement, List<XElement> newSacAdditionalPropertyElements)
    {
      IEnumerable<XElement> source = sacAdditionalInformationElement.Elements(GenericDocumentMapper.XmlConstants.SacAdditionalMonetaryTotal);
      if (source.Any<XElement>())
        source.LastOrDefault<XElement>().AddAfterSelf((object) newSacAdditionalPropertyElements);
      else
        this.AppendAsFirstAdditionalInformationChildElement(sacAdditionalInformationElement, newSacAdditionalPropertyElements);
    }

    private void AppendAsFirstAdditionalInformationChildElement(XElement sacAdditionalInformationElement, List<XElement> newSacAdditionalPropertyElements)
    {
      sacAdditionalInformationElement.AddFirst((object) newSacAdditionalPropertyElements);
    }

    private static class XmlConstants
    {
      public static readonly XNamespace Ext = (XNamespace) "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2";
      public static readonly XNamespace Cts = (XNamespace) "urn:carvajal:names:specification:ubl:peru:schema:xsd:CarvajalAggregateComponents-1";
      public static readonly XNamespace Sac = (XNamespace) "urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1";
      public static readonly XNamespace Cbc = (XNamespace) "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
      public static readonly XName UblExtensions = GenericDocumentMapper.XmlConstants.Ext + "UBLExtensions";
      public static readonly XName UblExtension = GenericDocumentMapper.XmlConstants.Ext + "UBLExtension";
      public static readonly XName UblExtensionContent = GenericDocumentMapper.XmlConstants.Ext + "ExtensionContent";
      public static readonly XName CtsAdditionalDocumentInformation = GenericDocumentMapper.XmlConstants.Cts + "AdditionalDocumentInformation";
      public static readonly XName CtsHeader = GenericDocumentMapper.XmlConstants.Cts + "Header";
      public static readonly XName CtsAdditionalProperty = GenericDocumentMapper.XmlConstants.Cts + "AdditionalProperty";
      public static readonly XName CtsId = GenericDocumentMapper.XmlConstants.Cts + "ID";
      public static readonly XName CtsValue = GenericDocumentMapper.XmlConstants.Cts + "Value";
      public static readonly XName SacAdditionalInformation = GenericDocumentMapper.XmlConstants.Sac + "AdditionalInformation";
      public static readonly XName SacAdditionalMonetaryTotal = GenericDocumentMapper.XmlConstants.Sac + "AdditionalMonetaryTotal";
      public static readonly XName SacAdditionalProperty = GenericDocumentMapper.XmlConstants.Sac + "AdditionalProperty";
      public static readonly XName CbcId = GenericDocumentMapper.XmlConstants.Cbc + "ID";
      public static readonly XName CbcValue = GenericDocumentMapper.XmlConstants.Cbc + "Value";
    }
  }
}
