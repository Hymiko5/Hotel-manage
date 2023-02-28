using Elasticsearch.Net;
using HotelAPI.Models;
using Nest;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelAPI.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static ElasticClient GetESClient()
        {
            ConnectionSettings connectionSettings;
            ElasticClient elasticClient;
            StaticConnectionPool connectionPool;

            var nodes = new Uri[]
            {
                new Uri("http://localhost:9200")
            };

            connectionPool = new StaticConnectionPool(nodes);
            connectionSettings = new ConnectionSettings(connectionPool);
            elasticClient = new ElasticClient(connectionSettings);

            return elasticClient;
        }

        public static void CreateRoomTypeDocument(ElasticClient elasticClient, string indexName, RoomType roomType, string documentId)
        {
            var response = elasticClient.Index(roomType, i => i
            .Index(indexName)
            .Id(documentId)
            .Refresh(Elasticsearch.Net.Refresh.True));
        }

        public static async Task<ISearchResponse<RoomType>> GetRoomTypeDocument(ElasticClient elasticClient, string indexName, string documentId)
        {
            return await elasticClient.SearchAsync<RoomType>(s => s.Index(indexName).Query(q => q.Term(t => t.Field("_id").Value(documentId))));
            
        }

        public static List<RoomType> ToRoomTypeList(ISearchResponse<RoomType> response)
        {
            List<RoomType> list = new List<RoomType>();
            foreach (var hit in response.Hits)
            {
                RoomType roomType = new RoomType()
                {
                    Id = hit.Source.Id,
                    TypeName = hit.Source.TypeName,
                    Description = hit.Source.Description,
                    Totals = hit.Source.Totals,
                };
                list.Add(roomType);
            }
            return list;
        }
        public static async Task<ISearchResponse<RoomType>> GetRoomTypeDocumentByName(ElasticClient elasticClient, string indexName, string typeName)
        {
            return await elasticClient.SearchAsync<RoomType>(s => s.Index(indexName).Query(q => q.Match(m => m.Field(f => f.TypeName).Query(typeName))));

        }

    }
}
