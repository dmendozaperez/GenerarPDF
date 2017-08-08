// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.TemplateSettings
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

namespace Carvajal.FEPE.TemplateEngine.Support
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
