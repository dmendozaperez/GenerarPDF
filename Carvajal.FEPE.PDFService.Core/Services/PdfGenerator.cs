// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.PDFService.Core.Services.PdfGenerator
// Assembly: Carvajal.FEPE.PDFService.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 63E25BB8-FD78-4265-A9A0-A22797765D0D
// Assembly location: D:\Fuentes\Generador PDF\Carvajal.FEPE.PDFService.Core.dll

using Carvajal.FEPE.PDFService.Core.Entities;
using Carvajal.FEPE.TemplateEngine.Mapper;
using Carvajal.FEPE.TemplateEngine.Services;
using Carvajal.FEPE.TemplateEngine.Support.IO;
//using log4net;
using System;
using System.IO;
using System.Xml;

namespace Carvajal.FEPE.PDFService.Core.Services
{
  public class PdfGenerator
  {
    //private readonly ILog Logger = LogManager.GetLogger(typeof (PdfGenerator));

    public string TemplatesRootDirectoryPath { get; private set; }

    public string ReferenceDataDirectoryPath { get; private set; }

        //public TemplateCacheManager TemplateCacheManager { get; set; }
        public TemplateCacheManager TemplateCacheManager = new TemplateCacheManager("","","","");
    public IDocumentMapper DocumentMapper { get; set; }
    

    public PdfGenerator()
    {
      this.TemplatesRootDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
      this.ReferenceDataDirectoryPath = Path.Combine(this.TemplatesRootDirectoryPath, "ReferenceData");
    }

    public string GeneratePdfFromXmlContent(string paymentReceiptXmlContent, string companyRuc, string paymentReceiptType, string templateCode)
    {
            string _html = "";
            try
            {                
                string templateFilePath = FileUtilities.GetCustomOrDefaultTemplateFilePath(this.TemplatesRootDirectoryPath, companyRuc, paymentReceiptType, templateCode);
                //this.Logger.Debug((object)("Ruta de la plantilla seleccionada: " + templateFilePath));
                CompiledTemplate compiledTemplate = this.TemplateCacheManager.AddOrGet(templateFilePath, paymentReceiptType);
                XmlDocument xmlDocument1 = new XmlDocument();
                xmlDocument1.LoadXml(paymentReceiptXmlContent);

                //XmlDocument xmlDocument2 =  this.DocumentMapper.Transform(xmlDocument1);

                XmlDocument xmlDocument2 = (XmlDocument)(xmlDocument1);
                string str1 = compiledTemplate.BuildHtmlContent(xmlDocument2);
                _html = str1;
            }
            catch(Exception exc)
            {
                _html = "";
            }
            return _html;
      //string str2 = str1;
      //byte[] numArray = compiledTemplate.BuildPdfDocument(str2);
      //return new PdfGeneratorOutput()
      //{
      //  HtmlFileOutputString = str1,
      //  PdfFileOutputBuffer = numArray
      //};
    }
  }
}
