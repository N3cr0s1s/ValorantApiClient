using ValorantClient.Cli.CliClient;

ICliClient cliClient = new CliClient();
cliClient.Debug = true;
await cliClient.StartAsync();

return;