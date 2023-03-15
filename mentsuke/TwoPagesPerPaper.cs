using System;
namespace mentsuke
{
  public static class TwoPagesPerPaper
  {
    public static List<int> SortPageNumber(int numberOfPages, bool firstPageIsLeft = false)
    {
      var pn = Enumerable.Range(1, numberOfPages).ToList();

      // 4の倍数ページに
      if (firstPageIsLeft)
      {
        pn.Insert(0, int.MinValue);
      }
      while (pn.Count % 4 != 0)
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

    static int IndexOfPage(int i, int numberOfPages)
    {
      return (i + numberOfPages) % numberOfPages;
    }
  }
}

