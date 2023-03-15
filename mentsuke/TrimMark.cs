using System;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using static mentsuke.Util;

namespace mentsuke
{
  public class TrimMark
  {
    readonly PdfCanvas canvas;

    public static void Draw(PdfPage page, float contentWidth, float contentHeight, float pageWidth, float pageHeight) {
      new TrimMark(page).Draw(contentWidth, contentHeight, pageWidth, pageHeight);
    }

    public TrimMark(PdfPage _page)
    {
      canvas = new PdfCanvas(_page);
      canvas.SetStrokeColorGray(0);
      canvas.SetLineWidth(0.25f);
    }

    public void Draw(float contentWidth, float contentHeight, float pageWidth, float pageHeight)
    {

      var xMargin = pt2mm((pageWidth - contentWidth) / 2);
      var yMargin = pt2mm((pageHeight - contentHeight) / 2);

      // Left-Bottom
      DrawLineRelative(
        new Point(xMargin, yMargin - 12),
        new List<Point> {
            new Point(0f, 9f),
            new Point(-12f, 0f),
        }
      );
      DrawLineRelative(
        new Point(xMargin - 3, yMargin - 12),
        new List<Point> {
            new Point(0f, 12f),
            new Point(-9f, 0f),
        }
      );


      // Left-Top
      DrawLineRelative(
        new Point(xMargin, yMargin + pt2mm(contentHeight) + 12),
        new List<Point> {
            new Point(0f, -9f),
            new Point(-12f, 0f),
        }
      );
      DrawLineRelative(
        new Point(xMargin - 3, yMargin + pt2mm(contentHeight) + 12),
        new List<Point> {
            new Point(0f, -12f),
            new Point(-9f, 0f),
        }
      );

      // Right-Bottom
      DrawLineRelative(
        new Point(xMargin + pt2mm(contentWidth), yMargin - 12),
        new List<Point> {
            new Point(0f, 9f),
            new Point(12f, 0f),
        }
      );
      DrawLineRelative(
        new Point(xMargin + pt2mm(contentWidth) + 3, yMargin - 12),
        new List<Point> {
            new Point(0f, 12f),
            new Point(9f, 0f),
        }
      );

      // Right-Top
      DrawLineRelative(
        new Point(xMargin + pt2mm(contentWidth), yMargin + pt2mm(contentHeight) + 12),
        new List<Point> {
            new Point(0f, -9f),
            new Point(12f, 0f),
        }
      );
      DrawLineRelative(
        new Point(xMargin + pt2mm(contentWidth) + 3, yMargin + pt2mm(contentHeight) + 12),
        new List<Point> {
            new Point(0f, -12f),
            new Point(9f, 0f),
        }
      );

      // Center-Top
      DrawLineRelative(
        new Point(xMargin + pt2mm(contentWidth / 2), yMargin + pt2mm(contentHeight) + 12),
        new List<Point> {
            new Point(0f, -9f),
        }
      );

      // Center-Bottom
      DrawLineRelative(
        new Point(xMargin + pt2mm(contentWidth / 2), yMargin - 12),
        new List<Point> {
            new Point(0f, 9f),
        }
      );

      // Middle-Left
      DrawLineRelative(
        new Point(xMargin - 12, yMargin + pt2mm(contentHeight / 2)),
        new List<Point> {
            new Point(9f, 0f),
        }
      );

      // Middle-Right
      DrawLineRelative(
        new Point(xMargin + pt2mm(contentWidth) + 12, yMargin + pt2mm(contentHeight / 2)),
        new List<Point> {
            new Point(-9f, 0f),
        }
      );
    }

    private void DrawLineRelative(Point origin, List<Point> points)
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
  }
}

