using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class SceneChanger : MonoBehaviour
    {
        private Animator _animator;

        private int _nextScene;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _animator = GetComponent<Animator>();
            
            SceneManager.sceneLoaded += OnsceneLoaded;
        }

        private void OnsceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _animator.SetTrigger("SceneLoaded");
        }

        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        public void ChangeNextScene()
        {
            SceneManager.LoadScene(_nextScene);
        }

        public void PlayGame()
        {
            _animator.SetTrigger("ChangeScene");
            _nextScene = 1;
        }
        
        public void ExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else 
            Application.Quit();
            #endif
        }
    }
}
