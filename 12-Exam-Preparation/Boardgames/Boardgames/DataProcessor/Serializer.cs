using Boardgames.DataProcessor.ExportDto;
using DataProcessor;
using Newtonsoft.Json;

namespace Boardgames.DataProcessor
{
    using Boardgames.Data;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            XmlHelper xmlHelper = new();

            var exportCreators = context
                .Creators
                .Where(c => c.Boardgames.Any())
                .Select(c => new ExportCreatorDto()
                {
                    CreatorName = c.FirstName + " " + c.LastName,
                    BoardgamesCount = c.Boardgames.Count,
                    Boardgames = c.Boardgames
                        .OrderBy(bg => bg.Name)
                        .Select(bg => new ExportBoardgameDto()
                        {
                            BoardgameName = bg.Name,
                            YearPublished = bg.YearPublished,
                        })
                        .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();

            return xmlHelper.Serialize(exportCreators, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var exportSellers = context
                .Sellers
                .Where(s => s.BoardgamesSellers.Any(bg =>
                    bg.Boardgame.YearPublished >= year && bg.Boardgame.Rating <= rating))
                .ToArray()
                .Select(s => new
                {
                    s.Name,
                    s.Website,
                    Boardgames = s.BoardgamesSellers
                        .Where(bg => bg.Boardgame.YearPublished >= year && bg.Boardgame.Rating <= rating)
                        .OrderByDescending(bg => bg.Boardgame.Rating)
                        .ThenBy(bg => bg.Boardgame.Name)
                        .ToArray()
                        .Select(bg => new
                        {
                            bg.Boardgame.Name,
                            bg.Boardgame.Rating,
                            bg.Boardgame.Mechanics,
                            Category = bg.Boardgame.CategoryType.ToString()
                        })
                        .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Length)
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(exportSellers, Formatting.Indented);
        }
    }
}