﻿#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif


namespace iSukces.Mathematics
{
  public interface IPoint12Mapper
  {
    Point MapPoint12(double x);
  }
}
