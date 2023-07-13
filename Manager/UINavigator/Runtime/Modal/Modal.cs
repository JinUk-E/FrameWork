using UnityEngine;
using UnityEngine.UI;

namespace Mine.Framework.Manager.UINavigator.Runtime.Modal
{
    public class Modal : UIView
    {
        [field: SerializeField] public CanvasGroup Background { get; internal set; }
        [field: SerializeField] public Button CloseButton { get; internal set; }
    }
}