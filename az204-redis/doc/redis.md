# Redis Notes

- Good overview with walkthroughs and code samples [here](https://azure.github.io/redis-on-azure-workshop/)

## Connection String Format

- Note port `6380` is for `https`, while `6379` is for `non-SSL`

```C#
string connectionString = $"{redis}.redis.cache.windows.net:6380,password={key},ssl=True,abortConnect=False";
```

## Helpful Commands

```bash

az resource show --resource-group $rg --name $redis --resource-type microsoft.cache.redis

az redis list-keys --resource-group $rg --name $redis

```