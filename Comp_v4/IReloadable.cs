namespace Comp_v4;

public interface IReloadable
{
    Func<Task> OnReload { get; set; }
}