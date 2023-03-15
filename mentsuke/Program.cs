using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.Kernel.Colors;

namespace mentsuke {
  class Program
  {
    const float MM_PER_INCH = 25.4f;
    const float PT_PER_INCH = 72f;
    const float PT_PER_MM = PT_PER_INCH / MM_PER_INCH;
    const float MM_PER_PT = MM_PER_INCH / PT_PER_INCH;

    static void Main(string[] args)
    {
      var pageSizes = new Dictionary<string, PageSize> {
        { "a4", new PageSize(mm2pt(210), mm2pt(297)) },
        { "a3", new PageSize(mm2pt(297), mm2pt(420)) },
        { "a3+r", new PageSize(mm2pt(483), mm2pt(329)) },
      };

      float pt2mm(float pt)
      {
        return pt * MM_PER_PT;
      }


      float mm2pt(float mm)
      {
        return mm * PT_PER_MM;
      }

      double mm2ptd(double mm)
      {
        return mm * PT_PER_MM;
      }

      void PutPageLeft(PdfDocument dest, PdfPage destPage, PdfPage srcPage)
      {
        var canvas = new PdfCanvas(destPage);
        var page = srcPage.CopyAsFormXObject(dest);

        var destSize = destPage.GetPageSize();
        var srcSize = srcPage.GetPageSize();
        var x = (destSize.GetWidth() - (srcSize.GetWidth() * 2)) / 2;
        var y = (destSize.GetHeight() - srcSize.GetHeight()) / 2;
        canvas.AddXObjectAt(page, x, y);
      }

      void PutPageRight(PdfDocument dest, PdfPage destPage, PdfPage srcPage)
      {
        var canvas = new PdfCanvas(destPage);
        var page = srcPage.CopyAsFormXObject(dest);

        var destSize = destPage.GetPageSize();
        var srcSize = srcPage.GetPageSize();
        var x = (destSize.GetWidth() - (srcSize.GetWidth() * 2)) / 2 + srcSize.GetWidth();
        var y = (destSize.GetHeight() - srcSize.GetHeight()) / 2;
        canvas.AddXObjectAt(page, x, y);
      }

      void DrawTrimMark(PdfDocument doc, PdfPage page, float contentWidth, float contentHeight, float pageWidth, float pageHeight)
      {
        var canvas = new PdfCanvas(page);
        canvas.SetStrokeColorGray(0);
        canvas.SetLineWidth(0.25f);

        var xMargin = pt2mm((pageWidth - contentWidth) / 2);
        var yMargin = pt2mm((pageHeight - contentHeight) / 2);

        // Left-Bottom
        DrawLineRelative(
          canvas,
          new Point(xMargin, yMargin - 12),
          new List<Point> {
            new Point(0f, 9f),
            new Point(-12f, 0f),
          }
        );
        DrawLineRelative(
          canvas,
          new Point(xMargin - 3, yMargin - 12),
          new List<Point> {
            new Point(0f, 12f),
            new Point(-9f, 0f),
          }
        );


        // Left-Top
        DrawLineRelative(
          canvas,
          new Point(xMargin, yMargin + pt2mm(contentHeight) + 12),
          new List<Point> {
            new Point(0f, -9f),
            new Point(-12f, 0f),
          }
        );
        DrawLineRelative(
          canvas,
          new Point(xMargin - 3, yMargin + pt2mm(contentHeight) + 12),
          new List<Point> {
            new Point(0f, -12f),
            new Point(-9f, 0f),
          }
        );

        // Right-Bottom
        DrawLineRelative(
          canvas,
          new Point(xMargin + pt2mm(contentWidth), yMargin - 12),
          new List<Point> {
            new Point(0f, 9f),
            new Point(12f, 0f),
          }
        );
        DrawLineRelative(
          canvas,
          new Point(xMargin + pt2mm(contentWidth) + 3, yMargin - 12),
          new List<Point> {
            new Point(0f, 12f),
            new Point(9f, 0f),
          }
        );

        // Right-Top
        DrawLineRelative(
          canvas,
          new Point(xMargin + pt2mm(contentWidth), yMargin + pt2mm(contentHeight) + 12),
          new List<Point> {
            new Point(0f, -9f),
            new Point(12f, 0f),
          }
        );
        DrawLineRelative(
          canvas,
          new Point(xMargin + pt2mm(contentWidth) + 3, yMargin + pt2mm(contentHeight) + 12),
          new List<Point> {
            new Point(0f, -12f),
            new Point(9f, 0f),
          }
        );

        // Center-Top
        DrawLineRelative(
          canvas,
          new Point(xMargin + pt2mm(contentWidth / 2), yMargin + pt2mm(contentHeight) + 12),
          new List<Point> {
            new Point(0f, -9f),
          }
        );

        // Center-Bottom
        DrawLineRelative(
          canvas,
          new Point(xMargin + pt2mm(contentWidth / 2), yMargin - 12),
          new List<Point> {
            new Point(0f, 9f),
          }
        );

        // Middle-Left
        DrawLineRelative(
          canvas,
          new Point(xMargin - 12, yMargin + pt2mm(contentHeight / 2)),
          new List<Point> {
            new Point(9f, 0f),
          }
        );

        // Middle-Right
        DrawLineRelative(
          canvas,
          new Point(xMargin + pt2mm(contentWidth) + 12, yMargin + pt2mm(contentHeight / 2)),
          new List<Point> {
            new Point(-9f, 0f),
          }
        );
      }

      void DrawLineRelative(PdfCanvas canvas, Point origin, List<Point> points)
      {
        var p = new Point(origin.x, origin.y);
        canvas.MoveTo(mm2ptd(p.x), mm2ptd(p.y));
        foreach (var point in points)
        {
          p.x += point.x;
          p.y += point.y;
          canvas.LineTo(mm2ptd(p.x), mm2ptd(p.y));
        }
        canvas.Stroke();
      }

      int IndexOfPage(int i, int numberOfPages)
      {
        return (i + numberOfPages) % numberOfPages;
      }
      List<int> SortPageNumber(int numberOfPages, bool firstPageIsLeft = false)
      {
        var pn = Enumerable.Range(1, numberOfPages).ToList();

        // 8の倍数ページに
        if (firstPageIsLeft) {
          pn.Insert(0, int.MinValue);
        }
        while (pn.Count % 8 != 0)
        {
          pn.Add(int.MinValue);
        }

        var buffer = new List<int>();

        var n = pn.Count / 4;
        for (int i = 0; i < n; i++)
        {
          buffer.Add(pn[IndexOfPage(-(i * 2) - 1, pn.Count)]); // -1, -3
          buffer.Add(pn[IndexOfPage(+(i * 2) + 0, pn.Count)]); //  0,  2
          buffer.Add(pn[IndexOfPage(+(i * 2) + 1, pn.Count)]); //  1,  3
          buffer.Add(pn[IndexOfPage(-(i * 2) - 2, pn.Count)]); // -2,  0
        }

        return buffer;
      }

      Console.WriteLine("Begin.");

      var pdfReader = new iText.Kernel.Pdf.PdfReader(args[0]);
      var src = new PdfDocument(pdfReader);

      var writer = new PdfWriter("out.pdf");
      var dest = new PdfDocument(writer);
      var destSize = pageSizes["a3+r"];
      dest.SetDefaultPageSize(destSize);
      var srcSize = src.GetPage(1).GetPageSize();

      var pn = SortPageNumber(src.GetNumberOfPages(), false);
      var n = pn.Count;
      for (var i = 0; i < n; i += 2)
      {
        var page = dest.AddNewPage();
        if (pn[i] != int.MinValue)
        {
          Console.WriteLine("Writing page: {0}", pn[i]);
          PutPageLeft(dest, page, src.GetPage(pn[i]));
        }
        if (pn[i + 1] != int.MinValue)
        {
          Console.WriteLine("Writing page: {0}", pn[i + 1]);
          PutPageRight(dest, page, src.GetPage(pn[i + 1]));
        }
        DrawTrimMark(dest, page, srcSize.GetWidth() * 2, srcSize.GetHeight(), destSize.GetWidth(), destSize.GetHeight());
      }

      dest.Close();
      src.Close();

      Console.WriteLine("Done.");
    }
  }
}


