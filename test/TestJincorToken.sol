pragma solidity ^0.4.2;

import "truffle/Assert.sol";
import "truffle/DeployedAddresses.sol";
import "../contracts/JincorToken.sol";

contract TestJincorToken {

  function testInitialBalanceUsingDeployedContract() {
    JincorToken token = JincorToken(DeployedAddresses.JincorToken());

    uint expected = 2000000000000000000000000;

    Assert.equal(token.balanceOf(tx.origin), expected, "Owner should have 2000000 JincorToken initially");
  }

  function testShouldGetSaleStart() {
    JincorToken token = JincorToken(DeployedAddresses.JincorToken());

    uint expected = 1508025600;

    Assert.equal(token.getSaleStart(), expected, "Start date should have been 1508025600");
  }

  function testShouldGetSaleEnd() {
    JincorToken token = JincorToken(DeployedAddresses.JincorToken());

    uint expected = 1509408000;

    Assert.equal(token.getSaleEnd(), expected, "End date should have been 1509408000");
  }

  function testShouldTotalSupply() {
    JincorToken token = JincorToken(DeployedAddresses.JincorToken());

    uint expected = 2000000000000000000000000;

    Assert.equal(token.totalSupply(), expected, "Total supply should be");
  }

  function testGetCurrentPrice() {
    JincorToken token = JincorToken(DeployedAddresses.JincorToken());

    uint expected = 500000000000000000000;

    Assert.equal(token.getCurrentPrice(), expected, "Current price should be ");
  }

  function testShouldNotHaveReachedSoftCap() {
    JincorToken token = JincorToken(DeployedAddresses.JincorToken());

    bool expected = false;

    Assert.equal(token.softCapReached(), expected, "Soft cap reached should have been false");
  }

  function testShouldNotBeInSalesPeriod() {
    JincorToken token = JincorToken(DeployedAddresses.JincorToken());

    bool expected = false;

    Assert.equal(token.inSalePeriod(), expected, "Should not be in sales period");
  }
}
