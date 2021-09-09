using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using iSukces.Mathematics.Compatibility;

#if COREFX
#else
using System.Windows.Media.Media3D;
#endif

namespace iSukces.Mathematics
{
    /// <summary>
    ///     Klasa reprezentująca płaszczyznę obcinającą profil na końcu
    /// </summary>
    public sealed class CuttingPlane : ICuttingSurface, INotifyPropertyChanged, IEquatable<CuttingPlane>, ICloneable
    {
        /// <summary>
        ///     Tworzy instancję obiektu
        /// </summary>
        public CuttingPlane()
        {
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="xfactor">współczynnik nachylenia zależny od parametru X</param>
        ///     <param name="yfactor">współczynnik nachylenia zależny od parametru Y</param>
        /// </summary>
        public CuttingPlane(double xfactor, double yfactor)
        {
            Xfactor = xfactor;
            Yfactor = yfactor;
        }

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="xfactor">współczynnik nachylenia zależny od parametru X</param>
        ///     <param name="yfactor">współczynnik nachylenia zależny od parametru Y</param>
        ///     <param name="zOffset">dodatkowe przesunięcie płaszczyzny obcięcia w kierunku osi Z</param>
        /// </summary>
        public CuttingPlane(double xfactor, double yfactor, double zOffset)
        {
            Xfactor = xfactor;
            Yfactor = yfactor;
            ZOffset = zOffset;
        }

        /// <summary>
        ///     Realizuje operator ==
        /// </summary>
        /// <param name="left">lewa strona porównania</param>
        /// <param name="right">prawa strona porównania</param>
        /// <returns><c>true</c> jeśli obiekty są równe</returns>
        public static bool operator ==(CuttingPlane left, CuttingPlane right)
        {
            return ReferenceEquals(left, null)
                ? ReferenceEquals(right, null)
                : !ReferenceEquals(right, null) && left._xfactor == right._xfactor && left._yfactor == right._yfactor &&
                  left._zOffset == right._zOffset;
        }

        /// <summary>
        ///     Realizuje operator !=
        /// </summary>
        /// <param name="left">lewa strona porównania</param>
        /// <param name="right">prawa strona porównania</param>
        /// <returns><c>true</c> jeśli obiekty są różne</returns>
        public static bool operator !=(CuttingPlane left, CuttingPlane right)
        {
            return !(left == right);
        }

        // Public Methods 

        /// <summary>
        ///     Oblicza wektor normalny we wskazanym punkcie
        /// </summary>
        /// <param name="x">współrzędna x</param>
        /// <param name="y">współrzędna y</param>
        /// <returns>wektor normalny</returns>
        public Vector3D? CalculateNormal(double x, double y)
        {
            return _normal;
        }

        public Point3D CalculatePoint(double x, double y)
        {
            return new Point3D(x, y, CalculateZ(x, y));
        }

        public Point3D CalculatePoint(double x, double y, double z)
        {
            return new Point3D(x, y, CalculateZ(x, y) + z);
        }

        /// <summary>
        ///     Oblicza wysokość obcięcia w punkcie x,y
        /// </summary>
        /// <param name="x">współrzędna x</param>
        /// <param name="y">współrzędna y</param>
        /// <returns>współrzędna z</returns>
        public double CalculateZ(double x, double y)
        {
            return x * _xfactor + y * _yfactor + _zOffset;
        }

        /// <summary>
        ///     Creates copy of object
        /// </summary>
        public object Clone()
        {
            return new CuttingPlane
            {
                Xfactor = Xfactor,
                Yfactor = Yfactor,
                ZOffset = ZOffset
            };
        }

        /// <summary>
        ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
        /// </summary>
        /// <param name="other">obiekt do porównania z obiektem bieżącym</param>
        /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
        public bool Equals(CuttingPlane other)
        {
            return other == this;
        }

        /// <summary>
        ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
        /// </summary>
        /// <param name="other">obiekt do porównania z obiektem bieżącym</param>
        /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
        public override bool Equals(object other)
        {
            return other is CuttingPlane plane && Equals(plane);
        }

        public string GetCompareString()
        {
            return string.Format("CuttingPlane {0}/{1}/{2}", _xfactor, _yfactor, _zOffset);
        }

        /// <summary>
        ///     Zwraca kod HASH obiektu
        /// </summary>
        /// <returns>kod HASH obiektu</returns>
        public override int GetHashCode()
        {
            // Good implementation suggested by Josh Bloch
            var _hash_ = 17;
            _hash_ = _hash_ * 31 + _xfactor.GetHashCode();
            _hash_ = _hash_ * 31 + _yfactor.GetHashCode();
            _hash_ = _hash_ * 31 + _zOffset.GetHashCode();
            return _hash_;
        }

        /// <summary>
        ///     Zwraca listę współrzędnych x, na których jest załamanie płaszczyzny
        /// </summary>
        /// <param name="xmin">początek zakresu</param>
        /// <param name="xmax">koniec zakresu zakresu</param>
        /// <returns>lista współrzędnych</returns>
        public IList<double> GetXEdges(double xmin, double xmax)
        {
            return null;
        }

        /// <summary>
        ///     Zwraca listę współrzędnych y, na których jest załamanie płaszczyzny
        /// </summary>
        /// <param name="ymin">początek zakresu</param>
        /// <param name="ymax">koniec zakresu zakresu</param>
        /// <returns>lista współrzędnych</returns>
        public IList<double> GetYEdges(double ymin, double ymax)
        {
            return null;
        }


        /// <summary>
        ///     Zwraca tekstową reprezentację obiektu
        /// </summary>
        /// <returns>Tekstowa reprezentacja obiektu</returns>
        public override string ToString()
        {
            return string.Format("CuttingPlane {0} {1}", _xfactor, _yfactor);
        }

        private void NotifyPropertyChanged([CallerMemberName] string property = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        // Private Methods 

        private void UpdateNormal()
        {
            _normal = new Vector3D(-Xfactor, -Yfactor, 1);
            _normal.Normalize();
        }

        public bool HasSharpYEdges
        {
            get { return false; }
        }

        /// <summary>
        ///     współczynnik nachylenia zależny od parametru X
        /// </summary>
        public double Xfactor
        {
            get { return _xfactor; }
            set
            {
                // ReSharper disable CompareOfFloatsByEqualityOperator
                if (value == _xfactor) return;
                _xfactor = value;
                UpdateNormal();
                NotifyPropertyChanged();
                // ReSharper restore CompareOfFloatsByEqualityOperator
            }
        }

        /// <summary>
        ///     współczynnik nachylenia zależny od parametru Y
        /// </summary>
        public double Yfactor
        {
            get { return _yfactor; }
            set
            {
                // ReSharper disable CompareOfFloatsByEqualityOperator
                if (value == _yfactor) return;
                _yfactor = value;
                UpdateNormal();
                NotifyPropertyChanged();
                // ReSharper restore CompareOfFloatsByEqualityOperator
            }
        }

        /// <summary>
        ///     dodatkowe przesunięcie płaszczyzny obcięcia w kierunku osi Z
        /// </summary>
        public double ZOffset
        {
            get { return _zOffset; }
            set
            {
                // ReSharper disable CompareOfFloatsByEqualityOperator
                if (value == _zOffset) return;
                _zOffset = value;
                NotifyPropertyChanged();
                // ReSharper restore CompareOfFloatsByEqualityOperator
            }
        }

        private Vector3D _normal = new Vector3D(0, 0, 1);
        private double _xfactor;
        private double _yfactor;
        private double _zOffset;

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}