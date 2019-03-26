namespace ExampleRegistry.Api.Tests.Infrastructure
{
    using System;
    using System.Linq;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using KellermanSoftware.CompareNetObjects;
    using KellermanSoftware.CompareNetObjects.TypeComparers;
    using Xunit;

    public static class CompareExtension
    {
        public static void IsEqual<TCommand>(this object actual, TCommand expected)
        {
            var command = Assert.IsType<CommandMessage<TCommand>>(actual);

            CompareExtensions.Config.MembersToIgnore.Add("CommandId");
            CompareExtensions.Config.MembersToIgnore.Add("Metadata");
            CompareExtensions.Config.MaxDifferences = 100;
            CompareExtensions.Config.CustomComparers.Add(new ValueObjectComparer(RootComparerFactory.GetRootComparer()));

            command.ShouldCompare(new CommandMessage<TCommand>(Guid.Empty, expected));
        }
    }

    public class ValueObjectComparer : BaseTypeComparer
    {
        public ValueObjectComparer(RootComparer rootComparer) : base(rootComparer) { }

        public override bool IsTypeMatch(Type type1, Type type2) => IsAssignableToGenericType(type1, typeof(ValueObject<>));

        public override void CompareType(CompareParms parms)
        {
            var item1 = (dynamic)parms.Object1;
            var item2 = (dynamic)parms.Object2;

            if (item1 == item2)
                return;

            var difference = new Difference
            {
                PropertyName = parms.BreadCrumb,
                Object1Value = parms.Object1.ToString(),
                Object2Value = parms.Object2.ToString(),
                Object1TypeName = item1.GetType().ToString(),
                Object2TypeName = item1.GetType().ToString(),
                Object1 = item1,
                Object2 = item2,
                ActualName = "actual",
                ExpectedName = "expected"
            };

            parms.Result.Differences.Add(difference);
        }

        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
                return true;

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }
    }
}
