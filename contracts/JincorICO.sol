pragma solidity ^0.4.13;

import "./SafeMath.sol";
import "./JincorToken.sol";

contract JincorICO {

    JincorToken token;
    address owner;

    uint256 private constant TOTAL_SUPPLY = 2000000 * 1 ether;
    uint256 private constant SOFT_CAP = 500 * 1 ether;

    uint256 private saleStart;
    uint256 private saleEnd;

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

    function inSalePeriod() constant returns (bool) {
        return now > saleStart && now < saleEnd
    }

    function JincorICO (uint256 _saleStart, address tokenAddress) {
        owner = msg.sender;

        if (_saleStart == 0) {
            saleStart = 1508025600; //Beginning: 10.15.2017
            saleEnd = 1509408000; //End: 10.31.2017
        } else {
            saleStart = _saleStart;
            saleEnd = _saleStart + 17 days;
        }

        token = JincorToken(tokenAddress);
    }

    function() payable {
        buyTokens();
    }

    function buyTokens() payable {

        require(inSalePeriod());

        uint amountInWei = msg.value;

        uint price = getCurrentPrice();
        uint tokenAmount = price * amountInWei / 1 ether;
        
        token.transfer(msg.sender, tokenAmount);        

        //Raise event
        TokenPurchase(msg.sender, amountInWei, 0);
    }

    function refund() {
        if (softCapReached() == true && now > saleEnd) {

            uint tokenAmount = token.balance;
            uint amount = 0;
            
            msg.sender.transfer(amount);
            Refund();
        }
    }

    event TokenPurchase(address indexed _purchaser, uint256 _value, uint256 _amount);
    event Refund();
}