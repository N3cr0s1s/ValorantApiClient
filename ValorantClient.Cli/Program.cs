using ValorantClient.Cli.CliClient;

CliClient cliClient = new CliClient();
cliClient.Debug = true;
await cliClient.StartAsync();

return;