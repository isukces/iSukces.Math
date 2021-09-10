#if COREFX
using iSukces.Mathematics.Compatibility;
#else
using System.Windows.Media.Media3D;
#endif

namespace iSukces.Mathematics
{
    public struct Section3D
    {
        public Section3D(Point3D begin, Point3D end)
            : this()
        {
            _begin = begin;
            _end   = end;
            _length     = double.NaN;
        }

        public override string ToString()
        {
            return string.Format("section {0} - {1}", _begin, _end);
        }

        private Point3D _begin;
        private Point3D _end;
        private double _length;

        /// <summary>
        /// </summary>
        public Point3D Begin
        {
            get { return _begin; }
            set
            {
                _length = double.NaN;
                _begin  = value;
            }
        }

        /// <summary>
        /// </summary>
        public Point3D End
        {
            get { return _end; }
            set
            {
                _length = double.NaN;
                _end    = value;
            }
        }

        public double Length
        {
            get
            {
                if (double.IsNaN(_length))
                    _length = (_begin - _end).Length;
                return _length;
            }
        }
    }
}