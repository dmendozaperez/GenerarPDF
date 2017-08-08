// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.PDFService.Core.Support.PdfServiceMessages
// Assembly: Carvajal.FEPE.PDFService.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 63E25BB8-FD78-4265-A9A0-A22797765D0D
// Assembly location: D:\Fuentes\Generador PDF\Carvajal.FEPE.PDFService.Core.dll

namespace Carvajal.FEPE.PDFService.Core.Support
{
  internal static class PdfServiceMessages
  {
    internal const string ServiceErrorMessage = "Se ha producido una excepción al ejecutar el servicio Carvajal.FEPE.PDFService: \n{0}";
    internal const string ServiceStopErrorMessage = "Se ha producido una excepción al intentar detener el servicio Carvajal.FEPE.PDFService: \n{0}";
    internal const string PaymentReceiptNotExistsMessage = "El comprobante con ID {0} especificado en la petición no existe.";
    internal const string WorkingDirectoriesNotConfiguredMessage = "La empresa con ID {0} no está registrada o no tiene configuradas las carpetas de entrada y salida.";
    internal const string PdfGeneratorErrorMessage = "Error en la generación del archivo PDF {0}. Estado de excepción interno: {1}.";
    internal const string PdfGenerationErrorEventMessage = "No fue posible generar el PDF para los comprobantes. Puede generarlos desde el visor de comprobantes del subsistema cliente. Los comprobantes para los cuales se presentó el error al generar el PDF son: {0}";
    internal const string PdfPrintScheduledToDefaultPrinterMessage = "La impresora especificada en el archivo de entrada no está configurada en el SC; por lo tanto, el documento se envió a imprimir a la impresora configurada por defecto. El archivo es: {0}";
    internal const string PrinterCodeNotFoundMessage = "Ocurrió un error al programar la impresión del comprobante, no se encuentra la impresora. El archivo es: {0}";
  }
}
