using BoDi;
using SpecFlowCalculator.Specs.Drivers;
using TechTalk.SpecFlow;

namespace SpecFlowCalculator.Specs.Hooks;

[Binding]
public class SharedBrowserHooks
{
    [BeforeTestRun]
    public static void BeforeTestRun(ObjectContainer testThreadContainer)
    {
        testThreadContainer.BaseContainer.Resolve<BrowserDriver>();
    }
}