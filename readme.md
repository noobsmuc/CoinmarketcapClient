## C# Client for  coinmarketcap ##

Here is an .NET Client for the [Coinmarket Api](https://coinmarketcap.com/api/)

## Available for:
- .NET Standard 2.0
- .NET 4.6.1


## Example:
```csharp
	ICoinmarketcapClient client = new CoinmarketcapClient(<YourApiKey>);
	Currency currency = client.GetCurrencyById("bitcoin");
```

## Licence:
http://choosealicense.com/licenses/bsd-2-clause/

## Support convert currency:
Version 2.0.0 support converted currency parameter.

## Available on Nuget.org
https://www.nuget.org/packages/NoobsMuc.Coinmarketcap.Client/
