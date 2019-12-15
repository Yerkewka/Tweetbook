﻿using Cosmonaut.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Domain.Cosmos
{
    [CosmosCollection("posts")]    
    public class CosmosPost
    {
        [CosmosPartitionKey]
        [JsonProperty("id")]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
