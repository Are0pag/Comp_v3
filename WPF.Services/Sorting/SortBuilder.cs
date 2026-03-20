using System.Linq.Expressions;

namespace WPF.Services.Sorting;

public class SortBuilder<T>
{
    private readonly IEnumerable<T> _source;
    private readonly Stack<SortOperation> _sortHistory = new();
    private IOrderedEnumerable<T> _orderedQuery;

    public SortBuilder(IEnumerable<T> source) {
        _source = source;
    }

    public SortBuilder<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector, bool descending = false) {
        if (_orderedQuery == null)
            _orderedQuery = descending
                ? _source.OrderByDescending(keySelector.Compile())
                : _source.OrderBy(keySelector.Compile());
        else
            _orderedQuery = descending
                ? _orderedQuery.ThenByDescending(keySelector.Compile())
                : _orderedQuery.ThenBy(keySelector.Compile());

        _sortHistory.Push(new SortOperation {
            PropertyName = GetPropertyName(keySelector),
            Descending = descending
        });

        return this;
    }

    public IEnumerable<T> Build() {
        return _orderedQuery ?? _source;
    }

    public void Clear() {
        _orderedQuery = null;
    }

    public IReadOnlyCollection<SortOperation> GetSortHistory() {
        return _sortHistory.ToList().AsReadOnly();
    }

    private string GetPropertyName<TKey>(Expression<Func<T, TKey>> expression) {
        return expression.Body is MemberExpression memberExpression
            ? memberExpression.Member.Name
            : throw new ArgumentException("Expression is not a property");
    }
}

public class SortOperation
{
    public string PropertyName { get; set; }
    public bool Descending { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}