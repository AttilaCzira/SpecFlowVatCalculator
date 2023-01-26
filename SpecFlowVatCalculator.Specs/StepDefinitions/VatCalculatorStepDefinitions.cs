using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SpecFlowCalculator.Specs.Drivers;
using SpecFlowCalculator.Specs.PageObjects;

namespace SpecFlowCalculator.Specs.StepDefinitions;

[Binding]
public class VatCalculatorStepDefinitions
{
    private readonly VatCalculatorPageObject _vatCalculatorPageObject;

    public VatCalculatorStepDefinitions(BrowserDriver browserDriver)
    {
        _vatCalculatorPageObject = new VatCalculatorPageObject(browserDriver.Current);
    }

    [Given(@"the country dropdown is visible")]
    public void GivenTheCountryDropdownVisible()
    {
        Assert.True(_vatCalculatorPageObject.CountryDropDownVisible);
    }

    [Then(@"(.*) is the selected country")]
    public void ThenTheSelectedCountryIs(string country)
    {
        Assert.Equal(country, _vatCalculatorPageObject.SelectedCountry);
    }


    [Given(@"the user selects (.*) in Country combobox")]
    public void GivenTheUserSelectsCountryInCountryCombobox(string country)
    {
        _vatCalculatorPageObject.SelectCountry(country);
    }

    [Then(@"the valid VAT rates are: (.*)")]
    public void ThenTheValidVATRatesAre(string spaceSeparatedValidVatRates)
    {
        var searchedRates = spaceSeparatedValidVatRates.Split(' ');
        var rateCount = _vatCalculatorPageObject.VatRateCount;
        Assert.Equal(rateCount, searchedRates.Length);

        if (searchedRates.Length > 1) 
        {
            foreach (var rate in searchedRates.SkipLast(1))
            {
                Assert.False(_vatCalculatorPageObject.VatRateIsSelected(int.Parse(rate)));
            }
        }
        Assert.True(_vatCalculatorPageObject.VatRateIsSelected(int.Parse(searchedRates.Last())));
    }
    [Then(@"if the user selects the first rate")]
    public void ThenIfTheUserSelectsTheFirstRate()
    {
        _vatCalculatorPageObject.SelectVatRateByIndex(1);
    }

    [When(@"the user selects rate (.*)")]
    public void WhenTheUserSelectsRate(int rateToSelect)
    {
        _vatCalculatorPageObject.SelectVatRate(rateToSelect);
    }

    [Then(@"the selected rate is (.*)")]
    public void ThenTheSelectedRateIs(int selectedRate)
    {
        Assert.True(_vatCalculatorPageObject.VatRateIsSelected(selectedRate));
    }

    [Then(@"(.*) amount radio button is displayed")]
    public void ThenamountRadioButtonIsDisplayed(string amountType)
    {
        Assert.True(_vatCalculatorPageObject.GetAmountRadioButtonByType(amountType).Enabled);
    }

    [Then(@"(.*) amount input field is displayed")]
    public void ThenAmountInputFieldIsDisplayed(string amountType)
    {
        Assert.True(_vatCalculatorPageObject.GetAmountInputFieldByType(amountType).Displayed);
    }

    [When(@"the user selects (.*) amount radio button")]
    public void WhenTheUserSelectsAmountRadioButton(string amountType)
    {
        _vatCalculatorPageObject.SelectAmountRadioButtonByType(amountType);
    }
    [Then(@"(.*) amount radio button is selected")]
    public void ThenAmountRadioButtonIsSelected(string amountType)
    {
        Assert.True(_vatCalculatorPageObject.GetAmountRadioButtonByType(amountType).Selected);
    }
    [Then(@"(.*) amount input field is enabled")]
    public void ThenAmountInputFieldIsEnabled(string amountType)
    {
        Assert.True(_vatCalculatorPageObject.GetAmountInputFieldByType(amountType).Enabled);
    }
    [Then(@"(.*) amount radio button is not selected")]
    public void ThenAmountRadioButtonIsNotSelected(string amountType)
    {
        Assert.False(_vatCalculatorPageObject.GetAmountRadioButtonByType(amountType).Selected);
    }
    [Then(@"(.*) amount input field is disabled")]
    public void ThenAmountInputFieldIsDisabled(string amountType)
    {
        Assert.False(_vatCalculatorPageObject.GetAmountInputFieldByType(amountType).Enabled);
    }
    [When(@"the user enters (.*) into (.*) amount input field")]
    public void WhenTheUserEntersValueIntoAmountInputField(string value, string amountType)
    {
        _vatCalculatorPageObject.EnterValueIntoInputField(value, amountType);
    }
    [Then(@"(.*) amount field displays (.*)")]
    public void ThenAmountFieldDisplays(string amountType, float expectedValue)
    {
        Assert.Equal(expectedValue, _vatCalculatorPageObject.GetInputFieldValue(amountType));
    }
    [Then(@"the negative values are invalid error message displayed")]
    public void ThenTheNegativeValuesAreInvalidErrorMessageDisplayed()
    {
        _vatCalculatorPageObject.NegativeValuesAreInvalidErrorMessageDisplayed();
    }

}
