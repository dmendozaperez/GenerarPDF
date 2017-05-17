using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Bata.FEPE.TemplateEngine.PDF417Lib
{
    public static class PDF417
    {
        public static Image GetPdf417(string hash)
        {
            PDF417Lib pd = new PDF417Lib();
            pd.setText(hash);
            pd.CodeColumns = 12;
            pd.Options = 146;
            pd.ErrorLevel = 5;
            pd.paintCode();
            return PDF417.DrawBarcodeImage(pd, 3, 6);
        }

        private static Image DrawBarcodeImage(PDF417Lib pd, int scale_x, int scale_y)
        {
            int num1 = 0;
            int num2 = -1;
            sbyte[] outBits = pd.OutBits;
            int num3 = (pd.BitColumns - 1) / 8 + 1;
            Bitmap bitmap = new Bitmap(pd.BitColumns * scale_x, pd.OutBits.Length / num3 * scale_y);
            for (int index1 = 0; index1 < outBits.Length; ++index1)
            {
                if (index1 % num3 == 0)
                {
                    ++num2;
                    num1 = 0;
                }
                int num4 = (int)outBits[index1] & (int)byte.MaxValue | 256;
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    Color color = (num4 & 128 >> index2) != 0 ? Color.White : Color.Black;
                    for (int index3 = 0; index3 < scale_x; ++index3)
                    {
                        for (int index4 = 0; index4 < scale_y; ++index4)
                            bitmap.SetPixel(num1 * scale_x + index3, num2 * scale_y + index4, color);
                    }
                    ++num1;
                    if (num1 == pd.BitColumns)
                        break;
                }
            }
            return (Image)bitmap;
        }
    }
}
