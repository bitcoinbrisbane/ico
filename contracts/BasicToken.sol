pragma solidity ^0.4.11;

import './SafeMath.sol';

contract BasicToken {
    using SafeMath for uint256;

    string public name = "Jincor Token";
    string public symbol = "SIO";
    uint256 public decimals = 18;

    uint256 private constant TOTAL_SUPPLY = 2000000;
    uint256 private constant SOFT_CAP = 500 * 1 ether;

    mapping(address => uint256) balances;

    function totalSupply() constant returns (uint totalSupply) {
        return TOTAL_SUPPLY;
    }

    function transfer(address _to, uint256 _value) returns (bool) {
        require(_to != address(0));

        // SafeMath.sub will throw if there is not enough balance.
        balances[msg.sender] = balances[msg.sender].sub(_value);
        balances[_to] = balances[_to].add(_value);
        Transfer(msg.sender, _to, _value);
        return true;
    }

    function balanceOf(address _owner) constant returns (uint256 balance) {
        return balances[_owner];
    }

    function() payable {
        buyTokens();
    }

    function buyTokens() payable {

        require(now > 1);

        uint amountInWei = msg.value;

        // uint price = getCurrentPrice();
        // uint tokenAmount = price * amountInWei / 1 ether;
        
        // require (balances[owner] >= tokenAmount);
        // require (tokenAmount < (SALE_CAP - tokensSold));

        // tokensSold = tokensSold.add(tokenAmount);
        // balances[owner] = balances[owner].sub(tokenAmount);
        // balances[msg.sender] = balances[msg.sender].add(tokenAmount);

        //Raise event
        TokenPurchase(msg.sender, amountInWei, 0);
    }

    event TokenPurchase(address indexed _purchaser, uint256 _value, uint256 _amount);
    event Transfer(address indexed _from, address indexed _to, uint _value);
    event Approval(address indexed _owner, address indexed _spender, uint _value);

}