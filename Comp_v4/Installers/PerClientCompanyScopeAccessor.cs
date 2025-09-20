using System.Collections.Concurrent;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace Comp_v4;

/*public class PerClientCompanyScopeAccessor : IScopeAccessor
{
    static private readonly ConcurrentDictionary<Guid, ILifetimeScope> collection = new();

    public ILifetimeScope GetScope(Castle.MicroKernel.Context.CreationContext context) {
        var companyID = ClientHelper.GetCurrentClientCompanyId();

        return collection.GetOrAdd(companyID, id => new ThreadSafeDefaultLifetimeScope());
    }

    public void Dispose() {
        foreach (var scope in collection) scope.Value.Dispose();
        collection.Clear();
    }
}


public class PerTableWindowScopeAccessor : IScopeAccessor
{
    static private readonly ConcurrentDictionary<Guid, ILifetimeScope> collection = new();

    public ILifetimeScope GetScope(Castle.MicroKernel.Context.CreationContext context) {
        var windowId = ClientHelper.GetCurrentClientCompanyId();

        return collection.GetOrAdd(windowId, id => new ThreadSafeDefaultLifetimeScope());
    }

    public void Dispose() {
        foreach (var scope in collection) scope.Value.Dispose();
        collection.Clear();
    }
}

public class ViewModelScopeAccessor : IScopeAccessor
{
    private readonly IDictionary<Guid, ILifetimeScope> _scopes = new Dictionary<Guid, ILifetimeScope>();
    private readonly ILifetimeScope _defaultScope;

    public ViewModelScopeAccessor()
        : this(new DefaultLifetimeScope()) {
    }

    public ViewModelScopeAccessor(ILifetimeScope defaultScope) {
        this._defaultScope = defaultScope;
    }

    public ILifetimeScope GetScope(CreationContext context) {
        var creator = context.Handler.ComponentModel.Implementation;
        var viewModel = creator as IViewModel;

        if (viewModel != null) {
            if (!_scopes.TryGetValue(viewModel.UID, out var scope)) {
                scope = new DefaultLifetimeScope();
                _scopes[viewModel.UID] = scope;
            }

            return scope;
        }
        else
            return _defaultScope;
    }

    public void Dispose() {
        foreach (var scope in _scopes) scope.Value.Dispose();

        _defaultScope.Dispose();
        _scopes.Clear();
    }
}*/


public class ThreadSafeDefaultLifetimeScope : ILifetimeScope
{
    static private readonly Action<Burden> emptyOnAfterCreated = delegate { };
    private readonly object @lock = new();
    private readonly Action<Burden> onAfterCreated;
    private IScopeCache scopeCache;

    public ThreadSafeDefaultLifetimeScope(IScopeCache scopeCache = null, Action<Burden> onAfterCreated = null) {
        this.scopeCache = scopeCache         ?? new ScopeCache();
        this.onAfterCreated = onAfterCreated ?? emptyOnAfterCreated;
    }

    public void Dispose() {
        lock (@lock) {
            if (scopeCache == null) return;

            var disposableCache = scopeCache as IDisposable;
            if (disposableCache != null) disposableCache.Dispose();
            scopeCache = null;
        }
    }

    public Burden GetCachedInstance(ComponentModel model, ScopedInstanceActivationCallback createInstance) {
        lock (@lock) {
            var burden = scopeCache[model];
            if (burden == null) scopeCache[model] = burden = createInstance(onAfterCreated);

            return burden;
        }
    }
}