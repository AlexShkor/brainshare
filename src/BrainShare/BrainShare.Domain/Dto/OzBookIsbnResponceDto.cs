using System;

namespace BrainShare.Domain.Dto
{
    [Serializable]
    public class OzBookIsbnResponceDto
    {
        public string Id { get; set; }
        public bool IsWishedBook { get; set; }
        public string Isbn { get; set; }
    }
}
