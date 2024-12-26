# Azure Service Bus

## Overview

- Message Broker
  - Same as with `Azure Queue Storage`, `Azure Service Bus` supports simple messaging mechanisms to guarantee delivery and sequencing or messages with FIFO approach
  - Supports message sizes up to 100 MB
- Transactional
  - Supports transactions, involving receiving and submitting multiple messages in multiple queues and keeping them hidden from other consumers until the transaction is committe
- Dynamic Load Balancing
  - Allows several consumers to receive messages from the queue and dynamically scale depending on queue message length.
  - Implements _competing consumer_ pattern
- Message Broadcast
  - _Topics_ can support one or many relationships between _publishers_ and _consumers_
- Relay Communication
  - Can be used to connect cloud services and hybrid applications safely without direct communication

## Pricing Model

- Generally, higher tiers provide access to messaging topics, advanced configuration options, high availability and redundancy
- Basic provides only _queue_ services, not advanced services. Message size is limited to 256 KB. Charges are consumption-based and incurred per one (1) million operations
- Standard provides topics and queues with advanced services except for redundancy and scale. Message size still limited to 256 KB. Thirteen (13) million operations are included in the monthly charge
- Premium offers all advanced features, including geo-redundancy and horizontal scaling by message units. Message size limit is raised to 100 MB. Charges are incurred hourly per message unit.

## Scaling

- Horizontal scaling is only available on the `Azure Service Bus` namespace level for the _Premium_ tier
- Up to 64 message units can be provisioned and scaled manually in this case
- Throughput can be increased by adding an extra message unit to the namespace
- Can also scale throughput by using _partitions_, depending on _partition_ key messages will be hosted by a specific message broker
- Throughput increases when messages are pulled from a specific partition
- Parallel execution on partitions in parallel can increase the total throughput

## Connectivity

- _Publisher_ and _Subscriber_ can use specific SAS keys (or policies)
- _Namespace_-level policies and _topic/queue_-level policies
- The policy allows _Listen_, _Send_ and _Manage_ activities accordingly
- RBAC is supported

## Advanced Features

- Message sessions enabled by providing a _session id_ property to the _sender_ who submitted the message in the queue or topic
- FIFO and request-response
- FIFO guarantess delivery of messages in the received order and provides the relationship between the messages
- request-response does not support sequences and is usually implemented via a _request_ queue and a _response_ queue
- Messages should be matched. This is the _relay_ scenario
- Message deferral - received postpones processing the messages and leaves them in the queue
- Dead-letter queues (DLQ's) - Special treatment for undelivered and poisoned messages by storing in a separate logical queue
- Deduplication
- Transactional Support
- Auto-forward - Can be configured only for the same namespace server
- Idle auto-delete - deletes the queue after a specified period of inactivity, where the minimum interval is five minutes
- Most turned-off by default

## Provisioning

- Provide a _namespace_ that is the same as the one for `Azure Event Hubs`
- Provision a _topic_ or a _queue_ for the existing _namespace_
- Pricing tier can be updated later
- Premium supports _topics_ and _message units_ to scale throughput
- Basic tier does not support _topics_ nor _scaling_
- Can also specify the total amount of messages (up to 5 GB), the maximum delivery count, the maximum time to live and the lock duration for the messages that are processed
