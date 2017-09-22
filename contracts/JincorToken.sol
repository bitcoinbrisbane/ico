pragma solidity ^0.4.13;

import "./SafeMath.sol";
import "./ERC20.sol";

contract JincorToken is ERC20 {
    using SafeMath for uint256;

    string public name = "Jincor Token";
    string public symbol = "SIO";
    uint256 public decimals = 18;

    uint256 private saleStart;
    uint256 private saleEnd;

    uint256 private constant TOTAL_SUPPLY = 2000000 * 1 ether;

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

    event Transfer(address indexed _from, address indexed _to, uint _value);
    event Approval(address indexed _owner, address indexed _spender, uint _value);
}