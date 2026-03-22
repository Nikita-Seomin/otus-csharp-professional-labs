namespace Homework05.Domain;

/// <summary>
/// Типобезопасный клон: возвращает конкретный тип, а не object.
/// </summary>
public interface IMyCloneable<out T>
{
    T MyClone();
}