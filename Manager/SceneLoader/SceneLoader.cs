using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mine.Framework.Manager.SceneLoader
{
    public enum LoadState
    {
        Preprocessing,
        Loading,
        Loaded
    }

    public class SceneLoader
    {
        #region Field

        private readonly Subject<LoadState> loadStateSubject = new();
        
        #endregion

        #region Properties

        public string CurrentScene { get; set; } = SceneManager.GetActiveScene().name;
        public string PrevScene { get; private set; }
        public LoadState LoadState { get; private set; } = LoadState.Loaded;

        #endregion

        #region Private Methods

        private async UniTask LoadAsync(string targetSceneName)
        {
            LoadState = LoadState.Preprocessing;
            loadStateSubject.OnNext(LoadState);
            // Perform any necessary preprocessing before loading the scene

            // Load the scene asynchronously
            var loadOperation = SceneManager.LoadSceneAsync(targetSceneName);
            loadOperation.allowSceneActivation = false;

            LoadState = LoadState.Loading;
            loadStateSubject.OnNext(LoadState);

            while (!loadOperation.isDone)
            {
                // Update progress or perform other tasks while the scene is loading

                if (loadOperation.progress >= 0.9f)
                {
                    // Scene is almost loaded, allow scene activation
                    loadOperation.allowSceneActivation = true;
                }
                await UniTask.Yield();
            }

            LoadState = LoadState.Loaded;
            PrevScene = CurrentScene;
            CurrentScene = targetSceneName;
            loadStateSubject.OnNext(LoadState);
        }

        #endregion


        #region Public Methods

        public async UniTask LoadSceneAsync(string targetName)
        {
            if (LoadState == LoadState.Loading)
            {
                Debug.LogWarning("Already loading scene. Please wait until the previous scene finishes loading.");
                return;
            }
            if (targetName == CurrentScene)
            {
                Debug.LogWarning("Scene is already loaded.");
                return;
            }

            await LoadAsync(targetName);
        }
        
        public async UniTask LoadSceneAsync(int targetIndex)
        {
            if (targetIndex < 0 || targetIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError("Invalid scene index.");
                return;
            }
            var targetName = SceneUtility.GetScenePathByBuildIndex(targetIndex);
            var SceneName = Path.GetFileNameWithoutExtension(targetName);
            await LoadSceneAsync(SceneName);
        }

        #endregion
        
        
    }
}