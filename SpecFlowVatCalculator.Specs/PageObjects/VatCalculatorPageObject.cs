using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit.Sdk;

namespace SpecFlowCalculator.Specs.PageObjects;

public class VatCalculatorPageObject
{
    private const string VatCalculatorUrl = "https://www.calkoo.com/en/vat-calculator";
    public const string VatCalculatorEnglishTitle = "Value-Added Tax (VAT) Calculator";

    private readonly IWebDriver _webDriver;

    public const int DefaultWaitInSeconds = 5;

    public VatCalculatorPageObject(IWebDriver webDriver)
    {
        _webDriver = webDriver;
    }

    private static By _cookiePopUp => By.XPath("//*[@id=\"cookieconsent:desc\"]");
    private static By _acceptCookieButton => By.XPath("/html/body/div[1]/div/a");
    private static By _negativeValueErrorMessage => By.XPath("//*[contains(@id,'google-visualization-errors-all')]/div/span");
    #region Country dropdown
    private static By _countryDropDown => By.XPath("//*[@id=\"vatcalculator\"]/div[2]/div[2]/select");
    private SelectElement CountryDropDown => new(_webDriver.FindElement(_countryDropDown));
    #endregion
    #region VAT rates
    private static By _vatRatesContainer => By.XPath("//*[@id=\"vatcalculator\"]/div[4]/div[2]");
    private static By _vatRadioButtonByRate(int rate) => By.Id($"VAT_{rate}");
    private static By _vatLabelSingle() => By.XPath("//*[@id=\"vatcalculator\"]/div[4]/div[2]/label");
    private static By _vatLabelByIndex(int index) => By.XPath($"//*[@id=\"vatcalculator\"]/div[4]/div[2]/label[{index}]");
    private static By _vatRadioButtons => By.Name("VAT");
    #endregion
    #region amounts
    private static By _netRadioButton => By.Id("F1");
    private static By _vatRadioButton => By.Id("F2");
    private static By _grossRadioButton => By.Id("F3");
    private static By _netLabel => By.XPath("//*[@id=\"vatcalculator\"]/div[6]/div[1]/label");
    private static By _vatLabel => By.XPath("//*[@id=\"vatcalculator\"]/div[7]/div[1]/label");
    private static By _grossLabel => By.XPath("//*[@id=\"vatcalculator\"]/div[8]/div[1]/label");
    private static By _netInputField => By.Id("NetPrice");
    private static By _vatInputField => By.Id("VATsum");
    private static By _grossInputField => By.Id("Price");
    private static By _netMultiplierInput => By.Id("VATpct2");
    private static By _grossMultiplierInput => By.Id("VATpct1");
    #endregion

    public string Title => _webDriver.Title;
    public bool CountryDropDownVisible => CountryDropDown.SelectedOption.Displayed;
    public string SelectedCountry => CountryDropDown.SelectedOption.Text;
    public int VatRateCount => _webDriver.FindElement(_vatRatesContainer).FindElements(_vatRadioButtons).Count;

    private void AcceptCookie()
    {
        var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(DefaultWaitInSeconds));
        try
        {
            wait.Until(e => e.FindElement(_cookiePopUp));
            _webDriver.FindElement(_acceptCookieButton).Click();
        }
        catch (WebDriverTimeoutException)
        {
            return;
        }
    }

    public void OpenPage()
    {
        if (_webDriver.Url != VatCalculatorUrl)
        {
            _webDriver.Url = VatCalculatorUrl;
            AcceptCookie();
        }   
    }
    public void SelectCountry(string countryName)
    {
        CountryDropDown.SelectByText(countryName);
        OpenPage();
    }

    public bool VatRateIsSelected(int ratePercent)
    {
        IWebElement ratesContainer = _webDriver.FindElement(_vatRatesContainer);
        IWebElement rate = ratesContainer.FindElement(_vatRadioButtonByRate(ratePercent));
        return rate.Selected;
    }
    public void SelectVatRateByIndex(int index)
    {
        if (index is 0) _webDriver.FindElement(_vatLabelSingle()).Click();
        else _webDriver.FindElement(_vatLabelByIndex(index)).Click();
    }
    public void SelectVatRate(int rate)
    {
        int index = GetLabelIndexByRate(rate);
        if (index is 0) _webDriver.FindElement(_vatLabelSingle()).Click();
        else _webDriver.FindElement(_vatLabelByIndex(index)).Click();
    }

    private int GetLabelIndexByRate(int rate)
    {
        var currentCuntry = SelectedCountry;
        List<int> rates = GetRatesByCountry(currentCuntry);
        int index = rates.IndexOf(rate);
        if (index is -1) throw new ArgumentOutOfRangeException($"{rate} is not a valid rate for the selected country: {currentCuntry}!");
        if (rates.Count is 1) return 0;
        return index + 1;
    }

    private List<int> GetRatesByCountry(string currentCuntry)
    {
        return RatesByCountry.Instance.ValidRates(currentCuntry);
    }

    public IWebElement GetAmountRadioButtonByType(string amountType)
    {
        By by = amountType switch
        {
            "Net" => _netRadioButton,
            "VAT" => _vatRadioButton,
            "Gross" => _grossRadioButton,
            _ => throw new ArgumentOutOfRangeException($"{amountType} is not a valid amount Type!"),
        };
        return _webDriver.FindElement(by);
    }

    public IWebElement GetAmountInputFieldByType(string amountType)
    {
        By by = amountType switch
        {
            "Net" => _netInputField,
            "VAT" => _vatInputField,
            "Gross" => _grossInputField,
            _ => throw new ArgumentOutOfRangeException($"{amountType} is not a valid amount Type!"),
        };
        return _webDriver.FindElement(by);
    }

    internal void SelectAmountRadioButtonByType(string amountType)
    {
        By by = amountType switch
        {
            "Net" => _netLabel,
            "VAT" => _vatLabel,
            "Gross" => _grossLabel,
            _ => throw new ArgumentOutOfRangeException($"{amountType} is not a valid amount Type!"),
        };
        _webDriver.FindElement(by).Click();
    }

    internal void EnterValueIntoInputField(string value, string amountType)
    {
        var inputField = GetAmountInputFieldByType(amountType);
        inputField.Click();
        inputField.SendKeys(value);
    }

    internal float GetInputFieldValue(string amountType)
    {
        var value = GetAmountInputFieldByType(amountType).GetAttribute("value");
        return float.Parse(value);
    }
    public void NegativeValuesAreInvalidErrorMessageDisplayed()
    {
        IWebElement errorMessage = _webDriver.FindElement(_negativeValueErrorMessage);
        Assert.True(errorMessage.Displayed);
        Assert.Equal("Negative values are invalid for a pie chart.×", errorMessage.Text);
    }
}
