using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;

const float MM_PER_INCH = 25.4f;
const float PT_PER_INCH = 72f;
const float PT_PER_MM = PT_PER_INCH / MM_PER_INCH;

var pageSizes = new Dictionary<string, PageSize> {
  { "a4", new PageSize(mm2pt(210), mm2pt(297)) },
  { "a3", new PageSize(mm2pt(297), mm2pt(420)) },
  { "a3+r", new PageSize(mm2pt(483), mm2pt(329)) },
};

float mm2pt(float mm)
{
  return mm * PT_PER_MM;
}

var pdfReader = new iText.Kernel.Pdf.PdfReader(args[0]);
var src = new PdfDocument(pdfReader);

var writer = new PdfWriter("out.pdf");
var dest = new PdfDocument(writer);
var destSize = pageSizes["a3+r"];
dest.SetDefaultPageSize(destSize);


void PutPageLeft(PdfDocument dest, PdfPage destPage, PdfPage srcPage) {
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

var page = dest.AddNewPage();
PutPageLeft(dest, page, src.GetPage(1));
PutPageRight(dest, page, src.GetPage(2));


dest.Close();
src.Close();

Console.WriteLine("Done.");
