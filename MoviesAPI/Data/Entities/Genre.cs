using System;

namespace MoviesAPI.Data.Entities
{
    [Flags]
    public enum Genre
    {
        Comedy = 1,
        Action = 2,
        Drama = 4,
        SciFi = 8,
        Crime = 16
    }
}
