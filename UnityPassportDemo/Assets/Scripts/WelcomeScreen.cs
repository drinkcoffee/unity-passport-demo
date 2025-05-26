// Copyright Immutable Pty Ltd 2025
// SPDX-License-Identifier: MIT
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Immutable.Passport;
using System.Threading.Tasks;

namespace UnityPassportDemo {
    public class WelcomeScreen : MonoBehaviour {

        public static bool UsingMainnet;

        public static string RedirectUri = "unityimmutablecheck://callback";
        public static string LogoutUri = "unityimmutablecheck://logout";

        // Passport Client ID: This is different for each game. Get this Immutable Hub.
        private static string testNetClientId = "l3l9rlc0w1eAPZqyIo2NCSu6Onrufwjw";
        private static string mainNetClientId = "MAgEqiV6HUzsnNwbh2JnZuu1X4lPlZco";

        public void Start() {
            AuditLog.Reset();
            AuditLog.Log("Welcome screen");
        }

        public async void OnButtonClick(string buttonText) {
            if (buttonText == "Mainnet") {
                UsingMainnet = true;
                await launch(true);
            }
            else if (buttonText == "Testnet") {
                await launch(false);
                UsingMainnet = false;
            }
            else {
                AuditLog.Log($"WelcomeScreen: Unknown button: {buttonText}");
            }
        }

        private async Task launch(bool useMainNet) {
            try {
                string network = useMainNet ? "Mainnet" : "Testnet";
                AuditLog.Log($"Launching: {network}");
                await init(useMainNet);

                // If the player is already logged in, then skip the login screen
                if (await Passport.Instance.HasCredentialsSaved()) {
                    // Try to log in using saved credentials
                    bool success = await Passport.Instance.Login(useCachedSession: true);
                    if (success) {
                        DeepLinkManager.Instance.LoginPath = DeepLinkManager.WELCOME;
                        SceneManager.LoadScene("CheckScene", LoadSceneMode.Single);
                    }
                    else {
                        SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
                    }
                }
                else {
                    SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
                }
            }
            catch (Exception ex) {
                AuditLog.Log($"ERROR during start-up: {ex.Message}\nStack: {ex.StackTrace}"); 
                SceneManager.LoadScene("UnexpectedErrorScene", LoadSceneMode.Single);
            }
        }

        private static async Task init(bool useMainNet) {
            string redirectUri = null;
            string logoutUri = null;
            #if (UNITY_ANDROID && !UNITY_EDITOR_WIN) || (UNITY_IPHONE && !UNITY_EDITOR_WIN) || UNITY_STANDALONE_OSX
                    redirectUri = RedirectUri;
                    logoutUri = LogoutUri;
            #endif
            
            // Set the environment to SANDBOX for testing or PRODUCTION for production
            string environment;
            string clientId;
            if (useMainNet) {
                environment = Immutable.Passport.Model.Environment.PRODUCTION;
                clientId = mainNetClientId;
            }
            else {
                environment = Immutable.Passport.Model.Environment.SANDBOX;
                clientId = testNetClientId;
            }

            if (Immutable.Passport.Passport.Instance == null) {
                await Immutable.Passport.Passport.Init(clientId, environment, redirectUri, logoutUri);
            }
        }
    }
}
