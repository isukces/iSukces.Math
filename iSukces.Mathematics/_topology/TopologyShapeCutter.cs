using System;
using System.Collections.Generic;
using System.Linq;
#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows;
#endif

namespace iSukces.Mathematics
{
    /// <summary>
    ///     Klasa wyliczająca przecięcia kształtów
    /// </summary>
    public sealed class TopologyShapeCutter  
    {
        /// <summary>
        ///     Obcina listę trójkątów przy pomocy linii - jedna część jest odrzucona
        /// </summary>
        /// <param name="src">Lista trójkątów</param>
        /// <param name="line">obcinacz</param>
        /// <returns></returns>
        public static TopologyTriangle[] CutByLine(TopologyTriangle[] src, TopologyTriangleLine line)
        {
            if (line == null)
                return src;
            return (from src1 in src
                from o in CutByLine(src1, line)
                select o).ToArray();
        }

        /// <summary>
        ///     Obcina listę trójkątów przy pomocy linii - jedna część jest odrzucona
        /// </summary>
        /// <param name="src">Lista trójkątów</param>
        /// <param name="c">obcinacz</param>
        /// <returns></returns>
        public static TopologyTriangle[] CutByLine(TopologyTriangle src, TopologyTriangleLine line)
        {
            if (line == null)
                return new[] {src};
            var p1 = TopologyTriangleLine.CrossByLL(src.LineAB, line);
            var p2 = TopologyTriangleLine.CrossByLL(src.LineAC, line);
            var p3 = TopologyTriangleLine.CrossByLL(src.LineBC, line);

            var nnItems = new[] {p1, p2, p3}.Where(Q => Q != null).ToArray();
            var dcp     = nnItems.Select(Q => Q.CrossPoint).Distinct().ToArray();
            if (dcp.Length < 2)
            {
                // brak przecięć albo cały albo pusty
                if (line.IsOnDarkSide(src.Center)) return Array.Empty<TopologyTriangle>();
                return new[] {src}; // cały
            }

            if (dcp.Length > 2)
                throw new NotSupportedException();
            // mamy 2 różne punkty przecięcia
            var nonDarkPoints = (from Point in new[] {src.PA, src.PB, src.PC}
                where !line.IsOnDarkSide(Point)
                select Point).ToArray();
            if (nonDarkPoints.Length == 3)
                return new[] {src}; // cały
            if (nonDarkPoints.Length == 1)
                // brak określenia, które krawędzie są uktyte - uzupełnić
                return TopologyShape.FromPoints(nonDarkPoints[0], dcp[0], dcp[1]).Triangles.ToArray();
            {
                var v1 = nonDarkPoints[0];
                var v2 = nonDarkPoints[1];
                var t1 = dcp[0];
                var t2 = dcp[1];
                {
                    // 3 lub mniej unikalnych punktów
                    var up = new[] {v1, v2, t1, t2}.Distinct().ToArray();
                    if (up.Length == 3)
                        return TopologyShape.FromPoints(up[0], up[1], up[2]).Triangles.ToArray();
                    if (up.Length < 3)
                        return Array.Empty<TopologyTriangle>();
                }
                var i1 = Vector.CrossProduct(v2 - v1, t2 - v2);
                var i2 = Vector.CrossProduct(t2 - v1, t1 - t2);
                if (System.Math.Sign(i1) != System.Math.Sign(i2))
                    // if ((v1 - t1).Length > (v1 - t2).Length)
                    Swap(ref t1, ref t2);
                // brak określenia, które krawędzie są uktyte - uzupełnić
                return TopologyShape.FromPoints(v1, v2, t2, t1).Triangles.ToArray();
            }
        }

        public static void SomeTest()
        {
            var a    = TopologyShape.FromPoints(new Point(0, 0), new Point(40, 0), new Point(41, 20));
            var line = new TopologyTriangleLine(new Point(100, 10), new Point(-100, 9), false);
            var b    = CutByLine(a.Triangles[0], line);
        }

        private static void Addx(IList<TopologySideCross> l, TopologySideCross p)
        {
            if (!ReferenceEquals(p, null))
                l.Add(p);
        }

        private static Point[] Find2And1(Point a, Point b, Point c, Point d)
        {
            if (a.Equals(c))
                return new[] {a, b, d};
            if (a.Equals(d))
                return new[] {a, b, c};
            if (b.Equals(c))
                return new[] {b, a, d};
            if (b.Equals(d))
                return new[] {b, a, c};
            throw new NotSupportedException();
        }

