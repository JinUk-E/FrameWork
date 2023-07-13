using UnityEngine;

namespace Mine.Framework.Manager.UINavigator.Runtime.Page
{
    public class Page : UIView
    {
        [field: SerializeField] public bool IsRecycle { get; private set; } = true;
    }
}