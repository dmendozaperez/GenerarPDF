// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.PDFService.Core.Services.IPdfService
// Assembly: Carvajal.FEPE.PDFService.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 63E25BB8-FD78-4265-A9A0-A22797765D0D
// Assembly location: D:\Fuentes\Generador PDF\Carvajal.FEPE.PDFService.Core.dll

using Common.Entities;
using System.Collections.Generic;

namespace Carvajal.FEPE.PDFService.Core.Services
{
  public interface IPdfService
  {
    void HandlePdfGenerationRequests();

    IList<PdfRequest> GetPdfRequestsInPendingOrProcessingStatus();
  }
}
