using System;

namespace iSukces.Mathematics
{
    public sealed class AngleRange : IEquatable<AngleRange>
    {
        /// <summary>
        ///     Tworzy instancję obiektu
        /// </summary>
        public AngleRange() { }

        /// <summary>
        ///     Tworzy instancję obiektu
        ///     <param name="min">minimalny kąt w stopniach</param>
        ///     <param name="length">długość kąta</param>
        /// </summary>
        public AngleRange(double min, double length)
        {
            this.Min    = min;
            this.Length = length;
        }


        /// <summary>
        ///     Realizuje operator ==
        /// </summary>
        /// <param name="left">lewa strona porównania</param>
        /// <param name="right">prawa strona porównania</param>
        /// <returns><c>true</c> jeśli obiekty są równe</returns>
        public static bool operator ==(AngleRange left, AngleRange right)
        {
            if (left == (object)null && right == (object)null) return true;
            if (left == (object)null || right == (object)null) return false;
            return left._min == right._min && left._length == right._length;
        }

        /// <summary>
        ///     Realizuje operator !=
        /// </summary>
        /// <param name="left">lewa strona porównania</param>
        /// <param name="right">prawa strona porównania</param>
        /// <returns><c>true</c> jeśli obiekty są różne</returns>
        public static bool operator !=(AngleRange left, AngleRange right)
        {
            if (left == (object)null && right == (object)null) return false;
            if (left == (object)null || right == (object)null) return true;
            return left._min != right._min || left._length != right._length;
        }

        double _GetMax() { return MathEx.NormalizeAngleDeg(_min + _length); }


        /// <summary>
        ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
        /// </summary>
        /// <param name="obj">obiekt do porównania z obiektem bieżącym</param>
        /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
        public bool Equals(AngleRange other) { return other == this; }

        /// <summary>
        ///     Sprawdza, czy wskazany obiekt jest równy bieżącemu
        /// </summary>
        /// <param name="obj">obiekt do porównania z obiektem bieżącym</param>
        /// <returns><c>true</c> jeśli wskazany obiekt jest równy bieżącemu; w przeciwnym wypadku<c>false</c></returns>
        public override bool Equals(object other)
        {
            if (!(other is AngleRange)) return false;
            return Equals((AngleRange)other);
        }

        /// <summary>
        ///     Zwraca kod HASH obiektu
        /// </summary>
        /// <returns>kod HASH obiektu</returns>
        public override int GetHashCode()
        {
            // Good implementation suggested by Josh Bloch
            int hash = 17;
            hash = hash * 31 + _min.GetHashCode();
            hash = hash * 31 + _length.GetHashCode();
            return hash;
        }


        public bool IsInsideExclusive(double angle)
        {
            angle = MathEx.NormalizeAngleDeg(angle);
            if (angle < _min) angle += 360;
            return angle > _min && angle < _min + _length;
        }

        public bool IsInsideInclusive(double angle)
        {
            angle = MathEx.NormalizeAngleDeg(angle);
            if (angle < _min) angle += 360;
            return angle >= _min && angle <= _min + _length;
        }

        void Update()
        {
            if (_length < 0)
            {
                _length = -_length;
                _min    = MathEx.NormalizeAngleDeg(_min - _length);
            }

            _length = MathEx.NormalizeAngleDeg(_length);
        }

        /// <summary>
        ///     minimalny kąt w stopniach
        /// </summary>
        public double Min
        {
            get => _min;
            set
            {
                value = MathEx.NormalizeAngleDeg(value);
                if (value == _min) return;
                _min = value;
                Update();
            }
        }

        /// <summary>
        ///     maksymalny kąt w stopniach; własność jest tylko do odczytu.
        /// </summary>
        public double Max => _GetMax();

        /// <summary>
        ///     długość kąta
        /// </summary>
        public double Length
        {
            get => _length;
            set
            {
                if (value == _length) return;
                _length = value;
                Update();
            }
        }

        private double _length;
        private double _min;
    }
}
