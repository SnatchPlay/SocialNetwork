//sing DAL.Enteties;
using Neo4jClient;
using Neo4jClient.Cypher;
using Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Repositories
{
    public class GraphRepository
    {
        private readonly IGraphClient _graphClient;

        public GraphRepository()
        {
            _graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "1234");
            _graphClient.Connect();
        }


        public IEnumerable<string> ConnectingPaths(RelationUser person1, RelationUser person2)
        {

            var query = _graphClient.Cypher
                .Match("path = shortestPath((p1:Person)-[:FOLLOW*..6]->(p2:Person))")
                .Where((RelationUser p1) => p1.EMail == person1.EMail)
                .AndWhere((RelationUser p2) => p2.EMail == person2.EMail)
                .Return(() => Return.As<IEnumerable<string>>("[n IN nodes(path) | n.nickname]"));

            return query.Results.Single();
        }

        public void CreatePerson(RelationUser person)
        {
            _graphClient.Cypher
                .Create("(np:Person {newPerson})")
                .WithParam("newPerson", person)
                .ExecuteWithoutResults();
        }
        public void CreatRelationShip(RelationUser whoStartFollow, RelationUser whomFollow)
        {
            _graphClient.Cypher
                .Match("(p1:Person {nickname: {p1NickName}})", "(p2:Person {nickname: {p2NickName}})")
                .WithParam("p1NickName", whoStartFollow.EMail)
                .WithParam("p2NickName", whomFollow.EMail)
                .Create("(p1)-[:FOLLOW]->(p2)")
                .ExecuteWithoutResults();
        }
    }
}