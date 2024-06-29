using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FlashcardGen.Core;
using FlashcardGen.Common;
using FlashcardGen.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Constants.LocalFiles.ConfigFileName, optional: false);

var services = new ServiceCollection();

services.AddSingleton<IConfiguration>(configBuilder.Build());
services.AddSingleton<ICardGenerator, CardGenerator>();

using (var connection = new SqliteConnection("DataSource=:memory:"))
{
    connection.Open();

    services.AddDbContext<OpenGreekNewTestamentContext>(
        options => options.UseSqlite(connection)
    );

    services.BuildServiceProvider()
    .GetService<ICardGenerator>()!
    .GenerateCards();
}