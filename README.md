# Pigeon

[![Nuget](https://img.shields.io/nuget/v/pigeon)](https://www.nuget.org/packages/Pigeon/)
[![](https://img.shields.io/github/license/ArmadaSuit/fluent-logger-csharp)](https://github.com/ArmadaSuit/fluent-logger-csharp/blob/main/LICENSE)

Pigeon is simple TCP(not SSL/TLS) client that supports part
of [Fluent forward protocol v1](https://github.com/fluent/fluentd/wiki/Forward-Protocol-Specification-v1).

## Requirements

We test Pigeon for server versions below.

- Fluentd v0.14.8 or higher
- Fluent Bit v1.6.0 or higher

## Installation

```
Install-Package Pigeon
```

## Usage

This is simple example.

```c#
// set host name and port number
var config = new new PigeonConfig("127.0.0.1", 24224);
// create client
using var pigeonClient = new PigeonClient(config);
{
    const string tag = "pigeon.example";
    var data = new Dictionary<string, object>
    {
        { "key", "value" }
    };

    // send single entry data
    await pigeonClient.SendAsync(tag, data);

    var entries = new List<Entry>
    {
        new Entry
        {
            Time = new EventTime(DateTime.Now),
            Record = data
        }
    };

    // send multiple entry data
    await pigeonClient.SendAsync(tag, entries);
}
```

And also Pigeon supports all mode.

- Message Mode
- Forward Mode
- PackedForward Mode
- CompressedPackedForward Mode

So, if you want to use specify mode.

 ```c#
// Message Mode
await pigeonClient.SendAsync(new MessageMode(){...});
// Forward Mode
await pigeonClient.SendAsync(new ForwardMode(){...});
// PackedForward Mode
await pigeonClient.SendAsync(new PackedForwardMode(){...});
// CompressedPackedForward Mode
await pigeonClient.SendAsync(new CompressedPackedForwardMode(){...});
 ```

### Server configuration example

Note: It is just temporary configuration.

#### Fluentd configuration example

Remember: Fluentd v0.14.8 or higher.

```
<source>
  @type  forward
  port  24224
</source>

<match **>
  @type stdout
</match>
```

#### Fluent Bit configuration example

Remember: Fluent Bit v1.6.0 or higher

```
[INPUT]
    Name              forward
    Listen            0.0.0.0
    Port              24224

[OUTPUT]
    Name    stdout
    Match   *
```

## Limitations

### Serialization

In Record or Option section (i.e. `Dictionary<string, object>` type), `DateTime` and `DateTimeOffset` is serialized to
ISO 8601 style `string` (
see [Reference](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#Roundtrip))
with default setting.

## License

Apache License, Version 2.0
