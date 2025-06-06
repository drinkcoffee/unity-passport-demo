// Copyright Immutable Pty Ltd 2025
// SPDX-License-Identifier: MIT
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace UnityPassportDemo {

    public class DeepLinkManager : MonoBehaviour {

        public static DeepLinkManager Instance { get; private set; }

        public const int NONE = 1; 
        public const int WELCOME = 2; 
        public const int LOGIN_THREAD = 3; 
        public const int LOGIN_SKIP = 4; 
        public const int DEEP_LINK = 5; 
        public int LoginPath = NONE;

        public void Start() {
            if (Instance == null) {
                Instance = this;
                // Register the deep link handler.
                Application.deepLinkActivated += onDeepLinkActivated;
                AuditLog.Log("Deep link handler registered");

                if (!string.IsNullOrEmpty(Application.absoluteURL)) {
                    // Cold start and Application.absoluteURL not null so process Deep Link.
                    onDeepLinkActivated(Application.absoluteURL);
                }
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }


        /**
        * Called when a deep link is activated.
        * Processes the URL, loads the specified scene if valid, and extracts additional parameters.
        *
        * @param url The URL received from the deep link.
        */
        private void onDeepLinkActivated(string url) {
            AuditLog.Log("Deep link activated: " + url);

            if (url == WelcomeScreen.RedirectUri) {
                AuditLog.Log("Deep link is login");
                LoginPath = DEEP_LINK;
                SceneManager.LoadScene("CheckScene", LoadSceneMode.Single);
            }
            else if (url == WelcomeScreen.LogoutUri) {
                AuditLog.Log("Deep link is logout");
                SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
            }
            else {
                AuditLog.Log("ERROR: Unknown deeplink: " + url);
            }
        }
    }
}