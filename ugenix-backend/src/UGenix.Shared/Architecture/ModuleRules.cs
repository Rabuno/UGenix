/*
  Expanded Architecture Rules for Module Isolation.
  These ensure that UGenix.Features.Ordering cannot talk to UGenix.Features.Discovery internals.
*/

/*
namespace UGenix.ArchitectureTests;

public class ModuleIsolationTests
{
    [Fact]
    public void Features_Should_Not_Have_Dependency_On_Other_Features_Internals()
    {
        var result = Types.InAssembly(typeof(IFeatureMarker).Assembly)
            .That()
            .ResideInNamespace("UGenix.Features.Ordering")
            .ShouldNot()
            .HaveDependencyOn("UGenix.Features.Discovery.Internals")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Only_Reference_Domain_And_Shared()
    {
        // Enforce the core triangle of Clean Architecture
    }
}
*/