        private static Point[] Find2And1(TopologyTriangleLine a, TopologyTriangleLine b)
        {
            return Find2And1(a.PointA, a.PointB, b.PointA, b.PointB);
        }

        private static void OrderByCountDesc(ref List<TopologySideCross> a, ref List<TopologySideCross> b,
            ref List<TopologySideCross>
                c)
        {
            List<TopologySideCross>[] tmp = {a, b, c};
            tmp = tmp.OrderBy(x => x.Count).ToArray();
            // if (tmp[0].Count != 0 || tmp[1].Count != 2 || tmp[2].Count != 2)
            //  throw new NotSupportedException();
            c = tmp[0];
            b = tmp[1];
            a = tmp[2];
        }

        private static void Situation_111(List<TopologySideCross> aa, List<TopologySideCross> bb,
            List<TopologySideCross> cc)
        {
        }

        private static void Swap(ref List<TopologySideCross> aa, ref List<TopologySideCross> bb)
        {
            var tmp = aa;
            aa = bb;
            bb = tmp;
        }

        private static void Swap(ref Point a, ref Point b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }

        public void CutTriangleByTriangle(TopologyTriangle t, TopologyTriangle tc)
        {
            _tc = tc;
            _t  = t;
            {
                var tB = t.Boundings;
                var cB = tc.Boundings;
                if (tB.Left >= cB.Right || cB.Left >= tB.Right || tB.Top >= cB.Bottom || cB.Top >= tB.Bottom)
                {
                    Output.AddTriangle(t.PA, t.PB, t.PC);
                    return;
                }
            }

            if (t.LineAB.TriangleOnMinusOrZeroSide(tc) || t.LineAC.TriangleOnMinusOrZeroSide(tc) ||
                t.LineBC.TriangleOnMinusOrZeroSide(tc))
            {
                Output.AddTriangle(t.PA, t.PB, t.PC);
                return;
            }

            _insidePoints = t.GetInsidePoints(tc).ToArray();
            var _ab       = GetCuttings(t.LineAB);
            var _ac       = GetCuttings(t.LineAC);
            var _bc       = GetCuttings(t.LineBC);
            var situation = _ab.Count + 10 * _ac.Count + 100 * _bc.Count;
            // DeveloperLog.Log(string.Format("cut {0} by {1} sit {2} internas {3}", t, tc, situation, insidePoints.Length));
            switch (situation)
            {
                case 0:
                    Situation_0();
                    return;
                case 002:
                    Situation_200(_ab, t.PC); // ab
                    return;
                case 020:
                    Situation_200(_ac, t.PB); // ac
                    return;
                case 200:
                    Situation_200(_bc, t.PA); // bc
                    return;

                case 222:
                    Situation_222(_ab, _ac, _bc);
                    return;

                case 111:
                    Situation_111(_ac, _ab, _bc);
                    return;
                case 110:
                    Situation_110(_ac, _bc); // ab
                    return;
                case 101:
                    Situation_110(_ab, _bc); // ac
                    return;
                case 011:
                    Situation_110(_ab, _ac); // bc
                    return;

                case 223:
                    Situation_322(_ab, _ac, _bc); // ab
                    return;
                case 232:
                    Situation_322(_ac, _ab, _bc); // ac
                    return;
                case 322:
                    Situation_322(_bc, _ab, _ac); // bc
                    return;

                case 003:
                    Situation_300(_ab); // ab
                    return;
                case 030:
                    Situation_300(_ac); // ac
                    // throw new Exception();
                    return;
                case 300:
                    Situation_300(_bc); // bc
                    return;

                case 220:
                    Situation_220(_ab, _ac, _bc); // ab = 0
                    return;
                case 202:
                    Situation_220(_ab, _bc, _ac); // ac
                    return;
                case 022:
                    Situation_220(_ab, _ac, _bc); // bc
                    return;

                case 332:
                    Situation_233(_ab, _ac, _bc); // ab
                    return;
                case 323:
                    Situation_233(_ac, _ab, _bc); // ac
                    return;
                case 233:
                    Situation_233(_bc, _ab, _ac); // bc
                    return;

                case 221:
                    Situation_221(_ab, _ac, _bc);
                    return;
                case 212:
                    Situation_221(_ab, _ac, _bc);
                    return;
                case 122:
                    Situation_221(_ab, _ac, _bc);
                    return;

                case 023:
                    Situation_320(_ab, _ac, _bc);
                    return;
                case 203:
                    Situation_320(_ab, _ac, _bc);
                    return;
                case 230:
                    Situation_320(_ab, _ac, _bc);
                    return;
                case 032:
                    Situation_320(_ab, _ac, _bc);
                    return;
                case 302:
                    Situation_320(_ab, _ac, _bc);
                    return;
                case 320:
                    Situation_320(_ab, _ac, _bc);
                    return;

                case 112:
                    Situation_112(_ab, _ac, _bc);
                    return;
                case 121:
                    Situation_112(_ab, _ac, _bc);
                    return;
                case 211:
                    Situation_112(_ab, _ac, _bc);
                    return;

                case 321:
                    Situation_321(_ab, _ac, _bc);
                    return;
                case 312:
                    Situation_321(_ab, _ac, _bc);
                    return;
                case 213:
                    Situation_321(_ab, _ac, _bc);
                    return;
                case 231:
                    Situation_321(_ab, _ac, _bc);
                    return;
                case 123:
                    Situation_321(_ab, _ac, _bc);
                    return;
                case 132:
                    Situation_321(_ab, _ac, _bc);
                    return;
            }

            _ab = GetCuttings(t.LineAB);
            _ac = GetCuttings(t.LineAC);
            _bc = GetCuttings(t.LineBC);
            throw new NotImplementedException(situation.ToString());
        }

