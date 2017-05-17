// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.IO.FileUtilities
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using Bata.FEPE.TemplateEngine.Support.Sunat;
using System;
using System.IO;

namespace Bata.FEPE.TemplateEngine.Support.IO
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
      string[] strArray = new string[5];
      int index1 = 0;
      string str1 = htmlFilesOutputDirectoryPath;
      strArray[index1] = str1;
      int index2 = 1;
      string str2 = "HTML";
      strArray[index2] = str2;
      int index3 = 2;
      string str3 = companyRuc;
      strArray[index3] = str3;
      int index4 = 3;
      string str4 = issueDate.ToString("yyyyMMdd");
      strArray[index4] = str4;
      int index5 = 4;
      string str5 = htmlFileName;
      strArray[index5] = str5;
      return Path.Combine(strArray);
    }
  }
}
