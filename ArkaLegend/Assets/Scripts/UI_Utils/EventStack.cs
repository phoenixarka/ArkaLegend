using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public delegate void UpdateStackEvent();

public class EventStack<T> : Stack<T>
{
    public event UpdateStackEvent OnPush;
    public event UpdateStackEvent OnPop;
    public event UpdateStackEvent OnClear;

    public EventStack() { }

    public EventStack(EventStack<T> eventStack) : base(eventStack){ }

    public new void Push(T obj) {
        base.Push(obj);

        if (OnPush != null) {
            OnPush();
        }
    }

    public new T Pop()
    {
        T obj = base.Pop();

        if (OnPop != null)
        {
            OnPop();
        }
        return obj;
    }

    public new void Clear() {
        base.Clear();

        if (OnClear != null)
        {
            OnClear();
        }
    }

}