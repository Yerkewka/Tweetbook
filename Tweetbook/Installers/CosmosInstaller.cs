using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tweetbook.Domain;
using Tweetbook.Domain.Cosmos;

namespace Tweetbook.Installers
{
    public class CosmosInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration Configuration)
        {
            //var cosmosSettings = new CosmosStoreSettings(
            //    Configuration["CosmosDB:DatabaseName"],
            //    Configuration["CosmosDB:AccountUri"],
            //    Configuration["CosmosDB:AccountKey"],
            //    new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp}
            //);

            //services.AddCosmosStore<CosmosPost>(cosmosSettings);
        }
    }
}