        public void Situation_0()
        {
            // TopologyTriangleLine l = new TopologyTriangleLine(
            if (_insidePoints.Length == 3)
            {
                SimpleTriangle t1, t2;
                if (_t.Area > 0)
                    t1 = new SimpleTriangle {PA = _t.PA, PB = _t.PB, PC = _t.PC};
                else
                    t1 = new SimpleTriangle {PA = _t.PA, PB = _t.PC, PC = _t.PB};

                if (_tc.Area > 0)
                    t2 = new SimpleTriangle {PA = _tc.PA, PB = _tc.PB, PC = _tc.PC};
                else
                    t2 = new SimpleTriangle {PA = _tc.PA, PB = _tc.PC, PC = _tc.PB};
                for (var i = 0; i < 3; i++)
                {
                    var st = new SimpleTriangle[3];
                    for (var j = 0; j < 3; j++)
                    {
                        var l = t2.GetLine(i + j + 1);
                        var p = t1.GetPoint(j);
                        st[j] = new SimpleTriangle {PA = p, PB = l.PointB, PC = l.PointA};
                    }

                    if (st[0].Area > 0 && st[1].Area > 0 && st[2].Area > 0)
                    {
                        for (var j = 0; j < 3; j++)
                            Output.AddTriangle(st[j].PA, st[j].PB, st[j].PC);
                        Output.AddTriangle(st[0].PA, st[0].PB, st[1].PA);
                        Output.AddTriangle(st[1].PA, st[1].PB, st[2].PA);
                        Output.AddTriangle(st[2].PA, st[2].PB, st[0].PA);
                        return;
                    }
                }

                return;
            }

            throw new NotSupportedException("ZERO Z");
        }

        private void AddTrianglesFor5(Point center, Point a, Point b, Point c, Point d)
        {
            // dodajemy 3 trójkąty do pięcioboku. center jest wspólnym wierzchołkiem wszystkich z nich
            Output.AddTriangle(center, a, b);
            Output.AddTriangle(center, b, c);
            Output.AddTriangle(center, c, d);
        }

        private void AddTrianglesFor6(Point center, Point a, Point b, Point c, Point d, Point e)
        {
            Output.AddTriangle(center, a, b);
            Output.AddTriangle(center, b, c);
            Output.AddTriangle(center, c, d);
            Output.AddTriangle(center, d, e);
        }

        private List<TopologySideCross> GetCuttings(TopologyTriangleLine line)
        {
            var p1 = TopologyTriangleLine.Cross(line, _tc.LineAB);
            var p2 = TopologyTriangleLine.Cross(line, _tc.LineAC);
            var p3 = TopologyTriangleLine.Cross(line, _tc.LineBC);
            var r  = new List<TopologySideCross>(3);
            Addx(r, p1);
            Addx(r, p2);
            Addx(r, p3);
            return r;
        }

