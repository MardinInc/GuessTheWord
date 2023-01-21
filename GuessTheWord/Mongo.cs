using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheWord
{
    public class Mongo
    {
        public static IMongoDatabase db = new MongoClient("mongodb://localhost").GetDatabase("GuessTheWord");

        public static void AddToDataBase(Question info)
        {
            var collection = db.GetCollection<Question>("Questions");
            collection.InsertOne(info);
        }
        public static void AddToDataBaseGame(GameInfo info)
        {
            var collection = db.GetCollection<GameInfo>("GameInfo");
            collection.InsertOne(info);
        }

        public static List<Question> GetCollection()
        {
            var filter = new BsonDocument();
            var collection = db.GetCollection<Question>("Questions").Find(new BsonDocument()).ToList();
            return collection;
        }
    }
}
