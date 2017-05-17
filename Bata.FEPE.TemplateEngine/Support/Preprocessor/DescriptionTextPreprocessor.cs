// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Preprocessor.DescriptionTextPreprocessor
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using System;
using System.Collections.Generic;
using System.Xml;

namespace Bata.FEPE.TemplateEngine.Support.Preprocessor
{
  public class DescriptionTextPreprocessor : IPaymentReceiptPreprocessor
  {
    private const string FirstDescriptionElementMarker = "esPrimero";
    private const string LastDescriptionElementMarker = "esUltimo";
    private readonly XmlNamespaceManager xmlNamespaceManager;

    public int DetailLineWidth { get; private set; }

    public PaymentReceiptAdjustmentXPaths.Value AdjustmentXPaths { get; private set; }

    public DescriptionTextPreprocessor(int detailLineWidth, PaymentReceiptAdjustmentXPaths.Value adjustmentXPaths, XmlNamespaceManager xmlNamespaceManager)
    {
      this.DetailLineWidth = detailLineWidth;
      this.AdjustmentXPaths = adjustmentXPaths;
      this.xmlNamespaceManager = xmlNamespaceManager;
    }

    public void Preprocess(XmlDocument paymentReceiptXmlDocument)
    {
      foreach (XmlNode descriptionXmlNode in paymentReceiptXmlDocument.SelectNodes(this.AdjustmentXPaths.Description, this.xmlNamespaceManager))
        this.TryAdjustDescriptionXmlNode(paymentReceiptXmlDocument, descriptionXmlNode);
    }

    private void TryAdjustDescriptionXmlNode(XmlDocument paymentReceiptXmlDocument, XmlNode descriptionXmlNode)
    {
      IDictionary<int, string> dictionary = this.AdjustText(descriptionXmlNode.FirstChild != null ? descriptionXmlNode.FirstChild.Value : descriptionXmlNode.InnerText);
      XmlNode refChild = descriptionXmlNode;
      foreach (KeyValuePair<int, string> keyValuePair in (IEnumerable<KeyValuePair<int, string>>) dictionary)
      {
        if (keyValuePair.Key == 1)
        {
          if (descriptionXmlNode.FirstChild != null)
            descriptionXmlNode.FirstChild.Value = keyValuePair.Value;
          else
            descriptionXmlNode.InnerText = keyValuePair.Value;
          descriptionXmlNode.Attributes.Append(paymentReceiptXmlDocument.CreateAttribute("esPrimero"));
          if (keyValuePair.Key == dictionary.Count)
          {
            XmlAttribute attribute = paymentReceiptXmlDocument.CreateAttribute("esUltimo");
            descriptionXmlNode.Attributes.Append(attribute);
          }
          refChild = descriptionXmlNode;
        }
        else
        {
          XmlNode newChild = refChild.CloneNode(true);
          if (newChild.Attributes.Count > 0)
            newChild.Attributes.RemoveAll();
          if (newChild.FirstChild != null)
            newChild.FirstChild.Value = keyValuePair.Value;
          else
            newChild.InnerText = keyValuePair.Value;
          if (keyValuePair.Key == dictionary.Count)
            newChild.Attributes.Append(paymentReceiptXmlDocument.CreateAttribute("esUltimo"));
          refChild.ParentNode.InsertAfter(newChild, refChild);
          refChild = newChild;
        }
      }
    }

    private IDictionary<int, string> AdjustText(string description)
    {
      Dictionary<int, string> dictionary1 = new Dictionary<int, string>();
      int num1 = (int) Math.Ceiling((double) description.Length / (double) this.DetailLineWidth);
      for (int index = 0; index < num1; ++index)
      {
        int startIndex = index * this.DetailLineWidth;
        int length = description.Length - startIndex;
        string str1 = length <= this.DetailLineWidth ? description.Substring(startIndex, length) : description.Substring(startIndex, this.DetailLineWidth);
        Dictionary<int, string> dictionary2 = dictionary1;
        int num2 = index + 1;
        string str2 = str1;
        int key = num2;
        string str3 = str2;
        dictionary2.Add(key, str3);
      }
      return (IDictionary<int, string>) dictionary1;
    }
  }
}
