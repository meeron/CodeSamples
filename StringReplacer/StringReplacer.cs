namespace CodeSamples
{
    using System;
    using System.Linq;

    internal abstract class StringReplacer
    {
        public static ReplacerImpl<T> Create<T>(Type type)
        {
            return new ReplacerImpl<T>(type.GetProperties().Select(p => new CompiledPropertyInfo<T>(p)).ToArray());
        }

        public abstract string Format(object obj, string unformatted);

        internal class ReplacerImpl<T> : StringReplacer
        {
            private readonly CompiledPropertyInfo<T>[] _properties;

            public ReplacerImpl(CompiledPropertyInfo<T>[] properties)
            {
                _properties = properties;
            }

            public override string Format(object obj, string unformatted)
            {
                var cast = (T)obj;
                return _properties.Aggregate(
                    unformatted,
                    (str, p) => p.Replace(str, cast));
            }
        }
    }
}
