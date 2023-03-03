namespace MusicHub
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Primitives;
    using MusicHub.Data.Models;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Console.WriteLine(ExportAlbumsInfo(context, 9));

            Console.WriteLine(ExportSongsAboveDuration(context, 4));

            //Test your solutions here
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var albums = context.Albums
                 .Where(a => a.ProducerId.HasValue && a.ProducerId.Value == producerId)
                 .ToArray()
                 .OrderByDescending(a => a.Price)
                 .Select(a => new
                 {
                     a.Name,
                     ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                     ProducerName = a.Producer?.Name,
                     AlbumSongs = a.Songs
                         .Select(s => new
                         {
                             s.Name,
                             Price = s.Price.ToString("f2"),
                             SongWriterName = s.Writer.Name
                         })
                         .OrderByDescending(s => s.Name)
                         .ThenBy(s => s.SongWriterName)
                         .ToArray(),
                     TotalAlbumPrice = a.Price.ToString("f2")
                 })
                 .ToArray();


            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.Name}")
                    .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                    .AppendLine($"-ProducerName: {album.ProducerName}")
                    .AppendLine("-Songs:");

                int count = 1;

                foreach (var song in album.AlbumSongs)
                {
                    sb.AppendLine($"---#{count}")
                        .AppendLine($"---SongName: {song.Name}")
                        .AppendLine($"---Price: {song.Price}")
                        .AppendLine($"---Writer: {song.SongWriterName}");

                    count++;
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                 .AsEnumerable()
                 .Where(s => s.Duration.TotalSeconds > duration)
                 .Select(s => new
                 {
                     SongName = s.Name,
                     Performers = s.SongPerformers
                                .Select(sp => sp.Performer.FirstName + " " + sp.Performer.LastName)
                                .OrderBy(sp => sp)
                                .ToArray(),
                     WriterName = s.Writer.Name,
                     AlbumProducer = s.Album == null ? null
                                                          : (s.Album.Producer == null ? null
                                                                                             : s.Album.Producer.Name),
                     Duration = s.Duration.ToString("c")
                 })
                 .OrderBy(s => s.SongName)
                 .ThenBy(s => s.WriterName)
                 .ToArray();

            StringBuilder sb = new StringBuilder();
            int songNumber = 1;

            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{songNumber}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Writer: {song.WriterName}");

                foreach (var performer in song.Performers)
                {
                    sb.AppendLine($"---Performer: {performer}");
                }

                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}")
                    .AppendLine($"---Duration: {song.Duration}");

                songNumber++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
