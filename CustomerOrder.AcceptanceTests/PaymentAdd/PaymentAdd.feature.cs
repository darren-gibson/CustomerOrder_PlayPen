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
namespace CustomerOrder.AcceptanceTests.PaymentAdd
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Payment Add")]
    public partial class PaymentAddFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PaymentAdd.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Payment Add", "In order to get the items in the order fulfilled, they need to be paid for.\r\nAs a" +
                    " Customer\r\nI want to Add a Payment to my Order \r\nSo that I can take ownership of" +
                    " the products", ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        "5000157024671",
                        "0.68 GBP",
                        "Each",
                        "Heinz Baked Beans In Tomato Sauce 415G"});
            table1.AddRow(new string[] {
                        "5053947861260",
                        "3.29 GBP",
                        "Each",
                        "Tesco Finest British 6 Lincolnshire Sausages 400G"});
#line 8
 testRunner.Given("the following products are know to the Price Service:", ((string)(null)), table1, "Given ");
#line 12
 testRunner.And("I have a unique order number {orderNo}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.And("I add a Product to the order by calling POST /orders/{orderNo}/productadd with a " +
                    "ProductID of \"5053947861260\" with a Quantity of \"3 Each\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add a cash payment to order")]
        [NUnit.Framework.CategoryAttribute("addPayment")]
        public virtual void AddACashPaymentToOrder()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add a cash payment to order", new string[] {
                        "addPayment"});
#line 16
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 17
 testRunner.When("I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tend" +
                    "er type of CASH and an amount of 10.00 GBP", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
 testRunner.Then("the result should be an HTTP 202 Accepted Status", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 19
 testRunner.And("the result should contain a Location header that matches /orders/{orderNo}/reques" +
                    "ts/.+", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("the result of calling payment adding a payment to the order should")]
        [NUnit.Framework.CategoryAttribute("addPayment")]
        [NUnit.Framework.CategoryAttribute("paymentAdded")]
        public virtual void TheResultOfCallingPaymentAddingAPaymentToTheOrderShould()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("the result of calling payment adding a payment to the order should", new string[] {
                        "addPayment",
                        "paymentAdded"});
#line 22
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 25
 testRunner.When("I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tend" +
                    "er type of CASH and an amount of 5.00 GBP", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
 testRunner.And("I GET the resource identified by the Uri in the Location Header with an Accept he" +
                    "ader of application/json", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.Then("the result should be an HTTP 200 OK Status", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 28
 testRunner.And("the result should have a Content-Type of \'application/vnd.tesco.CustomerOrder.Pay" +
                    "mentAdded+json\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table2.AddRow(new string[] {
                        "tenderType",
                        "CASH"});
            table2.AddRow(new string[] {
                        "amount",
                        "5.00 GBP"});
#line 29
 testRunner.And("the result should contain:", ((string)(null)), table2, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot pay more than the value of the order.")]
        [NUnit.Framework.CategoryAttribute("addPayment")]
        public virtual void CannotPayMoreThanTheValueOfTheOrder_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot pay more than the value of the order.", new string[] {
                        "addPayment"});
#line 35
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 37
 testRunner.When("I add a Payment to an order by calling PUT /orders/{orderNo}/payments with a tend" +
                    "er type of CASH and an amount of 10.00 GBP", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
 testRunner.And("I GET the resource identified by the Uri in the Location Header with an Accept he" +
                    "ader of application/json", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
 testRunner.Then("the result should be an HTTP 200 OK Status", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
 testRunner.And("the result should have a Content-Type of \'application/vnd.tesco.CustomerOrder.Pay" +
                    "mentExceededAmountDueException+json\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table3.AddRow(new string[] {
                        "amountDue",
                        "9.87 GBP"});
#line 41
 testRunner.And("the PaymentExceededAmountDueException should contain:", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
