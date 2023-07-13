using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Mine.Framework.Manager.UINavigator.Runtime.Sheet
{
    public class SheetContainer : UIContainer
    {
        [field:SerializeField] public List<Sheet> RegisteredSheets { get; private set; } = new();
        [field:SerializeField] public bool IsDefault { get; private set; } = false;
        Dictionary<Type, Sheet> Sheets { get; set; }
        
        public Sheet CurrentView { get; private set; }
        
        protected void Awake()
        {
            RegisteredSheets = RegisteredSheets.Select(x => x.IsRecycle ? UnityEngine.Object.Instantiate(x, transform) : x).GroupBy(x => x.GetType()).Select(x => x.FirstOrDefault()).ToList();
            Sheets = RegisteredSheets.ToDictionary(sheet => sheet.GetType(), sheet => sheet);

            foreach (var sheet in RegisteredSheets.Where(x => x.IsRecycle))
            {
                sheet.UIContainer = this;
                sheet.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            if (!RegisteredSheets.Any() || !IsDefault) return;
            var nextSheet = Sheets[RegisteredSheets.First().GetType()];
            if (CurrentView) CurrentView.HideAsync(false).Forget();
            CurrentView = nextSheet;
            CurrentView.ShowAsync(false).Forget();
        }

        #region Public Methods

        public async UniTask<T> NextAsync<T>() where T : Sheet
        {
            if (Sheets.TryGetValue(typeof(T), out var nextSheet))
                return await NextAsync(nextSheet as T);
            Debug.LogError($"Not Found Sheet : {typeof(T)}");
            return null;
        }

        #endregion
        


        #region Private Methods

        private async UniTask<T> NextAsync<T>(T nextSheet) where T : Sheet
        {
            if (!CurrentView.Equals(null))
            {
                if(CurrentView.VisibleState is VisibleState.Appearing or VisibleState.Disappearing) return null;
                if(CurrentView.Equals(nextSheet)) return null;
            }

            var prev = CurrentView;
            nextSheet.gameObject.SetActive(false);
            nextSheet = nextSheet.IsRecycle ? nextSheet : Instantiate(nextSheet, transform);
            
            nextSheet.UIContainer = this;
            
            if (prev)
            {
                prev.HideAsync(false).Forget();
                if(!prev.IsRecycle) Destroy(prev.gameObject);
            }

            await CurrentView.ShowAsync();
            return CurrentView as T;
        }

        #endregion
        
    }
}