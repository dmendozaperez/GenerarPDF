// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Support.TemplateXmlUrlResolver
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using System;
using System.Xml;

namespace Bata.FEPE.TemplateEngine.Support
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
