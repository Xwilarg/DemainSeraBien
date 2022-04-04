using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LudumDare50
{
	public class LoadAdditiveScenes : MonoBehaviour
	{
		[SerializeField] private string[] _scenes;

		public string[] Scenes { get => _scenes; set => _scenes = value; }


		private IEnumerator Start()
		{
			foreach (string scenePath in _scenes)
			{
				Scene scene = SceneManager.GetSceneByName(scenePath);
				if (scene.IsValid() && scene.isLoaded)
					continue;

				AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
				yield return sceneLoading;
			}
		}
	}

}
