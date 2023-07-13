using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Mine.Framework.Extension;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Mine.Framework.Manager.UINavigator.Runtime.Modal
{
    public class ModalContainer : UIContainer
    {
        #region Fields

        [SerializeField] private BackGround modalBackGround; // 생성된 모달의 뒤에 배치될 레이어

        #endregion
        
        #region Properties

        [field: SerializeField] public List<Modal> RegisterModals { get; private set; } = new();
        
        private Dictionary<Type, Modal> Modals { get; set; }

        public Modal CurrentView { get; private set; }
        
        #endregion

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();
            Modals = RegisterModals.GroupBy(x => x.GetType()).Select(x => x.FirstOrDefault()).ToDictionary(modal => modal.GetType(), modal => modal);
        }

        private void OnEnable()
        {
            var nextModal = Modals.Values.FirstOrDefault();
            
            if (CurrentView)
            {
                CurrentView.HideAsync(false).Forget();
                Destroy(CurrentView.gameObject);
            }
            CurrentView = Instantiate(nextModal, transform);
        }

        #endregion

        #region Public Methods

        public async UniTask PrevAsync()
        {
            if (!CurrentView) return;
            if (CurrentView.VisibleState is VisibleState.Appearing or VisibleState.Disappearing) return;

            CurrentView.HideAsync().Forget();
            await UniTask.WaitUntil(() => CurrentView.VisibleState == VisibleState.Disappeared);
            Destroy(CurrentView.gameObject);
            CurrentView.HideAsync();
        }

        #endregion
        
        
        #region Private Methods

        
        private async UniTask<CanvasGroup> ShowBackGround()
        {
            if(modalBackGround.Equals(null)) return null;
            
            var backGround = Instantiate(modalBackGround.gameObject, transform,true);

            if (!backGround.TryGetComponent<CanvasGroup>(out var canvasGroup))
                canvasGroup = backGround.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;

            var rectTransform = (RectTransform)backGround.transform;
            await rectTransform.SetCenterAndFillParent();

            return canvasGroup;
        }
        
        private async UniTask<T> NextAsync<T>(T nextModal) where T : Modal
        {
            if(CurrentView.VisibleState is VisibleState.Appearing or VisibleState.Disappearing) return null;
            
            var backGround = await ShowBackGround();
            
            nextModal.gameObject.SetActive(true);
            nextModal.UIContainer = this;
            if (backGround)
            {
                nextModal.Background = backGround;
                if (!nextModal.Background.TryGetComponent<Button>(out var button))
                    button = nextModal.Background.gameObject.AddComponent<Button>();

                button.OnClickAsObservable().Subscribe(_ => PrevAsync().Forget());
            }

            CurrentView = nextModal;
            #pragma warning disable 4014
            if (nextModal.Background) CurrentView.Background.DOFade(1, 0.2f);
            #pragma warning restore 4014
            
            await CurrentView.ShowAsync();
            return CurrentView as T;
        }

        #endregion
        
    }
}