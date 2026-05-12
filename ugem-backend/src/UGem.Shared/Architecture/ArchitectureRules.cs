/* 
  NOTE: This requires the NetArchTest.eNet standard library.
  These tests should be part of a separate XUnit project: UGem.ArchitectureTests
*/

/*
using NetArchTest.Rules;
using UGem.Domain.Abstractions;
using UGem.Application.Abstractions;

namespace UGem.ArchitectureTests;

public class ArchitectureTests
{
    private const string DomainNamespace = "UGem.Domain";
    private const string ApplicationNamespace = "UGem.Application";
    private const string InfrastructureNamespace = "UGem.Infrastructure";
    private const string PersistenceNamespace = "UGem.Persistence";
    private const string ApiNamespace = "UGem.API";

    [Fact]
    public void Domain_Should_Not_Have_Dependency_On_Other_Projects()
    {
        var result = Types.InAssembly(typeof(BaseEntity<>).Assembly)
            .ShouldNot()
            .HaveDependencyOnAll(InfrastructureNamespace, PersistenceNamespace, ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_Have_Dependency_On_Infrastructure_Or_Persistence()
    {
        var result = Types.InAssembly(typeof(IApplicationMarker).Assembly)
            .ShouldNot()
            .HaveDependencyOnAll(InfrastructureNamespace, PersistenceNamespace, ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}
*/
