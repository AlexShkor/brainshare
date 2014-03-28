using System;
using System.Text;
using MongoDB.Bson;

namespace Brainshare.Infrastructure.Platform.Mongo
{
    /// <summary>
    /// THIS TRICKY CLASS IS NEEDED ONLY BECAUSE OF NANCY WEBFORMS AUTHENTICATION REQUIRE GUIDS
    /// It can't be used as real converter for any Guid as there is no strong corRelation between Guid and ObjectId
    /// So the only correct convertion scenario is ObjectId -> "bad" Guid -> ObjectId, as ObjectId is "less" than Guid
    /// </summary>
    public class ObjectIdConverter
    {
        private const int ObjectIdLength = 24;
        private const int GuidLength = 32;
        // last 8 meaningless characters that will be removed during back convertion
        private const string DummyPostfix = "00000000";
        private const string DummyChar = "a";
        private const string FiveDummyChar = "aaaaa";

        public static Guid ToGuid(string objectId)
        {
            if (objectId.Length == ObjectIdLength)
            {
                return new Guid(objectId + DummyPostfix);
            }
            else
            {
                var dummyCharsCount = GuidLength - objectId.Length;
                var dummyStr = new StringBuilder();
                for (int i = 0; i < dummyCharsCount; i++)
                    dummyStr.Append(DummyChar);

                return new Guid(objectId + dummyStr);
            }
        }

        public static string FromGuid(Guid guid)
        {
            if (guid.ToString().Contains(FiveDummyChar))
            {
                return guid.ToString().Replace(DummyChar, String.Empty).Replace("-", "");
            }
            else
            {
                return new ObjectId(guid.ToString().Replace("-", "").Substring(0, ObjectIdLength)).ToString();
            }

        }
    }
}