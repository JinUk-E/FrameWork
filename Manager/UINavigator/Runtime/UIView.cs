using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mine.Framework.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace Mine.Framework.Manager.UINavigator.Runtime
{
    public enum VisibleState
    {
        Appearing, // 등장 중
        Appeared, // 등장 완료
        Disappearing, // 사라지는 중
        Disappeared // 사라짐
    }
    
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class UIView : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        
        public static List<UIView> ActiveViews { get; } = new();
        public UIContainer Container { get; private set; }
        public CanvasGroup CanvasGroup => canvasGroup ? canvasGroup : canvasGroup = GetComponent<CanvasGroup>();
        public UIContainer UIContainer { get; set; }
        public VisibleState VisibleState { get; private set; } = VisibleState.Disappeared;
        
        
        #region Public Methods
        internal async UniTask ShowAsync(bool useAnim = true)
        {
            ActiveViews.Add(this);
            var rectTransform = (RectTransform) transform;
            await rectTransform.SetCenterAndFillParent();
            CanvasGroup.alpha = 1;

            await BeforeProcessAsync();
            gameObject.SetActive(true);

            if (useAnim) Debug.Log("Show Animation");
            await BeginProcessAsync();
        }
        
        internal async UniTask HideAsync(bool useAnim = true)
        {
            ActiveViews.Remove(this);
            await AfterProcessAsync();
            gameObject.SetActive(false);
            if (useAnim) Debug.Log("Hide Animation");
            await FinishProcessAsync();
        }
        
        #endregion

        #region Abstract Methods

        protected virtual UniTask BeforeProcessAsync() => UniTask.CompletedTask;
        protected virtual UniTask BeginProcessAsync() => UniTask.CompletedTask;
        protected virtual UniTask AfterProcessAsync() => UniTask.CompletedTask;
        protected virtual UniTask FinishProcessAsync() => UniTask.CompletedTask;
        
        #endregion
    }
}