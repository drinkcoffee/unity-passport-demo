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
    public class WelcomeScreen : MonoBehaviour {
        public async Task Start() {
            AuditLog.Log("Welcome screen");
            await PassportLogin.Init();

            // If the player is already logged in, then skip the login screen
            if (PassportStore.IsLoggedIn() || await Passport.Instance.HasCredentialsSaved()) {
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
    }
}
