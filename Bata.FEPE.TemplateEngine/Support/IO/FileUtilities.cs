// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.IO.FileUtilities
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using Carvajal.FEPE.TemplateEngine.Support.Sunat;
using System;
using System.IO;

namespace Carvajal.FEPE.TemplateEngine.Support.IO
{
  public static class FileUtilities
  {
    public static string GetCustomOrDefaultTemplateFilePath(string templatesRootDirectoryPath, string companyRuc, string paymentReceiptType, string templateCode)
    {
      string path = Path.Combine(templatesRootDirectoryPath, companyRuc, FileUtilities.GetCustomTemplateFileName(templateCode, paymentReceiptType));
      if (File.Exists(path))
        return path;
      return Path.Combine(templatesRootDirectoryPath, "00000000000", FileUtilities.GetDefaultTemplateFileName(paymentReceiptType));
    }

    public static string GetCustomTemplateFileName(string templateCode, string paymentReceiptType)
    {
      return string.Format("{0}_{1}.xslt", (object) templateCode, (object) PaymentReceiptTypeCodes.GetCode(paymentReceiptType));
    }

    public static string GetDefaultTemplateFileName(string paymentReceiptType)
    {
      return string.Format("{0}.xslt", (object) PaymentReceiptTypeCodes.GetCode(paymentReceiptType));
    }

    public static string GetXmlFilePath(string xmlFilesInputDirectoryPath, string xmlFileName)
    {
      return Path.Combine(xmlFilesInputDirectoryPath, xmlFileName);
    }

    public static string GetPdfFilePath(string pdfFilesOutputDirectoryPath, string companyRuc, DateTime issueDate, string pdfFileName)
    {
      return Path.Combine(pdfFilesOutputDirectoryPath, companyRuc, issueDate.ToString("yyyyMMdd"), pdfFileName);
    }

    public static string GetHtmlFilePath(string htmlFilesOutputDirectoryPath, string companyRuc, DateTime issueDate, string htmlFileName)
    {
      return Path.Combine(htmlFilesOutputDirectoryPath, "HTML", companyRuc, issueDate.ToString("yyyyMMdd"), htmlFileName);
    }
  }
}
