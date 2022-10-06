using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB_Winform_Demo.Models
{
    [Serializable]
    public class Book
    {
        [BsonId, BsonElement("_id")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public int pages { get; set; }
        public string[] genres { get; set; }
        public int rating { get; set; }
    }
}
