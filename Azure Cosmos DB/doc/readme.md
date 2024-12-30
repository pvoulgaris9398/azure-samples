# Azure Cosmos DB

## Consistency Levels

- Default consistency is setup at the `Container` level and is `Session Consistency`

### Strong Consistency

- This offers linearizability guarantee. Guarantees sequence of operation and that a _read_ will return the latest values
- When a user performs a _write_ operation on the _primary_ database it is duplicates to all _replicas_
- Only when all _replicas_ have been commited and confirmed is the _write_ operation visible on the main _replica_

### Bounded Staleness Consistency

- Guarantees sequence of operation but operations are _replicated_ asynchronously with a _staleness_ window.
- Allows you to specify the maximum lag in operations or time

### Session Consistency

- Strong consistency **for the session**
- Data from other sessions comes in the correct _order_, it just might not be up-to-date
- Allows a single client to execute updates and reads in it's session including monotonic reads and writes, _write-follows-reads_ and _read-your-writes_
- All operations in a session are _monotonic_ and consistent across _primary_ and _replicas_
- The application can extract a token from the response header and provide the token for the next request

### Consistent Prefix Consistency

- Data is accurate, just not necessary current, no guarantees on how current
- Ensures _out-of-order writes_ are **never seen by readers**.
- Shaky consistency but ensures updates appear in replicas in the right order (as prefixes to other updates) and without gaps

### Eventual Consistency

- Loosest consistency and effectively commits writes immediately. Replica transactions are asynchronous and will _eventually_ match the _primary_.
- Highest speed and throughput for apps that don't need to wait for replicas to commit before completing transactions

## Networking

- All connections to the internet allowed by default
- Create custom rules to allows connections from a range of _IP Addresses_, a _single IP address_ and selected _private networks_
- Can also use _private endpoints_ configured for specific Azure services

## Encryption

- Encrypted _at rest_ by default, using a Microsoft key.
- Can use customer-manged keys, which can be referenced from `Azure Key Vault`

## Backups & Recovery

- Continuous backup mode allows to restore to any point in time within the past 30 days
- Periodic backup (default) allows backups to be taken based on some specific policy
- Retention policy is limited by month, backup intervals minimum of one (1) hour
- Restoring a backup must be done by requesting support team assistance

## Partitions

- Can manage the amount of _logical_ partitions and their documents physical partitions managed by the service
- Affect number of _logical_ partitions via the `PartitionKey`
- Multiple _partitions_ with a few documents each, best practice, avoid _hot_ partitions

## Indexing

- Each _json_ document in the database is converted to a _tree_ representation
- Queries use an `Index Seek` algorithm
- Good index strategy will reduce the number of `Request Unit` (RU) charges
- Index can include/exclude certain nodes. _Parent_ nodes of exclude _child_ nodes will be excluded as well
- Some examples are: `/Custoemr/Name/?` or `/Custoemr/*`
- Wildcard includes all nested nodes, while the `?` includes only the exact value of the current node
- Default indexing policy indexes _all properties_.

## Types of Indices

### Range Index

- Suits a single field containing a list of string or number values.
- Useful in _range_ queries with _filtering_, _ordering_ and _joining_ requests
- The _range_ index is the default indexing policy
- Optimized for best performance
- Recommend configuring _range_ indexes for an single string or number of properties

### Composite Index

- Improves the efficiency when filtering or ordering operations on multiple fields
- Query performance will be optimal if queries hit a composite index in the same order as the fields it set up
- Can also include a sort order0

### Spatial Index

- Uses _geospacial_ objects

## Time to Live

- `TTL` feature allows you to remove documents from the container automatically after a specified time
- You can the `TTL` at the container level, applied to all documents default
- You can also set the `TTL` at the document level to override the inherited `TTL` from the container level
- `TTL` is set in seconds and `-1` would indicate _infinity_, `null` is the default value

## Classes

- `CosmosClient` - logical representation of the `Azure Cosmos DB` account on the client side
- `Container` - operations for reading, replacing, deleting specific _containers_ and _documents_ in a _container_
- `Database` - operations for reading or deleitng an existing _database_ and building a _container_ class instance

## User-Defined Function(s) & Stored Procedure(s) & Triggers

- `UDFs` should provide _in_ arguments (many or none) and return a single result
- Stored procedures execute against documents in a _single partition_

- Defined using _javascript_
- Triggers must be explicitly run by the calling code
- _Pre_ and _Post_ triggers available

```c#
CreateItemAsync(doc, partionKey), new ItemRequestOptions() {PreTriggers = []});
```

## Rest API

```json
https://{account}.documents.azure.com/dbs/{db_id}/colls/{coll-id}/docs
{
    "query": "SELECT * FROM Orders o WHERE o.id = @id",
    "parameters": [
        {"name":"@id", "value":"NL-21"}
    ]
}

```

## Optimistic Concurrency

- Supported through use of `_etag` system property
- `etag` is updated every time a document changes
- Compare `etag` from their version of the document to what is in the database

```c#
var ac = new AccessCondition { Condition = readDoc.ETag, Type = AccessConditionType.IfMatch };
await client.ReplaceDocumentAsync(readDoc, new RequestOptions {AccessCondition = ac});
```

## Change Feed

- Ordered log of operations
- Transactions with any changes are tracked in the _change feed_
- Outputs list of changed documents in the order in which they were modified
- Change feed is _fifo_ and asynchronous
- Change feed is enabled by default
- Can request changes from initial starting point via `ChangeFeedOptions.StartTime`
- Processed in same way as _post-insert/update_ triggers
- Can support referential integrity between containers in a database via triggering an Azure Function
- Azure Logic Apps can also be triggered to call _third-party_ messaging services depending on document modification
- Initially, _change feed_ does not include _deletion_ of documents
- Workaround for this is to update a _soft-delete_ flag on the document to get it into the _change feed_
- Then setting a non-zero `TTL` will automatically delete the document
- Processing changes requires a reference to the _lease container_ to persist the current position of the feed
- If the client fails, it can be restarted from the point-of-failure
- `ChangesHandler<>` - delegsate provided to the `ProcessFeed` function, bounded on the input context with document reference and information about changes
- `ChangeFeedProcessor` - `Processor` class that needs to be started to listen for changes
- `StartFeed` and `StopFeed`, as needed
- Bound to a specific container and lease container

## Sample Setup & Instructions/Notes

### Run `az login`

### Create `.env` file with the following values:

- `RESOURCE_GROUP_NAME` &rarr; The resource group name
- `REGION` &rarr; The region to use
- `STORAGE_ACCOUNT_NAME` &rarr; The name of the storage account
- `STORAGE_ACCOUNT_TABLE_NAME` &rarr; The name of the table to create
- `AZURE_TABLE_CONNECTION_STRING` &rarr; The connection string to use

### Run `az group create -l $REGION -n $RESOURCE_GROUP_NAME`

### Run `az storage account create --name $STORAGE_ACCOUNT_NAME --resource-group $RESOURCE_GROUP_NAME`

### Run `az storage account show-connection-string -g $RESOURCE_GROUP_NAME -n $STORAGE_ACCOUNT_NAME`

- Note this works, but the order of the arguments is important
- For some reason, using the `-n $STORAGE_ACCOUNT_NAME` alone does not work

![](Screenshot%202024-12-10%20131933.png)

### Settings

`C:\Users\{username}\AppData\Roaming\Code\User`
