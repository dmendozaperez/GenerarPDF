// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.Preprocessor.PaymentReceiptAdjustmentXPaths
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

namespace Bata.FEPE.TemplateEngine.Support.Preprocessor
{
  public static class PaymentReceiptAdjustmentXPaths
  {
    public static readonly PaymentReceiptAdjustmentXPaths.Value InvoiceOrBallot = new PaymentReceiptAdjustmentXPaths.Value("/r:Invoice/cac:InvoiceLine/cac:Item/cbc:Description");
    public static readonly PaymentReceiptAdjustmentXPaths.Value CreditNote = new PaymentReceiptAdjustmentXPaths.Value("/r:CreditNote/cac:CreditNoteLine/cac:Item/cbc:Description");
    public static readonly PaymentReceiptAdjustmentXPaths.Value DebitNote = new PaymentReceiptAdjustmentXPaths.Value("/r:DebitNote/cac:DebitNoteLine/cac:Item/cbc:Description");
    public const string DetailLineWidth = "/xsl:stylesheet/xsl:variable[@name='anchoLinea']";

    public class Value
    {
      public string Description { get; private set; }

      public Value(string description)
      {
        this.Description = description;
      }
    }
  }
}
