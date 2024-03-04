using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyAnimeApi.src.models
{
    public record AnimeSearchResult([property: JsonPropertyName("data")]List<AnimeSearch> Results);
    public record class AnimeSearch (
        [property: JsonPropertyName("node.id")] int Id,
        [property: JsonPropertyName("node.title")] string Title
    );
}
