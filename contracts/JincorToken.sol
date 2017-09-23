pragma solidity ^0.4.13;

import "./SafeMath.sol";

contract JincorToken {
	using SafeMath for uint256;
	string public name = "Jincor Token";
    string public symbol = "SIO";
    uint256 public decimals = 18;

    uint256 private saleStart;
    uint256 private saleEnd;

    uint256 private constant TOTAL_SUPPLY = 2000000 * 1 ether;
    uint256 private constant SOFT_CAP = 500 * 1 ether;

	mapping (address => uint) balances;

	address private owner;

    function getSaleStart() constant returns (uint256) {
        return saleStart;
    }

    function getSaleEnd() constant returns (uint256) {
        return saleEnd;
    }

    function totalSupply() constant returns (uint totalSupply) {
        totalSupply = TOTAL_SUPPLY;
    }

    function getCurrentPrice() constant returns (uint price) {
        //Token price, ETH: 0,002
        price = 500 * 1 ether;
    }

    function softCapReached() constant returns (bool) {
        return this.balance > SOFT_CAP;
    }

    function inSalePeriod() constant returns (bool) {
        return now > saleStart && now < saleEnd;
    }

	function JincorToken() {
        owner = msg.sender;
		uint _saleStart = 0;
        if (_saleStart == 0) {
            saleStart = 1508025600; //Beginning: 10.15.2017
            saleEnd = 1509408000; //End: 10.31.2017
        } else {
            saleStart = _saleStart;
            saleEnd = _saleStart + 17 days;
        }

        balances[owner] = 2000000 * 10 ** decimals;
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
        balance = balances[_owner];
    }

    function() payable {
        buyTokens();
    }

    function buyTokens() payable {

        require(inSalePeriod());

        uint amountInWei = msg.value;

        uint price = getCurrentPrice();
        uint tokenAmount = price * amountInWei / 1 ether;
        
        transfer(msg.sender, tokenAmount);        

        //Raise event
        TokenPurchase(msg.sender, amountInWei, 0);
    }

    function refund() {
        if (softCapReached() == true && now > saleEnd) {

            uint tokenAmount = balanceOf(msg.sender);
            uint amount = tokenAmount.div(1 ether);
            
            msg.sender.transfer(amount);
            Refund();
        }
    }

	event Transfer(address indexed _from, address indexed _to, uint _value);
    event Approval(address indexed _owner, address indexed _spender, uint _value);
    event TokenPurchase(address indexed _purchaser, uint256 _value, uint256 _amount);
    event Refund();
}