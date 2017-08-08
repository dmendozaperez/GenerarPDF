// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Sunat.PaymentReceiptTypeCodes
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using System.Collections.Generic;

namespace Carvajal.FEPE.TemplateEngine.Support.Sunat
{
  public static class PaymentReceiptTypeCodes
  {
    private static readonly Dictionary<string, string> Collection = new Dictionary<string, string>()
    {
      {
        "01",
        "FA"
      },
      {
        "03",
        "BO"
      },
      {
        "07",
        "NC"
      },
      {
        "08",
        "ND"
      },
      {
        "20",
        "20"
      },
      {
        "40",
        "40"
      }
    };
    public const string InvoiceCode = "FA";
    public const string BallotCode = "BO";
    public const string CreditNoteCode = "NC";
    public const string DebitNoteCode = "ND";
    public const string PerceptionCode = "40";
    public const string RetentionCode = "20";

    public static string GetCode(string number)
    {
      string str;
      if (!PaymentReceiptTypeCodes.Collection.TryGetValue(number, out str))
        return "FA";
      return str;
    }
  }
}
