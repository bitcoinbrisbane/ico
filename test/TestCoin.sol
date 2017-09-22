pragma solidity ^0.4.13;

import "truffle/Assert.sol";
import "truffle/DeployedAddresses.sol";
import "../contracts/SafeMath.sol";
import "../contracts/ERC20.sol";
import "../contracts/JincorToken.sol";

contract TestCoin {
  function testInitialBalanceUsingDeployedContract() {
    JincorToken token = JincorToken(1505974450);

    uint expected = 2000000 * 1 ether;

    Assert.equal(token.balanceOf(msg.sender), expected, "Owner should have 2000000 * 10^18 JincorToken initially");
  }
}