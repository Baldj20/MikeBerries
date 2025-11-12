using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace ProductService.UnitTests;

internal class AutoDataCustomAttribute : AutoDataAttribute
{
    public AutoDataCustomAttribute() : base(() =>
    {
        var fixture = new Fixture();

        fixture.Customize(new AutoNSubstituteCustomization
        {
            ConfigureMembers = true
        });

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        return fixture;
    }) { }
}
