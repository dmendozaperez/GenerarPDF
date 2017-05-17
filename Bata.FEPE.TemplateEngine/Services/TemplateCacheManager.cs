// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Services.TemplateCacheManager
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AB4FD6BE-70AA-4F27-A8BF-3F770A12367A
// Assembly location: D:\David\Generador PDF\PDFGenerator\Proy2015\Proy2015\Proy2015\bin\Debug\Carvajal.FEPE.TemplateEngine.dll

using System;
using System.Collections.Specialized;
using System.Runtime.Caching;

namespace Bata.FEPE.TemplateEngine.Services
{
  public class TemplateCacheManager
  {
    private static readonly Guid CacheGuid = Guid.NewGuid();
    private static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromMinutes(2.0);
    private static readonly TimeSpan DefaultItemSlidingExpiration = TimeSpan.FromHours(1.0);
    private const string CacheMemoryLimitMegabytesPropertyName = "cacheMemoryLimitMegabytes";
    private const string PhysicalMemoryLimitPercentagePropertyName = "physicalMemoryLimitPercentage";
    private const string PollingIntervalPropertyName = "pollingInterval";
    private const int DefaultCacheMemoryLimit = 256;
    private const int DefaultPhysicalMemoryLimitPercentage = 10;
    private readonly CacheItemPolicy defaultCacheItemPolicy;
    private readonly MemoryCache cacheMemory;

    public long Count
    {
      get
      {
        return this.cacheMemory.GetCount((string) null);
      }
    }

    public TemplateCacheManager(string memoryLimit, string physicalMemoryLimitPercentage, string pollingInterval, string itemSlidingExpiration)
    {
      this.defaultCacheItemPolicy = this.SetDefaultCacheItemPolicy(itemSlidingExpiration);
      NameValueCollection config = this.SetCacheMemoryConfiguration(memoryLimit, physicalMemoryLimitPercentage, pollingInterval);
      this.cacheMemory = new MemoryCache(TemplateCacheManager.CacheGuid.ToString(), config);
    }

    private CacheItemPolicy SetDefaultCacheItemPolicy(string itemSlidingExpiration)
    {
      TimeSpan result;
      bool flag = TimeSpan.TryParse(itemSlidingExpiration, out result);
      CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
      TimeSpan timeSpan = flag ? result : TemplateCacheManager.DefaultItemSlidingExpiration;
      cacheItemPolicy.SlidingExpiration = timeSpan;
      return cacheItemPolicy;
    }

    private NameValueCollection SetCacheMemoryConfiguration(string memoryLimit, string physicalMemoryLimitPercentage, string pollingInterval)
    {
      int result1;
      bool flag1 = int.TryParse(memoryLimit, out result1);
      int result2;
      bool flag2 = int.TryParse(physicalMemoryLimitPercentage, out result2);
      TimeSpan result3;
      bool flag3 = TimeSpan.TryParse(pollingInterval, out result3);
      NameValueCollection nameValueCollection = new NameValueCollection();
      string name1 = "cacheMemoryLimitMegabytes";
      string str1 = flag1 ? result1.ToString() : 256.ToString();
      nameValueCollection.Add(name1, str1);
      string name2 = "physicalMemoryLimitPercentage";
      string str2 = flag2 ? result2.ToString() : 10.ToString();
      nameValueCollection.Add(name2, str2);
      string name3 = "pollingInterval";
      string str3 = flag3 ? pollingInterval.ToString() : TemplateCacheManager.DefaultPollingInterval.ToString();
      nameValueCollection.Add(name3, str3);
      return nameValueCollection;
    }

    public CompiledTemplate AddOrGet(string templateFilePath, string paymentReceiptType)
    {
      if (this.cacheMemory.Contains(templateFilePath, (string) null))
        return (CompiledTemplate) this.cacheMemory[templateFilePath];
      CompiledTemplate compiledTemplate = CompiledTemplateFactory.Build(templateFilePath, paymentReceiptType);
      this.cacheMemory.Add(templateFilePath, (object) compiledTemplate, this.defaultCacheItemPolicy, (string) null);
      return compiledTemplate;
    }
  }
}
