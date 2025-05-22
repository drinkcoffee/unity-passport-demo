// Copyright Immutable Pty Ltd 2025
// SPDX-License-Identifier: MIT

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using Immutable.Passport;
using Immutable.Passport.Model;

namespace UnityPassportDemo {
    public class PassportFunctionCalls {
        public enum TransactionStatus {
            Init = 0,
            Success = 1,
            Failed = 2
        }

        public const int MAX_RETRIES = 3;
        public int retryCount;
        public string contractAddress;

        public PassportFunctionCalls() {
            retryCount = 0;
        }

        public async Task<(bool success, TransactionReceiptResponse receipt)> executeTransaction(byte[] abiEncoding) {
            while (true) {
                try {
                    DateTime start = DateTime.Now;
                    TransactionReceiptResponse response = 
                        await Passport.Instance.ZkEvmSendTransactionWithConfirmation(
                            new TransactionRequest() {
                                to = contractAddress,
                                data = "0x" + BitConverter.ToString(abiEncoding).Replace("-", "").ToLower(),
                                value = "0"
                            }
                        );
                    DateTime end = DateTime.Now;
                    TimeSpan diff = end.Subtract(start);
                    AuditLog.Log($"Transaction status: {response.status}, time span: {diff.TotalMilliseconds}, hash: {response.transactionHash}");

                    if (response.status != "1") {
                        return (false, response);
                    }
                    else {
                        return (true, response);
                    }
                }
                catch (System.Exception ex) {
                    string errorMessage = $"Tx exception: {ex.Message}\nStack: {ex.StackTrace}";
                    if (errorMessage.IndexOf("TimeoutException:") != -1) {
                        if (retryCount == MAX_RETRIES) {
                            AuditLog.Log($"Transaction: Timed out: Too many retries: {retryCount}");
                            return (false, null);
                        }
                        else {
                            AuditLog.Log($"Transaction: Timed out: Retry count: {retryCount}");
                            retryCount++;
                        }
                    }
                    else {
                        AuditLog.Log(errorMessage);
                        return (false, null);
                    }
                }
            }
        }
    }
}
