using System.Collections.Generic;
using UnityEngine;

class Pair<T>
{
    private T a;
    public T A {
        get { return a; }
    }
    private T b;
    public T B {
        get {
            Debug.Assert(!defective);
            return b;
        }
    }
    private bool defective;
    public bool Defective {
        get { return defective; }
    }

    public Pair(T _a, T _b)
    {
        this.a = _a;
        this.b = _b;
    }

    public Pair(T value)
    {
        this.a = value;
        defective = true;
    }
    
    public bool Has(T value)
    {
        return EqualityComparer<T>.Default.Equals(value, a) || (!defective && EqualityComparer<T>.Default.Equals(value, b));
    }

    public bool IsMatch(T _a, T _b)
    {
        return !defective && Has(_a) && Has(_b)
                && (!EqualityComparer<T>.Default.Equals(_a, _b) || EqualityComparer<T>.Default.Equals(a, b));
    }

    public override string ToString()
    {
        return defective ? "Defective Pair [" + a + ", --]" : "Pair [" + a + ", " + b + "]";
    }
}
