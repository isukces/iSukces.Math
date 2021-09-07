using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
#if COREFX
using iSukces.Mathematics.Compatibility;
using ThePoint=iSukces.Mathematics.Compatibility.Point;
using TheVector=iSukces.Mathematics.Compatibility.Vector;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif
#if ALLFEATURES
using iSukces.Mathematics.TypeConverters;
#endif

namespace iSukces.Mathematics
{
#if ALLFEATURES
    [TypeConverter(typeof(MinMaxITypeConverter))]
#endif    
   
    public sealed class MinMaxI : MinMaxGeneric<int>, IEquatable<MinMaxI>
    {
        public MinMaxI()
            : base(0, -1)
        {
        }

        public MinMaxI(IEnumerable<int> items)
            : base(0, -1)
        {
            Add(items);
        }

        public MinMaxI(params MinMaxI[] a)
            : base(0, -1)
        {
            foreach (var i in a)
            {
                if (i.IsZeroOnInvalid) continue;
                Add(i.Min);
                Add(i.Max);
            }
        }

        public MinMaxI(int min, int max)
            : base(min, max)
        {
        }

        public static List<MinMaxI> Cut(MinMaxI src, IEnumerable<MinMaxI> cutters)
        {
            return Cut(new[] {src}, cutters);
        }

        public static List<MinMaxI> Cut(IEnumerable<MinMaxI> src, MinMaxI cutter)
        {
            var result = new List<MinMaxI>();
            foreach (var rr in src)
                result.AddRange(rr.Cut(cutter));
            return result;
        }

        public static List<MinMaxI> Cut(IEnumerable<MinMaxI> src, IEnumerable<MinMaxI> cutters)
        {
            if (cutters != null)
                foreach (var c in cutters)
                {
                    var a = Cut(src, c);
                    src = a;
                }
            return src is List<MinMaxI> ? src as List<MinMaxI> : src.ToList();
        }

        public static MinMaxI FromBeginAndLength(int begin, int length)
        {
            return new MinMaxI(begin, begin + length);
        }

        public static MinMaxI FromValues(IEnumerable<int> x)
        {
            var t = new MinMaxI();
            t.Add(x);
            return t;
        }

        public static MinMaxI FromValues(params int[] x)
        {
            var t = new MinMaxI();
            t.Add(x);
            return t;
        }

        public static bool IsInAnyRange(IEnumerable<MinMaxI> ranges, int x)
        {
            return ranges.Any(r => r.Includes(x));
        }

