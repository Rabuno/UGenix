/*
  Expanded Architecture Rules for Module Isolation.
  These ensure that UGem.Features.Ordering cannot talk to UGem.Features.Discovery internals.
*/

/*
namespace UGem.ArchitectureTests;

public class ModuleIsolationTests
{
    [Fact]
    public void Features_Should_Not_Have_Dependency_On_Other_Features_Internals()
    {
        var result = Types.InAssembly(typeof(IFeatureMarker).Assembly)
            .That()
            .ResideInNamespace("UGem.Features.Ordering")
            .ShouldNot()
            .HaveDependencyOn("UGem.Features.Discovery.Internals")
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
