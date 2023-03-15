using System;
namespace mentsuke
{
  public static class Util
  {
    const float MM_PER_INCH = 25.4f;
    const float PT_PER_INCH = 72f;
    const float PT_PER_MM = PT_PER_INCH / MM_PER_INCH;
    const float MM_PER_PT = MM_PER_INCH / PT_PER_INCH;

    public static float pt2mm(float pt)
    {
      return pt * MM_PER_PT;
    }

    public static float mm2pt(float mm)
    {
      return mm * PT_PER_MM;
    }

    public static double mm2ptd(double mm)
    {
      return mm * PT_PER_MM;
    }
  }
}

