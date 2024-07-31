﻿using CinemaApp.Core.Contracts;
using CinemaApp.Core.Models;
using CinemaApp.Infrastructure.Data;
using CinemaApp.Infrastructure.Data.Models;
using CinemaApp.Infrastructure.Data.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

public static class ConsoleInterface
{
    public static void Run(ICinemaService cinemaService, IMovieService movieService)
    {
        Console.WriteLine("Welcome to CinemaApp!");
        Console.WriteLine();

        while (true)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("0. Insert additional movies from JSON");
            Console.WriteLine("1. List all movies");
            Console.WriteLine("2. List all cinemas");
            Console.WriteLine("4. List all animations");
            //Console.WriteLine("5. Do some random stuff");

            string? input = Console.ReadLine();

            if (input == "0")
            {
                List<Movie> extractedMovies = ExtractAdditionalMoviesFromJson();

                if (extractedMovies == null)
                {
                    continue;
                }

                cinemaService.InsertAdditionalMovies(extractedMovies);
                Console.WriteLine($"{extractedMovies.Count} movies have been inserted successfully.");
            }
            else if (input == "1")
            {
                var movies = movieService.GetAllMovies();

                if (movies.Count == 0)
                {
                    Console.WriteLine("No movies available.");
                    continue;
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("Movies:");

                    foreach (Movie movie in movies)
                    {
                        stringBuilder.AppendLine($"Title: {movie.Title}");
                        stringBuilder.AppendLine($"Genre: {movie.Genre}");

                        if (movie.Description != null)
                        {
                            stringBuilder.AppendLine($"Description: {movie.Description}");
                        }
                        else
                        {
                            stringBuilder.AppendLine("Description: N/A");
                        }
                        stringBuilder.AppendLine();
                    }
                    Console.WriteLine(stringBuilder.ToString().Trim());
                }
            }
            else if (input == "2")
            {
                List<Cinema> cinemas = cinemaService.GetAllCinemas();

                if (cinemas.Count == 0)
                {
                    Console.WriteLine("No cinemas available.");
                    continue;
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("Cinemas:");

                    foreach (Cinema cinema in cinemas)
                    {
                        stringBuilder.AppendLine($"Name: {cinema.Name}");
                        stringBuilder.AppendLine($"Address: {cinema.Address}");
                        stringBuilder.AppendLine();
                    }
                    Console.WriteLine(stringBuilder.ToString().Trim());
                }
            }
            else if (input == "3")
            {
                //var animations = movieService
                //        .GetAllMovies(m => m.Genre == Genre.Animation);

                //foreach (var a in animations)
                //{
                //    Console.WriteLine($"{a.Title} - {a.Genre}");
                //}
                //Console.WriteLine();


                Console.WriteLine("Page 1:");
                var animationsPage1 = movieService.GetAllMoviesPaged(1, 3);
                foreach (var a in animationsPage1)
                {
                    Console.WriteLine($"{a.Title} - {a.Genre}");
                }
                Console.WriteLine();

                Console.WriteLine("Page 2:");
                var animationsPage2 = movieService.GetAllMoviesPaged(2, 3);
                foreach (var a in animationsPage2)
                {
                    Console.WriteLine($"{a.Title} - {a.Genre}");
                }
                Console.WriteLine();
            }
            else if (input == "4")
            {
                var cinema = cinemaService.GetAllCinemasByTown("Sofia");
                foreach (var c in cinema)
                {

                    Console.WriteLine($"{c.Name} is in {c.Address} and has a {c.NumberOfHalls} halls");
                }
            }
            else
            {
                Console.WriteLine("Invalid option chosen! Try again...");
                continue;
            }
        }
    }

    private static List<Movie> ExtractAdditionalMoviesFromJson()
    {
        string jsonFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "Datasets", "additionalMovies.json");

        if (File.Exists(jsonFilePath))
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            var movieModels = JsonSerializer.Deserialize<MovieModel[]>(jsonData);

            List<Movie> movies = new List<Movie>();

            if (movieModels != null && movieModels.Any())
            {
                foreach (var movieModel in movieModels)
                {
                    if (!IsValid(movieModel))
                    {
                        Console.WriteLine("Invalid movie.");
                        continue;
                    }

                    Genre genre;
                    if (!Enum.TryParse(movieModel.Genre, out genre))
                    {
                        Console.WriteLine("Invalid movie.");
                        continue;
                    }

                    Movie movie = new Movie()
                    {
                        Title = movieModel.Title,
                        Genre = Enum.Parse<Genre>(movieModel.Genre),
                        Description = movieModel.Description
                    };

                    movies.Add(movie);
                }

                return movies;
            }
            else
            {
                Console.WriteLine("No movies found in the JSON file.");
                return null;
            }
        }
        else
        {
            Console.WriteLine("File not found.");
            return null;
        }
    }

    private static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }
}