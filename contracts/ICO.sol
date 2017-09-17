pragma solidity ^0.4.13;

library SafeMath {
  function mul(uint256 a, uint256 b) internal constant returns (uint256) {
    uint256 c = a * b;
    assert(a == 0 || c / a == b);
    return c;
  }

  function div(uint256 a, uint256 b) internal constant returns (uint256) {
    // assert(b > 0); // Solidity automatically throws when dividing by 0
    uint256 c = a / b;
    // assert(a == b * c + a % b); // There is no case in which this doesn't hold
    return c;
  }

  function sub(uint256 a, uint256 b) internal constant returns (uint256) {
    assert(b <= a);
    return a - b;
  }

  function add(uint256 a, uint256 b) internal constant returns (uint256) {
    uint256 c = a + b;
    assert(c >= a);
    return c;
  }
}

contract ERC20 {
    function totalSupply() constant returns (uint totalSupply);
    function balanceOf(address _owner) constant returns (uint balance);
    function transfer(address _to, uint _value) returns (bool success);
    function transferFrom(address _from, address _to, uint _value) returns (bool success);
    function approve(address _spender, uint _value) returns (bool success);
    function allowance(address _owner, address _spender) constant returns (uint remaining);

    event Transfer(address indexed _from, address indexed _to, uint _value);
    event Approval(address indexed _owner, address indexed _spender, uint _value);
}

contract JincorToken {
    //using SafeMath for uint256;

    uint256 private constant TOTAL_SUPPLY = 2000000 * 1 ether;
    uint256 private constant SOFT_CAP = 500 * 1 ether;

    uint256 private saleStart;
    uint256 private saleEnd;

    string public name = "Jincor Token";
    string public symbol = "SIO";
    uint256 public decimals = 18;

    mapping(address => uint256) public balances;

    address owner;

    function JincorToken (uint256 _saleStart) {
        owner = msg.sender;

        if (_saleStart == 0) {
            saleStart = 1508025600; //Beginning: 10.15.2017
            saleEnd = 1509408000; //End: 10.31.2017
        } else {
            saleStart = _saleStart;
            saleEnd = _saleStart + 17 days;
        }

        balances[owner] = 2000000 * 1 ether;
    }

    function transfer(address _to, uint256 _value) returns (bool) {
        require(_to != address(0));

        // SafeMath.sub will throw if there is not enough balance.
        balances[msg.sender] -= _value; // balances[msg.sender].sub(_value);
        balances[_to] += _value; //balances[_to].add(_value);
        Transfer(msg.sender, _to, _value);
        return true;
    }

    function balanceOf(address _owner) constant returns (uint256 balance) {
        return balances[_owner];
    }

    function getSaleStart() constant returns (uint256 total) {
        return saleStart;
    }

    function getSaleEnd() constant returns (uint256 total) {
        return saleEnd;
    }

    function totalSupply() constant returns (uint totalSupply) {
        return TOTAL_SUPPLY;
    }

    function getCurrentPrice() constant returns (uint price) {
        //Token price, ETH: 0,002
        return 500 * 1 ether;
    }

    function softCapReached() constant returns (bool) {
        this.balance > SOFT_CAP;
    }

    function() payable {
        buyTokens();
    }

    function buyTokens() payable {

        require(now > saleStart && now < saleEnd);

        uint amountInWei = msg.value;

        uint price = getCurrentPrice();
        uint tokenAmount = price * amountInWei / 1 ether;
        
        transfer(msg.sender, tokenAmount);        

        //Raise event
        TokenPurchase(msg.sender, amountInWei, 0);
    }

    function refund() {
        if (softCapReached() == true && now > saleEnd) {

            //uint tokenAmount = [msg.sender];
            // Remember to zero the pending refund before
            // sending to prevent re-entrancy attacks
            //pendingWithdrawals[msg.sender] = 0;
            
            //msg.sender.transfer(amount);
            Refund();
        }
    }

    event Transfer(address indexed _from, address indexed _to, uint _value);
    event Approval(address indexed _owner, address indexed _spender, uint _value);
    event TokenPurchase(address indexed _purchaser, uint256 _value, uint256 _amount);
    event Refund();
}