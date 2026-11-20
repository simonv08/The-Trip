using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace BNG {

    /// <summary>
    /// This script will toggle a GameObject whenever the provided InputAction is executed
    /// </summary>
    public class ToggleActiveOnInputAction : MonoBehaviour {

        public InputActionReference InputAction = default;
        public GameObject ToggleObject = default;

        public ControllerBinding controllerBinding = ControllerBinding.None;

        void Update() {
            if (controllerBinding != ControllerBinding.None) {
                if(InputBridge.Instance.GetControllerBindingValue(controllerBinding) || Input.GetKeyDown(KeyCode.P)) {
                    ToggleObject.SetActive(!ToggleObject.activeSelf);
                    TogglePause();
                }
            }
        }

        private void OnEnable() {
            if(InputAction) {
                InputAction.action.performed += ToggleActive;
            }
        }

        private void OnDisable() {
            if (InputAction) {
                InputAction.action.performed -= ToggleActive;
            }
        }

        public void ToggleActive(InputAction.CallbackContext context) {
            if(ToggleObject) {
                ToggleObject.SetActive(!ToggleObject.activeSelf);
            }
        }

        //public GameObject pauseMenuCanvas;
        private bool isPaused = false;

        public void TogglePause()
        {
            isPaused = !isPaused;
            //pauseMenuCanvas.SetActive(isPaused);

            if (isPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }


        public void ContinueGame()
        {
            isPaused = false;
            //pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }


        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}

