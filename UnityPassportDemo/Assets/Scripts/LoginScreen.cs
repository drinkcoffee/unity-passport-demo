// Copyright Immutable Pty Ltd 2025
// SPDX-License-Identifier: MIT
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Immutable.Passport;
using System.Threading.Tasks;

namespace UnityPassportDemo {

    public class LoginScreen : MonoBehaviour {
        private Coroutine loginCheckRoutine;
        private bool isRunning = false;

        public void Start() {
            AuditLog.Log("Login screen");
            startCoroutine();
        }

        public void OnDisable() {
            stopCoroutine();
        }

        public async void OnButtonClick(string buttonText) {
            if (buttonText == "Login") {
                try {
#if (UNITY_ANDROID && !UNITY_EDITOR_WIN) || (UNITY_IPHONE && !UNITY_EDITOR_WIN) || UNITY_STANDALONE_OSX
                    await Passport.Instance.LoginPKCE();
#else
                    await Passport.Instance.Login();
#endif
                }
                catch (System.Exception e) {
                    AuditLog.Log($"Login failed: {e.Message}");
                }
            }
            else {
                AuditLog.Log("Login Screen: Unknown button");
            }
        }

        private void startCoroutine() {
            if (!isRunning) {
                loginCheckRoutine = StartCoroutine(LoginCheckRoutine());
                isRunning = true;
            }
        }

        public void stopCoroutine() {
            if (isRunning && loginCheckRoutine != null) {
                StopCoroutine(loginCheckRoutine);
                isRunning = false;
            }
        }

        IEnumerator LoginCheckRoutine() {
            while (true) {
                _ = CheckLoginAsync();
                yield return new WaitForSeconds(1f);
            }
        }

        private async Task CheckLoginAsync() {
            try {
                bool loggedIn = await Passport.Instance.HasCredentialsSaved();
                AuditLog.Log("CheckLogin: Loggedin: " + loggedIn);
                if (loggedIn) {
                    PassportStore.SetLoggedIn(true);
                    PassportStore.SetLoggedInChecked();
                    DeepLinkManager.Instance.LoginPath = DeepLinkManager.LOGIN_THREAD;
                    SceneManager.LoadScene("CheckScene", LoadSceneMode.Single);
                    stopCoroutine();
                }
            }
            catch (System.Exception e) {
                AuditLog.Log($"CheckLogin failed: {e.Message}");
            }
        }
    }
}