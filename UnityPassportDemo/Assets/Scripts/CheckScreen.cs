// Copyright Immutable Pty Ltd 2025
// SPDX-License-Identifier: MIT
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Numerics;
using Immutable.Passport;

namespace UnityPassportDemo {

    public class CheckImmutable : MonoBehaviour {
        public Button checkButton;
        public TextMeshProUGUI outputText;
        public TextMeshProUGUI passportText;

        private const int TIME_PER_DOT = 1000;
        DateTime timeOfLastDot;

        string status;
        private bool isProcessing = false;


        public async void Start() {
            AuditLog.Log("Check screen");

            // Set up provider
            await Immutable.Passport.Passport.Instance.ConnectEvm();
            // Set up wallet (includes creating a wallet for new players)
            List<string> accounts = await Passport.Instance.ZkEvmRequestAccounts();
            if (accounts.Count ==0) {
                passportText.text = "Logged In (" + 
                                DeepLinkManager.Instance.LoginPath + 
                                ")"; 
            }
            else {
                string account = accounts[0];
                passportText.text = "Logged In (" + 
                                DeepLinkManager.Instance.LoginPath + 
                                ") as\n" + 
                                account;
            }
        }

        public void OnButtonClick(string buttonText) {
            if (buttonText == "Check") {
                checkButton.interactable = false;
                testProcess();
            }
            else {
                AuditLog.Log($"CheckScreen: Unknown button: {buttonText}");
            }
        }


        private async void testProcess() {
            if (isProcessing) {
                return;
            }
            isProcessing = true;

            status = WelcomeScreen.UsingMainnet ? "Checking Mainnet\n" : "Checking Testnet\n";

            timeOfLastDot = DateTime.Now;

            try {
                CheckContract contract = new CheckContract(WelcomeScreen.UsingMainnet);

                try {
                    addToStatus("Calling SetValue(17)");
                    var success = await contract.SetValue(17);
                    if (!success) {
                        addToStatus("Error during SetValue(17): Transaction failed");
                    }
                    else {
                        addToStatus("Completed SetValue(17)");
                    }
                }
                catch (Exception ex) {
                    addToStatus($"Error during SetValue(17): Exception: {ex.Message}");
                }

                try {
                    addToStatus("Calling GetValue()");
                    var val = await contract.GetValue();
                    addToStatus($" GetValue returned: {val}");
                }
                catch (Exception ex) {
                    addToStatus($" Error during GetValue(17): Exception: {ex.Message}");
                }
            } 
            finally {
                isProcessing = false;
            }
            addToStatus("Done");
            checkButton.interactable = true;
        }


        public void Update() {
            // if (isProcessing) {
            //     DateTime now = DateTime.Now;
            //     if ((now - timeOfLastDot).TotalMilliseconds > TIME_PER_DOT) {
            //         timeOfLastDot = now;
            //         status = status + ".";
            //     }
            // }
            outputText.text = status;
        }


        private void setStatus(string s) {
            status = "";
            addToStatus(s);
        }

        private void addToStatus(string s) {
            AuditLog.Log(s);
            string timestamp = DateTime.Now.ToString("yyyyMMdd: HHmmss.fff");
            string logEntry = $"{timestamp}: {s}";
            status = status + logEntry + "\n";
        }

    }
}