        public static List<MinMaxI> Merge(IEnumerable<MinMaxI> src, MinMaxI append)
        {
            src = src.Where(x => !x.IsZeroOnInvalid);
            if (append.IsZeroOnInvalid)
                return src.ToList();
            var r = new List<MinMaxI>(src) {append};
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
                        var b = r[j];
                        MinMaxI c;
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

        public static List<MinMaxI> Merge(IEnumerable<MinMaxI> src, IEnumerable<MinMaxI> append)
        {
            if (append != null)
                foreach (var a in append)
                    src = Merge(src, a);
            return src.ToList();
        }

        public static MinMaxI operator +(MinMaxI a, int b)
        {
            return a.IsInvalid ? new MinMaxI() : new MinMaxI(a.Min + b, a.Max + b);
        }

        public static bool operator ==(MinMaxI left, MinMaxI right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MinMaxI left, MinMaxI right)
        {
            return !Equals(left, right);
        }


        public static MinMaxI operator *(MinMaxI a, MinMaxI b)
        {
            if (a == null || a.IsInvalid || b == null || b.IsInvalid) return new MinMaxI();
            return new MinMaxI(Math.Max(a.Min, b.Min), Math.Min(a.Max, b.Max));
        }

        public static MinMaxI operator -(MinMaxI mm, int substract)
        {
            if (mm == null || mm.IsInvalid) return mm;
            return new MinMaxI(mm.Min - substract, mm.Max - substract);
        }


         

        public static MinMaxI operator -(MinMaxI mm)
        {
            if (mm == null || mm.IsInvalid) return mm;
            return new MinMaxI(-mm.Max, -mm.Min);
        }

        /// <summary>
        ///     Zwraca tablicę przedziałów utworzoną na podstawie listy punktów krańcowych
        /// </summary>
        /// <param name="edges">lista punktów krańcowych</param>
        /// <returns>tablica przedziałów</returns>
        public static MinMaxI[] RangesFromEdges(IEnumerable<int> edges)
        {
            var edges1 = edges.Distinct().OrderBy(a => a).ToArray();
            var cnt = edges1.Length - 1;
            if (cnt < 1) return new MinMaxI[0];
            var result = new MinMaxI[cnt];
            for (var i = 0; i < cnt; i++)
                result[i] = new MinMaxI(edges1[i], edges1[i + 1]);
            return result;
        }

         

        public void Add(int x)
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

        public void Add(MinMaxI m)
        {
            if (m.IsInvalid)
                return;
            Add(m.Min);
            Add(m.Max);
        }

        public void Add(IEnumerable<int> items)
        {
            foreach (var x in items)
                Add(x);
        }

        public MinMaxI Clone()
        {
            return new MinMaxI(Min, Max);
        }

        public MinMaxI CloneX()
        {
            return new MinMaxI(Min, Max);
        }

        public int CommonAreaWith(MinMaxI a)
        {
            if (a == null || a.IsInvalid || IsInvalid) return 0;
            var p1 = Math.Max(a.Min, Min);
            var p2 = Math.Min(a.Max, Max);
            var d = p2 - p1;
            return d <= 0 ? 0 : d;
        }

        public List<MinMaxI> Cut(IEnumerable<MinMaxI> cutList)
        {
            var itemsToCut = new List<MinMaxI> {this};
            var minMaxes = cutList as MinMaxI[] ?? cutList.ToArray();
            if (cutList == null || !minMaxes.Any())
                return itemsToCut;

            foreach (var cutter in minMaxes)
            {
                var cuttedResult = new List<MinMaxI>();
                foreach (var itemToCut in itemsToCut)
                {
                    var o1 = itemToCut.Cut(cutter);
                    cuttedResult.AddRange(o1);
                }
                itemsToCut = cuttedResult;
            }
            return itemsToCut;
        }

        public IList<MinMaxI> Cut(MinMaxI r)
        {
            var result = new List<MinMaxI>();
            if (r.Max < Min || r.Min > Max)
                result.Add(this); // rozłączne
            else if (r.Min > Min && r.Max < Max)
            {
                // wycina
                result.Add(new MinMaxI(Min, r.Min));
                result.Add(new MinMaxI(r.Max, Max));
            }
            else if (r.Min <= Min && r.Max >= Max)
            {
                // całkowicie wycina
                return result;
            }
            else
            {
                if (r.Min > Min)
                    result.Add(new MinMaxI(Min, Math.Min(Max, r.Min)));
                if (r.Max < Max)
                    result.Add(new MinMaxI(Math.Max(Min, r.Max), Max));
            }
            return result;
        }

        public int CutToRange(int a)
        {
            return a < Min ? Min : a > Max ? Max : a;
        }

        public IEnumerable<int> Enumerate()
        {
            return Min > Max ? new int[0] : Enumerable.Range(Min, Max - Min + 1);
        }

        public bool Equals(MinMaxI other)
        {
            if (other == null)
                return false;
            if (IsInvalid && other.IsInvalid)
                return true;
            if (IsInvalid || other.IsInvalid)
                return false;
            return Min == other.Min && Max == other.Max;
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

        /// <summary>
        ///     Czy ten i inny zakres mają kawałek wspólny
        /// </summary>
        /// <param name="o">inny zakres</param>
        /// <param name="acceptPointOnly">akceptuje także stykanie się końcami</param>
        /// <returns></returns>
        public bool HasCommonAreaWith(MinMaxI o, bool acceptPointOnly)
        {
            if (o == null) return false;
            if (o.IsZeroOnInvalid || IsZeroOnInvalid)
                return false;
            if (acceptPointOnly)
                return Max >= o.Min && o.Max >= Min;
            return !(o.Max < Min) && !(o.Min > Max);
        }


        /// <summary>
        ///     Czy liczba zawiera się w przedziale zamkniętym [min;max]
        /// </summary>
        /// <param name="x">liczba</param>
        /// <returns><c>true</c> jeśli liczba jest w przedziale zamkniętym [min;max] </returns>
        public bool Includes(double x)
        {
            return x >= Min && x <= Max;
        }

        /// <summary>
        ///     Czy liczba zawiera się w przedziale zamkniętym [min;max]
        /// </summary>
        /// <param name="x">liczba</param>
        /// <returns><c>true</c> jeśli liczba jest w przedziale zamkniętym [min;max] </returns>
        public bool Includes(int x)
        {
            return x >= Min && x <= Max;
        }


        /// <summary>
        ///     Czy liczba zawiera się w przedziale otwartym [min;max]
        /// </summary>
        /// <param name="x">liczba</param>
        /// <returns><c>true</c> jeśli liczba jest w przedziale otwartym [min;max] </returns>
        public bool IncludesExclusive(double x)
        {
            return x > Min && x < Max;
        }

        /// <summary>
        ///     Czy liczba zawiera się w przedziale otwartym (min;max)
        /// </summary>
        /// <param name="x">liczba</param>
        /// <returns><c>true</c> jeśli liczba jest w przedziale otwartym (min;max) </returns>
        public bool IncludesExclusive(int x)
        {
            return x > Min && x < Max;
        }

        public bool ShouldSerializeCenter()
        {
            return false;
        }

        public bool ShouldSerializeHasPositiveLength()
        {
            return false;
        }

        public bool ShouldSerializeIsEmpty()
        {
            return false;
        }

        public bool ShouldSerializeIsZeroOnInvalid()
        {
            return false;
        }

        public bool ShouldSerializeLength()
        {
            return false;
        }


        public override string ToString()
        {
            return string.Format("{2}MinMax {0}..{1}", Min, Max, IsZeroOnInvalid ? "INVALID" : "");
        }

        /// <summary>
        ///     Próbuje wykonać sumę zakresów; zakłada, że oba są prawidłowe i niezerowe
        /// </summary>
        /// <param name="other"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryMergeValidRanges(MinMaxI other, out MinMaxI result)
        {
            // obydwa są prawidłowe
            if (Min > other.Max || Max < other.Min)
            {
                result = this;
                return false;
            }
            result = new MinMaxI(Math.Min(Min, other.Min), Math.Max(Max, other.Max));
            return true;
        }

        public static MinMaxI Full
        {
            get { return new MinMaxI(int.MinValue, int.MaxValue); }
        }

        public MinMaxI[] ArrayOfValid
        {
            get { return IsZeroOnInvalid ? new MinMaxI[0] : new[] {this}; }
        }

        public int Center
        {
            get { return (Min + Max) / 2; }
        }

        public bool IsInvalid
        {
            get { return Max < Min; }
        }

        public bool IsZeroOnInvalid
        {
            get { return Max <= Min; }
        }

        /// <summary>
        ///     Długość przedziału
        /// </summary>
        public int Length
        {
            get { return Max - Min; }
        }
    }
}