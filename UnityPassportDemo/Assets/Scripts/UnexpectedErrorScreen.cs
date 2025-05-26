// Copyright Immutable Pty Ltd 2025
// SPDX-License-Identifier: MIT
using UnityEngine;
using TMPro;

namespace UnityPassportDemo {

    public class UnexpectedErrorScreen : MonoBehaviour {
        public TextMeshProUGUI outputText;

        public void Start() {
            AuditLog.Log("Unexpected Error Screen");
            outputText.text = AuditLog.GetLogs();
        }
    }
}