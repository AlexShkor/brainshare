using System;

namespace BrainShare.Domain.Dto
{
    [Serializable]
    public class OzBookIsbnRequestDto
    {
        public string Id { get; set; }
        public bool IsWishedBook { get; set; }
    }
}
