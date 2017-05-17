// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.TemplateSettings
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

namespace Bata.FEPE.TemplateEngine.Support
{
  public class TemplateSettings
  {
    public string PaymentReceiptType { get; private set; }

    public string PageSize { get; private set; }

    public double ScaleXFactor { get; private set; }

    public double ScaleYFactor { get; private set; }

    public string AdjustmentType { get; private set; }

    public TemplateSettings(string paymentReceiptType, string pageSize, double scaleXFactor, double scaleYFactor, string adjustmentType)
    {
      this.PaymentReceiptType = paymentReceiptType;
      this.PageSize = pageSize;
      this.ScaleXFactor = scaleXFactor;
      this.ScaleYFactor = scaleYFactor;
      this.AdjustmentType = adjustmentType;
    }
  }
}
