// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Preprocessor.DescriptionWordsPreprocessor
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Carvajal.FEPE.TemplateEngine.Support.Preprocessor
{
  public class DescriptionWordsPreprocessor : IPaymentReceiptPreprocessor
  {
    private readonly XmlNamespaceManager xmlNamespaceManager;
    private const string FirstDescriptionElementMarker = "esPrimero";
    private const string LastDescriptionElementMarker = "esUltimo";
    private const char WordsSeparator = ' ';

    public int DetailLineWidth { get; private set; }

    public PaymentReceiptAdjustmentXPaths.Value AdjustmentXPaths { get; private set; }

    public DescriptionWordsPreprocessor(int detailLineWidth, PaymentReceiptAdjustmentXPaths.Value adjustmentXPaths, XmlNamespaceManager xmlNamespaceManager)
    {
      this.DetailLineWidth = detailLineWidth;
      this.AdjustmentXPaths = adjustmentXPaths;
      this.xmlNamespaceManager = xmlNamespaceManager;
    }

    public void Preprocess(XmlDocument paymentReceiptXmlDocument)
    {
      foreach (XmlNode selectNode in paymentReceiptXmlDocument.SelectNodes(this.AdjustmentXPaths.Description, this.xmlNamespaceManager))
        this.TryAdjustDescriptionXmlNode(paymentReceiptXmlDocument, selectNode);
    }

    private void TryAdjustDescriptionXmlNode(XmlDocument paymentReceiptXmlDocument, XmlNode descriptionXmlNode)
    {
      Dictionary<int, string> dictionary = this.AdjustWords(descriptionXmlNode.FirstChild != null ? descriptionXmlNode.FirstChild.Value : descriptionXmlNode.InnerText);
      XmlNode refChild = descriptionXmlNode;
      foreach (KeyValuePair<int, string> keyValuePair in dictionary)
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
          if (keyValuePair.Key == dictionary.Count && newChild.Attributes != null)
            newChild.Attributes.Append(paymentReceiptXmlDocument.CreateAttribute("esUltimo"));
          refChild.ParentNode.InsertAfter(newChild, refChild);
          refChild = newChild;
        }
      }
    }

    private Dictionary<int, string> AdjustWords(string line)
    {
      string[] strArray = line.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
      Dictionary<int, string> dictionary = new Dictionary<int, string>();
      StringBuilder stringBuilder = new StringBuilder();
      int key = 1;
      foreach (string str in strArray)
      {
        if (stringBuilder.Length + str.Length < this.DetailLineWidth)
        {
          if (stringBuilder.Length == 0)
            stringBuilder.Append(str);
          else
            stringBuilder.Append(" " + str);
        }
        else
        {
          dictionary.Add(key, stringBuilder.ToString());
          stringBuilder = new StringBuilder();
          ++key;
        }
      }
      dictionary.Add(key, stringBuilder.ToString());
      return dictionary;
    }
  }
}
