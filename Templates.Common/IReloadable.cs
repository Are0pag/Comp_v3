namespace Templates.Common;

public interface IReloadable
{
    Func<Task> OnReload { get; set; }
}