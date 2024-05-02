using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyAnimeApi.src.models
{
    public record AnimeSearchResult([property: JsonPropertyName("data")]List<AnimeNode> Results);

    public record AnimeNode(AnimeSearch Node);

    public record class AnimeSearch (
        int Id,
        string Title,
        [property: JsonPropertyName("main_picture")] Pictures Pictures
    );

    public record Pictures (
        string Medium,
        string Large
    );
}
