@VatCalculator
Feature: VatCalculator

As a traveller, I want to calculate the various purchase/tax 
amounts to verify that my shop receipt is correct, and the
merchant correctly charged the VAT for my recently bought items.

	
Scenario: The country select combobox is enabled and usable to select a Contry from the list
	Given The country dropdown is visible
	And the user selects Monaco in Country combobox
	Then Monaco is the selected country

Scenario:The user can select VAT rates
	Given the user selects Germany in Country combobox
	Then Germany is the selected country
	And the valid VAT rates are: 7 19
	When the user selects rate 7
	Then the selected rate is 7
	When the user selects rate 19
	Then the selected rate is 19

Scenario: The amount elements are displayed
	Given the user selects France in Country combobox
	Then Net amount radio button is displayed
	* Net amount input field is displayed
	* VAT amount radio button is displayed
	* VAT amount input field is displayed
	* Gross amount radio button is displayed
	* Gross amount input field is displayed

Scenario: The selected amount has the input
	Given the user selects Thailand in Country combobox
	When the user selects VAT amount radio button
	Then VAT amount radio button is selected
	* VAT amount input field is enabled
	* Net amount radio button is not selected
	* Net amount input field is disabled
	* Gross amount radio button is not selected
	* Gross amount input field is disabled

Scenario: The website calculates and shows the amounts
	Given the user selects Hungary in Country combobox
	When the user selects rate 5
	And the user selects VAT amount radio button
	And the user enters 50 into VAT amount input field
	Then Net amount field displays 1000.00
	* VAT amount field displays 50
	* Gross amount field displays 1050.00
	When the user selects rate 18
	And the user selects Net amount radio button
	And the user enters 100 into Net amount input field
	Then Net amount field displays 100
	* VAT amount field displays 18.00
	* Gross amount field displays 118.00
	When the user selects rate 27
	And the user selects Gross amount radio button
	And the user enters 25400 into Gross amount input field
	Then Net amount field displays 20000.00
	* VAT amount field displays 5400.00
	* Gross amount field displays 25400

Scenario: The website provides error in case of negative input
	Given the user selects Austria in Country combobox
	When the user selects rate 13
	And the user selects Gross amount radio button
	And the user enters -1000 into Gross amount input field
	Then the negative values are invalid error message displayed