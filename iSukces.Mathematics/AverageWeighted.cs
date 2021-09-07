namespace iSukces.Mathematics
{
    public sealed class AverageWeighted
    {
        public void Add(double x, double w)
        {
            _licznik   += x * w;
            _mianownik += w;
            Count++;
        }

        /// <summary>
        ///     Zwraca tekstową reprezentację obiektu
        /// </summary>
        /// <returns>Tekstowa reprezentacja obiektu</returns>
        public override string ToString() { return string.Format("{0} from {1} points", Average, Count); }

        /// <summary>
        ///     ilosc dodanych punktów; własność jest tylko do odczytu.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///     wartość średnia; własność jest tylko do odczytu.
        /// </summary>
        public double Average => _licznik / _mianownik;

        /// <summary>
        ///     Własność jest tylko do odczytu.
        /// </summary>
        public double Weight1 => _mianownik / Count;

        double _licznik, _mianownik;
    }
}
