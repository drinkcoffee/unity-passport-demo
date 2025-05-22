// Copyright (c) Whatgame Studios 2024 - 2024
// SPDX-License-Identifier: PROPRIETORY
pragma solidity ^0.8.20;

import "forge-std/Script.sol";
import {UnityImmutableCheck} from "../src/UnityImmutableCheck.sol";

contract UnityImmutableCheckScript is Script {
    function deploy() public {
        address deployer = vm.envAddress("DEPLOYER_ADDRESS");
        vm.broadcast(deployer);
        UnityImmutableCheck impl = new UnityImmutableCheck();
        console.logString("Done");
        console.logAddress(address(impl));
    }
}
