namespace CodeSamples
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    internal class CompiledPropertyInfo<T>
    {
        private readonly string _name;

        private readonly Func<T, string> _getter;

        public CompiledPropertyInfo(PropertyInfo propertyInfo)
        {
            _name = $"{{{propertyInfo.GetCustomAttribute<StringKeyAttribute>()?.Key ?? propertyInfo.Name}}}";
            _getter = BuildUntypedGetter(propertyInfo);
        }

        public string Replace(string str, T obj) => str?.Replace(_name, _getter(obj));

        // https://stackoverflow.com/questions/17660097/is-it-possible-to-speed-this-method-up/17669142#17669142
        private static Func<T, string> BuildUntypedGetter(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType != typeof(string))
            {
                throw new Exception($"Property `{propertyInfo.Name}` is not a string. Only strings can be used in string interpolation");
            }

            var targetType = propertyInfo.DeclaringType;
            var methodInfo = propertyInfo.GetGetMethod();

            var target = Expression.Parameter(targetType, "t");
            var body1 = Expression.Call(target, methodInfo);
            var body2 = Expression.Convert(body1, typeof(string));

            var lambda = Expression.Lambda<Func<T, string>>(body2, target);

            return lambda.Compile();
        }
    }
}
