using System;

namespace Brainshare.Infrastructure.Dto
{
    [Serializable]
    public class OzBookIsbnResponceDto
    {
        public string Id { get; set; }
        public bool IsWishedBook { get; set; }
        public string Isbn { get; set; }
    }
}
