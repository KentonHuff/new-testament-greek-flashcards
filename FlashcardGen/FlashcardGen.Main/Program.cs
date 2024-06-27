using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FlashcardGen.Core;
using FlashcardGen.Common;

var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Constants.LocalFiles.ConfigFileName, optional: false);

var services = new ServiceCollection();

services.AddSingleton<IConfiguration>(configBuilder.Build());
services.AddSingleton<ICardGenerator, CardGenerator>();

services.BuildServiceProvider()
    .GetService<ICardGenerator>()!
    .GenerateCards();