        private void Situation_110(List<TopologySideCross> listA, List<TopologySideCross> listB)
        {
            var cA = listA[0];
            var cB = listB[0];

            Point c, b, a;
            {
                var abc = Find2And1(cA.First, cB.First);
                c = abc[0];
                a = cA.First.GetOtherVertex(c);
                b = cB.First.GetOtherVertex(c);
            }
            if (_insidePoints.Length == 1)
            {
                AddTrianglesFor5(_insidePoints[0], cA.CrossPoint, a, b, cB.CrossPoint);
                return;
            }

            if (_insidePoints.Length != 0)
                throw new NotSupportedException("110 a");

            if (_tc.IsInside(c))
                Output.AddRectangle(b, cB.CrossPoint, cA.CrossPoint, a);
            else
                Output.AddTriangle(c, cA.CrossPoint, cB.CrossPoint);
        }

        private void Situation_112(List<TopologySideCross> sa, List<TopologySideCross> sb, List<TopologySideCross> d)
        {
            OrderByCountDesc(ref d, ref sa, ref sb);
            if (_insidePoints.Length == 0)
            {
                var c  = _t.GetOtherPointOfTrangle(d[0].First);
                var a  = sa[0].First.GetOtherVertex(c);
                var b  = sb[0].First.GetOtherVertex(c);
                var tt = d.OrderBy(x => (x.CrossPoint - a).LengthSquared).ToArray();
                // throw new NotSupportedException("112 a");
                Output.AddTriangle(a, sa[0].CrossPoint, tt[0].CrossPoint);
                Output.AddTriangle(b, sb[0].CrossPoint, tt[1].CrossPoint);
                return;
            }

            throw new NotSupportedException("112 z");
        }

        private void Situation_200(List<TopologySideCross> cl, Point c)
        {
            var l = cl.First().First;
            var a = l.PointA;
            var b = l.PointB;
            if (_insidePoints.Length == 0)
            {
                // dwa przecięcia na jednym tylko boku i brak punktów w środku - wierzchołki
                Output.AddTriangle(a, b, c);
                return;
            }

            if (_insidePoints.Length == 1)
            {
                // posortowane punkty na boku ab według odległości od punktu a
                var tmp = cl.OrderBy(x => (x.CrossPoint - a).Length).ToArray();
                var i   = _insidePoints[0];
                Output.AddRectangle(i, tmp[0].CrossPoint, a, c);
                Output.AddRectangle(i, c, b, tmp[1].CrossPoint);
                return;
            }

            if (_insidePoints.Length == 2)
            {
                var sorted = cl.OrderBy(x => (x.CrossPoint - a).LengthSquared).ToArray();
                Output.AddTriangle(c, _insidePoints[0], _insidePoints[1]);
                if (sorted[0].Second.IsVertex(_insidePoints[0]))
                {
                    Output.AddRectangle(_insidePoints[0], sorted[0].CrossPoint, a, c);
                    Output.AddRectangle(_insidePoints[1], sorted[1].CrossPoint, b, c);
                }
                else
                {
                    Output.AddRectangle(_insidePoints[1], sorted[0].CrossPoint, a, c);
                    Output.AddRectangle(_insidePoints[0], sorted[1].CrossPoint, b, c);
                }

                return;
            }

            throw new NotSupportedException();
        }

