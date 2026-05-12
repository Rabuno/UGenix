/* 
  NOTE: This requires the NetArchTest.eNet standard library.
  These tests should be part of a separate XUnit project: UGenix.ArchitectureTests
*/

/*
using NetArchTest.Rules;
using UGenix.Domain.Abstractions;
using UGenix.Application.Abstractions;

namespace UGenix.ArchitectureTests;

public class ArchitectureTests
{
    private const string DomainNamespace = "UGenix.Domain";
    private const string ApplicationNamespace = "UGenix.Application";
    private const string InfrastructureNamespace = "UGenix.Infrastructure";
    private const string PersistenceNamespace = "UGenix.Persistence";
    private const string ApiNamespace = "UGenix.API";

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

