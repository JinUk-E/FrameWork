using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Mine.Framework.Manager.UINavigator.Runtime.Page
{
    public class PageContainer : UIContainer
    {
        #region Properties

        [field: SerializeField] public List<Page> RegisteredPages { get; private set; } = new();
        [field: SerializeField] public bool IsDefaultPage { get; private set; }
        internal Page DefaultPage { get; private set; }
        
        private Dictionary<Type, Page> Pages { get; set; }
        
        private Stack<Page> History { get; } = new();
        
        public Page CurrentView => History.TryPeek(out var currentView) ? currentView : null;
        private bool IsRemainHistory => DefaultPage ? History.Count > 1 : History.Count > 0;

        #endregion
        
        #region Unity Lifecycle
        
        protected override void Awake()
        {
            base.Awake();
            
            RegisteredPages = RegisteredPages.Select(x => x.IsRecycle ? Instantiate(x, transform) : x).GroupBy(x => x.GetType()).Select(x => x.FirstOrDefault()).ToList();
            Pages = RegisteredPages.ToDictionary(page => page.GetType(), page => page);

            foreach (var page in RegisteredPages.Where(x => x.IsRecycle))
            {
                page.UIContainer = this;
                page.gameObject.SetActive(false);
            }

            if(RegisteredPages.Any()&& IsDefaultPage) DefaultPage = Pages[RegisteredPages.First().GetType()];
        }

        private void OnEnable()
        {
            if (!IsDefaultPage || !Pages.TryGetValue(DefaultPage.GetType(), out var nextPage)) return;
            if (CurrentView)
            {
                CurrentView.HideAsync(false).Forget();
                if(!CurrentView.IsRecycle) Destroy(CurrentView.gameObject);
            }
            
            nextPage = nextPage.IsRecycle? nextPage: Instantiate(nextPage, transform);
            nextPage.ShowAsync(false).Forget();
            History.Push(nextPage);
        }

        #endregion

        #region Public

        public async UniTask<T> NextAsync<T>() where T : Page
        {
            if (Pages.TryGetValue(typeof(T), out var nextPage)) return await NextAsync(nextPage as T);
            Debug.LogError($"Not Found Page Type: {typeof(T)}");
            return null;
        }

        public async UniTask PrevAsync(int count = 1)
        {
            count = Mathf.Clamp(count, 1, History.Count);
            if(!IsRemainHistory) return;
            if(CurrentView.VisibleState is VisibleState.Appearing or VisibleState.Disappearing) return;
            
            CurrentView.HideAsync().Forget();
            
            for (var i = 0; i < count; i++)
            {
                if(!CurrentView.IsRecycle) Destroy(CurrentView.gameObject);
                History.Pop();
            }
            
            if(!CurrentView) return;
            
            await CurrentView.ShowAsync();
        }
        
        
        #endregion

        #region Private

       private async UniTask<T> NextAsync<T>(T nextPage) where T : Page
        {
            if (CurrentView && CurrentView.VisibleState is VisibleState.Appearing or VisibleState.Disappearing) return null;
            if (CurrentView && CurrentView == nextPage) return null;

            nextPage.gameObject.SetActive(false);
            nextPage = nextPage.IsRecycle ? nextPage : Instantiate(nextPage, transform);
            nextPage.UIContainer = this;
            
            if (CurrentView) CurrentView.HideAsync().Forget();
            History.Push(nextPage);

            await CurrentView.ShowAsync();

            return CurrentView as T;
        }


        #endregion
    }
}