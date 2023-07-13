using System.Collections.Generic;
using UnityEngine;

namespace Mine.Framework.Manager.UINavigator.Runtime
{
    public abstract class UIContainer : MonoBehaviour
    {
        // start game with this container
        // this container will be the root of all UI
        // This container will be the parent container of the UI without a parent set
        private static readonly Dictionary<int, UIContainer> CacheContainers = new();
        
        #region Properties
        
        [field: SerializeField] public string ContainerName { get; private set; }
        
        #endregion
        
#region Unity Lifecycle
    
        protected virtual void Awake()
        {
            var containerHash = GetInstanceID();
            if (CacheContainers.ContainsKey(containerHash)) return;
            CacheContainers.Add(containerHash, this);
        }

#endregion
        

        #region Public Methods

        public UIContainer of(RectTransform rect, bool useCache = true)
        {
            var containerHash = rect.GetInstanceID();
            if(useCache && CacheContainers.TryGetValue(containerHash, out var container))
            {
                return container;
            }
            container = rect.GetComponentInParent<UIContainer>();
            if (container.Equals(null)) return null;
            CacheContainers.Add(containerHash, container);
            return container;
        }
        
        public UIContainer of(Transform transform, bool useCache = true) => of((RectTransform)transform, useCache);

        #endregion
    }
}