using System;

namespace Brainshare.Infrastructure.Dto
{
    [Serializable]
    public class OzBookIsbnRequestDto
    {
        public string Id { get; set; }
        public bool IsWishedBook { get; set; }
    }
}
