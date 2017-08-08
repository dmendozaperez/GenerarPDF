// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.PDFService.Core.Entities.Exceptions.PdfRequestValidationException
// Assembly: Carvajal.FEPE.PDFService.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 63E25BB8-FD78-4265-A9A0-A22797765D0D
// Assembly location: D:\Fuentes\Generador PDF\Carvajal.FEPE.PDFService.Core.dll

using System;

namespace Carvajal.FEPE.PDFService.Core.Entities.Exceptions
{
  public class PdfRequestValidationException : Exception
  {
    public PdfRequestValidationException()
    {
    }

    public PdfRequestValidationException(string message)
      : base(message)
    {
    }

    public PdfRequestValidationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
