## C# Client for  coinmarketcap ##

Here is an .NET Client for the [Coinmarket Api](https://coinmarketcap.com/api/)

## Available for:
- .NET Standard 2.0
- .NET Standard 1.5
- .NET Standard 1.3
- .NET 4.6.1
- .NET 4.5

## Example:
```csharp
	ICoinmarketcapClient client = new CoinmarketcapClient();
	Currency currency = client.GetCurrencyById("bitcoin");
```

## Licence:
http://choosealicense.com/licenses/bsd-2-clause/

## Support convert currency:
Version 1.0.0 support converted currency parameter.