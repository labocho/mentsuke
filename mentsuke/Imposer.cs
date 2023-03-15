using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using System;
using iText.Kernel.Pdf.Canvas;
using static mentsuke.Util;

namespace mentsuke
{
  public class Imposer
  {
    readonly PdfDocument src;
    readonly PdfDocument dest;

    public static void Impose(string src, string dest, bool firstPageIsLeft)
    {
      using var srcDoc = new PdfDocument(new PdfReader(src));
      using var destDoc = new PdfDocument(new PdfWriter(dest));
      new Imposer(srcDoc, destDoc).Impose(firstPageIsLeft);
    }

    public static void Impose(PdfDocument src, PdfDocument dest, bool firstPageIsLeft) {
      new Imposer(src, dest).Impose(firstPageIsLeft);
    }

    public Imposer(PdfDocument _src, PdfDocument _dest)
    {
      src = _src;
      dest = _dest;
    }

    public void Impose(bool firstPageIsLeft) {
      var pageSizes = new Dictionary<string, PageSize> {
        { "a4", new PageSize(mm2pt(210), mm2pt(297)) },
        { "a3", new PageSize(mm2pt(297), mm2pt(420)) },
        { "a3+r", new PageSize(mm2pt(483), mm2pt(329)) },
      };

      var srcSize = src.GetPage(1).GetPageSize();
      var destSize = pageSizes["a3+r"];
      dest.SetDefaultPageSize(destSize);

      var pn = TwoPagesPerPaper.SortPageNumber(src.GetNumberOfPages(), firstPageIsLeft);
      var n = pn.Count;
      for (var i = 0; i < n; i += 2)
      {
        var page = dest.AddNewPage();
        if (pn[i] != int.MinValue)
        {
          Console.WriteLine("Writing page: {0}", pn[i]);
          PutPageLeft(page, src.GetPage(pn[i]));
        }
        if (pn[i + 1] != int.MinValue)
        {
          Console.WriteLine("Writing page: {0}", pn[i + 1]);
          PutPageRight(page, src.GetPage(pn[i + 1]));
        }
        TrimMark.Draw(page, srcSize.GetWidth() * 2, srcSize.GetHeight(), destSize.GetWidth(), destSize.GetHeight());
      }
    }

    void PutPageLeft(PdfPage destPage, PdfPage srcPage)
    {
      var canvas = new PdfCanvas(destPage);
      var page = srcPage.CopyAsFormXObject(dest);

      var destSize = destPage.GetPageSize();
      var srcSize = srcPage.GetPageSize();
      var x = (destSize.GetWidth() - (srcSize.GetWidth() * 2)) / 2;
      var y = (destSize.GetHeight() - srcSize.GetHeight()) / 2;
      canvas.AddXObjectAt(page, x, y);
    }

    void PutPageRight(PdfPage destPage, PdfPage srcPage)
    {
      var canvas = new PdfCanvas(destPage);
      var page = srcPage.CopyAsFormXObject(dest);

      var destSize = destPage.GetPageSize();
      var srcSize = srcPage.GetPageSize();
      var x = (destSize.GetWidth() - (srcSize.GetWidth() * 2)) / 2 + srcSize.GetWidth();
      var y = (destSize.GetHeight() - srcSize.GetHeight()) / 2;
      canvas.AddXObjectAt(page, x, y);
    }
  }
}

