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


namespace UnityPassportDemo {


    public class CheckContract : PassportFunctionCalls {
        public const string TESTNET_ADDRESS = "0xe54431e297c3126d619cde60728cfafeac78a07e";
        public const string TESTNET_RPC_URL = "https://rpc.testnet.immutable.com/";

        public const string MAINNET_RPC_URL = "https://rpc.immutable.com/";

        UnityImmutableCheckService service;

        public CheckContract(bool mainnet) : base() {
            string rpc;
            if (mainnet) {
                rpc = MAINNET_RPC_URL;
                AuditLog.Log("Not implemented yet");
                throw new Exception("Not implemented yet");
            }
            else {
                contractAddress = TESTNET_ADDRESS;
                rpc = TESTNET_RPC_URL;
            }
            var network = mainnet ? "Mainnet" : "Testnet";
            AuditLog.Log($"Configuration: Network: {network}, Contract: {contractAddress}");

            var web3 = new Web3(rpc);
            service = new UnityImmutableCheckService(web3, contractAddress);

        }


        public async Task<uint> GetValue() {
            ValueFunction func = new ValueFunction();
            BigInteger val = await service.ValueQueryAsync(func);
            if (val < 0 || val > uint.MaxValue) {
                AuditLog.Log($"ERROR: Value {val} is outside uint range");
                // Use 7 to indicate an error.
                return 7;
            }
            else {
                return (uint) val;
            }
        }

        public async Task<bool> SetValue(BigInteger value) {
            var func = new SetValueFunction();
                func.Value = value;
            var (success, response) = await executeTransaction(func.GetCallData());
            return success;
        }
    }
}
