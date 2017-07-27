## C# Client for  coinmarketcap ##

Here is an .NET Client for the [Coinmarket Api](https://coinmarketcap.com/api/)

## Available for:
- .NET Standard 1.1
- .NET Standard 1.3
- .NET 4.5

## Example:
```csharp
	ICoinmarketcapClient client = new CoinmarketcapClient();
	Currency currency = client.GetCurrencyById("bitcoin");
```

## Licence:
http://choosealicense.com/licenses/bsd-2-clause/

## Todo:
At the moment we don't return the converted currency. 
The will be implemented in version 1.0