using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Web3;
using System.Numerics;
using Nethereum.Util;

namespace console
{
    //Test RPC accounts
    //testrpc --account="0x221bbb8b9b508c2841a60f862e9d03c1997097f99ee83db94e077ff180265247,100000000000000000000" --account="0x9790dbc40d24723c34cf942f4dafac69ceb9e52bb9c92135221596ac25ba4270,100000000000000000000" --account="0x68296c6629c546483664ea232e33f187f60ca4ba123692c707168f2ac330dacf,100000000000000000000"
    class Program : Helper
    {
        //0x221bbb8b9b508c2841a60f862e9d03c1997097f99ee83db94e077ff180265247
        static String owner = "0xe2356d29d5dfecb4ee43c031204aeded24749959";

        //0x9790dbc40d24723c34cf942f4dafac69ceb9e52bb9c92135221596ac25ba4270
        static String alice = "0xa5f8ff129c19dbc0849619916c16010738ab5b1f";

        //0x68296c6629c546483664ea232e33f187f60ca4ba123692c707168f2ac330dacf
        static String bob = "0xaa727c20b128c298c13d56de8f087e998da28ab1";

        //0xb5b03722b215f55bd753814b6bce7b7ff98704b312a49b95fafa5ccfbee08ab9
        static String team = "0xe7b2bdd46ee2a540436773e5127c77b160a9af0c";

        static Int64 OCTOBER_6_2017 = 1507323600;
        static Int64 OCTOBER_9_2017 = 1507582800;
        static Int64 OCTOBER_10_2017 = 1507669200;
        static Int64 OCTOBER_16_2017 = 1508187600;
        static Int64 OCTOBER_23_2017 = 1508792400;

        static String contractAddress;

        //static Boolean TestRPC = true;

        //private static String password = "Test1234";

        //private static String account;

        static Web3 web3 = new Web3("http://localhost:8545");

        static string contractPath;

        static string contractName;

