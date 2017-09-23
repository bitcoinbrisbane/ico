var SafeMath = artifacts.require("./SafeMath.sol");
var Token = artifacts.require("./JincorToken.sol");

module.exports = function(deployer) {
  deployer.deploy(SafeMath);
  deployer.link(SafeMath, Token);
  deployer.deploy(Token);
};
