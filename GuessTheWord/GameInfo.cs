using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheWord
{
    public  class GameInfo
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public int rightAnswer = 0;
        public DateTime date = DateTime.Now;
    }
}
