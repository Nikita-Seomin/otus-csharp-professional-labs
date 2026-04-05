using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Homework07.Serialization;

public sealed class CsvReflectionSerializer
{
    private static readonly ConcurrentDictionary<Type, TypeMetadata> MetadataCache = new();

    public string Serialize<T>(T instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var metadata = GetMetadata(typeof(T));
        var headers = string.Join(",", metadata.Members.Select(static member => Escape(member.Name)));
        var values = string.Join(",", metadata.Members.Select(member => Escape(ConvertToString(member.GetValue(instance!)))));

        return $"{headers}{Environment.NewLine}{values}";
    }

    public T Deserialize<T>(string csv) where T : new()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(csv);

        using var reader = new StringReader(csv);
        var headerLine = reader.ReadLine() ?? throw new FormatException("CSV должен содержать строку заголовков.");
        var valueLine = reader.ReadLine() ?? throw new FormatException("CSV должен содержать строку значений.");

        var headers = ParseCsvLine(headerLine);
        var values = ParseCsvLine(valueLine);

        if (headers.Count != values.Count)
        {
            throw new FormatException("Количество заголовков и значений в CSV не совпадает.");
        }

        var metadata = GetMetadata(typeof(T));
        var instance = new T();

        for (var index = 0; index < headers.Count; index++)
        {
            if (!metadata.MembersByName.TryGetValue(headers[index], out var member))
            {
                continue;
            }

            var converted = ConvertFromString(values[index], member.MemberType);
            member.SetValue(instance!, converted);
        }

        return instance;
    }

    private static TypeMetadata GetMetadata(Type type)
    {
        return MetadataCache.GetOrAdd(type, static currentType =>
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var allMembers = new List<MemberAccessor>();

            foreach (var field in currentType.GetFields(flags))
            {
                if (field.IsStatic || field.IsDefined(typeof(CompilerGeneratedAttribute), false))
                {
                    continue;
                }

                allMembers.Add(new FieldAccessor(field));
            }

            foreach (var property in currentType.GetProperties(flags))
            {
                if (property.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                if (property.GetMethod is null || property.SetMethod is null)
                {
                    continue;
                }

                allMembers.Add(new PropertyAccessor(property));
            }

            var uniqueMembers = allMembers
                .OrderBy(member => GetInheritanceDepth(member.DeclaringType))
                .ThenBy(member => member.MetadataToken)
                .GroupBy(member => member.Name, StringComparer.OrdinalIgnoreCase)
                .Select(static group => group.First())
                .ToArray();

            if (uniqueMembers.Length == 0)
            {
                throw new InvalidOperationException($"Тип '{currentType.Name}' не содержит доступных для сериализации полей или свойств.");
            }

            var membersByName = uniqueMembers.ToDictionary(member => member.Name, StringComparer.OrdinalIgnoreCase);
            return new TypeMetadata(uniqueMembers, membersByName);
        });
    }

    private static int GetInheritanceDepth(Type? type)
    {
        var depth = 0;
        var current = type;

        while (current is not null)
        {
            depth++;
            current = current.BaseType;
        }

        return depth;
    }

    private static object? ConvertFromString(string rawValue, Type memberType)
    {
        if (memberType == typeof(string))
        {
            return rawValue;
        }

        var nullableUnderlyingType = Nullable.GetUnderlyingType(memberType);
        var targetType = nullableUnderlyingType ?? memberType;

        if (string.IsNullOrEmpty(rawValue))
        {
            if (nullableUnderlyingType is not null || !targetType.IsValueType)
            {
                return null;
            }

            return Activator.CreateInstance(targetType);
        }

        if (targetType == typeof(DateTime))
        {
            return DateTime.Parse(rawValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        if (targetType == typeof(DateTimeOffset))
        {
            return DateTimeOffset.Parse(rawValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }

        if (targetType == typeof(Guid))
        {
            return Guid.Parse(rawValue);
        }

        if (targetType == typeof(TimeSpan))
        {
            return TimeSpan.Parse(rawValue, CultureInfo.InvariantCulture);
        }

        if (targetType.IsEnum)
        {
            return Enum.Parse(targetType, rawValue, ignoreCase: true);
        }

        return Convert.ChangeType(rawValue, targetType, CultureInfo.InvariantCulture);
    }

    private static string ConvertToString(object? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        return value switch
        {
            DateTime dateTime => dateTime.ToString("O", CultureInfo.InvariantCulture),
            DateTimeOffset dateTimeOffset => dateTimeOffset.ToString("O", CultureInfo.InvariantCulture),
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? string.Empty
        };
    }

    private static string Escape(string value)
    {
        if (!value.Contains(',') && !value.Contains('"') && !value.Contains('\n') && !value.Contains('\r'))
        {
            return value;
        }

        return $"\"{value.Replace("\"", "\"\"")}\"";
    }

    private static List<string> ParseCsvLine(string line)
    {
        var values = new List<string>();
        var current = new StringBuilder();
        var inQuotes = false;

        for (var index = 0; index < line.Length; index++)
        {
            var symbol = line[index];

            if (inQuotes)
            {
                if (symbol == '"')
                {
                    var hasEscapedQuote = index + 1 < line.Length && line[index + 1] == '"';
                    if (hasEscapedQuote)
                    {
                        current.Append('"');
                        index++;
                    }
                    else
                    {
                        inQuotes = false;
                    }
                }
                else
                {
                    current.Append(symbol);
                }

                continue;
            }

            if (symbol == ',')
            {
                values.Add(current.ToString());
                current.Clear();
                continue;
            }

            if (symbol == '"')
            {
                inQuotes = true;
                continue;
            }

            current.Append(symbol);
        }

        if (inQuotes)
        {
            throw new FormatException("Некорректная CSV-строка: не закрыта кавычка.");
        }

        values.Add(current.ToString());
        return values;
    }

    private sealed class TypeMetadata(
        IReadOnlyList<MemberAccessor> members,
        IReadOnlyDictionary<string, MemberAccessor> membersByName)
    {
        public IReadOnlyList<MemberAccessor> Members { get; } = members;

        public IReadOnlyDictionary<string, MemberAccessor> MembersByName { get; } = membersByName;
    }

    private abstract class MemberAccessor
    {
        public abstract string Name { get; }

        public abstract Type MemberType { get; }

        public abstract Type? DeclaringType { get; }

        public abstract int MetadataToken { get; }

        public abstract object? GetValue(object instance);

        public abstract void SetValue(object instance, object? value);
    }

    private sealed class FieldAccessor(FieldInfo fieldInfo) : MemberAccessor
    {
        public override string Name => fieldInfo.Name;

        public override Type MemberType => fieldInfo.FieldType;

        public override Type? DeclaringType => fieldInfo.DeclaringType;

        public override int MetadataToken => fieldInfo.MetadataToken;

        public override object? GetValue(object instance)
        {
            return fieldInfo.GetValue(instance);
        }

        public override void SetValue(object instance, object? value)
        {
            fieldInfo.SetValue(instance, value);
        }
    }

    private sealed class PropertyAccessor(PropertyInfo propertyInfo) : MemberAccessor
    {
        public override string Name => propertyInfo.Name;

        public override Type MemberType => propertyInfo.PropertyType;

        public override Type? DeclaringType => propertyInfo.DeclaringType;

        public override int MetadataToken => propertyInfo.MetadataToken;

        public override object? GetValue(object instance)
        {
            return propertyInfo.GetValue(instance);
        }

        public override void SetValue(object instance, object? value)
        {
            propertyInfo.SetValue(instance, value);
        }
    }
}
