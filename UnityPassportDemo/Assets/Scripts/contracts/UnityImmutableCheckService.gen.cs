// Autogenerated code using 
// https://docs.nethereum.com/en/latest/nethereum-codegen-vscodesolidity/

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
    public partial class UnityImmutableCheckService: UnityImmutableCheckServiceBase
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.IWeb3 web3, UnityImmutableCheckDeployment unityImmutableCheckDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<UnityImmutableCheckDeployment>().SendRequestAndWaitForReceiptAsync(unityImmutableCheckDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.IWeb3 web3, UnityImmutableCheckDeployment unityImmutableCheckDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<UnityImmutableCheckDeployment>().SendRequestAsync(unityImmutableCheckDeployment);
        }

        public static async Task<UnityImmutableCheckService> DeployContractAndGetServiceAsync(Nethereum.Web3.IWeb3 web3, UnityImmutableCheckDeployment unityImmutableCheckDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, unityImmutableCheckDeployment, cancellationTokenSource);
            return new UnityImmutableCheckService(web3, receipt.ContractAddress);
        }

        public UnityImmutableCheckService(Nethereum.Web3.IWeb3 web3, string contractAddress) : base(web3, contractAddress)
        {
        }

    }


    public partial class UnityImmutableCheckServiceBase: ContractWeb3ServiceBase
    {

        public UnityImmutableCheckServiceBase(Nethereum.Web3.IWeb3 web3, string contractAddress) : base(web3, contractAddress)
        {
        }

        public virtual Task<string> SetValueRequestAsync(SetValueFunction setValueFunction)
        {
             return ContractHandler.SendRequestAsync(setValueFunction);
        }

        public virtual Task<TransactionReceipt> SetValueRequestAndWaitForReceiptAsync(SetValueFunction setValueFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setValueFunction, cancellationToken);
        }

        public virtual Task<string> SetValueRequestAsync(BigInteger value)
        {
            var setValueFunction = new SetValueFunction();
                setValueFunction.Value = value;
            
             return ContractHandler.SendRequestAsync(setValueFunction);
        }

        public virtual Task<TransactionReceipt> SetValueRequestAndWaitForReceiptAsync(BigInteger value, CancellationTokenSource cancellationToken = null)
        {
            var setValueFunction = new SetValueFunction();
                setValueFunction.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setValueFunction, cancellationToken);
        }

        public Task<BigInteger> ValueQueryAsync(ValueFunction valueFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ValueFunction, BigInteger>(valueFunction, blockParameter);
        }

        
        public virtual Task<BigInteger> ValueQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ValueFunction, BigInteger>(null, blockParameter);
        }

        public override List<Type> GetAllFunctionTypes()
        {
            return new List<Type>
            {
                typeof(SetValueFunction),
                typeof(ValueFunction)
            };
        }

        public override List<Type> GetAllEventTypes()
        {
            return new List<Type>
            {
                typeof(ValueSetEventDTO)
            };
        }

        public override List<Type> GetAllErrorTypes()
        {
            return new List<Type>
            {

            };
        }
    }
}
