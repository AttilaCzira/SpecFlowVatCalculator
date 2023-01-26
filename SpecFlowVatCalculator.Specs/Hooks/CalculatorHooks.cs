using SpecFlowCalculator.Specs.Drivers;
using SpecFlowCalculator.Specs.PageObjects;
using TechTalk.SpecFlow;

namespace SpecFlowCalculator.Specs.Hooks;

/// <summary>
/// Calculator related hooks
/// </summary>
[Binding]
public class CalculatorHooks
{
    ///<summary>
    ///  Reset the calculator before each scenario tagged with "VatCalculator"
    /// </summary>
    [BeforeScenario("VatCalculator")]
    public static void BeforeScenario(BrowserDriver browserDriver)
    {
        var vatCalculatorPageObject = new VatCalculatorPageObject(browserDriver.Current);
        vatCalculatorPageObject.OpenPage();
    }
}