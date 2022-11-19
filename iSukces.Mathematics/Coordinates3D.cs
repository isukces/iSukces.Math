using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
#if COREFX
using ThePoint = iSukces.Mathematics.Compatibility.Point;
using TheVector = iSukces.Mathematics.Compatibility.Vector;
using iSukces.Mathematics.Compatibility;
#else
using System.Windows.Media.Media3D;
using ThePoint = System.Windows.Point;
using TheVector = System.Windows.Vector;
#endif
#if ALLFEATURES
using iSukces.Mathematics.TypeConverters;
#endif


namespace iSukces.Mathematics
{
    /// <summary>
    ///     Reprezentuje układ współrzędnych montażu elementu
    /// </summary>
#if ALLFEATURES
    [TypeConverter(typeof(Coordinates3DTypeConverter))]
#endif
    public sealed class Coordinates3D : ICloneable, INotifyPropertyChanged, IEquatable<Coordinates3D>
    {
        static Coordinates3D()
        {
            XMinusVersor = new Vector3D(-1, 0, 0);
            XVersor      = new Vector3D(1, 0, 0);
            YMinusVersor = new Vector3D(0, -1, 0);
            YVersor      = new Vector3D(0, 1, 0);
            ZMinusVersor = new Vector3D(0, 0, -1);
            ZVersor      = new Vector3D(0, 0, 1);

            RotationX180 = new Coordinates3D(XVersor, YMinusVersor);
            RotationY180 = new Coordinates3D(XMinusVersor, YVersor);
            RotationZ180 = new Coordinates3D(XMinusVersor, YMinusVersor);
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        /// </summary>
        public Coordinates3D()
        {
            _x = XVersor;
            _y = YVersor;
            _z = ZVersor;
        }


        /// <summary>
        ///     Tworzy instancję obiektu
        /// </summary>
        /// <param name="x">kierunek osi X</param>
        /// <param name="y">kierunek osi Y</param>
        public Coordinates3D(Vector3D x, Vector3D y)
        {
            SetFromXY(x, y);
            _origin = new Point3D();
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        /// </summary>
        /// <param name="x">kierunek osi X</param>
        /// <param name="y">kierunek osi Y</param>
        /// <param name="origin">Początek układu współrzędnych</param>
        public Coordinates3D(Vector3D x, Vector3D y, Point3D origin)
        {
            SetFromXY(x, y);
            _origin = origin;
        }


        public Coordinates3D(Vector3D x, Vector3D y, double x0, double y0,
            double z0)
        {
            SetFromXY(x, y);
            _origin = new Point3D(x0, y0, z0);
        }

        // Assume vector length equals 1
        public static Vector3D BeautyVersor(Vector3D v)
        {
            const double one = 1;
#if DEBUG
            if (double.IsNaN(v.X) || double.IsNaN(v.Y) || double.IsNaN(v.Z)) throw new Exception("Błędny wektor");
#endif
            if (v.X == 1 || v.X == -1)
                return new Vector3D(v.X, 0, 0);
            if (v.Y == 1 || v.Y == -1)
                return new Vector3D(0, v.Y, 0);
            if (v.Z == 1 || v.Z == -1)
                return new Vector3D(0, 0, v.Z);

            if (MathEx.PitagorasCSquared(v.X, v.Z) == one)
            {
                v = new Vector3D(v.X, 0, v.Z);
                return v;
            }

            if (MathEx.PitagorasCSquared(v.X, v.Y) == one)
            {
                v = new Vector3D(v.X, v.Y, 0);
                return v;
            }

            if (MathEx.PitagorasCSquared(v.Y, v.Z) == one)
            {
                v = new Vector3D(0, v.Y, v.Z);
                return v;
            }

            return v;
        }

        public static Coordinates3D Coalesce(Coordinates3D value)
        {
            if (value != null) return value;
            return NORMAL;
        }

        /*
        /// <summary>
        /// Deserializuje watrość Coordinates3DBase
        /// </summary>
        /// <param name="value">postać tekstowa</param>
        /// <returns>wartość zdeserializowana</returns>
        [DataSerialize]
        [System.Reflection.Obfuscation(Exclude = true)]
        public static Coordinates3DBase Deserialize(string value)
        {
            if (value == "<NULL>")
                return null;
            var m = Regex2.Match(value, @"\[(.+)\];\[(.+)\];\[(.+)\];\[(.+)\]");
            if (m == (object)null)
                return null;
            SerializerInfo si;
            if (!DataObjectManager.Instance.Serializers.TryGetValue(typeof(Vector3D), out si))
                return null;

            Vector3D x = si.Deserialize<Vector3D>(m[1]);
            Vector3D y = si.Deserialize<Vector3D>(m[2]);

            if (!DataObjectManager.Instance.Serializers.TryGetValue(typeof(Point3D), out si))
                return null;
            Point3D p = si.Deserialize<Point3D>(m[4]);

            return new Coordinates3DBase(x, y, p);
        }
        */

        public static Coordinates3D From2Points(Point3D a, Point3D b, Vector3D vx)
        {
            return FromZXO(b - a, vx, a);
        }

        public static Coordinates3D FromFreeRotate(Vector3D v, double angleDeg)
        {
            var co = MathEx.CosDeg(angleDeg);
            var si = MathEx.SinDeg(angleDeg);
            var x = new Vector3D(
                v.X * v.X * (1 - co) + co,
                v.X * v.Y * (1 - co) + v.Z * si,
                v.X * v.Z * (1 - co) - v.Y * si
            );

            var y = new Vector3D(
                v.X * v.Y * (1 - co) - v.Z * si,
                v.Y * v.Y * (1 - co) + co,
                v.Z * v.Y * (1 - co) + v.X * si
            );
            return new Coordinates3D(x, y);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coordinates3D FromZRotationDegrees(double angleDeg)
        {
            return FromZRotationRadians(angleDeg * MathEx.DEGTORAD);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coordinates3D FromZRotationRadians(double angle)
        {
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            return FromZXO(ZVersor, new Vector3D(cos, sin, 0));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coordinates3D FromXRotationRadians(double angle)
        {
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            return new Coordinates3D(XVersor, new Vector3D(0, cos, sin));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coordinates3D FromXRotationDegrees(double angle)
        {
            return FromXRotationRadians(angle * MathEx.DEGTORAD);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coordinates3D FromYRotationRadians(double angle)
        {
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            return FromYZO(YVersor, new Vector3D(sin, 0, cos));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coordinates3D FromYRotationDegrees(double angle)
        {
            return FromYRotationRadians(angle * MathEx.DEGTORAD);
        }


        public static Coordinates3D FromTranslate(double dx, double dy, double dz)
        {
            return new Coordinates3D(XVersor, YVersor, new Point3D(dx, dy, dz));
        }

        public static Coordinates3D FromTranslate(Vector3D translateVector)
        {
            return new Coordinates3D(XVersor, YVersor, (Point3D)translateVector);
        }
        
        public static Coordinates3D FromTranslate(Point3D translateVector)
        {
            return new Coordinates3D(XVersor, YVersor, translateVector);
        }

        
        public static Coordinates3D FromYXO(Vector3D yVector, Vector3D xVector, Point3D o = new Point3D())
        {
            var r = new Coordinates3D
            {
                _y = MakeVersor(yVector)
            };
            r._z = MakeVersor(xVector, r._y);
            r._x = MakeVersor(r._y, r._z);
            r._origin = o;
            return r;
        }
        
        public static Coordinates3D FromXZO(Vector3D xVector, Vector3D zVector, Point3D o = new Point3D())
        {
            var r = new Coordinates3D();
            r._x      = MakeVersor(xVector);
            r._y      = MakeVersor(zVector, r._x);
            r._z      = MakeVersor(r._x, r._y);
            r._origin = o;
            return r;
        }

        public static Coordinates3D FromXZO(Vector3D xVector, Vector3D zVector, double x, double y,
            double z)
        {
            var r = new Coordinates3D();
            r._x      = MakeVersor(xVector);
            r._y      = MakeVersor(zVector, r._x);
            r._z      = MakeVersor(r._x, r._y);
            r._origin = new Point3D(x, y, z);
            return r;
        }

        public static Coordinates3D FromYZO(Vector3D y, Vector3D z, Point3D o)
        {
            var r = new Coordinates3D();
            r._y      = MakeVersor(y);
            r._x      = MakeVersor(r._y, z);
            r._z      = MakeVersor(r._x, r._y);
            r._origin = o;
            return r;
        }

        public static Coordinates3D FromYZO(Vector3D y, Vector3D z)
        {
            var r = new Coordinates3D();
            r._y = MakeVersor(y);
            r._x = MakeVersor(r._y, z);
            r._z = MakeVersor(r._x, r._y);
            return r;
        }

        public static Coordinates3D FromYZO(Vector3D y, Vector3D z, double xo, double yo,
            double zo)
        {
            var r = new Coordinates3D();
            r._y      = MakeVersor(y);
            r._x      = MakeVersor(r._y, z);
            r._z      = MakeVersor(r._x, r._y);
            r._origin = new Point3D(xo, yo, zo);
            return r;
        }

        public static Coordinates3D FromZXO(Vector3D z, Vector3D x, Point3D o = new Point3D())
        {
            var r = new Coordinates3D();
            r._z      = MakeVersor(z);
            r._y      = MakeVersor(r._z, x);
            r._x      = MakeVersor(r._y, r._z);
            r._origin = o;
            return r;
        }

        public static Coordinates3D FromZYO(Vector3D z, Vector3D y, Point3D o = new Point3D())
        {
            var r = new Coordinates3D();
            r._z      = MakeVersor(z);
            r._x      = MakeVersor(y, r._z);
            r._y      = MakeVersor(r._z, r._x);
            r._origin = o;
            return r;
        }

        /// <summary>
        ///     Realizuje operator równości
        /// </summary>
        /// <param name="a">pierwszy obiekt do porównania</param>
        /// <param name="b">drugi obiekt do porównania</param>
        /// <returns></returns>
        public static bool operator ==(Coordinates3D a, Coordinates3D b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a._x.Equals(b._x) &&
                   a._y.Equals(b._y) &&
                   a._z.Equals(b._z) &&
                   a._origin.Equals(b._origin);
        }

        /// <summary>
        ///     Rzutuje <see cref="Coordinates3D">Coordinates3DBase</see> na <see cref="Transform3D">Transform3D</see>
        /// </summary>
        /// <param name="src">obiekt źródłowy</param>
        /// <returns>wynik rzutowania</returns>
        public static implicit operator Transform3D(Coordinates3D src)
        {
            return new MatrixTransform3D(src.Matrix);
        }

        /// <summary>
        ///     Realizuje operator różności
        /// </summary>
        /// <param name="a">pierwszy obiekt do porównania</param>
        /// <param name="b">drugi obiekt do porównania</param>
        /// <returns></returns>
        public static bool operator !=(Coordinates3D a, Coordinates3D b)
        {
            return !(a == b);
        }

        public static Coordinates3D operator *(Coordinates3D src, Coordinates3D c)
        {
            if (c == null)
                return src;
#warning można to przyspieszyć
            var x = MakeVersor(src._x * c);
            var y = MakeVersor(src._y * c);
            var p = src._origin * c;
            return new Coordinates3D(x, y, p);
        }

        public static Point3D[] operator *(Point3D[] v, Coordinates3D c)
        {
            var result = new Point3D[v.Length];
            for (var index = result.Length - 1; index >= 0; index--)
                result[index] = v[index] * c;
            return result;
        }

        public static IReadOnlyList<Point3D> operator *(IReadOnlyList<Point3D> v, Coordinates3D c)
        {
            var result = new Point3D[v.Count];
            for (var index = result.Length - 1; index >= 0; index--)
                result[index] = v[index] * c;
            return result;
        }

        /// <summary>
        ///     Wykonuje transformację punktu z układu współrzędnych lokalnego (c) do globalnego
        /// </summary>
        /// <param name="p">punkt</param>
        /// <param name="c">lokalny układ współrzędnych</param>
        /// <returns>wektor transformowany</returns>
        public static Point3D operator *(Point3D v, Coordinates3D c)
        {
            return new Point3D(
                c.X.X * v.X +
                c.Y.X * v.Y +
                c.Z.X * v.Z +
                c._origin.X,
                c.X.Y * v.X +
                c.Y.Y * v.Y +
                c.Z.Y * v.Z +
                c._origin.Y,
                c.X.Z * v.X +
                c.Y.Z * v.Y +
                c.Z.Z * v.Z +
                c._origin.Z);
        }

        /// <summary>
        ///     Wykonuje transformację punktu z układu współrzędnych lokalnego (c) do globalnego
        /// </summary>
        /// <param name="p">punkt</param>
        /// <param name="c">lokalny układ współrzędnych</param>
        /// <returns>wektor transformowany</returns>
        public static Point3D operator *(ThePoint v, Coordinates3D c)
        {
            return new Point3D(
                c.X.X * v.X +
                c.Y.X * v.Y +
                //c.Z.X * v.Z +
                c._origin.X,
                c.X.Y * v.X +
                c.Y.Y * v.Y +
                //c.Z.Y * v.Z +
                c._origin.Y,
                c.X.Z * v.X +
                c.Y.Z * v.Y +
                //c.Z.Z * v.Z +
                c._origin.Z);
        }

        /// <summary>
        ///     Wykonuje transformację wektora z układu współrzędnych lokalnego (c) do globalnego
        /// </summary>
        /// <param name="v">wektor</param>
        /// <param name="c">lokalny układ współrzędnych</param>
        /// <returns>wektor transformowany</returns>
        public static Vector3D operator *(Vector3D v, Coordinates3D c)
        {
            if (c == null)
                return v;
            // return c.X * v.X + c.Y * v.Y + c.Z * v.Z;
            var a = new Vector3D(
                c.X.X * v.X +
                c.Y.X * v.Y +
                c.Z.X * v.Z,
                c.X.Y * v.X +
                c.Y.Y * v.Y +
                c.Z.Y * v.Z,
                c.X.Z * v.X +
                c.Y.Z * v.Y +
                c.Z.Z * v.Z);
            return a;
            // return BeautyVersor(a);
        }

        internal static Coordinates3D FromDeserialized(Vector3D x, Vector3D y, Point3D o = new Point3D())
        {
            var a = new Coordinates3D(x, y, o);
            a._y = y;
            return a;
        }

        public static Coordinates3D FromMatrix(ref Matrix3D m)
        {
            return new Coordinates3D(
                new Vector3D(m.M11, m.M12, m.M13),
                new Vector3D(m.M21, m.M22, m.M23),
                new Point3D(m.OffsetX, m.OffsetY, m.OffsetZ)
            );
        }

        private static Vector3D MakeVersor(Vector3D c)
        {
            c.Normalize();
            c = BeautyVersor(c);
            return c;
        }

        private static Vector3D MakeVersor(Vector3D a, Vector3D b)
        {
            var c = Vector3D.CrossProduct(a, b);
            c.Normalize();
            c = BeautyVersor(c);
            return c;
        }

        private static string VectorToStr(Vector3D x)
        {
            if (x == XVersor)
                return "X";
            if (x == YVersor)
                return "Y";
            if (x == ZVersor)
                return "Z";

            if (x == XMinusVersor)
                return "-X";
            if (x == YMinusVersor)
                return "-Y";
            if (x == ZMinusVersor)
                return "-Z";
            return x.ToString();
        }

        /// <summary>
        ///     Tworzy kopię obiektu. Zobacz <see cref="ICloneable">ICloneable</see>
        /// </summary>
        /// <returns>kopia obiektu </returns>
        /// <see cref="ICloneable" />
        public Coordinates3D Clone()
        {
            var r = new Coordinates3D(_x, _y, _origin);
            r._z = _z;
            return r;
        }


        /// <summary>
        ///     Punkt przecięcia z płaszczyzną z
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public Point3D? CrossZ(Point3D a, Point3D b)
        {
            a *= this;
            b *= this;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (a.Z == b.Z)
                return null;
            var m = (0 - a.Z) / (b.Z - a.Z);
            return new Point3D(a.X + m * (b.X - a.X), a.Y + m * (b.Y - a.Y), 0);
        }

        /// <summary>
        ///     Porównuje wskazany obiekt z bieżącym
        /// </summary>
        /// <param name="obj">obiekt do porównania</param>
        /// <returns><c>true</c> jeśli obiekty są równe; <c>false</c> w przeciwnym wypadku</returns>
        public bool Equals(Coordinates3D obj)
        {
            return this == obj;
        }

        /// <summary>
        ///     Porównuje wskazany obiekt z bieżącym
        /// </summary>
        /// <param name="obj">obiekt do porównania</param>
        /// <returns><c>true</c> jeśli obiekty są równe; <c>false</c> w przeciwnym wypadku</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj as Coordinates3D);
        }

        /// <summary>
        ///     Zwraca hash kod obiektu
        /// </summary>
        /// <returns>hash kod obiektu</returns>
        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode() ^ _origin.GetHashCode();
        }

        public Point3D MapPoint23(ThePoint srcPoint)
        {
            return new Point3D(srcPoint.X, srcPoint.Y, 0) * this;
        }

        public Coordinates3D RotateX(double angleDeg)
        {
            double s, c;
            MathEx.GetSinCos(angleDeg, out s, out c);
            var tmp = new Coordinates3D(XVersor, new Vector3D(0, c, -s));
            return this * tmp;
        }

        public Coordinates3D RotateY(double angleDeg)
        {
            double s, c;
            MathEx.GetSinCos(angleDeg, out s, out c);
            var tmp = new Coordinates3D(new Vector3D(c, 0, s), YVersor);
            return this * tmp;
        }

        public Coordinates3D RotateZ(double angleDeg)
        {
            double s, c;
            MathEx.GetSinCos(angleDeg, out s, out c);
            var tmp = new Coordinates3D(new Vector3D(c, -s, 0), new Vector3D(s, c, 0));
            return this * tmp;
        }

        public Coordinates3D RotateZ180()
        {
            return this * RotationZ180;
        }

        public override string ToString()
        {
            return string.Format("x=<{0}> y=<{1}> o=<{2}>", VectorToStr(X), VectorToStr(Y), Origin);
        }

        public Coordinates3D Translate(double dx, double dy, double dz)
        {
            return this * new Coordinates3D(XVersor, YVersor, new Point3D(dx, dy, dz));
        }

        public Coordinates3D Translate(Vector3D translateVector)
        {
            return this * new Coordinates3D(XVersor, YVersor, (Point3D)translateVector);
        }

        /// <summary>
        ///     Przesuwa układ współrzędnych o wektor umiejscowiony w tym samym układzie współrzędnych
        /// </summary>
        /// <param name="x">przesunięcie w osi X lokalnego układu współrzędnych</param>
        /// <param name="y">przesunięcie w osi Y lokalnego układu współrzędnych</param>
        /// <param name="z">przesunięcie w osi Z lokalnego układu współrzędnych</param>
        public void TranslateLocal(double x, double y, double z)
        {
            TranslateLocal(new Vector3D(x, y, z));
        }

        /// <summary>
        ///     Przesuwa układ współrzędnych o wektor umiejscowiony w tym samym układzie współrzędnych
        /// </summary>
        /// <param name="v">wektor przesunięcia</param>
        public void TranslateLocal(Vector3D v)
        {
            Origin += v * this;
        }

        public Coordinates3D TranslateX(double dx)
        {
            return this * new Coordinates3D(XVersor, YVersor, new Point3D(dx, 0, 0));
        }

        public Coordinates3D TranslateY(double dy)
        {
            return this * new Coordinates3D(XVersor, YVersor, new Point3D(0, dy, 0));
        }

        public Coordinates3D TranslateZ(double dz)
        {
            return this * new Coordinates3D(XVersor, YVersor, new Point3D(0, 0, dz));
        }

        /// <summary>
        ///     Tworzy kopię obiektu. Zobacz <see cref="ICloneable">ICloneable</see>
        /// </summary>
        /// <returns>kopia obiektu </returns>
        /// <see cref="ICloneable" />
        object ICloneable.Clone()
        {
            var r = new Coordinates3D(_x, _y, _origin);
            r._z = _z;
            return r;
        }

        private void SetFromXY(Vector3D newX, Vector3D newY)
        {
            _x = MakeVersor(newX);
            _z = MakeVersor(_x, newY);
            _y = MakeVersor(_z, _x);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("X"));
                PropertyChanged(this, new PropertyChangedEventArgs("Y"));
                PropertyChanged(this, new PropertyChangedEventArgs("Z"));
            }
        }

        /// <summary>
        ///     Układ współrzędnych o osiach naturalnie skierowanych (brak obrotów) i umiejscowiony w punkcie (0,0,0)
        /// </summary>
        public static Coordinates3D NORMAL => new Coordinates3D(new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));

        /// <summary>
        ///     Macierz reprezentująca ten układ
        /// </summary>
        public Matrix3D Matrix => new Matrix3D(
            _x.X, _x.Y, _x.Z, 0.0,
            _y.X, _y.Y, _y.Z, 0.0,
            _z.X, _z.Y, _z.Z, 0.0,
            _origin.X, _origin.Y, _origin.Z, 1.0);

        /// <summary>
        ///     Wektor Z
        /// </summary>
        public Point3D Origin
        {
            get => _origin;
            set
            {
                if (_origin.Equals(value)) return;
                _origin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Origin"));
            }
        }

 

/// <summary>
///     Zwraca układ odwrotny
/// </summary>
public Coordinates3D Reversed
        {
            get
            {
                var xX = _x.X;
                var xY = _x.Y;
                var xZ = _x.Z;

                var yX = _y.X;
                var yY = _y.Y;
                var yZ = _y.Z;

                var zX = _z.X;
                var zY = _z.Y;
                var zZ = _z.Z;

                var originX = _origin.X;
                var originY = _origin.Y;
                var originZ = _origin.Z;

                var b = xX * (yY * zZ - yZ * zY) + xY * (1.0 * yZ * zX - yX * zZ) +
                        xZ * (1.0 * yX * zY - yY * zX);

                var x = new Vector3D(
                    yY * zZ - yZ * zY,
                    xZ * zY - xY * zZ,
                    xY * yZ - xZ * yY
                ) / b;

                var y = new Vector3D(
                    yZ * zX - 1.0 * yX * zZ,
                    xX * zZ - 1.0 * xZ * zX,
                    xZ * yX - 1.0 * xX * yZ
                ) / b;

                var o = new Point3D(
                    -yX * (originZ * zY - originY * zZ) + yY * (originZ * zX - originX * zZ) -
                    yZ * (originY * zX - originX * zY),
                    xX * (originZ * zY - originY * zZ) - xY * (originZ * zX - originX * zZ) +
                    xZ * (originY * zX - originX * zY),
                    -xX * (originZ * yY - originY * yZ) + xY * (originZ * yX - originX * yZ) -
                    xZ * (originY * yX - originX * yY)
                );
                o = new Point3D(o.X / b, o.Y / b, o.Z / b);
                return new Coordinates3D(x, y, o);
            }
        }

/// <summary>
///     Wektor X
/// </summary>
public Vector3D X => _x;

/// <summary>
///     Wektor Y
/// </summary>
public Vector3D Y => _y;

/// <summary>
///     Wektor Z
/// </summary>
public Vector3D Z => _z;

        /*
        /// <summary>
        /// Serializuje wartość Coordinates3DBase
        /// </summary>
        /// <param name="value">wartość do zamiany na postać tekstową</param>
        /// <returns>postać tekstowa</returns>
        [DataSerialize]
        [System.Reflection.Obfuscation(Exclude = true)]
        public static string Serialize(Coordinates3DBase value)
        {
            if (value == (object)null)
                return "<NULL>";
            List<string> l = new List<string>();
            l.Add(DataObjectManager.Instance.Serializers.Serialize(value.X));
            l.Add(DataObjectManager.Instance.Serializers.Serialize(value.Y));
            l.Add(DataObjectManager.Instance.Serializers.Serialize(value.Z));
            l.Add(DataObjectManager.Instance.Serializers.Serialize(value.Origin));
            return "[" + string.Join("];[", l.ToArray()) + "]";
        }
         */

        public static readonly Coordinates3D RotationX180;
        public static readonly Coordinates3D RotationY180;
        public static readonly Coordinates3D RotationZ180;
        public static readonly Vector3D XMinusVersor;
        public static readonly Vector3D XVersor;
        public static readonly Vector3D YMinusVersor;
        public static readonly Vector3D YVersor;
        public static readonly Vector3D ZMinusVersor;
        public static readonly Vector3D ZVersor;

        private Point3D _origin;

        private Vector3D _x;

        private Vector3D _y;

        private Vector3D _z;

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Przechowuje informacje o osi i kącie obrotu oraz translacji która przekształca jeden układ współrzędnych w inny
        /// </summary>
        public sealed class Transformation
        {
            public Coordinates3D GetCoordinates(double factor)
            {
                /*
                var a = Vector3D.CrossProduct( RotateAxis, XVersor);
                var b = Vector3D.CrossProduct( RotateAxis, YVersor);
                var c = Vector3D.CrossProduct( RotateAxis, ZVersor);
                if (b.Length>a.Length)
                    a = b;
                if (c.Length>a.Length)
                    a = c;

                Coordinates3DBase a2 = new Coordinates3DBase(RotateAxis, a);
                Coordinates3DBase a2o = Coordinates3DBase.NORMAL.RotateX(-AngleDeg * factor);
                var tmp1 = RotateAxis * a2.Reversed * a2o * a2;

                Coordinates3DBase aaa = a2.Reversed * a2o * a2;
                aaa = aaa.Translate(Translate * factor);
                */

                var aa = FromFreeRotate(RotateAxis, AngleDeg * factor);
                aa = aa.Translate(Translate * factor);
                return aa;
            }

            public override string ToString()
            {
                return Coordinates.ToString();
            }

            /// <summary>
            ///     Kąt w stopniach
            /// </summary>
            public double AngleDeg { get; set; }

            public Coordinates3D Coordinates => GetCoordinates(1);

            /// <summary>
            ///     Oś obrotu
            /// </summary>
            public Vector3D RotateAxis { get; set; }

            /// <summary>
            ///     Przesunięciu początku układu współrzędnych
            /// </summary>
            public Vector3D Translate { get; set; }
        }

        public static Transformation GetRotateVectorAndAngle(Coordinates3D src, Coordinates3D
            dst)
        {
            var reversed = src.Reversed;
            var ttt = new Transformation
            {
                Translate  = dst.Origin * reversed - src.Origin * reversed,
                RotateAxis = new Vector3D(1, 0, 0),
                AngleDeg   = 0
            };
            if (src.X == dst.X && src.Y == dst.Y)
                return ttt;

            var x1 = Vector3D.CrossProduct(src.X, dst.X);
            var y1 = Vector3D.CrossProduct(src.Y, dst.Y);
            var z1 = Vector3D.CrossProduct(src.Z, dst.Z);

            var x2 = src.X + dst.X;
            var y2 = src.Y + dst.Y;
            var z2 = src.Z + dst.Z;
            // oś źródłową X można obrócić dookoła dowolnej osi leżącej na płaszczyźnie wyznaczonej przez x1 i x2
            // oś źródłową Y można obrócić dookoła dowolnej osi leżącej na płaszczyźnie wyznaczonej przez y1 i y2
            // zabawa polega na znalezieniu wspólnej osi czyli osi przecięcia tych płaszczyzn
            var lx  = x1.Length;
            var ly  = y1.Length;
            var lz  = z1.Length;
            var min = Math.Min(Math.Min(lx, ly), lz);

            Vector3D vp1, vp2, v1, v2;
            if (lx == min)
            {
                vp1 = Vector3D.CrossProduct(y1, y2);
                vp2 = Vector3D.CrossProduct(z1, z2);
                v1  = src.Y;
                v2  = dst.Y;
            }
            else if (ly == min)
            {
                vp1 = Vector3D.CrossProduct(x1, x2);
                vp2 = Vector3D.CrossProduct(z1, z2);
                v1  = src.X;
                v2  = dst.X;
            }
            else
            {
                vp1 = Vector3D.CrossProduct(x1, x2);
                vp2 = Vector3D.CrossProduct(y1, y2);
                v1  = src.X;
                v2  = dst.X;
            }

            vp1.Normalize();
            vp2.Normalize();
            // Transormation t = new Transormation();
            var tmp = Vector3D.CrossProduct(vp1, vp2);
            tmp.Normalize();
            if (double.IsNaN(tmp.X)) tmp = vp1;
            ttt.RotateAxis = tmp;

            var c1  = Vector3D.DotProduct(ttt.RotateAxis, v1);
            var c2  = Vector3D.DotProduct(ttt.RotateAxis, v2);
            var v1a = v1 - c1 * ttt.RotateAxis;
            var v1b = v2 - c2 * ttt.RotateAxis;

            var ac         = new Coordinates3D(v1a, ttt.RotateAxis);
            var acReversed = ac.Reversed;
            var v2a        = v1a * acReversed;
            var v2b        = v1b * acReversed;
            v2a.Normalize();
            v2b.Normalize();

            ttt.AngleDeg = -MathEx.Atan2Deg(v2b.Z, v2b.X);
            if (ttt.AngleDeg < 0)
                ttt.AngleDeg += 360;
            if (ttt.AngleDeg > 180)
            {
                ttt.AngleDeg   = 360 - ttt.AngleDeg;
                ttt.RotateAxis = -ttt.RotateAxis;
            }

            return ttt;
        }

        public static Point3D[] operator /(Point3D[] points, Coordinates3D inversed)
        {
            inversed = inversed.Reversed;
            var result = new Point3D[points.Length];
            for (var index = 0; index < points.Length; index++)
            {
                var src = points[index];
                result[index] = src * inversed;
            }

            return result;
        }

        public static Point3D operator /(Point3D point, Coordinates3D c)
        {
            return point * c.Reversed;
        }

        public static Coordinates3D operator /(Coordinates3D src, Coordinates3D c)
        {
            return src * c.Reversed;
        }

        public static Vector3D operator /(Vector3D src, Coordinates3D c)
        {
            return src * c.Reversed;
        }
    }
}
