# Azure Function Apps

## `Monday, 12/30/24`

- Using `Azurite` and how to handle all the files it generates
- See [here](https://mikestephenson.me/2021/12/28/dealing-with-annoying-azurite-files-when-developing-logic-apps/)

## Comparison to `Logic Apps`

- `Logic App` development is more _declarative_
- `Function App` development is more _imperative_
- You can monitor `Function Apps` with `Application Insights`
- `Logic Apps` can only be monitored via the Azure _portal_ and `Azure Monitor`
- `App Service WebJobs` and `Azure Functions` are both built with the `WebJobs SDK`
- Additional features `Azure Functions` have:
  - _Serverless_ application model and ability to autoscale without additional configuration
  - Ability to develop and test within the browser
  - Trigger on _HTTP/webhook_ and _Azure Event Grid_ events
  - More options for langues, development environments, pricing and integration with other Azure services
  - Pay-per-use pricing

## Hosting Options

### Consumption

- _Serverless_ is the default plan, providing automatic scaling of function instances
- Pay for only _number of executions_, _execution time_ and _memory_ use.
- Measured in _GB-seconds_
- Free grant of 1,000,000 _executions_ and 400,000 _GB-seconds_ each month
- After idle period, instances scaled down to zero (0)
- Cold startup for subsequent requests

### Premium

- Auto scales using _pre-warmed_ instances, meaning _no latency_
- More powerful _instances_ and ability to connect to _virtual networks_
- Intended for function apps that need to run continuously or for longer that the _execution time limit_ on the **Consumption** plan or need to run _on a custom linux image_
- Uses **Elastic Premium EP** App Service plans
- Billing based on _number of core-seconds_ and _memory allocation_ across **all** instances
- No _execution charge_, but there is a _minimum_ charge each month

### App Service Plan

- Previously: _Dedicated_
- Same `App Service Plan` as usual
- Useful to make use of _underutilized_ _App Service Plans_
- Make sure that _Configuration_ &rarr; _General Settings_ &rarr; _Always On_ is set to _On_
- Should be the default

### Other Options

- Hosting via `App Service Environments` for fully-isolated
- `Kubernetes`

### Scaling

- `Function Apps` are the _unit of deployment_, but also the _unit of scale_
- `Scale Controller` monitors the rate of events to determine whether to scale in or out
- Different logic based on the type of trigger used
- For example will take into accout the _queue lenth_ and _age of the oldest queue message_ into account for `Azure Queue Storage` triggers
- No limit on concurrent executions
- A `Function App` can scale out to a maximum of _200_ instances on the _Consumption_ plan for _Windows_ and _100_ instances on the _Consumption_ plan for _linux_
- A `Function App` can scale out to a maximum of _100_ instances on the _Premium_ plan for both _Windows_ and _linux_
- Limits can be reduced to control costs

### Triggers & Bindings

- `Data Operations`, `Timers` and `webhooks`
- `Input Bindings` are received by the function as parameters
- `Output Bindings` use the return value of the function
- `Triggers` create an `Input Binding` by default.
- `C#` class library can configure _triggers_ and _bindings_ by _decorating_ _methods_ and _parameters_ with _C# attributes_
- Also, `function.json` file
- Specify _binding_ as _input_ or _output_ by specifying _in_ or _out_ as the _direction_. Some also support _inout_ directions
- Each _binding_ needs a _type_, _direction_ and _name_ value to be defined
- `wwwroot` folder will contain a `host.json` file, containing the _configuration_ for all the functions in the `Function App`
- If you create and publish a function from a local project, do not modify from within the portal
- Once you deploy a project developed locally, the creation of new functions (via the portal) within the same function app is not allowed
