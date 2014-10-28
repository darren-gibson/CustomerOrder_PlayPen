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
namespace CustomerOrder.AcceptanceTests.Order
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Order Priced Event")]
    public partial class OrderPricedEventFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "OrderPricedEvent.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Order Priced Event", @"In order for the customer to pay for the products in their order, a customer needs to be informed of the total price for the order.
As a Customer
I want to know the total amount that the Order will costs 
So that I can make a decision of whether I want to purchase it", ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 13
 testRunner.And("I have a unique order number {orderNo}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Adding a product to the order causes an OrderPriceEvent to be generated")]
        [NUnit.Framework.CategoryAttribute("addProduct")]
        public virtual void AddingAProductToTheOrderCausesAnOrderPriceEventToBeGenerated()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Adding a product to the order causes an OrderPriceEvent to be generated", new string[] {
                        "addProduct"});
#line 16
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 17
 testRunner.When("I add a Product to a new order by calling POST /orders/{orderNo}/productadd with " +
                    "a ProductID of \"5053947861260\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
 testRunner.Then("the result should be an HTTP 202 Accepted Status", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 19
 testRunner.When("I GET /orders/{orderNo}/events/orderPriced with an Accept header of application/a" +
                    "tom+xml", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 20
 testRunner.Then("the result should be an HTTP 200 OK Status", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table2.AddRow(new string[] {
                        "order",
                        "{orderNo}"});
            table2.AddRow(new string[] {
                        "netTotal",
                        "3.29 GBP"});
#line 21
 testRunner.And("the result should contain:", ((string)(null)), table2, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
