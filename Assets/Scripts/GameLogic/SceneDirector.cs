using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameLogic
{
    // [CreateAssetMenu(menuName = "ZnZproductions/SceneDirector", fileName = "SceneDirector")]
    public class SceneDirector : SingletonBase<SceneDirector>
    {
        private static bool startedUp = false;
        [SerializeField] private Scene startupScene;

        // same order as in build settings
        public enum Scene
        {
            Core,
            Main,
            Menu,
            Story
        }

        public static void LoadScene(Scene scene)
        {
            SceneManager.LoadScene((int) Scene.Core);
            SceneManager.LoadSceneAsync((int) scene, LoadSceneMode.Additive);
        }

        // public static void LoadScene(Scene scene)
        // {
        //     SceneManager.LoadScene((int) scene);
        // }


        private void Start()
        {
#if !UNITY_EDITOR
            if (!startedUp)
                SceneManager.LoadSceneAsync((int) startupScene, LoadSceneMode.Additive);
#endif
            startedUp = true;
        }
    }
}