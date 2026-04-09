using System.Linq;
using Xunit;

namespace iSukces.Mathematics.Test;

public sealed class RectTopologyTests
{
    [Fact]
    public void T01_Add_should_force_coordinate_order()
    {
        var t = new RectTopology();
        var r = t.Add(10, 20, 1, 2);
        Assert.Equal(new Rect(1, 2, 9, 18), r);
        Assert.Single(t.Items);
    }

    [Fact]
    public void T02_Substract_non_overlapping_rect_should_keep_original()
    {
        var t = (RectTopology)new Rect(0, 0, 10, 10);
        var o = t.Substract(new Rect(20, 20, 5, 5));
        Assert.Single(o.Items);
        Assert.Equal(new Rect(0, 0, 10, 10), o.Items[0]);
    }

    [Fact]
    public void T03_Substract_inner_rect_should_split_into_four_parts()
    {
        var t = (RectTopology)new Rect(0, 0, 10, 10);
        var o = t.Substract(new Rect(3, 3, 4, 4));
        Assert.Equal(4, o.Items.Count);
        var area = o.Items.Sum(a => a.Width * a.Height);
        Assert.Equal(84, area, 12);
    }

    [Fact]
    public void T04_Substract_topology_should_apply_all_cutters()
    {
        var src    = (RectTopology)new Rect(0, 0, 10, 10);
        var cutter = new RectTopology([new Rect(0, 0, 5, 10), new Rect(5, 0, 5, 5)]);
        var o      = src.Substract(cutter);
        Assert.True(o.HasAnyElement);
        var area = o.Items.Sum(a => a.Width * a.Height);
        Assert.Equal(25, area, 12);
    }
}
