using System.Linq.Expressions;

namespace Shift_System.Shared.Helpers
{
    public static class QueryFilterHelper
    {
        public static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, List<Filter> filters)
        {
            foreach (var filter in filters)
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var property = Expression.Property(parameter, filter.Field);

                object value = ConvertValue(filter.Value, property.Type);
                var constant = Expression.Constant(value, Nullable.GetUnderlyingType(property.Type) ?? property.Type);

                Expression comparison;

                switch (filter.Operator.ToLower())
                {
                    case "eq":
                        if (Nullable.GetUnderlyingType(property.Type) != null)
                        {
                            // Nullable<bool> karşılaştırması
                            var hasValueExpression = Expression.Property(property, "HasValue");
                            var valueExpression = Expression.Property(property, "Value");
                            var equalExpression = Expression.Equal(valueExpression, constant);

                            comparison = Expression.AndAlso(hasValueExpression, equalExpression);
                        }
                        else
                        {
                            comparison = Expression.Equal(property, constant);
                        }
                        break;

                    case "contains":
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        comparison = Expression.Call(property, containsMethod!, constant);
                        break;

                    case "gt":
                        comparison = Expression.GreaterThan(property, constant);
                        break;

                    case "lt":
                        comparison = Expression.LessThan(property, constant);
                        break;

                    default:
                        throw new NotSupportedException($"Operator '{filter.Operator}' is not supported.");
                }

                var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
                query = query.Where(lambda);
            }

            return query;
        }

        private static object ConvertValue(object value, Type targetType)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");

            // Eğer hedef tür Nullable bir türse, gerçek tipi çıkarıyoruz
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            // Eğer değer zaten doğru türdeyse dönüşüm yapmadan geri döndür
            if (value.GetType() == underlyingType)
                return value;

            try
            {
                if (underlyingType == typeof(bool))
                {
                    if (value is bool boolValue)
                        return boolValue;

                    return bool.Parse(value.ToString() ?? throw new InvalidOperationException());
                }
                if (underlyingType == typeof(DateTime))
                {
                    return DateTime.Parse(value.ToString() ?? throw new InvalidOperationException());
                }
                if (underlyingType == typeof(Guid))
                {
                    if (Guid.TryParse(value.ToString(), out var guidValue))
                        return guidValue;

                    throw new InvalidCastException($"Failed to convert '{value}' to type 'Guid'.");
                }
                if (underlyingType.IsEnum)
                {
                    return Enum.Parse(underlyingType, value.ToString() ?? throw new InvalidOperationException());
                }

                // Genel dönüşüm işlemi
                return Convert.ChangeType(value, underlyingType);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Failed to convert '{value}' to type '{targetType.Name}'.", ex);
            }
        }
    }
}