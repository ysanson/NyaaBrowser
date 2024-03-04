using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyAnimeApi.src.models
{
    public record class AnimeInfo
    (
        int Id,
        string Title,
        string Synopsis,
        [property: JsonPropertyName("main_picture.large")] string LargePicture,
        [property: JsonPropertyName("main_picture.medium")] string MediumPicture,
        [property: JsonPropertyName("alternative_titles.ja")] string JapaneseTitle,
        DateTime StartDate,
        DateTime EndDate,
        float Mean,
        int Rank,
        string Nsfw,
        int NumEpisodes
    );
}
