﻿// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Preprocessor.NoPreprocessing
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using System.Xml;

namespace Carvajal.FEPE.TemplateEngine.Support.Preprocessor
{
  public class NoPreprocessing : IPaymentReceiptPreprocessor
  {
    public static readonly NoPreprocessing Instance = new NoPreprocessing();

    private NoPreprocessing()
    {
    }

    public void Preprocess(XmlDocument paymentReceiptXmlDocument)
    {
    }
  }
}