        private void Situation_220(List<TopologySideCross> dual1, List<TopologySideCross> dual2, List<TopologySideCross>
            single)
        {
            OrderByCountDesc(ref dual1, ref dual2, ref single);

            if (_insidePoints.Length == 0)
            {
                Situation_220_NoInsidePoints(dual1, dual2);
                return;
            }

            if (_insidePoints.Length != 1)
                throw new NotSupportedException();

            TopologySideCross[] allcross = {dual1[0], dual1[1], dual2[0], dual2[1]};
            var grupy = allcross.GroupBy(x => x.Second).OrderBy(x => -x.Count()).Select(x => x.Key).ToArray();
            var dualPointLine = grupy[0];
            var dwapunktyNaJednymKrosie = allcross.Where(x => x.Second.Equals(dualPointLine)).ToArray();

            Point a, b, c;
            {
                var abc = Find2And1(dual1[0].First, dual2[0].First);
                a = abc[0];
                b = abc[1];
                c = abc[2];
            }
            var aa = dualPointLine.RelativeDistance(_insidePoints[0]);
            var bb = dualPointLine.RelativeDistance(a);
            if (aa > 0 && bb < 0 || aa < 0 && bb > 0)
            {
                // wierzchołek trójkąta wycinającego jest po drugiej stronie linii tnącej niż wierzchołek a
                Output.AddTriangle(dwapunktyNaJednymKrosie[0].CrossPoint, dwapunktyNaJednymKrosie[1].CrossPoint, a);
                var pozostale = allcross.Where(x => !x.Second.Equals(dualPointLine)).ToArray();
                if (pozostale[0].First.IsVertex(b))
                    AddTrianglesFor5(_insidePoints[0], pozostale[0].CrossPoint, b, c, pozostale[1].CrossPoint);
                else
                    AddTrianglesFor5(_insidePoints[0], pozostale[0].CrossPoint, c, b, pozostale[1].CrossPoint);
            }
            else
            {
                var pozostale = allcross.Where(x => !x.Second.Equals(dualPointLine)).ToArray();
                if (dwapunktyNaJednymKrosie[0].First.IsVertex(b))
                    Output.AddRectangle(dwapunktyNaJednymKrosie[0].CrossPoint, b, c,
                        dwapunktyNaJednymKrosie[1].CrossPoint);
                else
                    Output.AddRectangle(dwapunktyNaJednymKrosie[1].CrossPoint, b, c,
                        dwapunktyNaJednymKrosie[0].CrossPoint);
                Output.AddRectangle(a, pozostale[0].CrossPoint, _insidePoints[0], pozostale[1].CrossPoint);
            }
        }

        private void Situation_220_NoInsidePoints(List<TopologySideCross> dual1, List<TopologySideCross> dual2)
        {
            var dwaJedenJeden = Find2And1(dual1[0].First, dual2[0].First);
            var c             = dwaJedenJeden[0];

            var tmpA = dual1.OrderBy(x => (x.CrossPoint - c).LengthSquared).ToArray();
            var tmpB = dual2.OrderBy(x => (x.CrossPoint - c).LengthSquared).ToArray();

            Output.AddTriangle(tmpA[0].CrossPoint, tmpB[0].CrossPoint, c);

            var a1  = tmpA[0].First.GetOtherVertex(c);
            var b1  = tmpB[0].First.GetOtherVertex(c);
            var aa1 = Output.AddRectangle(a1, tmpA[1].CrossPoint, tmpB[1].CrossPoint, b1);
            // var aa2 = output.AddRectangle(a1, tmpA[0].CrossPoint, tmpB[0].CrossPoint, b1);
        }

        private void Situation_221(List<TopologySideCross> dual1, List<TopologySideCross> dual2, List<TopologySideCross>
            single)
        {
            OrderByCountDesc(ref dual1, ref dual2, ref single);
            if (_insidePoints.Length != 0)
                throw new NotSupportedException("221 a");
            var points = Find2And1(dual1[0].First, dual2[0].First);
            var c      = points[0];
            var tmp2   = dual1.Concat(dual2).Where(x => !x.IsCrossVertexOfFirst).ToArray();
            if (tmp2.Length == 2)
            {
                Output.AddTriangle(tmp2[0].CrossPoint, tmp2[1].CrossPoint, c);
                return;
            }

            if (tmp2.Length == 0)
            {
                var tmp = single[0];
                if (!_tc.IsInside(tmp.First.PointA))
                    Output.AddTriangle(c, tmp.CrossPoint, tmp.First.PointA);
                else
                    Output.AddTriangle(c, tmp.CrossPoint, tmp.First.PointB);
                return;
                // wynikiem jest jeden trójkąt
            }

            throw new NotSupportedException("221 d");
        }

        private void Situation_222(List<TopologySideCross> aa, List<TopologySideCross> bb, List<TopologySideCross> cc)
        {
            if (_insidePoints.Length != 0)
                throw new NotSupportedException();
            if (!bb[0].CrossPoint.Equals(bb[1].CrossPoint))
                Swap(ref aa, ref bb);
            else if (!cc[0].CrossPoint.Equals(cc[1].CrossPoint))
                Swap(ref aa, ref cc);
            // aa.first to bok zawierający rzeczywiste przecięcia
            var b = aa[0].First.PointA;
            var c = aa[0].First.PointB;
            var a = bb[0].CrossPoint; // wspólny wierzchołek

            var sorted = aa.Select(x => x.CrossPoint).OrderBy(x => (x - b).LengthSquared).ToArray();
            Output.AddTriangle(a, sorted[0], b);
            Output.AddTriangle(a, sorted[1], c);
        }

