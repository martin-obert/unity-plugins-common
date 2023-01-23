using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Obert.Common.Runtime.Extensions
{
    public static class SceneManagerExtensions
    {
        public static void SetActiveScene(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);
        }

        public static Scene[] GetLoadedScenes()
        {
            var result = new Scene[SceneManager.sceneCount];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = SceneManager.GetSceneAt(i);
            }

            return result;
        }

        public static bool IsSceneLoaded(this IEnumerable<Scene> source, string searchedScene)
            => IsSceneLoaded(source.Select(x => x.name), sceneName => string.Equals(searchedScene, sceneName));

        private static bool IsSceneLoaded(this IEnumerable<string> source, Func<string, bool> searchedScene)
        {
            var array = source as string[] ?? source.ToArray();
            array.ThrowIfEmptyOrNull();
            return array.Any(searchedScene);
        }

        public static string[] GetLoadedScenesNames()
        {
            var result = new string[SceneManager.sceneCount];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = SceneManager.GetSceneAt(i).name;
            }

            return result;
        }
    }
}