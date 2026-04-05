namespace Homework08.Extensions;

public static class EnumerableExtensions
{
    public static T GetMax<T>(this IEnumerable<T> collection, Func<T, float> convertToNumber)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(convertToNumber);

        using var enumerator = collection.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException("Коллекция не содержит элементов.");
        }

        var maxItem = enumerator.Current;
        var maxValue = convertToNumber(maxItem);

        while (enumerator.MoveNext())
        {
            var currentItem = enumerator.Current;
            var currentValue = convertToNumber(currentItem);

            if (currentValue > maxValue)
            {
                maxValue = currentValue;
                maxItem = currentItem;
            }
        }

        return maxItem;
    }
}