        static void Main(string[] args)
        {
            contractPath = "/home/lucascullen/Projects/skrilla-smart-contract/bin/src/contracts/";

            if (args.Length > 0)
            {
                contractName = args[0];
            }
            else
            {
                contractName = "SkrillaToken";
            }

            Console.WriteLine();
            Console.WriteLine("*** Test deploy and constructor");

            //Set date to October 9, 9pm, 2017 GMT
            //Set now to test
            Object[] constructorParms = new Object[3] { OCTOBER_6_2017, team, growth };
            DeplyContract(contractPath, contractName, constructorParms);

            var totalSupply = Should_Get_Total_Supply().Result;
            Console.WriteLine("Total supply should be 1,000,000,0000, actual {0}", totalSupply);

            var ownerBalance = Should_Get_Token_Balance(puntaa).Result;
            Console.WriteLine("Owner balance on deploy should be 600,000,0000, actual {0}", ownerBalance);

            var teamBalance = Should_Get_Token_Balance(team).Result;
            Console.WriteLine("Owner balance on deploy should be 300,000,0000, actual {0}", teamBalance);

            var growthBalance = Should_Get_Token_Balance(growth).Result;
            Console.WriteLine("Owner balance on deploy should be 100,000,0000, actual {0}", growthBalance);

            var zeroBalance = Should_Get_0_Token_Balance().Result;
            Console.WriteLine("Should be 0 balance on deploy, actual {0}", zeroBalance);

            var totalSold = Should_Get_Total_Sold().Result;
            Console.WriteLine("Should be 0 on total sold on deploy, actual {0}", totalSold);

            Console.WriteLine();
            Console.WriteLine("*** Test dates");

            var preSale = Should_Get_Presale_Date().Result;
            Console.WriteLine("Presale start date should be {0} 6/10/2017 9:00:00 PM, actual {1} {2}", OCTOBER_6_2017, preSale, FromUnixTime(preSale));

            var firstStage = Should_Get_Stage_1_Date().Result;
            Console.WriteLine("Stage 1 start date should be {0} 9/10/2017 9:00:00 PM, actual {1} {2}", OCTOBER_9_2017, firstStage, FromUnixTime(firstStage));

            var secondStage = Should_Get_Stage_2_Date().Result;
            Console.WriteLine("Stage 2 start date should be {0} 10/10/2017 9:00:00 PM, actual {1} {2}", OCTOBER_10_2017, secondStage, FromUnixTime(secondStage));

            var thirdStage = Should_Get_Stage_3_Date().Result;
            Console.WriteLine("Stage 3 start date should be {0} 16/10/2017 9:00:00 PM, actual {1} {2}", OCTOBER_16_2017, thirdStage, FromUnixTime(thirdStage));

            var endSale = Should_Get_Sale_End_Date().Result;
            Console.WriteLine("Sale end date should be {0} 23/10/2017 9:00:00 PM, actual {1} {2}", OCTOBER_23_2017, endSale, FromUnixTime(endSale));

            Console.WriteLine();
            Console.WriteLine("*** Test stages");

            var stage0Price = Should_Get_Presale_Price().Result;
            Console.WriteLine("Pre sale amount of tokens per ether should be 2500, actual {0}", stage0Price);

            var stage1Price = Should_Get_Stage_1_Price().Result;
            Console.WriteLine("Stage 1 amount of tokens per ether should be 2400, actual {0}", stage1Price);

            var stage2Price = Should_Get_Stage_2_Price().Result;
            Console.WriteLine("Stage 2 amount of tokens per ether should be 2200, actual {0}", stage2Price);

            var stage3Price = Should_Get_Stage_3_Price().Result;
            Console.WriteLine("Stage 3 amount of tokens per ether should be 2000, actual {0}", stage3Price);

            Console.WriteLine();
            Console.WriteLine("*** Test future stages (pre sale)");

            var stageIndex = Should_Get_Current_Stage_Index().Result;
            Console.WriteLine("Current stage index should be 0, actual {0}", stageIndex);

            DateTimeOffset dto = new DateTimeOffset(DateTime.Now.AddHours(-1));
            Int64 epoch = dto.ToUnixTimeSeconds();
            constructorParms = new Object[3] { epoch, team, growth };
            DeplyContract(contractPath, contractName, constructorParms);

            var currentPrice = Should_Get_Current_Price().Result;
            stageIndex = Should_Get_Current_Stage_Index().Result;
            Console.WriteLine("Current stage index should be 0 and price 2500, actual {0} {1}", stageIndex, currentPrice);

            preSale = Should_Get_Presale_Date().Result;
            Console.WriteLine("Presale start date should be {0}, actual {1} {2}", epoch, preSale, FromUnixTime(preSale));

            firstStage = Should_Get_Stage_1_Date().Result;
            Console.WriteLine("Stage 1 start date should be {0}, actual {1} {2}", epoch + 3 * 24 * 7 * 60 * 60, firstStage, FromUnixTime(firstStage));

            Console.WriteLine();
            Console.WriteLine("*** Test future stages (Stage 1)");

            //Three days 1 hour ago
            dto = new DateTimeOffset(DateTime.Now.AddHours(-(3 * 24 + 1)));
            epoch = dto.ToUnixTimeSeconds();
            constructorParms = new Object[3] { epoch, team, growth };
            DeplyContract(contractPath, contractName, constructorParms);

            currentPrice = Should_Get_Current_Price().Result;
            stageIndex = Should_Get_Current_Stage_Index().Result;
            Console.WriteLine("Current stage index should be 1 and price 2400, actual {0} {1}", stageIndex, currentPrice);

            Console.WriteLine();
            Console.WriteLine("*** Test future stages (Stage 2)");

            //Seven days 1 hour ago
            dto = new DateTimeOffset(DateTime.Now.AddHours(-(7 * 24 + 1)));
            epoch = dto.ToUnixTimeSeconds();
            constructorParms = new Object[3] { epoch, team, growth };
            DeplyContract(contractPath, contractName, constructorParms);

            currentPrice = Should_Get_Current_Price().Result;
            stageIndex = Should_Get_Current_Stage_Index().Result;
            Console.WriteLine("Current stage index should be 2 and price 2200, actual {0} {1}", stageIndex, currentPrice);

            Console.WriteLine();
            Console.WriteLine("*** Test future stages (Stage 3)");

            //Forteen days 1 hour ago
            dto = new DateTimeOffset(DateTime.Now.AddHours(-(14 * 24 + 1)));
            epoch = dto.ToUnixTimeSeconds();
            constructorParms = new Object[3] { epoch, team, growth };
            DeplyContract(contractPath, contractName, constructorParms);

            currentPrice = Should_Get_Current_Price().Result;
            stageIndex = Should_Get_Current_Stage_Index().Result;
            Console.WriteLine("Current stage index should be 3 and price 2000, actual {0} {1}", stageIndex, currentPrice);


            Console.WriteLine();
            Console.WriteLine("*** Test transfers");

            var transfer = Should_Transfer_2000_Tokens_From_Owner_To_Bob().Result;
            Console.WriteLine("Should transfer 2000 tokens to be true, actual {0}", transfer);

            var newBalance = Should_Get_Token_Balance(puntaa).Result;
            Console.WriteLine("New balance for owner should be 5,999,998,000, actual {0}", newBalance);

            newBalance = Should_Get_Token_Balance(bob).Result;
            Console.WriteLine("New balance for Bob should be 2000, actual {0}", newBalance);

            var txId = Should_Transfer_50_Tokens_From_Bob_To_Alice().Result;
            Console.WriteLine("Transfering 50 tokens from Bob to Alice in tx {0}", txId);

            newBalance = Should_Get_Token_Balance(bob).Result;
            Console.WriteLine("New balance for Bob should be 1950, actual {0}", newBalance);

            Console.WriteLine();
            Console.WriteLine("*** Test buy");

            // var buy = Should_Not_Buy_More_Than_Total_Supply().Result;
            // Console.WriteLine(buy);

            ownerBalance = Should_Get_Owner_Balance().Result;
            Console.WriteLine("Owner balance should be 600,000,000, actual {0}", ownerBalance);

            var buy = Should_Buy_1ETH_Of_Tokens_From_Buyer().Result;
            Console.WriteLine(buy);

            totalSold = Should_Get_Total_Sold().Result;
            Console.WriteLine("Should be 2000 total sold with 1 ETH, actual {0}", totalSold);

            newBalance = Should_Get_Token_Balance(bob).Result;
            Console.WriteLine("New balance for Bob should be 3950, actual {0}", newBalance);

            buy = Should_Buy_Tokens_With_ETH_From_Buyer(0.1M).Result;
            Console.WriteLine(buy);

            totalSold = Should_Get_Total_Sold().Result;
            Console.WriteLine("Should be 2200 total sold with 1.1 ETH, actual {0}", totalSold);

            newBalance = Should_Get_Token_Balance(bob).Result;
            Console.WriteLine("New balance for Bob should be 4150, actual {0}", newBalance);

            buy = Should_Buy_Tokens_With_ETH_From_Buyer(2).Result;
            Console.WriteLine(buy);

            totalSold = Should_Get_Total_Sold().Result;
            Console.WriteLine("Should be 7200 total sold with 3.1 ETH, actual {0}", totalSold);

            newBalance = Should_Get_Token_Balance(bob).Result;
            Console.WriteLine("New balance for Bob should be 9150, actual {0}", newBalance);

            Console.WriteLine();
            Console.WriteLine("*** Test withdraw");

            txId = Should_Withdraw_ETH_From_Contract_To_Alice().Result;
            Console.WriteLine("Should withdraw ETH from contract to Alice in tx {0}", txId);
            
            txId = Should_Not_Withdraw_ETH_From_Contract().Result;
            Console.WriteLine("Should withdraw ETH from contract to Alice in tx {0}", txId);


            Console.WriteLine();

            //should be 1506816000
            firstStage = Should_Get_Stage_1_Date().Result;
            Console.WriteLine("Stage 1 date should be 1506816000, actual {0}", firstStage);

            //should be 1507366800 (+1 days)
            secondStage = Should_Get_Stage_2_Date().Result;
            Console.WriteLine("Stage 2 date should be, actual {0}", secondStage);

            //should be 1507334400 (+7 days)
            thirdStage = Should_Get_Stage_3_Date().Result;
            Console.WriteLine("Stage 3 date should be, actual {0}", thirdStage);
        }

