// Copyright Immutable Pty Ltd 2025
// SPDX-License-Identifier: MIT
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.SceneManagement;

using Immutable.Passport;
using Immutable.Passport.Model;

namespace UnityPassportDemo {

    public class PassportLogin {

        public static string RedirectUri = "unityimmutablecheck://callback";
        public static string LogoutUri = "unityimmutablecheck://logout";

        // Passport Client ID: This is different for each game. Get this Immutable Hub.
        public static string ClientId = "l3l9rlc0w1eAPZqyIo2NCSu6Onrufwjw";


        public static async Task Init() {
            string redirectUri = null;
            string logoutUri = null;
            #if (UNITY_ANDROID && !UNITY_EDITOR_WIN) || (UNITY_IPHONE && !UNITY_EDITOR_WIN) || UNITY_STANDALONE_OSX
                    redirectUri = RedirectUri;
                    logoutUri = LogoutUri;
            #endif
            
            // Set the environment to SANDBOX for testing or PRODUCTION for production
            string environment = Immutable.Passport.Model.Environment.SANDBOX;

            if (Immutable.Passport.Passport.Instance == null) {
                await Immutable.Passport.Passport.Init(ClientId, environment, redirectUri, logoutUri);
            }
        }

        public static async Task Login() {
            // Check login
            bool isLoggedIn = PassportStore.IsLoggedIn();
            bool recentlyCheckedLogin = PassportStore.WasLoggedInRecently();
            AuditLog.Log("isloggedIn: " + isLoggedIn + ", recentlyCheckedLogin: " + recentlyCheckedLogin);
            
            if (isLoggedIn) {
                if (!recentlyCheckedLogin) {
                    if (await Immutable.Passport.Passport.Instance.HasCredentialsSaved()) {
                        // Try to log in using saved credentials
                        bool success = await Immutable.Passport.Passport.Instance.Login(useCachedSession: true);
                        if (success) {
                            PassportStore.SetLoggedInChecked();
                        }
                        else {
                            AuditLog.Log("PassportLogin: Login using cached credentials failed. Going to login screen");
                            SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
                        }
                    }
                    else {
                        AuditLog.Log("PassportLogin: Does not have cached credentials. Going to login screen");
                        SceneManager.LoadScene("LoginScene", LoadSceneMode.Single);
                    }
                }
                // Set up provider
                await Immutable.Passport.Passport.Instance.ConnectEvm();
                // Set up wallet (includes creating a wallet for new players)
                List<string> accounts = await Immutable.Passport.Passport.Instance.ZkEvmRequestAccounts();
                if (accounts.Count !=0) {
                    string account = accounts[0];
                    PassportStore.SetPassportAccount(account);
                    AuditLog.Log($"Logged in as {account} of {accounts.Count} accounts");
                }
            }
        }

    }
}



