#nullable disable
#if WPFFEATURES


#if !WPFFEATURES
using iSukces.Mathematics.Compatibility;
#else
using System.Windows.Media.Media3D;
#endif
using Newtonsoft.Json;
using Xunit;

namespace iSukces.Mathematics.test.CompatPresentationCore3D;

public class Point3DTests
{
#if WPFFEATURES
    [Fact]
#else
    [Fact(Skip = "Tylko WPFFEATURES" )]
#endif
    public void T01_Shoud_Serialize_Point3D()
    {
        var srcJson = "\"-26749,60309,-17014\"";
        var tmp     = JsonConvert.DeserializeObject<Point3D>(srcJson);
        Assert.Equal(-26749, tmp.X);
        Assert.Equal(60309, tmp.Y);
        Assert.Equal(-17014, tmp.Z);
        var json = JsonConvert.SerializeObject(tmp);
        Assert.Equal(srcJson, json);
    }
        
        
#if WPFFEATURES
        [Fact]
#else
    [Fact(Skip = "Tylko WPFFEATURES" )]
#endif
    public void T02_Shoud_Serialize_Vector3D()
    {
        var srcJson = "\"-26749,60309,-17014\"";
        var tmp     = JsonConvert.DeserializeObject<Vector3D>(srcJson);
        Assert.Equal(-26749, tmp.X);
        Assert.Equal(60309, tmp.Y);
        Assert.Equal(-17014, tmp.Z);
        var json = JsonConvert.SerializeObject(tmp);
        Assert.Equal(srcJson, json);
    }
        
        
#if WPFFEATURES
        [Fact]
#else
    [Fact(Skip = "Tylko WPFFEATURES" )]
#endif
    public void T03_Should_Serialize_Complex_object()
    {
        var json =
            "{\"DataType\":\"WsChangeCamera\",\"LookDirection\":\"-26749,60309,-17014\",\"Position\":\"29513,-50904,23446\"}";
        var x = JsonConvert.DeserializeObject<WsChangeCamera>(json);
    }
        
}
    
public class WsChangeCamera  
{
    public string    DataType      { get; set; } = nameof(WsChangeCamera);
    public Vector3D? LookDirection { get; set; }
    public Point3D?  Position      { get; set; }
}
#endif
