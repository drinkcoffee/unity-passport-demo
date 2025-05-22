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
        public Button mainnetButton;
        public Button testnetButton;
        public TextMeshProUGUI outputText;
        public TextMeshProUGUI passportText;

        private const int TIME_PER_DOT = 1000;
        DateTime timeOfLastDot;

        string status;
        private bool isProcessing = false;


        public async void Start() {
            AuditLog.Log("Check screen");

            passportText.text = "Loading";
            bool isLoggedIn = PassportStore.IsLoggedIn();
            if (isLoggedIn) {
                await PassportLogin.Init();
                await PassportLogin.Login();

                // Set up wallet (includes creating a wallet for new players)
                List<string> accounts = await Passport.Instance.ZkEvmRequestAccounts();
                if (accounts.Count ==0) {
                    passportText.text = "Logged In";
                }
                else {
                    string account = accounts[0];
                    passportText.text = "Logged In (" + 
                                    DeepLinkManager.Instance.LoginPath + 
                                    ") as\n" + 
                                    account;
                }
            }
            else {
                passportText.text = "Not Logged In";
            }
        }



        

        public void OnButtonClick(string buttonText) {
            if (buttonText == "Mainnet") {
                mainnetButton.interactable = false;
                testnetButton.interactable = false;
                setStatus("Checking MainNet");
                testProcess(true);

            }
            else if (buttonText == "Testnet") {
                mainnetButton.interactable = false;
                testnetButton.interactable = false;
                status = "Checking TestNet\n";
                testProcess(false);
            }
            else {
                AuditLog.Log($"CheckScreen: Unknown button: {buttonText}");
            }
        }


        private async void testProcess(bool useMainnet) {
            if (isProcessing) {
                return;
            }
            isProcessing = true;

            timeOfLastDot = DateTime.Now;

            try {
                CheckContract contract = new CheckContract(useMainnet);

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
            mainnetButton.interactable = true;
            testnetButton.interactable = true;
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