using UnityEngine;
using UnityEngine.UI;

public interface IClickable
{
    Image Icon { get; set; }
    int ThisCount { get; }
    Text StackText { get; }
}