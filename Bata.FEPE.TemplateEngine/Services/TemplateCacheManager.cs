// Decompiled with JetBrains decompiler
// Type: Carvajal.FEPE.TemplateEngine.Services.TemplateCacheManager
// Assembly: Carvajal.FEPE.TemplateEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E45B097E-B0D8-406E-B5BE-61961D953F9A
// Assembly location: D:\Fuentes\Generador PDF\dllcompiler\Carvajal.FEPE.TemplateEngine\Carvajal.FEPE.TemplateEngine.dll

using System;
using System.Collections.Specialized;
using System.Runtime.Caching;

namespace Carvajal.FEPE.TemplateEngine.Services
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
      return new CacheItemPolicy()
      {
        SlidingExpiration = flag ? result : TemplateCacheManager.DefaultItemSlidingExpiration
      };
    }

    private NameValueCollection SetCacheMemoryConfiguration(string memoryLimit, string physicalMemoryLimitPercentage, string pollingInterval)
    {
      int result1;
      bool flag1 = int.TryParse(memoryLimit, out result1);
      int result2;
      bool flag2 = int.TryParse(physicalMemoryLimitPercentage, out result2);
      TimeSpan result3;
      bool flag3 = TimeSpan.TryParse(pollingInterval, out result3);
      return new NameValueCollection()
      {
        {
          "cacheMemoryLimitMegabytes",
          flag1 ? result1.ToString() : 256.ToString()
        },
        {
          "physicalMemoryLimitPercentage",
          flag2 ? result2.ToString() : 10.ToString()
        },
        {
          "pollingInterval",
          flag3 ? pollingInterval.ToString() : TemplateCacheManager.DefaultPollingInterval.ToString()
        }
      };
    }

    public CompiledTemplate AddOrGet(string templateFilePath, string paymentReceiptType)
    {
      if (this.cacheMemory.Contains(templateFilePath, (string) null))
        return (CompiledTemplate) this.cacheMemory[templateFilePath];
      CompiledTemplate compiledTemplate = CompiledTemplateFactory.Build(templateFilePath, paymentReceiptType);
      this.cacheMemory.Add(templateFilePath, (object) compiledTemplate, this.defaultCacheItemPolicy, (string) null);
      return compiledTemplate;
    }

    public CompiledTemplate AddOrGet(string cacheEntryKey, string templateFilePath, string paymentReceiptType)
    {
      if (this.cacheMemory.Contains(cacheEntryKey, (string) null))
        return (CompiledTemplate) this.cacheMemory[cacheEntryKey];
      CompiledTemplate compiledTemplate = CompiledTemplateFactory.Build(templateFilePath, paymentReceiptType);
      this.cacheMemory.Add(cacheEntryKey, (object) compiledTemplate, this.defaultCacheItemPolicy, (string) null);
      return compiledTemplate;
    }
  }
}
