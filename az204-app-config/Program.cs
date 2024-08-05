using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();

DotNetEnv.Env.Load();

builder.AddAzureAppConfiguration(Environment.GetEnvironmentVariable("CONNECTION_STRING"));

var config = builder.Build();
var settingName = "AppConfigExample:Settings:Message";
var settingValue = "Default Value (Hello World!)";
Console.WriteLine(
    "Found Value: {0} for Name: {1}",
    config[settingName] ?? settingValue,
    settingName
);
