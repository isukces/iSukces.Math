#if !WPFFEATURES
namespace iSukces.Mathematics.Compatibility
{
    public struct Matrix
    {
        public Matrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
        {
            _m11 = m11;
            _m12 = m12;
            _m21 = m21;
            _m22 = m22;
            _offsetX = offsetX;
            _offsetY = offsetY;
            _type = MatrixTypes.Unknown;
            DeriveMatrixType();
        }

        public static bool Equals(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.IsDistinguishedIdentity || matrix2.IsDistinguishedIdentity)
                return matrix1.IsIdentity == matrix2.IsIdentity;
            if (matrix1.M11.Equals(matrix2.M11) && matrix1.M12.Equals(matrix2.M12) && matrix1.M21.Equals(matrix2.M21) &&
                matrix1.M22.Equals(matrix2.M22) && matrix1.OffsetX.Equals(matrix2.OffsetX))
                return matrix1.OffsetY.Equals(matrix2.OffsetY);
            return false;
        }


        public static bool operator ==(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.IsDistinguishedIdentity || matrix2.IsDistinguishedIdentity)
                return matrix1.IsIdentity == matrix2.IsIdentity;
            if (matrix1.M11 == matrix2.M11 && matrix1.M12 == matrix2.M12 && matrix1.M21 == matrix2.M21 &&
                matrix1.M22 == matrix2.M22 && matrix1.OffsetX == matrix2.OffsetX)
                return matrix1.OffsetY == matrix2.OffsetY;
            return false;
        }


        public static bool operator !=(Matrix matrix1, Matrix matrix2)
        {
            return !(matrix1 == matrix2);
        }

        public override bool Equals(object? o)
        {
            if (!(o is Matrix _))
                return false;
            return Equals(this, (Matrix)o);
        }


        public bool Equals(Matrix value)
        {
            return Equals(this, value);
        }


        public override int GetHashCode()
        {
            if (IsDistinguishedIdentity)
                return 0;
            return M11.GetHashCode()
                   ^ M12.GetHashCode()
                   ^ M21.GetHashCode()
                   ^ M22.GetHashCode()
                   ^ OffsetX.GetHashCode()
                   ^ OffsetY.GetHashCode();
        }


        public override string ToString()
        {
            if (IsIdentity)
                return "Identity";
            var numericListSeparator = Utils.GetNumericListSeparator(null);
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}", numericListSeparator, _m11, _m12, _m21, _m22,
                _offsetX, _offsetY);
        }

        public Point Transform(Point point)
        {
            var x = point.X;
            var y = point.Y;
            MultiplyPoint(ref x, ref y);
            return new Point(x, y);
        }


        public Vector Transform(Vector vector)
        {
            var x = vector.X;
            var y = vector.Y;
            MultiplyVector(ref x, ref y);
            return new Vector(x, y);
        }

        private void DeriveMatrixType()
        {
            _type = MatrixTypes.Identity;
            if (_m21 != 0.0 || _m12 != 0.0)
            {
                _type = MatrixTypes.Unknown;
            }
            else
            {
                if (_m11 != 1.0 || _m22 != 1.0)
                    _type = MatrixTypes.Scaling;
                if (_offsetX != 0.0 || _offsetY != 0.0)
                    _type = _type | MatrixTypes.Translation;
                if ((_type & (MatrixTypes.Translation | MatrixTypes.Scaling)) !=
                    MatrixTypes.Identity)
                    return;
                _type = MatrixTypes.Identity;
            }
        }

        private void MultiplyPoint(ref double x, ref double y)
        {
            switch (_type)
            {
                case MatrixTypes.Identity:
                    break;
                case MatrixTypes.Translation:
                    x = x + _offsetX;
                    y = y + _offsetY;
                    break;
                case MatrixTypes.Scaling:
                    x = x * _m11;
                    y = y * _m22;
                    break;
                case MatrixTypes.Translation | MatrixTypes.Scaling:
                    x = x * _m11;
                    x = x + _offsetX;
                    y = y * _m22;
                    y = y + _offsetY;
                    break;
                default:
                    var num1 = y * _m21 + _offsetX;
                    var num2 = x * _m12 + _offsetY;
                    x = x * _m11;
                    x = x + num1;
                    y = y * _m22;
                    y = y + num2;
                    break;
            }
        }

        private void MultiplyVector(ref double x, ref double y)
        {
            switch (_type)
            {
                case MatrixTypes.Identity:
                    break;
                case MatrixTypes.Translation:
                    break;
                case MatrixTypes.Scaling:
                case MatrixTypes.Translation | MatrixTypes.Scaling:
                    x = x * _m11;
                    y = y * _m22;
                    break;
                default:
                    var num1 = y * _m21;
                    var num2 = x * _m12;
                    x = x * _m11;
                    x = x + num1;
                    y = y * _m22;
                    y = y + num2;
                    break;
            }
        }

        private void SetMatrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY,
            MatrixTypes type)
        {
            _m11 = m11;
            _m12 = m12;
            _m21 = m21;
            _m22 = m22;
            _offsetX = offsetX;
            _offsetY = offsetY;
            _type = type;
        }

        public bool IsIdentity
        {
            get
            {
                if (_type == MatrixTypes.Identity)
                    return true;
                if (_m11 == 1.0 && _m12 == 0.0 && _m21 == 0.0 && _m22 == 1.0 && _offsetX == 0.0)
                    return _offsetY == 0.0;
                return false;
            }
        }


        public double Determinant
        {
            get
            {
                switch (_type)
                {
                    case MatrixTypes.Identity:
                    case MatrixTypes.Translation:
                        return 1.0;
                    case MatrixTypes.Scaling:
                    case MatrixTypes.Translation | MatrixTypes.Scaling:
                        return _m11 * _m22;
                    default:
                        return _m11 * _m22 - _m12 * _m21;
                }
            }
        }


        public double M11
        {
            get
            {
                if (_type == MatrixTypes.Identity)
                    return 1.0;
                return _m11;
            }
        }


        public double M12
        {
            get
            {
                if (_type == MatrixTypes.Identity)
                    return 0.0;
                return _m12;
            }
        }


        public double M21
        {
            get
            {
                if (_type == MatrixTypes.Identity)
                    return 0.0;
                return _m21;
            }
        }


        public double M22
        {
            get
            {
                if (_type == MatrixTypes.Identity)
                    return 1.0;
                return _m22;
            }
        }


        public double OffsetX
        {
            get
            {
                if (_type == MatrixTypes.Identity)
                    return 0.0;
                return _offsetX;
            }
        }


        public double OffsetY
        {
            get
            {
                if (_type == MatrixTypes.Identity)
                    return 0.0;
                return _offsetY;
            }
        }

        private bool IsDistinguishedIdentity => _type == MatrixTypes.Identity;

        private double _m11;
        private double _m12;
        private double _m21;
        private double _m22;
        private double _offsetX;
        private double _offsetY;
        internal MatrixTypes _type;
    }
}
#endif

