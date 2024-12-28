using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();

DotNetEnv.Env.Load();

builder.AddAzureAppConfiguration(Environment.GetEnvironmentVariable("CONNECTION_STRING"));

var config = builder.Build();
var settingKey = "TestApp:Settings:Test";
var settingValue = "Default Value (Hello World!)";
Console.WriteLine(
    "Found Value: '{0}' for Key: '{1}'",
    config[settingKey] ?? settingValue,
    settingKey
);
