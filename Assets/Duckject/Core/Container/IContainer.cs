using System;

namespace Duckject.Core.Container
{
    public interface IContainer
    {
        IContainer Bind<T>();

        IContainer FromInstance<T>(T instance);

        IContainer Bind<T>(T instance);

        IContainer AsCached();

        IContainer AsTransient();

        IContainer To(params object[] target);

        IContainer To(params Type[] types);

        IContainer To<T>();

        IContainer SetNonLazy();

        IContainer SetIdentifier(object identifier);
    }
}
