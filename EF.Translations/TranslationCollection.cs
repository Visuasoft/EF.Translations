using System.Collections;
using System.Linq.Expressions;

namespace EF.Translations;

public class TranslationCollection<TTranslation> : ICollection<TTranslation>
    where TTranslation : class, IEntityTranslation
{
    private readonly List<TTranslation> _data = new List<TTranslation>();

    public IEnumerator<TTranslation> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(TTranslation item)
    {
        _data.Add(item);
    }

    public void Clear()
    {
        _data.Clear();
    }

    public bool Contains(TTranslation item)
    {
        return _data.Contains(item);
    }

    public void CopyTo(TTranslation[] array, int arrayIndex)
    {
        _data.CopyTo(array, arrayIndex);
    }

    public bool Remove(TTranslation item)
    {
        return _data.Remove(item);
    }

    public int Count => _data.Count;
    public bool IsReadOnly => false;

    public TTranslation this[string language] =>
        _data.SingleOrDefault(t => t.Language.Equals(language, StringComparison.OrdinalIgnoreCase));

    public void AddTranslations(Expression<Func<TTranslation, object>> expression, TranslatedItem[] translations)
    {
        if (!(expression?.Body is MemberExpression exp)) return;

        foreach (var item in translations)
        {
            AddTranslation(expression, item);
        }
    }

    public void AddTranslation(Expression<Func<TTranslation, object>> expression, TranslatedItem item)
    {
        if (!(expression?.Body is MemberExpression exp)) return;

        TTranslation translation;
        if (!_data.Exists(translation =>
                translation.Language.Equals(item.Language, StringComparison.OrdinalIgnoreCase)))
        {
            translation = Activator.CreateInstance<TTranslation>();
            translation.Language = item.Language;

            _data.Add(translation);
        }

        translation = this[item.Language];
        translation.GetType().GetProperty(exp.Member.Name).SetValue(translation, item.Value);
    }

    public TranslatedItem[] TranslatedValues<TTranslation>(Expression<Func<TTranslation, object>> expression)
    {
        if (!(expression?.Body is MemberExpression exp)) return null;
        return _data.Select(d => new TranslatedItem
        {
            Language = d.Language,
            Value = d.GetType().GetProperty(exp.Member.Name).GetValue(d)
        }).ToArray();
    }


    public TTranslatedValue[] ValuesFor<TTranslatedValue>(Expression<Func<TTranslation, TTranslatedValue>> expression)
    {
        if (expression?.Body is not MemberExpression exp)
        {
            throw new NotSupportedException($"{nameof(ValuesFor)} should be called with a member expression");
        }

        var array = _data
            .Select(item => item.GetType().GetProperty(exp.Member.Name).GetValue(item))
            .Cast<TTranslatedValue>()
            .ToArray();

        return array;
    }

    
}