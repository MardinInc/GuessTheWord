using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheWord
{
    public class Question
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public string textQuestion;
        public string answer;

        public Question(string textQuestion, string answer)
        {
            this.textQuestion = textQuestion;
            this.answer = answer;
        }
    }
}
