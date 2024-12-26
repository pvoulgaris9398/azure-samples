# Azure Event Hub

## `Thursday, 12/26/24` Notes

### `Azure Event Hubs`

- Scalable event processing service
- Big data scenarios
- Millions of events/second
- Decouples sending and receiving data
- Integrates with other _Azure_ and non-Azure services
- Captures events to _Azure_ blob storage or data lake

### Scenarios

- Telemetry, Data Archival, Transaction Processing, Anomaly Detection

### Components

- `Namespace` &rarr; A container for _Event Hub(s)_
  - Scoping container, contains one-or-more _Event Hubs_, _Options_ apply to all, goes to _throughput_ units
- `Event Hub` &rarr; _Namespace_ must be created before
  - _Message Retention_ for 1-7 day(s)
  - _Partition Count_ for 2-32 partition(s)
- `Event Producers` &rarr; Send data to _Event Hub(s)_
- `Partitions` &rarr; Buckets of messages
  - LIke a bucket for _Event_ messages
  - Hold events time-ordered as they arrive
  - Events not deleted once read
  - Event Hubs decides which partition events are sent to but can specify partition with partition key
  - Maximum 32 partitions
  - Create as many partitions as expected concurrent subscribers
- `Consumer Groups` &rarr; View(s) of an _Event Hub_
- `Subscribers` &rarr; Read data from _Event Hub(s)_
  - Connection info needed is namepsace and endpoint, where endpoint will include the key
  - `EventHubConsumerClient` &rarr; Connection remains open, specify the partition, the offset, date and sequence number (if desired)

### Azure Events

- `Azure Event Hub` meant for \_big data)
- Decouples sending and receiving
- Namespaces hold multiple _Event Hub(s)_

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
