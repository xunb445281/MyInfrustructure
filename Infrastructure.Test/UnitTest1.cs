using System;
using System.Reflection;
using Xunit;
using System.Linq;

namespace Standard.Infrastructure.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            //types = types.Where(f => MatchGeneric(f, typeof(IEventHandler<>))).ToArray();
            types = types.Where(f => MatchGeneric(typeof(IEventHandler<>), f)).ToArray();
        }

        private bool MatchGeneric(Type findType, Type type)
        {
            if (findType.IsGenericTypeDefinition == false)
                return false;
            var definition = findType.GetGenericTypeDefinition();
            foreach (var implementedInterface in type.FindInterfaces((filter, criteria) => true, null))
            {
                if (implementedInterface.IsGenericType == false)
                    continue;
                return definition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
            }
            return false;
        }
    }

    public interface IEventHandler<T> where T:class
    {

    }

    public class MyEventHandler : IEventHandler<MyEvent>
    {

    }

    public class MyEvent
    {

    }
}
