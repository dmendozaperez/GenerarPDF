// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.TemplateXmlUrlResolver
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using System;
using System.Xml;

namespace Carvajal.FEPE.TemplateEngine.Support
{
  public class TemplateXmlUrlResolver : XmlUrlResolver
  {
    public Uri TemplateFileDirectoryUri { get; private set; }

    public TemplateXmlUrlResolver(string templateFileDirectoryUri)
    {
      this.TemplateFileDirectoryUri = new Uri(templateFileDirectoryUri);
    }

    public override Uri ResolveUri(Uri baseUri, string relativeUri)
    {
      if (baseUri != (Uri) null)
        return base.ResolveUri(baseUri, relativeUri);
      return base.ResolveUri(this.TemplateFileDirectoryUri, relativeUri);
    }
  }
}
