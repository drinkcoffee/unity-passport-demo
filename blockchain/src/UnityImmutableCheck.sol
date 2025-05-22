// Copyright (c) Immutable Pty Ltd 2025
// SPDX-License-Identifier: MIT
pragma solidity ^0.8.28;

contract UnityImmutableCheck {
    event ValueSet(uint256 _value);

    uint256 public value;

    function setValue(uint256 _value) external {
        value = _value;
    }
}
