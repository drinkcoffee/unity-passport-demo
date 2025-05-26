# Unity - Passport Demo project

This demo provides an example of logging in and submitting a transaction using Passport using Unity. The latency of transactions and view calls (to read the state of the chain) are displayed. 

# Create Your Own Similar Project

To create a similar project to this you will need to do the following:

Install:

* Use Unity 2D as a starting point
* Install Passport
  * Install UniTask: https://github.com/Cysharp/UniTask#upm-package
  * Install Passport: https://docs.immutable.com/x/sdks/unity/
* Install Nethereum: https://github.com/Nethereum/Nethereum.Unity

Setup:

* Create a project in Immutable Hub (hub.immutable.com)
* Deploy your contract. You could either use presets via Immutable Hub or write your own. See the `blockchain` directory.
* Register your contracts in Immutable Hub
* Use your own ClientID from Immutable Hub when you are calling `Passport.Init`.

Code Generation: 

* The contract code wrapper was generated using the methodology described here: https://docs.nethereum.com/en/latest/nethereum-codegen-vscodesolidity/

# Notes

If this code was created as a mobile app, you would need to be aware that game players could switch to the app at any screen. Hence, care must be taken with caching whether the user is logged in, and whether the cached credentials are current or not.
