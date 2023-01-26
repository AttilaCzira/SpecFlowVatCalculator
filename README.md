# SpecFlowVatCalculator

Calkoo introduced a simple online VAT calculator for calculating tax on their website:
<<http://www.calkoo.com/?lang=3&page=8>>

### User story
As a traveller, I want to calculate the various purchase/tax amounts to verify that my shop receipt is correct, and the merchant correctly charged the VAT for my recently bought items.

### ACCEPTANCE CRITERIA
• User must select a country which applies VAT scheme   
• User must be able to choose a valid VAT rate for the selected country   
• One of the following amounts are supported as an input:  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o Net  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o Gross  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o VAT  
• Amounts can be entered with maximum 2 decimal digit precision  
• Given all mandatory fields (country, vat rate, one of the amounts) are provided, the website will calculate and show the other 2 amounts which were not provided originally as an input value  
• The API provides an error with meaningful error message, in case of:  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o negative input  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;o amount >999.999.999  
