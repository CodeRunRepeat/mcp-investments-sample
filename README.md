# Securities prices MCP server sample

This is a sample of an MCP server that publishes securities related information
as MCP tools. It is based on the C# MCP SDK samples [here](https://github.com/modelcontextprotocol/csharp-sdk/tree/main/samples).

## Running the sample in Claude for Desktop

1. Clone the repo and open the root directory in VS Code.
1. Build the solution with `dotnet build`.
1. Follow the instructions [here](https://modelcontextprotocol.io/quickstart/server#testing-your-server-with-claude-for-desktop-5)
to configure Claude for Desktop to load your server; the `mcpServers`
section should contain a snippet like this:
```json
    "securities": {
        "command": "dotnet",
        "args": [
            "run",
            "--project",
            "<path>\\mcp-investments-sample\\mcp-server",
            "--no-build"
        ]
    }
```
## Tools
This sample contains 4 tools:

- `GetCurrentSecurityInfo` gets current price, volume, etc. information for the
supplied ticker symbol.
- `GetSecuritiesBySector` gets information for all securities in the given sector.
- `GetHistoricalData` gets historical price, volume, etc. information for a
ticker symbol between the provided dates.
- `GetSectorPerformance` gets price movement and average volume for a sector.

Using these tools, you can ask Claude questions such as 

> "How does AAPL's current
price compare with sector performance?"

Data is loaded from static json files in the `./api/Data` directory.