using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicDistory.Data.Migrations
{
    public partial class seeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO dbo.Artists (Name) VALUES ('Linkin Park')");
            migrationBuilder.Sql("INSERT INTO dbo.Artists (Name) VALUES ('Skillet')");
            migrationBuilder.Sql("INSERT INTO dbo.Artists (Name) VALUES ('Evanescences')");
            migrationBuilder.Sql("INSERT INTO dbo.Artists (Name) VALUES ('Nickelbeck')");

            migrationBuilder.Sql("INSERT INTO Dbo.Musics (ArtistId, Name) Values ((SELECT Id FROM Dbo.Artists WHERE Name = 'Linkin Park'), 'In The End')");
            migrationBuilder.Sql("INSERT INTO Dbo.Musics (ArtistId, Name) Values ((SELECT Id FROM Dbo.Artists WHERE Name = 'Linkin Park'), 'Numb')");
            migrationBuilder.Sql("INSERT INTO Dbo.Musics (ArtistId, Name) Values ((SELECT Id FROM Dbo.Artists WHERE Name = 'Skillet'), 'Hero')");
            migrationBuilder.Sql("INSERT INTO Dbo.Musics (ArtistId, Name) Values ((SELECT Id FROM Dbo.Artists WHERE Name = 'Skillet'), 'Awake And Alive')");
            migrationBuilder.Sql("INSERT INTO Dbo.Musics (ArtistId, Name) Values ((SELECT Id FROM Dbo.Artists WHERE Name = 'Nickelbeck'), 'How You Remind Me')");
            migrationBuilder.Sql("INSERT INTO Dbo.Musics (ArtistId, Name) Values ((SELECT Id FROM Dbo.Artists WHERE Name = 'Nickelbeck'), 'What Are You Waiting For')");
            migrationBuilder.Sql("INSERT INTO Dbo.Musics (ArtistId, Name) Values ((SELECT Id FROM Dbo.Artists WHERE Name = 'Nickelbeck'), 'Rockstar')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM dbo.Artists");
            migrationBuilder.Sql("DELETE FROM dbo.Musics");
        }
    }
}
