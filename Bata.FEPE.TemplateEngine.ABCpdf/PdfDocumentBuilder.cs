using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bata.FEPE.TemplateEngine.ABCpdf
{
    public static class PdfDocumentBuilder
    {
        private const int LoadTimeout = 30000;
        private const int RetryCount = 2;

        public static byte[] BuildPdfDocument(string htmlContent, string pageSize, double scaleXFactor, double scaleYFactor)
        {
            return (byte[])null;
        }
    }
}
