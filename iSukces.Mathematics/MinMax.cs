#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif

#if WPFFEATURES
using iSukces.Mathematics.TypeConverters;
#endif
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace iSukces.Mathematics
{
#if WPFFEATURES
    [TypeConverter(typeof(MinMaxTypeConverter))]
#endif
    public sealed class MinMax : MinMaxGeneric<double>, ICloneable
    {
        public MinMax()
            : base(0, -1)
        {
        }

        public MinMax(double min, double max)
            : base(min, max)
        {
        }

        public MinMax(IEnumerable<double> items)
            : base(0, -1)
        {
            Add(items);
        }


        public MinMax(params MinMax[] a)
            : base(0, -1)
        {
            foreach (var i in a)
            {
                if (i.IsZeroOnInvalid) continue;
                Add(i.Min);
                Add(i.Max);
            }
        }

        public static MinMax CommonRange(MinMax a, MinMax b)
        {
            /*
            if (a.IsZeroOnInvalid)
                return b;
            if (b.IsZeroOnInvalid)
                return a;
            if (a.max < b.min || a.min > b.Max)
                return new MinMax();
             */
            var aa = new MinMax(Math.Max(a.Min, b.Min), Math.Min(a.Max, b.Max));
            return aa.IsZeroOnInvalid ? new MinMax() : aa;
        }

        public static List<MinMax> Compact(IEnumerable<MinMax> src)
        {
            var aa    = src.Where(a => !a.IsZeroOnInvalid).ToList();
            var again = true;
            while (again)
            {
                again = false;
                for (var i = 0; i < aa.Count; i++)
                for (var j = i + 1; j < aa.Count; j++)
                {
                    MinMax o;
                    if (!aa[i].TryMergeValidRanges(aa[j], out o)) continue;
                    aa[i] = o;
                    aa.RemoveAt(j);
                    again = true;
                }
            }

            aa.Sort(CompareSort);
            return aa;
        }


        private static int CompareSort(MinMax a, MinMax b)
        {
            var i = a.Min.CompareTo(b.Min);
            return i != 0 ? i : a.Max.CompareTo(b.Max);
        }

        public static List<MinMax> Cut(IEnumerable<MinMax> src, IEnumerable<MinMax> cutters)
        {
            // ReSharper disable once InvertIf
            if (cutters != null)
                foreach (var c in cutters)
                {
                    var a = Cut(src, c);
                    src = a;
                }

            return src is List<MinMax> ? src as List<MinMax> : src.ToList();
        }

        public static List<MinMax> Cut(IEnumerable<MinMax> src, MinMax cutter)
        {
            var result = new List<MinMax>();
            foreach (var rr in src)
                result.AddRange(rr.Cut(cutter));
            return result;
        }

        public static IList<MinMax> Cut(IList<MinMax> src, MinMax cutter)
        {
            var result = new List<MinMax>();
            foreach (var rr in src)
                result.AddRange(rr.Cut(cutter));
            return result;
        }

        public static List<MinMax> Cut(MinMax src, IEnumerable<MinMax> cutters) { return Cut(new[] { src }, cutters); }

        public static MinMax From2Values(double a, double b) { return a < b ? new MinMax(a, b) : new MinMax(b, a); }

        public static MinMax FromCenterAndSize(double center, double size)
        {
            size /= 2.0;
            return new MinMax(center - size, center + size);
        }

        [Obsolete]
        public static MinMax FromList<T>(IEnumerable<T> src, Func<T, double> function)
        {
            return new MinMax(src.Select(function));
        }

        public static MinMax FromValues(params double[] a)
        {
            if (a.Length == 0)
                return new MinMax();
            var aMin = a.Min();
            var aMax = a.Max();
            return new MinMax(aMin, aMax);
        }

        public static MinMax FromValues(IEnumerable<double> a)
        {
            var items = a as double[] ?? a.ToArray();
            if (!items.Any())
                return new MinMax();
            var aMin = items.Min();
            var aMax = items.Max();
            return new MinMax(aMin, aMax);
        }

        public static MinMax FromValues(IEnumerable<int> a)
        {
            var enumerable = a as int[] ?? a.ToArray();
            if (!enumerable.Any())
                return new MinMax();
            var aMin = enumerable.Min();
            var aMax = enumerable.Max();
            return new MinMax(aMin, aMax);
        }


        /// <summary>
        ///     Sprawdza czy x znajduje się w domkniętym przedziale min-max. Nie sprawdza kolejności min/max
        /// </summary>
        /// <param name="x">sprawdzana liczba</param>
        /// <param name="min">wartość min zakresu</param>
        /// <param name="max">wartość max zakresu</param>
        /// <returns><c>true</c> jeśli x jest w zakresie min-max</returns>
        public static bool IsInRange(double x, double min, double max) { return x >= min && x <= max; }

        public static List<MinMax> Merge(IEnumerable<MinMax> src, IEnumerable<MinMax> append)
        {
            if (append == null) return src.ToList();
            src = append.Aggregate(src, Merge);
            return src.ToList();
        }

        public static IEnumerable<MinMax> Merge(IEnumerable<MinMax> range)
        {
            if (range == null) return new List<MinMax>();
            var result = range.Where(i => !i.IsZeroOnInvalid).OrderBy(i => i.Min).ToList();
            {
                for (var idx = result.Count - 1; idx > 0; idx--)
                {
                    var prev = result[idx - 1];
                    var next = result[idx];
                    if (prev.Max < next.Min)
                        continue;
                    prev.Max = Math.Max(prev.Max, next.Max);
                    result.RemoveAt(idx);
                }
            }
            return result;
        }

        private static List<MinMax> Merge(IEnumerable<MinMax> src, MinMax append)
        {
            src = src.Where(x => !x.IsZeroOnInvalid);
            if (append.IsZeroOnInvalid)
                return src.ToList();
            var r = new List<MinMax>(src);
            r.Add(append);
            while (true)
            {
                var l = r.Count;
                if (l < 2) return r;
                var wasChange = false;
                for (var i = 0; i < l; i++)
                {
                    var a = r[i];
                    for (var j = i + 1; j < l; j++)
                    {
                        var    b = r[j];
                        MinMax c;
                        if (!a.TryMergeValidRanges(b, out c)) continue;
                        r[i] = c;
                        r.RemoveAt(l - 1);
                        wasChange = true;
                        break;
                    }

                    if (wasChange) break;
                }

                if (!wasChange) break;
            }

            return r;
        }

        public static MinMax operator +(MinMax a, MinMax b)
        {
            if (a.IsZeroOnInvalid) return b.CloneX();
            if (b.IsZeroOnInvalid) return a.CloneX();
            var c = a.CloneX();
            c.Add(b.Min);
            c.Add(b.Max);
            return c;
        }

        public static MinMax operator +(MinMax a, double b)
        {
            return a.IsZeroOnInvalid ? a.CloneX() : new MinMax(a.Min + b, a.Max + b);
        }

        public static MinMax operator /(MinMax a, double b)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (a == null || a.IsZeroOnInvalid || b == 0) return new MinMax();
            return b > 0 ? new MinMax(a.Min / b, a.Max / b) : new MinMax(a.Max / b, a.Min / b);
        }


        public static bool operator ==(MinMax left, MinMax right) { return Equals(left, right); }

        public static bool operator !=(MinMax left, MinMax right) { return !Equals(left, right); }


        public static MinMax operator *(MinMax a, double b)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (a == null || a.IsZeroOnInvalid || b == 0) return new MinMax();
            return b > 0 ? new MinMax(a.Min * b, a.Max * b) : new MinMax(a.Max * b, a.Min * b);
        }

        public static MinMax operator *(MinMax a, MinMax b)
        {
            if (a == null || a.IsZeroOnInvalid || b == null || b.IsZeroOnInvalid) return new MinMax();
            var a1 = Math.Max(a.Min, b.Min);
            var a2 = Math.Min(a.Max, b.Max);
            return a1 > a2 ? new MinMax() : new MinMax(a1, a2);
        }

        public static MinMax operator -(MinMax a, double b)
        {
            return a == null ? null : new MinMax(a.Min - b, a.Max - b);
        }

        public static MinMax operator -(MinMax a) { return a == null ? null : new MinMax(-a.Max, -a.Min); }

        /// <summary>
        ///     Zwraca tablicę przedziałów utworzoną na podstawie listy punktów krańcowych
        /// </summary>
        /// <param name="edges">lista punktów krańcowych</param>
        /// <returns>tablica przedziałów</returns>
        public static MinMax[] RangesFromEdges(IEnumerable<double> edges)
        {
            var edges1 = edges.Distinct().OrderBy(a => a).ToArray();
            var cnt    = edges1.Length - 1;
            if (cnt < 1) return Array.Empty<MinMax>();
            var result = new MinMax[cnt];
            for (var i = 0; i < cnt; i++)
                result[i] = new MinMax(edges1[i], edges1[i + 1]);
            return result;
        }

        public static MinMax[] RangesFromEdges2(IEnumerable<double> edges)
        {
            var    edges1 = edges.Distinct().OrderBy(a => a).ToArray();
            var    maxI   = edges1.Length;
            var    result = new MinMax[maxI + 1];
            MinMax item;
            for (var i = 0; i <= maxI; i++)
            {
                result[i] = item = new MinMax();
                if (i == 0)
                    item.Min = double.MinValue;
                else
                    item.Min = edges1[i - 1];
                if (i < maxI)
                    item.Max = edges1[i];
                else
                    item.Max = double.MaxValue;
            }

            return result;
        }

        public void Add(double x)
        {
            if (Min > Max)
            {
                Min = x;
                Max = x;
            }
            else if (x < Min)
                Min = x;
            else if (x > Max)
                Max = x;
        }

        /*
                public MinMax ExpandInt()
                {
                    return new MinMax(Math.Floor(min), System.Math.Ceiling(max));
                }
        */

        public void Add(params double[] items)
        {
            foreach (var x in items)
                Add(x);
        }

        public void Add(MinMax m)
        {
            if (m.IsInvalid)
                return;
            Add(m.Min);
            Add(m.Max);
        }

        public void Add(IEnumerable<double> items)
        {
            foreach (var x in items)
                Add(x);
        }


        public object Clone() { return new MinMax(Min, Max); }

        public MinMax CloneX() { return new MinMax(Min, Max); }

        public List<MinMax> Cut(IEnumerable<MinMax> cutList)
        {
            var itemsToCut = new List<MinMax> { this };
            if (cutList == null)
                return itemsToCut;
            var minMaxes = cutList as IList<MinMax> ?? cutList.ToArray();
            if (!minMaxes.Any())
                return itemsToCut;

            foreach (var cutter in minMaxes)
            {
                var cuttedResult = new List<MinMax>();
                foreach (var itemToCut in itemsToCut)
                {
                    var o1 = itemToCut.Cut(cutter);
                    cuttedResult.AddRange(o1);
                }

                itemsToCut = cuttedResult;
            }

            return itemsToCut;
        }

        private IEnumerable<MinMax> Cut(MinMax r)
        {
            var result = new List<MinMax>();
            if (r.Max < Min || r.Min > Max)
                result.Add(this); // rozłączne
            else if (r.Min > Min && r.Max < Max)
            {
                // wycina
                result.Add(new MinMax(Min, r.Min));
                result.Add(new MinMax(r.Max, Max));
            }
            else if (r.Min <= Min && r.Max >= Max)
            {
                // całkowicie wycina
                return result;
            }
            else
            {
                if (r.Min > Min)
                    result.Add(new MinMax(Min, Math.Min(Max, r.Min)));
                if (r.Max < Max)
                    result.Add(new MinMax(Math.Max(Min, r.Max), Max));
            }

            return result;
        }


        public double CutToRange(double a) { return a < Min ? Min : a > Max ? Max : a; }


        /// <summary>
        ///     Odległość punktu do bliższego końca zakresu
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double DistanceToNearEnd(double x) { return Math.Min(Math.Abs(x - Min), Math.Abs(x - Max)); }


        public bool Equals(MinMax other)
        {
            if (other == null)
                return false;
            if (IsInvalid && other.IsInvalid)
                return true;
            if (IsInvalid || other.IsInvalid)
                return false;
            return Equals(Min, other.Min) && Equals(Max, other.Max);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return ReferenceEquals(this, obj) || Equals(obj as MinMaxI);
        }

        public override int GetHashCode()
        {
            if (IsZeroOnInvalid)
                return 0;
            return Min.GetHashCode() ^ Max.GetHashCode();
        }

        public MinMax Grow(double margin)
        {
            Min = Min - margin;
            Max = Max + margin;
            return this;
        }

        public void Grow(double marginMin, double marginMax)
        {
            Min = Min - marginMin;
            Max = Max + marginMax;
        }

        /// <summary>
        ///     Czy ten i inny zakres mają kawałek wspólny
        /// </summary>
        /// <param name="o">inny zakres</param>
        /// <param name="acceptPointOnly">akceptuje także stykanie się końcami</param>
        /// <returns></returns>
        public bool HasCommonAreaWith(MinMax o, bool acceptPointOnly)
        {
            if (o == null) return false;
            if (acceptPointOnly)
            {
                throw new NotImplementedException("Uzupełnić");
            }

            if (o.IsZeroOnInvalid || IsZeroOnInvalid)
                return false;
            return !(o.Max < Min) && !(o.Min > Max);
        }

        public bool HasCommonRangeWithPositiveLength(double min, double max) { return max > Min && min < Max; }

        public bool HasCommonRangeWithPositiveLength(Range q) { return q.Max > Min && q.Min < Max; }

        /// <summary>
        ///     Czy liczba zawiera się w przedziale domkniętym [min;max]
        /// </summary>
        /// <param name="x">liczba</param>
        /// <returns><c>true</c> jeśli liczba jest w przedziale domkniętym [min;max] </returns>
        public bool Includes(double x) { return x >= Min && x <= Max; }

        /// <summary>
        ///     Czy liczba zawiera się w przedziale otwartym [min;max]
        /// </summary>
        /// <param name="x">liczba</param>
        /// <returns><c>true</c> jeśli liczba jest w przedziale otwartym [min;max] </returns>
        public bool IncludesExclusive(double x) { return x > Min && x < Max; }

        public double InsideOrEnd(double x) { return x < Min ? Min : x > Max ? Max : x; }

        public MinMax Intersection(MinMax other)
        {
            if (IsZeroOnInvalid || other.IsZeroOnInvalid)
                return new MinMax(0, -1);
            return new MinMax(
                Math.Max(Min, other.Min),
                Math.Min(Max, other.Max)
            );
        }

        public bool IsInsideInclusive(double x) { return x >= Min && x <= Max; }

        public bool IsInsideInclusive(MinMax x) { return x.Min >= Min && x.Max <= Max; }

        /// <summary>
        ///     Mapuje wartość X na przedział 0 - 1, gdzie jeśli x=xmin => 0 , x=xmax => 1
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double MapTo01(double x) { return (x - Min) / (Max - Min); }

        /// <summary>
        ///     Mapuje wartość X na przedział -1 - +1, gdzie jeśli x=xmin => -1 , x=xmax => +1
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double MapTo11(double x) { return (x - Min) / (Max - Min) * 2 - 1; }

        public double MapToAny(double x, double aMin, double aMax)
        {
            x = MapTo01(x);
            return (aMax - aMin) * x + aMin;
        }

        public MinMaxI Round() { return new MinMaxI((int)Math.Round(Min), (int)Math.Round(Max)); }

        public bool ShouldSerializeCenter() { return false; }

        public bool ShouldSerializeHasPositiveLength() { return false; }

        public bool ShouldSerializeIsEmpty() { return false; }

        public bool ShouldSerializeIsZeroOnInvalid() { return false; }

        public bool ShouldSerializeLength() { return false; }

        public override string ToString()
        {
            return string.Format("{2}MinMax {0}..{1}", Min, Max, IsZeroOnInvalid ? "INVALID" : "");
        }

        public bool TryMerge(MinMax other, out MinMax result)
        {
            if (IsZeroOnInvalid)
            {
                // ten jest pusty więc zwracam tego drugiego
                result = other;
                return true;
            }

            result = this;
            return !other.IsZeroOnInvalid && TryMergeValidRanges(other, out result);
        }

        /// <summary>
        ///     Próbuje wykonać sumę zakresów; zakłada, że oba są prawidłowe i niezerowe
        /// </summary>
        /// <param name="other"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool TryMergeValidRanges(MinMax other, out MinMax result)
        {
            // obydwa są prawidłowe
            if (Min > other.Max || Max < other.Min)
            {
                result = this;
                return false;
            }

            result = new MinMax(Math.Min(Min, other.Min), Math.Max(Max, other.Max));
            return true;
        }

        public static MinMax Invalid => new MinMax(0, -1);

        public bool IsZeroOnInvalid => Max <= Min;

        public bool IsInvalid => Max < Min;

        /// <summary>
        ///     Długość przedziału
        /// </summary>
        public double Length => Max - Min;

        public MinMax[] ArrayOfValid
        {
            get { return IsZeroOnInvalid ? Array.Empty<MinMax>() : new[] { this }; }
        }

        public double Center => (Min + Max) / 2;
    }
}
