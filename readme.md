## C# Client for  coinmarketcap ##

Here is an .Net Client for the [Coinmarket Api](https://coinmarketcap.com/api/)

## Available for:
- .net Standart 1.1
- .net Standart 1.3
- .net 4.5

## Example:
```csharp
	ICoinCapClient client = new CoinCapClient();
	Currency currency = client.GetCurrencyById("bitcoin");
```

## Licence:
http://choosealicense.com/licenses/bsd-2-clause/