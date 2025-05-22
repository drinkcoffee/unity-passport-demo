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
                    
        // private const int TIME_PER_DOT = 1000;
        // DateTime timeOfLastDot;

        private CheckContract contract;

        // string status;
        // private bool isProcessing = false;
        // private bool hasError = false;
        // private string errorMessage = "";
        // private BigInteger tokenId = BigInteger.Zero;

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

//            status = "Claiming ";
            contract = new CheckContract();
            // timeOfLastDot = DateTime.Now;
            // StartClaimProcess();
        }



        

        public void OnButtonClick(string buttonText) {
            if (buttonText == "Mainnet") {
                mainnetButton.interactable = false;
                testnetButton.interactable = false;
                outputText.text = "Checking MainNet";

            }
            else if (buttonText == "Testnet") {
                mainnetButton.interactable = false;
                testnetButton.interactable = false;
                outputText.text = "Checking TestNet";
            }
            else {
                AuditLog.Log($"CheckScreen: Unknown button: {buttonText}");
            }
        }


        // private async void StartClaimProcess() {
        //     if (isProcessing) {
        //         return;
        //     }
        //     isProcessing = true;
        //     hasError = false;
        //     errorMessage = "";
        //     tokenId = BigInteger.Zero;

        //     try {
        //         AuditLog.Log("Claim transaction");
        //         var (claimSuccess, claimedTokenId) = await claimContract.Claim();
        //         if (!claimSuccess) {
        //             hasError = true;
        //             errorMessage = "Error during claim: type 1";
        //         }
        //         else {
        //             tokenId = claimedTokenId;
        //         }
        //     }
        //     catch (Exception ex) {
        //         hasError = true;
        //         errorMessage = "Error during claim: type 2";
        //         AuditLog.Log($"Exception in claim process: {ex.Message}");
        //     }
        //     finally {
        //         isProcessing = false;
        //     }
        // }


        // public void Update() {
        //     if (hasError) {
        //         info.text = errorMessage;
        //         return;
        //     }

        //     if (isProcessing) {
        //         DateTime now = DateTime.Now;
        //         if ((now - timeOfLastDot).TotalMilliseconds > TIME_PER_DOT) {
        //             timeOfLastDot = now;
        //             status = status + ".";
        //         }
        //         info.text = status;
        //     }
        //     else {
        //         status = $"Claim process complete! Token ID: {tokenId}";
        //         info.text = status;
        //     }
        // }
    }
}