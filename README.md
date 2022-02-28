# Duckject

Dependency injection container for unity

## Hello World Example

```csharp
using Duckject.Core.Container;
using Duckject.Core.Installer;
using UnityEngine;

 public class HelloWorldInstaller : InstallerBase
 {
    public override void Install(IContainer container)
    {
        container.Bind<HelloWorld>().AsCached().SetNonLazy();
    }

    private class HelloWorld
    {
        public HelloWorld() => Debug.Log("Hello World!");
    }
 }
```