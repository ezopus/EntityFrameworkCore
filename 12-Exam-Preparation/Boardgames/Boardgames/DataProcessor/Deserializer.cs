using Boardgames.Data.Models;
using Boardgames.Data.Models.Enums;
using Boardgames.DataProcessor.ImportDto;
using DataProcessor;
using Newtonsoft.Json;
using System.Text;

namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlHelper xmlHelper = new();

            var importCreatorsDtos = xmlHelper.Deserialize<ImportCreatorDto[]>(xmlString, "Creators");

            ICollection<Creator> creators = new HashSet<Creator>();

            foreach (var creatorDto in importCreatorsDtos)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName,
                };

                foreach (var boardgameDto in creatorDto.ImportBoardgameDtos)
                {
                    if (!IsValid(boardgameDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgame = new()
                    {
                        Name = boardgameDto.Name,
                        Rating = boardgameDto.Rating,
                        YearPublished = boardgameDto.YearPublished,
                        CategoryType = (CategoryType)boardgameDto.CategoryType,
                        Mechanics = boardgameDto.Mechanics,
                    };

                    creator.Boardgames.Add(boardgame);
                }

                creators.Add(creator);

                sb.AppendLine(String.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName,
                    creator.Boardgames.Count));
            }

            context.Creators.AddRange(creators);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new();

            var importSellersDtos = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString);

            ICollection<Seller> sellers = new HashSet<Seller>();

            foreach (var sellerDto in importSellersDtos)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller seller = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website,
                };

                foreach (int gameId in sellerDto.Boardgames.Distinct())
                {
                    var game = context.Boardgames.Find(gameId);

                    if (game == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    seller.BoardgamesSellers.Add(new BoardgameSeller()
                    {
                        BoardgameId = gameId,
                        Seller = seller,
                    });
                }

                sellers.Add(seller);
                sb.AppendLine(String.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
            }

            context.Sellers.AddRange(sellers);

            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
