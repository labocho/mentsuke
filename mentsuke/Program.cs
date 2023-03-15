using iText.Kernel.Pdf;

namespace mentsuke {
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Begin.");

      var pdfReader = new PdfReader(args[0]);
      using var src = new PdfDocument(pdfReader);

      var writer = new PdfWriter("out.pdf");
      using var dest = new PdfDocument(writer);

      Imposer.Impose(src, dest);

      Console.WriteLine("Done.");
    }
  }
}


