namespace CodeSamples
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;

    public class QueryStringParser
    {
        private readonly object _queryStringObj;

        private readonly NameValueCollection _parameters;

        public QueryStringParser(object queryStringObj)
        {
            _queryStringObj = queryStringObj;

            _parameters = HttpUtility.ParseQueryString(string.Empty);
        }

        public override string ToString()
        {
            var properties = _queryStringObj.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                var value = FormatValue(propertyInfo.GetValue(_queryStringObj));
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _parameters.Add(propertyInfo.Name, value);
                }
            }

            return _parameters.Count > 0 ? $"?{_parameters}" : string.Empty;
        }

        private static string FormatValue(object valueObj) =>
            valueObj switch
            {
                DateTime dateTime => dateTime.ToIsoDateString(),
                IEnumerable collection => string.Join(",", collection.Cast<object>().Select(FormatValue)),
                _ => valueObj?.ToString()?.ToLower()
            };
    }
}
