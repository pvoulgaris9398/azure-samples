# Azure Event Hub

## `12/23/24`

- Working through `Chapter 12` of `Developing-Solutions-for-Microsoft-Azure-AZ-204-Exam-Guide-2nd-Edition`
- Source code for book is [here](https://github.com/PacktPublishing/Developing-Solutions-for-Microsoft-Azure-AZ-204-Exam-Guide-2nd-Edition)
- Requires environment variable:

```text
EVENT_HUB_CONNECTION_STRING=****
STORAGE_ACCOUNT_CONNECTION_STRING=****
```

- Note, I was getting weird compile errors in `vscode` despite having the updated nuget package names and references in my project. - I did a `dotnet workload update` on my development machine and that seemed to clear the issue.
- Maybe I had an old workload out there that was mucking things up

## `11/13/23`

- Added a C# console app to exercise Azure Event Hubs objects
- Added some bash shell scripts to create and delete Azure resources
- [Next](https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-send?tabs=connection-string%2Croles-azure-portal)
