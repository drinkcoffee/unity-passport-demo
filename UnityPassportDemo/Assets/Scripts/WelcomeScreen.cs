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
        public static bool UseMainnet;


        public void Start() {
            AuditLog.Log("Welcome screen");
        }

        public async void OnButtonClick(string buttonText) {
            if (buttonText == "Mainnet") {
                await InitPassport(true);
            }
            else if (buttonText == "Testnet") {
                await InitPassport(false);
            }
            else {
                AuditLog.Log($"WelcomeScreen: Unknown button: {buttonText}");
            }
        }


        private async Task InitPassport(bool useMainnet) {
            try {
                UseMainnet = useMainnet;
                AuditLog.Log("0");
                await PassportLogin.Init(useMainnet);
                AuditLog.Log("1");
                AuditLog.Log($"is logged in {PassportStore.IsLoggedIn()}, has credentials {await Passport.Instance.HasCredentialsSaved()}");

                // If the player is already logged in, then skip the login screen
                if (PassportStore.IsLoggedIn() || await Passport.Instance.HasCredentialsSaved()) {
                AuditLog.Log("2");
                    // Try to log in using saved credentials
                    bool success = await Passport.Instance.Login(useCachedSession: true);
                    PassportStore.SetLoggedIn(success);
                    if (success) {
                        PassportStore.SetLoggedInChecked();
                        DeepLinkManager.Instance.LoginPath = DeepLinkManager.WELCOME;
                        SceneManager.LoadScene("CheckScene", LoadSceneMode.Single);
                    }
                    else {
                        SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
                    }
                }
                else {
                    PassportStore.SetLoggedIn(false);
                    SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
                }
            }
            catch (Exception ex) {
                AuditLog.Log($"Exception: {ex.Message}");
            }
        }
    }
}