        private void Situation_233(List<TopologySideCross> dual, List<TopologySideCross> three1, List<TopologySideCross>
            three2)
        {
            if (_insidePoints.Length == 0)
            {
                var tt = three1.Where(x => !x.IsCrossVertexOfFirst).Concat(three2.Where(x => !x.IsCrossVertexOfFirst))
                    .ToArray();
                if (tt.Length != 1) throw new NotSupportedException();
                var cross = tt[0];
                var eq1   = cross.Second.RelativeDistance(cross.First.PointA) > 0;
                var eq2   = cross.Second.RelativeDistance(cross.First.PointB) > 0;
                if (!(eq1 ^ eq2))
                    throw new NotSupportedException();
                var a = eq2 ? cross.First.PointA : cross.First.PointB;
                var c = _t.GetOtherPointOfTrangle(cross.First.PointA, cross.First.PointB);
                Output.AddTriangle(a, c, cross.CrossPoint);
                return;
            }

            throw new NotSupportedException();
        }

        private void Situation_300(List<TopologySideCross> aa)
        {
            var a = aa[0].First.PointA;
            var b = aa[0].First.PointB;
            if (_insidePoints.Length == 1)
            {
                var aas = aa.Select(x => x.CrossPoint).Where(x => !x.Equals(a) && !x.Equals(b)).Distinct()
                    .OrderBy(x => (x - a).LengthSquared).ToArray();
                //var tmp1 = (new Point[] { t.PA, t.PB, t.PC }).Where(x => !x.Equals(a) && !x.Equals(b)).ToArray();
                // if (tmp1.Length != 1) throw new NotSupportedException();
                var c = _t.GetOtherPointOfTrangle(a, b);
                AddTrianglesFor6(_insidePoints[0], aas[0], a, c, b, aas[1]);
                return;
            }

            throw new NotSupportedException();
        }

        private void Situation_320(List<TopologySideCross> cl, List<TopologySideCross> c2)
        {
            var l = cl.First().First;
            var a = l.PointA;
            var b = l.PointB;
            var c = _t.GetOtherPointOfTrangle(a, b);
            if (_insidePoints.Length == 0)
            {
                var l2 = c2.First().First;
                if (!l2.IsVertex(a))
                {
                    var tmpa = a;
                    a = b;
                    b = tmpa;
                }

                var c2sorted = c2.Select(x => x.CrossPoint).OrderBy(x => (x - a).LengthSquared).ToArray();
                var tmp      = cl.OrderBy(x => x.IsCrossVertexOfSecond).ToArray();
                Output.AddTriangle(a, c2sorted[0], tmp[0].CrossPoint);
                Output.AddRectangle(b, c, c2sorted[1], tmp[2].CrossPoint);
                return;
            }

            if (_insidePoints.Length == 1)
            {
                // poszukiwanie tego wspólnego wierzchołka
                var grupy = cl.GroupBy(x => x.CrossPoint).ToDictionary(x => x.Count(), x => x.Key);
                if (!grupy.ContainsKey(1) || !grupy.ContainsKey(2)) throw new NotSupportedException();
                var commonVertex = grupy[2];
                if (!commonVertex.Equals(a) && !commonVertex.Equals(b))
                    throw new NotSupportedException();
                if (commonVertex.Equals(b))
                    Swap(ref a, ref b);
                // wp jest równe a
                var insidePoint = _insidePoints[0];
                var crossed     = grupy[1];
                Output.AddTriangle(insidePoint, crossed, b);
                Output.AddTriangle(insidePoint, b, c);
                Output.AddTriangle(insidePoint, c, a);
                return;
            }

            throw new NotSupportedException();
        }

        private void Situation_320(List<TopologySideCross> a, List<TopologySideCross> b, List<TopologySideCross> c)
        {
            List<TopologySideCross>[] tmp = {a, b, c};
            var                       z   = tmp.OrderBy(x => x.Count).ToArray();
            if (z[0].Count == 0 && z[1].Count == 2 && z[2].Count == 3)
            {
                Situation_320(z[2], z[1]);
                return;
            }

            throw new NotSupportedException("320");
        }

