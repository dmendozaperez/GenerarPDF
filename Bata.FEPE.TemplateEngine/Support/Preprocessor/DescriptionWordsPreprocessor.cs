// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Preprocessor.DescriptionWordsPreprocessor
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Bata.FEPE.TemplateEngine.Support.Preprocessor
{
  public class DescriptionWordsPreprocessor : IPaymentReceiptPreprocessor
  {
    private const string FirstDescriptionElementMarker = "esPrimero";
    private const string LastDescriptionElementMarker = "esUltimo";
    private const char WordsSeparator = ' ';
    private readonly XmlNamespaceManager xmlNamespaceManager;

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
      foreach (XmlNode descriptionXmlNode in paymentReceiptXmlDocument.SelectNodes(this.AdjustmentXPaths.Description, this.xmlNamespaceManager))
        this.TryAdjustDescriptionXmlNode(paymentReceiptXmlDocument, descriptionXmlNode);
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
      string str1 = line;
      char[] separator = new char[1];
      int index = 0;
      int num1 = 32;
      separator[index] = (char) num1;
      int num2 = 1;
      string[] strArray = str1.Split(separator, (StringSplitOptions) num2);
      Dictionary<int, string> dictionary = new Dictionary<int, string>();
      StringBuilder stringBuilder = new StringBuilder();
      int key = 1;
      foreach (string str2 in strArray)
      {
        if (stringBuilder.Length + str2.Length < this.DetailLineWidth)
        {
          if (stringBuilder.Length == 0)
            stringBuilder.Append(str2);
          else
            stringBuilder.Append(" " + str2);
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
