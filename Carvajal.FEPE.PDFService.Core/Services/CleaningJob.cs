// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.PDFService.Core.Services.CleaningJob
// Assembly: Carvajal.FEPE.PDFService.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 63E25BB8-FD78-4265-A9A0-A22797765D0D
// Assembly location: D:\Fuentes\Generador PDF\Carvajal.FEPE.PDFService.Core.dll

//using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Carvajal.FEPE.PDFService.Core.Services
{
  public class CleaningJob
  {
    private static readonly TimeSpan DefaultCleaningProcessInterval = TimeSpan.FromHours(24.0);
    private readonly string InstallationDirectoryPath;
    private const string TemporalDirectoriesNamePattern = "*-*-*-*";
    //private readonly ILog logger;

    public string CleaningProcessInterval { get; set; }

    public CleaningJob()
    {
      this.InstallationDirectoryPath = Environment.CurrentDirectory;
      //this.logger = LogManager.GetLogger(typeof (CleaningJob));
    }

    public TimeSpan GetCleaningProcessInterval()
    {
      TimeSpan result;
      if (TimeSpan.TryParse(this.CleaningProcessInterval, out result))
        return result;
      return CleaningJob.DefaultCleaningProcessInterval;
    }

    public void Run()
    {
      try
      {
        this.CleanInstallationDirectory();
      }
      catch (Exception ex)
      {
        //this.logger.Info((object) "CleaningJob - Ha ocurrido un error inesperado.", ex);
      }
    }

    private void CleanInstallationDirectory()
    {
      DateTime now = DateTime.Now;
      //this.logger.Info((object) string.Format("Limpieza del directorio de instalación iniciada el {0}", (object) now.ToLongDateString()));
      foreach (string temporalDirectory in this.FindTemporalDirectories(now))
        Directory.Delete(temporalDirectory, true);
    }

    private IEnumerable<string> FindTemporalDirectories(DateTime cleaningProcessDate)
    {
      DateTime cleaningStartDate = cleaningProcessDate.Add(this.GetCleaningProcessInterval().Negate());
      return ((IEnumerable<string>) Directory.GetDirectories(this.InstallationDirectoryPath, "*-*-*-*", SearchOption.TopDirectoryOnly)).Where<string>((Func<string, bool>) (directoryPath => new DirectoryInfo(directoryPath).LastWriteTime <= cleaningStartDate));
    }
  }
}
