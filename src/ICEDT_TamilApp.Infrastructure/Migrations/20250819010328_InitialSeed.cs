using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ICEDT_TamilApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityTypes",
                columns: table => new
                {
                    ActivityTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTypes", x => x.ActivityTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    LevelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LevelName = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SequenceOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.LevelId);
                });

            migrationBuilder.CreateTable(
                name: "MainActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    LessonId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LessonName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    SequenceOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    LessonImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    LevelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.LessonId);
                    table.ForeignKey(
                        name: "FK_Lessons_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "LevelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SequenceOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    ContentJson = table.Column<string>(type: "TEXT", nullable: false),
                    LessonId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActivityTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    MainActivityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_Activities_ActivityTypes_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityTypes",
                        principalColumn: "ActivityTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "LessonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_MainActivities_MainActivityId",
                        column: x => x.MainActivityId,
                        principalTable: "MainActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCurrentProgress",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserCurrentProgressId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastActivityAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CurrentLessonId = table.Column<int>(type: "INTEGER", nullable: false),
                    LessonId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCurrentProgress", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserCurrentProgress_Lessons_CurrentLessonId",
                        column: x => x.CurrentLessonId,
                        principalTable: "Lessons",
                        principalColumn: "LessonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCurrentProgress_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "LessonId");
                    table.ForeignKey(
                        name: "FK_UserCurrentProgress_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProgresses",
                columns: table => new
                {
                    ProgressId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActivityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgresses", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK_UserProgresses_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "ActivityTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "PronunciationPractice" },
                    { 2, "AudioImageRecognition" },
                    { 3, "Dictation" },
                    { 4, "Matching" },
                    { 5, "SortingAndClassification" },
                    { 6, "OddOneOut" },
                    { 7, "FillInTheBlanks" },
                    { 8, "WordScramble" },
                    { 9, "SentenceScramble" },
                    { 10, "WordFormation" },
                    { 11, "SentenceBuilding" },
                    { 12, "GrammarPuzzle" },
                    { 13, "MultipleChoiceQuestion" },
                    { 14, "TrueOrFalse" },
                    { 15, "ReadingComprehension" },
                    { 16, "StorySequencing" },
                    { 17, "TimedChallenge" },
                    { 18, "InteractiveDialogue" }
                });

            migrationBuilder.InsertData(
                table: "Levels",
                columns: new[] { "LevelId", "CoverImageUrl", "LevelName", "SequenceOrder", "Slug" },
                values: new object[,]
                {
                    { 1, null, "மழலையர் நிலை", 1, "malalaiyar-nilai" },
                    { 2, null, "சிறுவர் நிலை", 2, "siruvar-nilai" },
                    { 3, null, "ஆண்டு 01", 3, "aandu-01" },
                    { 4, null, "ஆண்டு 02", 4, "aandu-02" },
                    { 5, null, "ஆண்டு 03", 5, "aandu-03" },
                    { 6, null, "ஆண்டு 04", 6, "aandu-04" },
                    { 7, null, "ஆண்டு 05", 7, "aandu-05" }
                });

            migrationBuilder.InsertData(
                table: "MainActivities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Video" },
                    { 2, "Sounds" },
                    { 3, "Learning" },
                    { 4, "Exercises" }
                });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "LessonId", "Description", "LessonImageUrl", "LessonName", "LevelId", "SequenceOrder", "Slug" },
                values: new object[,]
                {
                    { 1, "Basic body parts", null, "பாடம் 01: உடல் உறுப்புகள்", 1, 1, "malalaiyar-udal-uruppukal" },
                    { 2, "Immediate family members", null, "பாடம் 02: எனது குடும்பம்", 1, 2, "malalaiyar-enathu-kudumbam" },
                    { 3, "Parts of a house", null, "பாடம் 03: எனது வீடு", 1, 3, "malalaiyar-enathu-veedu" },
                    { 4, "Common foods and tastes", null, "பாடம் 04: உணவுகள்", 1, 4, "malalaiyar-unavugal" },
                    { 5, "Colors", null, "பாடம் 05: வண்ணங்கள்", 1, 5, "malalaiyar-vannangal" },
                    { 6, "Flowers", null, "பாடம் 06: பூக்கள்", 1, 6, "malalaiyar-pookkal" },
                    { 7, "Birds", null, "பாடம் 07: பறவைகள்", 1, 7, "malalaiyar-paravaigal" },
                    { 8, "Animals", null, "பாடம் 08: விலங்குகள்", 1, 8, "malalaiyar-vilangugal" },
                    { 9, "Games and play", null, "பாடம் 09: விளையாட்டுகள்", 1, 9, "malalaiyar-vilaiyattukal" },
                    { 10, "Celebrations", null, "பாடம் 10: கொண்டாட்டம்", 1, 10, "malalaiyar-kondaattam" },
                    { 11, "Myself and body parts", null, "பாடம் 01: நான்", 2, 1, "siruvar-naan" },
                    { 12, "Extended family", null, "பாடம் 02: என் குடும்பம்", 2, 2, "siruvar-en-kudumbam" },
                    { 13, "Rooms in a house", null, "பாடம் 03: எனது வீடு", 2, 3, "siruvar-enathu-veedu" },
                    { 14, "Food types including seafood", null, "பாடம் 04: உணவுகள்", 2, 4, "siruvar-unavugal" },
                    { 15, "Clothing", null, "பாடம் 05: உடைகள்", 2, 5, "siruvar-udaigal" },
                    { 16, "Wild animals", null, "பாடம் 06: விலங்குகள்", 2, 6, "siruvar-vilangugal" },
                    { 17, "Birthday celebrations", null, "பாடம் 07: பிறந்தநாள்", 2, 7, "siruvar-piranthanaal" },
                    { 18, "Classroom items", null, "பாடம் 08: வகுப்பறை", 2, 8, "siruvar-vagupparai" },
                    { 19, "Vehicles", null, "பாடம் 09: உந்துகள்", 2, 9, "siruvar-unthugal" },
                    { 20, "Verbs for play", null, "பாடம் 10: விளையாட்டு", 2, 10, "siruvar-vilaiyattu" },
                    { 21, "The Tamil school", null, "பாடம் 01: தமிழ்ப்பள்ளி", 3, 1, "aandu-01-paadam-01" },
                    { 22, "Vacation", null, "பாடம் 02: விடுமுறை", 3, 2, "aandu-01-paadam-02" },
                    { 23, "Friends", null, "பாடம் 03: நண்பர்கள்", 3, 3, "aandu-01-paadam-03" },
                    { 24, "Mother's Day", null, "பாடம் 04: அன்னையர் நாள்", 3, 4, "aandu-01-paadam-04" },
                    { 25, "Thai Pongal festival", null, "பாடம் 05: தைப்பொங்கல்", 3, 5, "aandu-01-paadam-05" },
                    { 26, "The Market", null, "பாடம் 06: அங்காடி", 3, 6, "aandu-01-paadam-06" },
                    { 27, "Domestic Animals", null, "பாடம் 07: வீட்டு விலங்குகள்", 3, 7, "aandu-01-paadam-07" },
                    { 28, "Winter Season", null, "பாடம் 08: பனி காலம்", 3, 8, "aandu-01-paadam-08" },
                    { 29, "Balanced Diet", null, "பாடம் 09: நிறையுணவு", 3, 9, "aandu-01-paadam-09" },
                    { 30, "Days and Months", null, "பாடம் 10: நாள்கள், மாதங்கள்", 3, 10, "aandu-01-paadam-10" },
                    { 31, "The Tamil Language", null, "பாடம் 01: தமிழ்மொழி", 4, 1, "aandu-02-paadam-01" },
                    { 32, "Our Motherland", null, "பாடம் 02: எங்கள் தாயகம்", 4, 2, "aandu-02-paadam-02" },
                    { 33, "Fine Arts", null, "பாடம் 03: இன்கலைகள்", 4, 3, "aandu-02-paadam-03" },
                    { 34, "Community Helpers", null, "பாடம் 04: கை கொடுப்போம்", 4, 4, "aandu-02-paadam-04" },
                    { 35, "King Cankiliyan", null, "பாடம் 05: சங்கிலியன்", 4, 5, "aandu-02-paadam-05" },
                    { 36, "Seasons", null, "பாடம் 06: பருவகாலங்கள்", 4, 6, "aandu-02-paadam-06" },
                    { 37, "Our Environment", null, "பாடம் 07: நாம் வாழும் சூழல்", 4, 7, "aandu-02-paadam-07" },
                    { 38, "Poet Somasundara Pulavar", null, "பாடம் 08: சோமசுந்தரப் புலவர்", 4, 8, "aandu-02-paadam-08" },
                    { 39, "Palmyra Tree", null, "பாடம் 09: பனைமரம்", 4, 9, "aandu-02-paadam-09" },
                    { 40, "Poet Avvaiyar", null, "பாடம் 10: ஔவையார்", 4, 10, "aandu-02-paadam-10" },
                    { 41, "Respecting Elders", null, "பாடம் 11: மூத்தோரை மதிப்போம்", 4, 11, "aandu-02-paadam-11" },
                    { 42, "Outer Space", null, "பாடம் 12: விண்வெளி", 4, 12, "aandu-02-paadam-12" },
                    { 43, "Our Language", null, "பாடம் 01: எமது மொழி", 5, 1, "aandu-03-paadam-01" },
                    { 44, "The Three Great Fruits", null, "பாடம் 02: முக்கனிகள்", 5, 2, "aandu-03-paadam-02" },
                    { 45, "Charity", null, "பாடம் 03: கொடை", 5, 3, "aandu-03-paadam-03" },
                    { 46, "Unity", null, "பாடம் 04: ஒற்றுமை", 5, 4, "aandu-03-paadam-04" },
                    { 47, "Batticaloa", null, "பாடம் 05: மட்டக்களப்பு", 5, 5, "aandu-03-paadam-05" },
                    { 48, "Pandara Vanniyan", null, "பாடம் 06: பண்டாரவன்னியன்", 5, 6, "aandu-03-paadam-06" },
                    { 49, "Folk Songs", null, "பாடம் 07: நாட்டார் பாடல்கள்", 5, 7, "aandu-03-paadam-07" },
                    { 50, "Thiruvalluvar", null, "பாடம் 08: திருவள்ளுவர்", 5, 8, "aandu-03-paadam-08" },
                    { 51, "Our Kite", null, "பாடம் 09: எங்கள் பட்டம்", 5, 9, "aandu-03-paadam-09" },
                    { 52, "Kallanai Dam", null, "பாடம் 10: கல்லணை", 5, 10, "aandu-03-paadam-10" },
                    { 53, "Water", null, "பாடம் 11: தண்ணீர்", 5, 11, "aandu-03-paadam-11" },
                    { 54, "Kilithattu Game", null, "பாடம் 12: கிளித்தட்டு", 5, 12, "aandu-03-paadam-12" },
                    { 55, "Tamil Language Deep Dive", null, "பாடம் 01: தமிழ்மொழி", 6, 1, "aandu-04-paadam-01" },
                    { 56, "Language Exercises", null, "பாடம் 02: மொழிப்பயிற்சி", 6, 2, "aandu-04-paadam-02" },
                    { 57, "The Greatness of Education", null, "பாடம் 03: கல்வியின் சிறப்பு", 6, 3, "aandu-04-paadam-03" },
                    { 58, "Grammatical Gender", null, "பாடம் 04: பால்", 6, 4, "aandu-04-paadam-04" },
                    { 59, "Communication", null, "பாடம் 05: தொடர்பாடல்", 6, 5, "aandu-04-paadam-05" },
                    { 60, "Aadi Pirappu festival", null, "பாடம் 06: ஆடிப்பிறப்பு", 6, 6, "aandu-04-paadam-06" },
                    { 61, "The Library", null, "பாடம் 07: நூலகம்", 6, 7, "aandu-04-paadam-07" },
                    { 62, "Musical Instruments", null, "பாடம் 08: இசைக்கருவிகள்", 6, 8, "aandu-04-paadam-08" },
                    { 63, "Trincomalee", null, "பாடம் 09: திருகோணமலை", 6, 9, "aandu-04-paadam-09" },
                    { 64, "Winter", null, "பாடம் 10: குளிர்காலம்", 6, 10, "aandu-04-paadam-10" },
                    { 65, "The Power of Truth", null, "பாடம் 11: உண்மையின் உயர்வு", 6, 11, "aandu-04-paadam-11" },
                    { 66, "Hero Stones", null, "பாடம் 12: நடுகல்", 6, 12, "aandu-04-paadam-12" },
                    { 67, "Tamil Arts", null, "பாடம் 01: தமிழர் கலைகள்", 7, 1, "aandu-05-paadam-01" },
                    { 68, "Discipline", null, "பாடம் 02: ஒழுக்கம்", 7, 2, "aandu-05-paadam-02" },
                    { 69, "Friendship", null, "பாடம் 03: நட்பு", 7, 3, "aandu-05-paadam-03" },
                    { 70, "Poet Bharathiyar", null, "பாடம் 04: பாரதியார்", 7, 4, "aandu-05-paadam-04" },
                    { 71, "Ilango Adigal", null, "பாடம் 05: இளங்கோ அடிகள்", 7, 5, "aandu-05-paadam-05" },
                    { 72, "Newspaper", null, "பாடம் 06: செய்தித்தாள்", 7, 6, "aandu-05-paadam-06" },
                    { 73, "Thai Thirunaal", null, "பாடம் 07: தைத்திருநாள்", 7, 7, "aandu-05-paadam-07" },
                    { 74, "Pavalakodi", null, "பாடம் 08: பவளக்கொடி", 7, 8, "aandu-05-paadam-08" },
                    { 75, "Mannar", null, "பாடம் 09: மன்னார்", 7, 9, "aandu-05-paadam-09" },
                    { 76, "A Healthy Life", null, "பாடம் 10: நோயற்ற வாழ்வு", 7, 10, "aandu-05-paadam-10" },
                    { 77, "Heroism", null, "பாடம் 11: மாவீரம்", 7, 11, "aandu-05-paadam-11" },
                    { 78, "The Olympics", null, "பாடம் 12: ஒலிம்பிக்", 7, 12, "aandu-05-paadam-12" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityTypeId",
                table: "Activities",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_LessonId_SequenceOrder",
                table: "Activities",
                columns: new[] { "LessonId", "SequenceOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_MainActivityId",
                table: "Activities",
                column: "MainActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LevelId_SequenceOrder",
                table: "Lessons",
                columns: new[] { "LevelId", "SequenceOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Slug",
                table: "Lessons",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Levels_SequenceOrder",
                table: "Levels",
                column: "SequenceOrder",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCurrentProgress_CurrentLessonId",
                table: "UserCurrentProgress",
                column: "CurrentLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCurrentProgress_LessonId",
                table: "UserCurrentProgress",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_ActivityId",
                table: "UserProgresses",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_UserId",
                table: "UserProgresses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCurrentProgress");

            migrationBuilder.DropTable(
                name: "UserProgresses");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ActivityTypes");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "MainActivities");

            migrationBuilder.DropTable(
                name: "Levels");
        }
    }
}
