namespace ShiftManager.DomainTest;

public class MonthTests
{
    [Fact]
    public void Can_Month_ReturnProperMonthValue()
    {
        // Assert.
        Assert.Equal(1, (int)Month.Jan);
        Assert.Equal(2, (int)Month.Feb);
        Assert.Equal(3, (int)Month.Mar);
        Assert.Equal(4, (int)Month.Apr);
        Assert.Equal(5, (int)Month.May);
        Assert.Equal(6, (int)Month.Jun);
        Assert.Equal(7, (int)Month.Jul);
        Assert.Equal(8, (int)Month.Aug);
        Assert.Equal(9, (int)Month.Sep);
        Assert.Equal(10, (int)Month.Oct);
        Assert.Equal(11, (int)Month.Nov);
        Assert.Equal(12, (int)Month.Dec);
    }
}