        private void Situation_321(List<TopologySideCross> s3, List<TopologySideCross> s2, List<TopologySideCross> s1)
        {
            OrderByCountDesc(ref s3, ref s2, ref s1);
            if (_insidePoints.Length == 0)
            {
                var tmp = s1.Concat(s2).Concat(s3).Where(x => !x.IsCrossVertexOfFirst).ToArray();
                if (tmp.Length == 2)
                {
                    var p = Find2And1(tmp[0].First, tmp[1].First);
                    Output.AddTriangle(p[0], tmp[0].CrossPoint, tmp[1].CrossPoint);
                    return;
                }

                throw new NotSupportedException();
            }

            throw new NotSupportedException();
        }

        private void Situation_322(List<TopologySideCross> cl, List<TopologySideCross> bezPrzeciec,
            List<TopologySideCross>
                dwaPrzeciecia)
        {
            if (_insidePoints.Length == 0)
            {
                {
                    var eq1 = bezPrzeciec[0].CrossPoint.Equals(bezPrzeciec[1].CrossPoint);
                    var eq2 = dwaPrzeciecia[0].CrossPoint.Equals(dwaPrzeciecia[1].CrossPoint);
                    if (!eq1 && !eq2)
                    {
                        // bardzo dziwny przypdek, kiedy wierzchołek trójkąta wycinanego leży na boku trójkąta wycinającego
                        // szukam wierzchołka na zewnątrz
                        var tmp1 = cl.Where(x => x.IsCrossVertexOfFirst).ToArray();
                        if (tmp1.Length != 2) throw new NotSupportedException();
                        var outsidePoint = tmp1[0].CrossPoint.Equals(tmp1[0].First.PointA)
                            ? tmp1[0].First.PointB
                            : tmp1[0].First.PointA;
                        tmp1 = tmp1 = cl.Where(x => !x.IsCrossVertexOfFirst).ToArray();
                        // if (tmp1.Length != 1) throw new NotSupportedException();
                        var cr1 = tmp1[0].CrossPoint;
                        TopologySideCross[] inne =
                        {
                            bezPrzeciec[0],
                            bezPrzeciec[1],
                            dwaPrzeciecia[0],
                            dwaPrzeciecia[1]
                        };
                        tmp1 = inne.Where(x => !x.IsCrossVertexOfFirst).ToArray();
                        if (tmp1.Length != 1) throw new NotSupportedException();
                        var cr2 = tmp1[0].CrossPoint;
                        Output.AddTriangle(outsidePoint, cr1, cr2);
                        return;
                    }

                    if (!(eq1 ^ eq2))
                        throw
                            new NotSupportedException(); // obie są przecięciami w wierzchołkach a może być dokładnie jeden
                    if (eq2)
                    {
                        var tmp = bezPrzeciec;
                        bezPrzeciec   = dwaPrzeciecia;
                        dwaPrzeciecia = tmp;
                    }
                }
                var grupy = cl.GroupBy(x => x.CrossPoint).ToDictionary(x => x.Count(), x => x.Key);
                if (!grupy.ContainsKey(1) || !grupy.ContainsKey(2)) throw new NotSupportedException();

                var a  = bezPrzeciec[0].CrossPoint; // wierzchołek wspólny
                var p1 = grupy[1];
                // x2 zawiera punkty na drugiej linii
                Point b;
                {
                    var lineab = cl[0].First; // linia ab zawiera jedno przecięcie w środku
                    if (lineab.PointA.Equals(a)) b = lineab.PointB;
                    else b                         = lineab.PointA;
                }
                Point c;
                {
                    var linebc = dwaPrzeciecia[0].First;
                    if (linebc.PointA.Equals(b)) c = linebc.PointB;
                    else c                         = linebc.PointA;
                }
                var pkt2 = dwaPrzeciecia.Select(x => x.CrossPoint).OrderBy(x => (x - b).Length).ToArray();
                Output.AddTriangle(pkt2[0], b, p1);
                Output.AddTriangle(a, c, pkt2[1]);
                return;
            }

            if (_insidePoints.Length == 1)
            {
                var p = Find2And1(bezPrzeciec[0].First, dwaPrzeciecia[0].First);
                Output.AddRectangle(p[0], p[1], _insidePoints[0], p[2]);
                return;
                // Point vertex =
            }

            throw new NotSupportedException();
        }

        /// <summary>
        ///     Kształt wyjściowy
        /// </summary>
        public TopologyShape Output { get; set; } = new TopologyShape();

        private Point[] _insidePoints;
        private TopologyTriangle _t;
        private TopologyTriangle _tc;
    }
}