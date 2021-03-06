﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace CustomerOrder.AcceptanceTests.ProductAdd
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Add Product")]
    public partial class AddProductFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "AddProduct.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Add Product", "In order to sell products, they need to be added to the order.\r\nAs a Customer\r\nI " +
                    "want to Add a Produt to my Order \r\nSo that I can purchase it", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 7
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Product Id",
                        "Unit Price",
                        "Sell by UOM",
                        "Friendly Name"});
            table1.AddRow(new string[] {
                        "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c",
                        "1.00 GBP",
                        "Each",
                        "Can of Soup"});
#line 8
 testRunner.Given("the following products are know to the Price Service:", ((string)(null)), table1, "Given ");
#line 11
 testRunner.And("the system is started", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And("I have a unique order number {orderNo}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add a product to an order using the default quantity")]
        [NUnit.Framework.CategoryAttribute("addProduct")]
        public virtual void AddAProductToAnOrderUsingTheDefaultQuantity()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add a product to an order using the default quantity", new string[] {
                        "addProduct"});
#line 15
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 16
 testRunner.When("I add a Product to a new order by calling POST /orders/{orderNo}/productadd with " +
                    "a ProductID of \"trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
 testRunner.Then("the result should be an HTTP 202 Accepted Status", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 18
 testRunner.And("the result should contain a Location header that matches /orders/{orderNo}/reques" +
                    "ts/.+", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("the result of adding a product to the order is available")]
        [NUnit.Framework.CategoryAttribute("addProduct")]
        [NUnit.Framework.CategoryAttribute("productAdded")]
        public virtual void TheResultOfAddingAProductToTheOrderIsAvailable()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("the result of adding a product to the order is available", new string[] {
                        "addProduct",
                        "productAdded"});
#line 21
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 24
 testRunner.Given("I add a Product to a new order by calling POST /orders/{orderNo}/productadd with " +
                    "a ProductID of \"trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c\" and" +
                    " a Quantity of 3 Each", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 25
 testRunner.When("I GET the resource identified by the Uri in the Location Header with an Accept he" +
                    "ader of application/json", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
 testRunner.Then("the result should be an HTTP 200 OK Status", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table2.AddRow(new string[] {
                        "Content-Type",
                        "application/vnd.tesco.CustomerOrder.ProductAdded+json"});
            table2.AddRow(new string[] {
                        "productIdentifier",
                        "trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-0cebd15d554c"});
            table2.AddRow(new string[] {
                        "quantity",
                        "3 Each"});
            table2.AddRow(new string[] {
                        "unitPrice",
                        "1.00 GBP"});
            table2.AddRow(new string[] {
                        "netPrice",
                        "3.00 GBP"});
#line 27
 testRunner.And("the result should contain:", ((string)(null)), table2, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("return NotComplete when a ProductAdd request is sent to an order, and the status " +
            "is checked before the request has completed")]
        public virtual void ReturnNotCompleteWhenAProductAddRequestIsSentToAnOrderAndTheStatusIsCheckedBeforeTheRequestHasCompleted()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("return NotComplete when a ProductAdd request is sent to an order, and the status " +
                    "is checked before the request has completed", ((string[])(null)));
#line 35
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 38
 testRunner.Given("I stop the service command runner", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 39
 testRunner.And("I add a Product to a new order by calling POST /orders/{orderNo}/productadd with " +
                    "a ProductID of \"trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-999999999999\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
 testRunner.When("I GET the resource identified by the Uri in the Location Header with an Accept he" +
                    "ader of application/json", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 41
 testRunner.Then("the result should be an HTTP 200 OK Status", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table3.AddRow(new string[] {
                        "Content-Type",
                        "application/vnd.tesco.CustomerOrder.NotComplete+json"});
#line 42
 testRunner.And("the result should contain:", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("return a ProductNotFoundException when an attempt is made to add a product that i" +
            "s not known to the price service")]
        [NUnit.Framework.CategoryAttribute("addProduct")]
        [NUnit.Framework.CategoryAttribute("productNotFoundException")]
        [NUnit.Framework.CategoryAttribute("exception")]
        public virtual void ReturnAProductNotFoundExceptionWhenAnAttemptIsMadeToAddAProductThatIsNotKnownToThePriceService()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("return a ProductNotFoundException when an attempt is made to add a product that i" +
                    "s not known to the price service", new string[] {
                        "addProduct",
                        "productNotFoundException",
                        "exception"});
#line 47
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 50
 testRunner.Given("I add a Product to a new order by calling POST /orders/{orderNo}/productadd with " +
                    "a ProductID of \"trn:tesco:product:uuid:1b4b0931-5854-489b-a77c-999999999999\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 51
 testRunner.When("I GET the resource identified by the Uri in the Location Header with an Accept he" +
                    "ader of application/json", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 52
 testRunner.Then("the result should be an HTTP 200 OK Status", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table4.AddRow(new string[] {
                        "Content-Type",
                        "application/vnd.tesco.CustomerOrder.ProductNotFoundException+json"});
#line 53
 testRunner.And("the result should contain:", ((string)(null)), table4, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
