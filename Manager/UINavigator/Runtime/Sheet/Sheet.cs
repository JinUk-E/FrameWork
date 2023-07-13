using UnityEngine;

namespace Mine.Framework.Manager.UINavigator.Runtime.Sheet
{
    public class Sheet : UIView
    {
        [field: SerializeField] public bool IsRecycle { get; private set; } = true;
    }
}