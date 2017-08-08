// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.PDFService.Core.Entities.Exceptions.PdfGenerationException
// Assembly: Carvajal.FEPE.PDFService.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 63E25BB8-FD78-4265-A9A0-A22797765D0D
// Assembly location: D:\Fuentes\Generador PDF\Carvajal.FEPE.PDFService.Core.dll

using System;

namespace Carvajal.FEPE.PDFService.Core.Entities.Exceptions
{
  public class PdfGenerationException : Exception
  {
    public string PdfFileName { get; private set; }

    public PdfGenerationException()
    {
    }

    public PdfGenerationException(string message, string pdfFileName)
      : base(message)
    {
      this.PdfFileName = pdfFileName;
    }

    public PdfGenerationException(string message, string pdfFileName, Exception innerException)
      : base(message, innerException)
    {
      this.PdfFileName = pdfFileName;
    }
  }
}
