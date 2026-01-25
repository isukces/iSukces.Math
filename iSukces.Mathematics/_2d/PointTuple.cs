#if !WPFFEATURES
#else
using System.Windows;
#endif

namespace iSukces.Mathematics;

public sealed class PointTuple : IPoint12Mapper
{
    public PointTuple() { }

    public PointTuple(Point p1, Point p2)
    {
        _beginPoint = p1;
        _endPoint   = p2;
        Update();
    }
        
    public static explicit operator Ray2D(PointTuple b)
    {
        return new Ray2D
        {
            BeginPoint = b.BeginPoint,
            Axis       = b.Axis,
            Distance   = b.Distance
        };
    }


    Point IPoint12Mapper.MapPoint12(double x)
    {
        if (x == _startValue)
            return _beginPoint;
        if (x == EndValue)
            return _endPoint;
        return new Point(_beginPoint.X + x * _axis.X, _beginPoint.Y + x * _axis.Y);
    }
        
    private void Update()
    {
        _axis    = _endPoint - _beginPoint;
        Distance = _axis.Length;
        _axis = _axis.GetNormalized();
        EndValue = _startValue + Distance;
    }


    /// <summary>
    ///     Punkt początkowy
    /// </summary>
    public Point BeginPoint
    {
        get => _beginPoint;
        set
        {
            if (value == _beginPoint) return;
            _beginPoint = value;
            Update();
        }
    }

    /// <summary>
    ///     Punkt końcowy
    /// </summary>
    public Point EndPoint
    {
        get => _endPoint;
        set
        {
            if (value == _endPoint) return;
            _endPoint = value;
            Update();
        }
    }

    /// <summary>
    ///     wartość wejściowa odpowiadająca punktowi początkowemu
    /// </summary>
    public double StartValue
    {
        get => _startValue;
        set
        {
            if (value == _startValue) return;
            _startValue = value;
            Update();
        }
    }

    /// <summary>
    ///     wartość wejściowa odpowiadająca punktowi końcowemu; własność jest tylko do odczytu.
    /// </summary>
    public double EndValue { get; private set; }

    /// <summary>
    ///     wektor; własność jest tylko do odczytu.
    /// </summary>
    public Vector Axis => _axis;

    /// <summary>
    ///     odległość między punktami; własność jest tylko do odczytu.
    /// </summary>
    public double Distance { get; private set; }

    private Vector _axis;

    private Point _beginPoint;

    private Point _endPoint;

    private double _startValue;
}