using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Obert.Common.Runtime.Attributes;

namespace Obert.Common.Runtime.Extensions
{
    public static class IdUniquenessChecker
    {
        public static bool HasUniqueIds(this IEnumerable<object> items, out string[] validationMessages)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                     | BindingFlags.Static;

            var idAttributes =
                items.Where(x => x != null).SelectMany(x =>
                {
                    var type = x.GetType();
                    var fieldInfos = type
                        .GetFields(bindFlags).ToArray();
                    var propInfos = type.GetProperties(bindFlags).ToArray();
                    
                    var a = propInfos.Select(y => (
                            prop: new Func<(string value, string obj)>(() => (y.GetValue(x)?.ToString(), x.ToString())),
                            attribute: y.GetCustomAttribute<IdAttribute>()))
                        .Where(y => y.attribute != null).ToArray();
                    
                    var b = fieldInfos
                        .Select(y => (
                            prop: new Func<(string value, string obj)>(() => (y.GetValue(x)?.ToString(), x.ToString())),
                            attribute: y.GetCustomAttribute<IdAttribute>()))
                        .Where(y => y.attribute != null).ToArray();
                    return a.Union(b);
                }).ToArray();

            var result = new List<string>();
            var usedValues = new List<string>();

            foreach (var (func, idAttribute) in idAttributes)
            {
                var (value, source) = func();
                if (string.IsNullOrWhiteSpace(value))
                {
                    if (idAttribute.IsRequired)
                    {
                        result.Add($"Missing Id. Object: {source}");
                    }

                    continue;
                }

                if (usedValues.Contains(value))
                {
                    result.Add($"Duplicate Id. Object: {source}, Value: {value}");
                    continue;
                }

                usedValues.Add(value);
            }

            validationMessages = result.ToArray();
            return validationMessages.IsNullOrEmpty();
        }
    }
}