        public static async Task<String> Should_Get_Toke_Name()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("name");

            return await functionToTest.CallAsync<String>();
        }

        public static async Task<Int64> Should_Get_Total_Supply()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("totalSupply");

            var result = await functionToTest.CallAsync<Int64>();

            return result;
        }

        public static async Task<Int64> Should_Get_Total_Sold()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("getTotalSold");

            var result = await functionToTest.CallAsync<Int64>();

            return result;
        }

        public static async Task<Int64> Should_Get_Total_Issued()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("getTotalIssued");

            var result = await functionToTest.CallAsync<Int64>();

            return result;
        }

        [Obsolete("Use generic method")]
        public static async Task<Int64> Should_Get_Owner_Balance()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("balanceOf");

            var result = await functionToTest.CallAsync<Int64>(puntaa);

            return result;
        }

        public static async Task<Int64> Should_Get_Token_Balance(String account)
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("balanceOf");

            return await functionToTest.CallAsync<Int64>(account);
        }

        public static async Task<Int64> Should_Get_0_Token_Balance()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("balanceOf");

            return await functionToTest.CallAsync<Int64>(alice);
        }

        public static async Task<Int64> Should_Get_Presale_Date()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("stageStartDates");

            return await functionToTest.CallAsync<Int64>(0);
        }

        public static async Task<Int64> Should_Get_Stage_1_Date()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("stageStartDates");

            return await functionToTest.CallAsync<Int64>(1);
        }

        public static async Task<Int64> Should_Get_Stage_2_Date()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("stageStartDates");

            return await functionToTest.CallAsync<Int64>(2);
        }

        public static async Task<Int64> Should_Get_Stage_3_Date()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("stageStartDates");

            return await functionToTest.CallAsync<Int64>(3);
        }

        public static async Task<Int64> Should_Get_Sale_End_Date()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("getSaleEnd");

            return await functionToTest.CallAsync<Int64>();
        }

        public static async Task<Int64> Should_Get_Presale_Price()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("prices");

            return await functionToTest.CallAsync<Int64>(0);
        }

        public static async Task<Int64> Should_Get_Stage_1_Price()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("prices");

            return await functionToTest.CallAsync<Int64>(1);
        }

        public static async Task<Int64> Should_Get_Stage_2_Price()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("prices");

            return await functionToTest.CallAsync<Int64>(2);
        }

        public static async Task<Int64> Should_Get_Stage_3_Price()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("prices");

            return await functionToTest.CallAsync<Int64>(3);
        }

        public static async Task<Int32> Should_Get_Current_Stage_Index()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("getStage");

            return await functionToTest.CallAsync<Int32>();
        }

        public static async Task<Int32> Should_Get_Current_Price()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("getCurrentPrice");

            return await functionToTest.CallAsync<Int32>();
        }

        public static async Task<Boolean> Should_Transfer_2000_Tokens_From_Owner_To_Bob()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("transfer");

            Nethereum.Hex.HexTypes.HexBigInteger gas = new Nethereum.Hex.HexTypes.HexBigInteger(2000000);

            //Boolean result = false;
            UInt64 tokens = 2000;
            Object[] functionParams = new Object[2] { bob, tokens };
            String txId = await functionToTest.SendTransactionAsync(puntaa, gas, null, functionParams);

            return await functionToTest.CallAsync<Boolean>();
        }

        public static async Task<String> Should_Transfer_50_Tokens_From_Bob_To_Alice()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("transfer");

            Nethereum.Hex.HexTypes.HexBigInteger gas = new Nethereum.Hex.HexTypes.HexBigInteger(2000000);

            UInt64 tokens = 50;
            Object[] functionParams = new Object[2] { alice, tokens };
            return await functionToTest.SendTransactionAsync(bob, gas, null, functionParams);
        }

        [Obsolete]
        public static async Task<String> Should_Buy_1ETH_Of_Tokens_From_Buyer()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("buyTokens");

            Nethereum.Hex.HexTypes.HexBigInteger gas = new Nethereum.Hex.HexTypes.HexBigInteger(2000000);
            BigInteger ethToSend = Nethereum.Util.UnitConversion.Convert.ToWei(1, Nethereum.Util.UnitConversion.EthUnit.Ether);
            Nethereum.Hex.HexTypes.HexBigInteger eth = new Nethereum.Hex.HexTypes.HexBigInteger(ethToSend); 

            Object[] functionParams = new Object[0];
            var result = await functionToTest.SendTransactionAsync(bob, gas, eth, functionParams);

            return result;
        }

        public static async Task<String> Should_Buy_Tokens_With_ETH_From_Buyer(Decimal amount)
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("buyTokens");

            Nethereum.Hex.HexTypes.HexBigInteger gas = new Nethereum.Hex.HexTypes.HexBigInteger(2000000);
            BigInteger ethToSend = Nethereum.Util.UnitConversion.Convert.ToWei(amount, Nethereum.Util.UnitConversion.EthUnit.Ether);
            Nethereum.Hex.HexTypes.HexBigInteger eth = new Nethereum.Hex.HexTypes.HexBigInteger(ethToSend); 

            Object[] functionParams = new Object[0];
            return await functionToTest.SendTransactionAsync(bob, gas, eth, functionParams);
        }

        public static async Task<String> Should_Withdraw_ETH_From_Contract_To_Alice()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("withdrawal");

            Nethereum.Hex.HexTypes.HexBigInteger gas = new Nethereum.Hex.HexTypes.HexBigInteger(2000000);

            Object[] functionParams = new Object[1] { alice };
            return await functionToTest.SendTransactionAsync(puntaa, gas, null, functionParams);
        }

        public static async Task<String> Should_Not_Withdraw_ETH_From_Contract()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("withdrawal");

            Nethereum.Hex.HexTypes.HexBigInteger gas = new Nethereum.Hex.HexTypes.HexBigInteger(2000000);

            Object[] functionParams = new Object[1] { alice };
            //BOB shouldnt be able to withdraw!
            return await functionToTest.SendTransactionAsync(bob, gas, null, functionParams);
        }

        public static async Task<String> Should_Not_Buy_More_Than_Total_Supply()
        {
            var contract = GetContract(contractName);
            var functionToTest = contract.GetFunction("buyTokens");

            Nethereum.Hex.HexTypes.HexBigInteger gas = new Nethereum.Hex.HexTypes.HexBigInteger(2000000);

            Nethereum.Hex.HexTypes.HexBigInteger ethToSend = new Nethereum.Hex.HexTypes.HexBigInteger("300000200000000000000");
            Object[] functionParams = new Object[0];
            return await functionToTest.SendTransactionAsync(bob, gas, ethToSend, functionParams);
        }

        public static void DeployContract()
        {
            DeplyContract(contractPath, contractName, null);
        }
        
        public static void DeplyContract(String contractPath, String contractName, Object[] param)
        {
            String bytes = GetBytesFromFile(contractPath + contractName + ".bin");
            Nethereum.Hex.HexTypes.HexBigInteger gas = new Nethereum.Hex.HexTypes.HexBigInteger(2000000);
            
            String abi = GetABIFromFile(String.Format("{0}{1}.abi", contractPath, contractName));

            if (param != null)
            {
                String tx =  web3.Eth.DeployContract.SendRequestAsync(abi, bytes, puntaa, gas, param).Result;
                contractAddress = MonitorTx(tx);
            }
            else
            {
                String tx =  web3.Eth.DeployContract.SendRequestAsync(bytes, puntaa, gas).Result;
                contractAddress = MonitorTx(tx);
            }
        }

        public static String MonitorTx(String transactionHash)
        {
            var receipt = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash).Result;

            while (receipt == null)
            {
                Console.WriteLine("Sleeping for 5 seconds");
                System.Threading.Thread.Sleep(5000);
                receipt = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash).Result;
            }

            Console.WriteLine("Contract address {0} block height {1}", receipt.ContractAddress, receipt.BlockNumber.Value);

            return receipt.ContractAddress;
        }

        private static Nethereum.Contracts.Contract GetContract(String contractName)
        {
            if (!String.IsNullOrEmpty(contractAddress))
            {
                //String abi = GetABIFromFile(@"Test.abi");
                String abi = GetABIFromFile(String.Format(@"{0}{1}.abi", contractPath, contractName));
                
                //Web3 web3 = new Web3("http://localhost:8545");
                return web3.Eth.GetContract(abi, contractAddress);
            }
            else
            {
                throw new ArgumentNullException("No contract address");
            }
        }
    }
}