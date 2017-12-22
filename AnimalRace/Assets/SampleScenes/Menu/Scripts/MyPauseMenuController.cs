using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.SampleScenes.Menu.Scripts
{
    class MyPauseMenuController : MonoBehaviour
    {
        public void Resume()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
        public void Restart()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        public void Quit()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            Application.Quit();
        }
    }
}
