// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.PDFService.Core.Services.PdfService
// Assembly: Carvajal.FEPE.PDFService.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 63E25BB8-FD78-4265-A9A0-A22797765D0D
// Assembly location: D:\Fuentes\Generador PDF\Carvajal.FEPE.PDFService.Core.dll

using Carvajal.FEPE.PDFService.Core.Entities;
using Carvajal.FEPE.PDFService.Core.Entities.Exceptions;
using Carvajal.FEPE.PDFService.Persistence;
using Carvajal.FEPE.PDFService.Persistence.Entities;
using Carvajal.FEPE.TemplateEngine.Support.IO;
using Common;
using Common.Entities;
using Common.Services;
using Common.Support;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Carvajal.FEPE.PDFService.Core.Services
{
  public class PdfService : IPdfService
  {
    private readonly Encoding Iso88591Encoding = Encoding.GetEncoding("ISO-8859-1");
    private const int DefaultBatchSize = 50;
    private readonly ILog logger;
    private readonly ApplicationLog applicationLog;

    public IPdfServiceDao PdfServiceDao { get; set; }

    private PdfGenerator PdfGenerator { get; set; }

    public string BatchSize { get; set; }

    public string HtmlOutputFlag { get; set; }

    public string XmlInputDirectoryFlag { get; set; }

    public PdfService()
    {
      this.logger = LogManager.GetLogger(typeof (PdfService));
      this.applicationLog = new ApplicationLog();
    }

    public void Run()
    {
      this.CheckStopSignal();
      try
      {
        this.HandlePdfGenerationRequests();
      }
      catch (Exception ex)
      {
        this.logger.Info((object) string.Format("Se ha producido una excepción al ejecutar el servicio Carvajal.FEPE.PDFService: \n{0}", (object) ex), ex);
      }
    }

    private void CheckStopSignal()
    {
      try
      {
        if (!new SvcAPP_Parameters().GetParameter((Constantes.APP_PARAMETERS) 15).ParamValue.Equals(SymCryptography.EncryptParametro("true")))
          return;
        Thread.Sleep(1000);
        Environment.Exit(0);
      }
      catch (Exception ex)
      {
        this.applicationLog.SaveErrorEvent(string.Format("Se ha producido una excepción al intentar detener el servicio Carvajal.FEPE.PDFService: \n{0}", (object) ex.Message));
      }
    }

    public void HandlePdfGenerationRequests()
    {
      this.LogPdfServiceStatus();
      IList<PdfRequest> processingStatus = this.GetPdfRequestsInPendingOrProcessingStatus();
      if (((ICollection<PdfRequest>) processingStatus).Count > 0)
      {
        this.logger.Info((object) string.Format("Cantidad de peticiones de generación de PDF pendientes por procesar: {0}", (object) ((ICollection<PdfRequest>) processingStatus).Count));
        this.SaveFailedPdfGenerationEvent(this.HandlePdfRequestsBatch((IEnumerable<PdfRequest>) processingStatus));
      }
      else
        this.logger.Info((object) "No hay peticiones de generación de PDF pendientes por procesar.");
    }

    private List<string> HandlePdfRequestsBatch(IEnumerable<PdfRequest> pdfRequests)
    {
      List<string> stringList = new List<string>();
      using (IEnumerator<PdfRequest> enumerator = pdfRequests.GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          PdfRequest current = enumerator.Current;
          this.LogPdfRequest(current);
          try
          {
            this.HandlePdfRequest(current);
          }
          catch (PdfRequestValidationException ex)
          {
            this.PdfServiceDao.RemovePdfRequest(current.PdfRequestId);
            this.logger.Error((object) ex.Message, (Exception) ex);
          }
          catch (PdfGenerationException ex)
          {
            this.PdfServiceDao.RemovePdfRequest(current.PdfRequestId);
            this.logger.Error((object) ex.Message, (Exception) ex);
            stringList.Add(ex.Message);
          }
        }
      }
      return stringList;
    }

    private void HandlePdfRequest(PdfRequest pdfRequest)
    {
      this.PdfServiceDao.SetProcessingStatus(pdfRequest.PdfRequestId);
      PaymentReceiptDescriptors receiptDescriptors = this.PdfServiceDao.GetPaymentReceiptDescriptors(pdfRequest.PaymentReceiptId);
      if (!PdfService.IsPaymentReceiptIdentified(receiptDescriptors))
        throw new PdfRequestValidationException(string.Format("El comprobante con ID {0} especificado en la petición no existe.", (object) pdfRequest.PaymentReceiptId));
      this.LogPaymentReceiptDescriptors(receiptDescriptors);
      string pdfFileName = DocumentFileNameFormatter.PdfFileName(receiptDescriptors.CompanyRuc, receiptDescriptors.PaymentReceiptType, receiptDescriptors.Serial, receiptDescriptors.CorrelativeNumber);
      WorkingDirectories workingDirectories = this.PdfServiceDao.GetWorkingDirectories(pdfRequest.CompanyId);
      if (!this.AreWorkingDirectoriesConfigured(workingDirectories))
        throw new PdfGenerationException(string.Format("La empresa con ID {0} no está registrada o no tiene configuradas las carpetas de entrada y salida.", (object) pdfRequest.CompanyId), pdfFileName);
      this.LogWorkingDirectories(workingDirectories);
      this.TryGenerateSaveAndPrintPdfFile(pdfRequest, receiptDescriptors, workingDirectories, pdfFileName);
      this.PdfServiceDao.RemovePdfRequest(pdfRequest.PdfRequestId);
    }

    private static bool IsPaymentReceiptIdentified(PaymentReceiptDescriptors paymentReceiptDescriptors)
    {
      if (paymentReceiptDescriptors != null && !string.IsNullOrEmpty(paymentReceiptDescriptors.CompanyRuc) && (!string.IsNullOrEmpty(paymentReceiptDescriptors.PaymentReceiptType) && !string.IsNullOrEmpty(paymentReceiptDescriptors.Serial)))
        return paymentReceiptDescriptors.CorrelativeNumber >= 0;
      return false;
    }

    private bool AreWorkingDirectoriesConfigured(WorkingDirectories workingDirectories)
    {
      if (this.GetXmlInputDirectoryFlag())
      {
        if (workingDirectories != null && !string.IsNullOrEmpty(workingDirectories.PdfFilesOutputDirectory))
          return !string.IsNullOrEmpty(workingDirectories.XmlFilesInputDirectory);
        return false;
      }
      if (workingDirectories != null)
        return !string.IsNullOrEmpty(workingDirectories.PdfFilesOutputDirectory);
      return false;
    }

    private void TryGenerateSaveAndPrintPdfFile(PdfRequest pdfRequest, PaymentReceiptDescriptors paymentReceiptDescriptors, WorkingDirectories workingDirectories, string pdfFileName)
    {
      try
      {
        PdfGeneratorOutput pdfFile = this.GeneratePdfFile(pdfRequest.PaymentReceiptId, paymentReceiptDescriptors, workingDirectories, pdfRequest.TemplateCode);
        string pdfFilePath = FileUtilities.GetPdfFilePath(workingDirectories.PdfFilesOutputDirectory, paymentReceiptDescriptors.CompanyRuc, paymentReceiptDescriptors.IssueDate, pdfFileName);
        this.SavePdfGeneratorOuput(pdfFile, pdfFilePath, paymentReceiptDescriptors, workingDirectories);
        this.logger.Info((object) string.Format("Se generó el archivo PDF para la petición con ID {0}", (object) pdfRequest.PdfRequestId));
        if (!this.IsPrintEnabledForCompany(pdfRequest.CompanyId))
        {
          this.PdfServiceDao.UpdatePaymentReceiptPrintStatus(pdfRequest.PaymentReceiptId, 0);
          this.logger.Debug((object) string.Format("La empresa con ID {0} no tiene habilitada la impresión local", (object) pdfRequest.CompanyId));
        }
        else if (!this.IsPrintRequired(pdfRequest))
        {
          this.PdfServiceDao.UpdatePaymentReceiptPrintStatus(pdfRequest.PaymentReceiptId, 0);
          this.logger.Debug((object) string.Format("La petición de generación de PDF con ID {0} no require impresión", (object) pdfRequest.PdfRequestId));
        }
        else
          this.SendToPrint(pdfRequest, pdfFileName, pdfFilePath);
      }
      catch (Exception ex)
      {
        throw new PdfGenerationException(string.Format("Error en la generación del archivo PDF {0}. Estado de excepción interno: {1}.", (object) pdfFileName, (object) (ex.Message + (ex.InnerException != null ? ex.InnerException.Message : string.Empty))), pdfFileName, ex);
      }
    }

    private bool IsPrintEnabledForCompany(int companyId)
    {
      return StringUtils.FromTruthString(SymCryptography.DecryptParametro(this.PdfServiceDao.GetPrintEnabledCompanyParameter(companyId)));
    }

    private bool IsPrintRequired(PdfRequest pdfRequest)
    {
      if (string.IsNullOrEmpty(pdfRequest.PrinterCode.Trim()))
        return false;
      int? numberOfCopies = pdfRequest.NumberOfCopies;
      int num = 0;
      if (numberOfCopies.GetValueOrDefault() <= num)
        return false;
      return numberOfCopies.HasValue;
    }

    private PdfGeneratorOutput GeneratePdfFile(long paymentReceiptId, PaymentReceiptDescriptors paymentReceiptDescriptors, WorkingDirectories workingDirectories, string templateCode)
    {
            return null;
                 
      //string paymentReceiptXmlContent;
      //if (this.GetXmlInputDirectoryFlag())
      //{
      //  string str = DocumentFileNameFormatter.XmlFileName(paymentReceiptDescriptors.CompanyRuc, paymentReceiptDescriptors.PaymentReceiptType, paymentReceiptDescriptors.Serial, paymentReceiptDescriptors.CorrelativeNumber);
      //  paymentReceiptXmlContent = File.ReadAllText(FileUtilities.GetXmlFilePath(workingDirectories.XmlFilesInputDirectory, str), this.Iso88591Encoding);
      //}
      //else
      //  paymentReceiptXmlContent = this.PdfServiceDao.GetXmlDocumentContent(paymentReceiptId);
      //return " this.PdfGenerator.GeneratePdfFromXmlContent(paymentReceiptXmlContent, paymentReceiptDescriptors.CompanyRuc, paymentReceiptDescriptors.PaymentReceiptType, templateCode);
    }

    private void SavePdfGeneratorOuput(PdfGeneratorOutput pdfGenerationOutput, string pdfFilePath, PaymentReceiptDescriptors paymentReceiptDescriptors, WorkingDirectories workingDirectories)
    {
      if (!Directory.Exists(Path.GetDirectoryName(pdfFilePath)))
        Directory.CreateDirectory(Path.GetDirectoryName(pdfFilePath));
      File.WriteAllBytes(pdfFilePath, pdfGenerationOutput.PdfFileOutputBuffer);
      if (!this.GetHtmlOutputFlag())
        return;
      string str = DocumentFileNameFormatter.HtmlFileName(paymentReceiptDescriptors.CompanyRuc, paymentReceiptDescriptors.PaymentReceiptType, paymentReceiptDescriptors.Serial, paymentReceiptDescriptors.CorrelativeNumber);
      string htmlFilePath = FileUtilities.GetHtmlFilePath(workingDirectories.PdfFilesOutputDirectory, paymentReceiptDescriptors.CompanyRuc, paymentReceiptDescriptors.IssueDate, str);
      if (!Directory.Exists(Path.GetDirectoryName(htmlFilePath)))
        Directory.CreateDirectory(Path.GetDirectoryName(htmlFilePath));
      File.WriteAllText(htmlFilePath, pdfGenerationOutput.HtmlFileOutputString);
    }

    private void SendToPrint(PdfRequest pdfRequest, string pdfFileName, string pdfFilePath)
    {
      bool flag1 = false;
      string str1="";
      bool flag2 = this.PdfServiceDao.TryGetPrinterNameByCode((long) pdfRequest.CompanyId, pdfRequest.PrinterCode, out str1);
      if (!flag2)
      {
        flag2 = this.PdfServiceDao.TryGetDefaultPrinterName(pdfRequest.CompanyId, out str1);
        flag1 = true;
      }
      if (flag2)
      {
        IPdfServiceDao pdfServiceDao = this.PdfServiceDao;
        PrintRequest printRequest = new PrintRequest();
        string str2 = pdfFilePath;
        printRequest.PdfFilePath=str2;
        long paymentReceiptId = pdfRequest.PaymentReceiptId;
        printRequest.PaymentReceiptId=paymentReceiptId;
        string str3 = str1;
        printRequest.SystemPrinterName=str3;
        int num1 = pdfRequest.NumberOfCopies ?? 1;
        printRequest.NumberOfCopies=num1;
        int num2 = 101;
        printRequest.RetryNumber=num2;
        DateTime now = DateTime.Now;
        printRequest.ScheduleDate=now;
        pdfServiceDao.SchedulePdfPrint(printRequest);
        if (flag1)
          this.SavePrintScheduledToDefaultPrinterEvent(pdfFileName);
        this.logger.Debug((object) string.Format("Se programó la impresión del archivo {0} en la impresora {1} con número de copias {2}", (object) pdfFileName, (object) str1, (object) pdfRequest.NumberOfCopies));
      }
      else
      {
        this.PdfServiceDao.UpdatePaymentReceiptPrintStatus(pdfRequest.PaymentReceiptId, 0);
        this.SavePrinterCodeNotFoundEvent(pdfFileName);
        this.logger.Info((object) string.Format("El código {0} no está asociado a una impresora configurada para la empresa con ID {1} ni la empresa tiene una impresora predeterminada configurada", (object) pdfRequest.PrinterCode, (object) pdfRequest.CompanyId));
      }
    }

    private void SaveFailedPdfGenerationEvent(List<string> notGeneratedPdfFileNamesList)
    {
      if (notGeneratedPdfFileNamesList.Count <= 0)
        return;
      this.applicationLog.SaveErrorEvent(string.Format("No fue posible generar el PDF para los comprobantes. Puede generarlos desde el visor de comprobantes del subsistema cliente. Los comprobantes para los cuales se presentó el error al generar el PDF son: {0}", (object) string.Join(",", (IEnumerable<string>) notGeneratedPdfFileNamesList)));
    }

    private void SavePrinterCodeNotFoundEvent(string pdfFileName)
    {
      this.applicationLog.SaveInformationEvent(string.Format("Ocurrió un error al programar la impresión del comprobante, no se encuentra la impresora. El archivo es: {0}", (object) pdfFileName));
    }

    private void SavePrintScheduledToDefaultPrinterEvent(string pdfFileName)
    {
      this.applicationLog.SaveInformationEvent(string.Format("La impresora especificada en el archivo de entrada no está configurada en el SC; por lo tanto, el documento se envió a imprimir a la impresora configurada por defecto. El archivo es: {0}", (object) pdfFileName));
    }

    private int GetBatchSize()
    {
      int result;
      if (int.TryParse(this.BatchSize, out result) && result >= 1)
        return result;
      return 50;
    }

    private bool GetHtmlOutputFlag()
    {
      return StringUtils.FromTruthString(this.HtmlOutputFlag);
    }

    private bool GetXmlInputDirectoryFlag()
    {
      return StringUtils.FromTruthString(this.XmlInputDirectoryFlag);
    }

    private void LogPdfServiceStatus()
    {
      this.logger.Info((object) "Servicio de Generación Local de PDF");
      this.logger.Info((object) string.Format("Ruta del Directorio de Plantillas: {0}", (object) this.PdfGenerator.TemplatesRootDirectoryPath));
      this.logger.Info((object) string.Format("Ruta del Directorio de Datos de Referencia: {0}", (object) this.PdfGenerator.ReferenceDataDirectoryPath));
      this.logger.Info((object) string.Format("# de Items en Caché de Plantillas: {0}", (object) this.PdfGenerator.TemplateCacheManager.Count));
    }

    private void LogPdfRequest(PdfRequest pdfRequest)
    {
      this.logger.Debug((object) string.Format("RECIBIDA Petición de Generación de PDF con ID {0}", (object) pdfRequest.PdfRequestId));
      this.logger.Debug((object) string.Format("Estado: {0} - ID Comprobante: {1} - ID de Empresa : {2} - Código de Plantilla: {3} - Código de Impresora: {4} - Número de Copias: {5}", (object) pdfRequest.PdfStatus, (object) pdfRequest.PaymentReceiptId, (object) pdfRequest.CompanyId, (object) pdfRequest.TemplateCode, (object) pdfRequest.PrinterCode, (object) pdfRequest.NumberOfCopies));
    }

    private void LogPaymentReceiptDescriptors(PaymentReceiptDescriptors paymentReceiptDescriptors)
    {
      this.logger.Debug((object) string.Format("Descriptores del Comprobante de Pago de la Empresa Emisora con RUC {0}", (object) paymentReceiptDescriptors.CompanyRuc));
      this.logger.Debug((object) string.Format("Tipo: {0} - Serie: {1} - Numero: {2} - Codigo Documento Receptor: {3} - Numero Documento Receptor: {4} - Fecha Emision: {5}", (object) paymentReceiptDescriptors.PaymentReceiptType, (object) paymentReceiptDescriptors.Serial, (object) paymentReceiptDescriptors.CorrelativeNumber, (object) paymentReceiptDescriptors.ReceiverDocumentCode, (object) paymentReceiptDescriptors.ReceiverDocumentNumber, (object) paymentReceiptDescriptors.IssueDate));
    }

    private void LogWorkingDirectories(WorkingDirectories workingDirectories)
    {
      this.logger.Debug((object) string.Format("Directorios de Trabajo de la Empresa con RUC {0}", (object) workingDirectories.CompanyRuc));
      this.logger.Debug((object) string.Format("Ruta del Directorio de Salida de Archivos PDF Generados: {0}", (object) workingDirectories.PdfFilesOutputDirectory));
      this.logger.Debug((object) string.Format("Ruta del Directorio de Entrada de Archivos XML: {0}", (object) workingDirectories.XmlFilesInputDirectory));
    }

    public IList<PdfRequest> GetPdfRequestsInPendingOrProcessingStatus()
    {
      return this.PdfServiceDao.GetPdfRequestsInPendingOrProcessingStatus(this.GetBatchSize());
    }
  }
}
