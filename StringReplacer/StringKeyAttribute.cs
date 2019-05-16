namespace CodeSamples
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    internal class StringKeyAttribute : Attribute
    {
        internal StringKeyAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }
    }
}
