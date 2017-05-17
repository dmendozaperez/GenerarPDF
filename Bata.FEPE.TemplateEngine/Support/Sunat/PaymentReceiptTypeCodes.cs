// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Sunat.PaymentReceiptTypeCodes
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using System.Collections.Generic;

namespace Bata.FEPE.TemplateEngine.Support.Sunat
{
  public static class PaymentReceiptTypeCodes
  {
    private static readonly Dictionary<string, string> Collection;
    public const string InvoiceCode = "FA";
    public const string BallotCode = "BO";
    public const string CreditNoteCode = "NC";
    public const string DebitNoteCode = "ND";

    static PaymentReceiptTypeCodes()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      string key1 = "01";
      string str1 = "FA";
      dictionary.Add(key1, str1);
      string key2 = "03";
      string str2 = "BO";
      dictionary.Add(key2, str2);
      string key3 = "07";
      string str3 = "NC";
      dictionary.Add(key3, str3);
      string key4 = "08";
      string str4 = "ND";
      dictionary.Add(key4, str4);
      PaymentReceiptTypeCodes.Collection = dictionary;
    }

    public static string GetCode(string number)
    {
      string str;
      return PaymentReceiptTypeCodes.Collection.TryGetValue(number, out str) ? str : "FA";
    }
  }
}
