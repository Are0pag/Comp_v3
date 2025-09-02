namespace Infrastructure.Command.v1;

public class ScheduledCommandComparer : IComparer<(DateTime, int)> 
{
    public int Compare((DateTime, int) x, (DateTime, int) y) {
        var timeComparison = x.Item1.CompareTo(y.Item1);
        return timeComparison != 0 ? timeComparison : y.Item2.CompareTo(x.Item2);
    }
}