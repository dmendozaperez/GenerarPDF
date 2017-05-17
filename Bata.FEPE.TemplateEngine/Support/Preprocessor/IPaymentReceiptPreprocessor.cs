// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Preprocessor.IPaymentReceiptPreprocessor
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using System.Xml;

namespace Bata.FEPE.TemplateEngine.Support.Preprocessor
{
  public interface IPaymentReceiptPreprocessor
  {
    void Preprocess(XmlDocument paymentReceiptXmlDocument);
  }
}
