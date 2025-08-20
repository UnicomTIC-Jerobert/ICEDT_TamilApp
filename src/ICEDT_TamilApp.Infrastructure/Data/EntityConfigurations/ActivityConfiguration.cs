using ICEDT_TamilApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICEDT_TamilApp.Infrastructure.Data.EntityConfigurations
{
  public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
  {
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
      builder.HasKey(a => a.ActivityId);
      builder.Property(a => a.LessonId).IsRequired();
      builder.Property(a => a.ActivityTypeId).IsRequired();
      builder.Property(a => a.Title).IsRequired().HasMaxLength(100);
      builder.Property(a => a.SequenceOrder).IsRequired();
      builder.Property(a => a.ContentJson).IsRequired();

      builder
          .HasOne(a => a.Lesson)
          .WithMany(l => l.Activities)
          .HasForeignKey(a => a.LessonId)
          .OnDelete(DeleteBehavior.Cascade);

      builder
          .HasOne(a => a.ActivityType)
          .WithMany(t => t.Activities)
          .HasForeignKey(a => a.ActivityTypeId)
          .OnDelete(DeleteBehavior.Restrict);

      builder
          .HasOne(a => a.MainActivity)
          .WithMany(m => m.Activities)
          .HasForeignKey(a => a.MainActivityId)
          .IsRequired();

      builder.HasIndex(a => new { a.LessonId, a.SequenceOrder }).IsUnique();

      builder.HasData(

          // === Activity for Grade 1, Lesson 1: Fill in the Equation (ID: 7) ===
          new Activity
          {
            ActivityId = 1,      // Must provide a unique PK
            LessonId = 21,       // Belongs to "ஆண்டு 01 - பாடம் 01"
            Title = "அ-ஓசை உயிர்மெய் எழுத்துகள் (க் + அ)",
            SequenceOrder = 1,
            ActivityTypeId = 7,  // ID for FillInTheBlanks
            MainActivityId = 3,  // ID for "Learning"
            ContentJson = @"[
      { ""leftOperand"": ""க்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""க"", ""options"": [""கா"", ""கி"", ""க"", ""கூ""] },
      { ""leftOperand"": ""ங்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ங"", ""options"": [""ஙா"", ""ஙி"", ""ங"", ""ஙூ""] },
      { ""leftOperand"": ""ச்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ச"", ""options"": [""சா"", ""சி"", ""ச"", ""சூ""] },
      { ""leftOperand"": ""ஞ்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ஞ"", ""options"": [""ஞா"", ""ஞி"", ""ஞ"", ""ஞூ""] },
      { ""leftOperand"": ""ட்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ட"", ""options"": [""டா"", ""டி"", ""ட"", ""டூ""] },
      { ""leftOperand"": ""ண்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ண"", ""options"": [""ணா"", ""ணி"", ""ண"", ""ணூ""] },
      { ""leftOperand"": ""த்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""த"", ""options"": [""தா"", ""தி"", ""த"", ""தூ""] },
      { ""leftOperand"": ""ந்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ந"", ""options"": [""நா"", ""நி"", ""ந"", ""நூ""] },
      { ""leftOperand"": ""ப்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ப"", ""options"": [""பா"", ""பி"", ""ப"", ""பூ""] },
      { ""leftOperand"": ""ம்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ம"", ""options"": [""மா"", ""மி"", ""ம"", ""மூ""] },
      { ""leftOperand"": ""ய்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ய"", ""options"": [""யா"", ""யி"", ""ய"", ""யூ""] },
      { ""leftOperand"": ""ர்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ர"", ""options"": [""ரா"", ""ரி"", ""ர"", ""ரூ""] },
      { ""leftOperand"": ""ல்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ல"", ""options"": [""லா"", ""லி"", ""ல"", ""லூ""] },
      { ""leftOperand"": ""வ்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""வ"", ""options"": [""வா"", ""வி"", ""வ"", ""வூ""] },
      { ""leftOperand"": ""ழ்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ழ"", ""options"": [""ழா"", ""ழி"", ""ழ"", ""ழூ""] },
      { ""leftOperand"": ""ள்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ள"", ""options"": [""ளா"", ""ளி"", ""ள"", ""ளூ""] },
      { ""leftOperand"": ""ற்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ற"", ""options"": [""றா"", ""றி"", ""ற"", ""றூ""] },
      { ""leftOperand"": ""ன்"", ""rightOperand"": ""அ"", ""correctAnswer"": ""ன"", ""options"": [""னா"", ""னி"", ""ன"", ""னூ""] }
    ]"
          },

          // === Activity for Grade 1, Lesson 1: First Letter Match (ID: 4) ===
          new Activity
          {
            ActivityId = 2,
            LessonId = 21,
            Title = "முதல் எழுத்துச் சொல் கண்டறிதல்",
            SequenceOrder = 2,
            ActivityTypeId = 4, // ID for Matching
            MainActivityId = 4, // ID for "Exercises"
            ContentJson = @"{
                      ""title"": ""Find the word that starts with the letter shown above."",
                      ""words"": [""பல்"", ""கல்"", ""கண்"", ""மண்"", ""வயல்"", ""மரம்"", ""படம்"", ""தடம்"", ""அப்பம்"", ""மன்னன்""]
                    }"
          },

          // === Activity for Grade 1, Lesson 5: Number Matching (ID: 4) ===
          new Activity
          {
            ActivityId = 3,
            LessonId = 25, // Belongs to "ஆண்டு 01 - பாடம் 05"
            Title = "எண்களை எழுத்துக்களுடன் பொருத்துக (1-5)",
            SequenceOrder = 1,
            ActivityTypeId = 4, // ID for Matching
            MainActivityId = 4, // ID for "Exercises"
            ContentJson = @"{
                      ""title"": ""Match the Number to the Word"",
                      ""columnA"": [
                        { ""id"": ""A1"", ""content"": ""1"", ""matchId"": ""B1"" },
                        { ""id"": ""A2"", ""content"": ""2"", ""matchId"": ""B2"" },
                        { ""id"": ""A3"", ""content"": ""3"", ""matchId"": ""B3"" },
                        { ""id"": ""A4"", ""content"": ""4"", ""matchId"": ""B4"" },
                        { ""id"": ""A5"", ""content"": ""5"", ""matchId"": ""B5"" }
                      ],
                      ""columnB"": [
                        { ""id"": ""B1"", ""content"": ""ஒன்று"", ""matchId"": ""A1"" },
                        { ""id"": ""B2"", ""content"": ""இரண்டு"", ""matchId"": ""A2"" },
                        { ""id"": ""B3"", ""content"": ""மூன்று"", ""matchId"": ""A3"" },
                        { ""id"": ""B4"", ""content"": ""நான்கு"", ""matchId"": ""A4"" },
                        { ""id"": ""B5"", ""content"": ""ஐந்து"", ""matchId"": ""A5"" }
                      ]
                    }"
          },

          // === Activity for Grade 2, Lesson 2: Word Bank Completion (ID: 7) ===
          new Activity
          {
            ActivityId = 4,
            LessonId = 32, // Belongs to "ஆண்டு 02 - பாடம் 02"
            Title = "வினைச்சொல் பொருத்தம் (Word Bank)",
            SequenceOrder = 1,
            ActivityTypeId = 7, // ID for FillInTheBlanks
            MainActivityId = 4, // ID for "Exercises"
            ContentJson = @"{
                      ""title"": ""Fill in the blanks with the correct word from the word bank."",
                      ""sentences"": [
                        { ""id"": 1, ""prefix"": """", ""suffix"": "" செழிப்பாக வளர்கின்றன."", ""correctAnswer"": ""அவை"" },
                        { ""id"": 2, ""prefix"": """", ""suffix"": "" பேருந்தில் செல்கிறாள்."", ""correctAnswer"": ""அவள்"" }
                      ],
                      ""wordBank"": [""அவை"", ""அவள்"", ""பறக்கிறது"", ""ஆடுகிறோம்""]
                    }"
          },

          // === Activity for Grade 3, Lesson 3: Dropdown Completion (ID: 7) ===
          new Activity
          {
            ActivityId = 5,
            LessonId = 45, // Belongs to "ஆண்டு 03 - பாடம் 03"
            Title = "பொருத்தமான வினைச்சொல்லைத் தெரிவு செய்க (Dropdown)",
            SequenceOrder = 1,
            ActivityTypeId = 7, // ID for FillInTheBlanks
            MainActivityId = 4, // ID for "Exercises"
            ContentJson = @"{
                      ""title"": ""Complete the sentences by selecting the correct verb."",
                      ""sentences"": [
                        {
                          ""id"": 1,
                          ""prefix"": ""மாதவி"",
                          ""suffix"": ""பாட்டு."",
                          ""options"": [""பாடுகிறாள்"", ""வரைகிறான்"", ""மீட்டுகிறாள்""],
                          ""correctAnswer"": ""பாடுகிறாள்""
                        },
                        {
                          ""id"": 2,
                          ""prefix"": ""மகிழன்"",
                          ""suffix"": ""ஓவியம்."",
                          ""options"": [""ஆடுகிறாள்"", ""வரைகிறான்"", ""ஊதுகிறாள்""],
                          ""correctAnswer"": ""வரைகிறான்""
                        }
                      ]
                    }"
          },

// === Activity for Preschool, Lesson 1: Letter Spotlight (ID: 2) ===
// Inside builder.HasData(...)

// === Activity for Preschool, Lesson 1: Letter Spotlight (SINGLE EXERCISE) ===
new Activity
{
  ActivityId = 7,
  LessonId = 1,
  Title = "உயிர் எழுத்து - அ",
  SequenceOrder = 1,
  ActivityTypeId = 2,
  MainActivityId = 3,
  ContentJson = @"{
      ""spotlightLetter"": ""அ"",
      ""words"": [
        { ""text"": ""அம்மா"" }, { ""text"": ""அரிசி"" }, { ""text"": ""அன்னம்"" },
        { ""text"": ""அடுப்பு"" }, { ""text"": ""அருவி"" }
      ]
    }"
},


// === Activity for Kindergarten, Lesson 5: Consonants (MULTIPLE EXERCISES in one Activity) ===
new Activity
{
  ActivityId = 8,
  LessonId = 15,
  Title = "மெய்யெழுத்துகள் பயிற்சி",
  SequenceOrder = 1, // Only ONE activity for this lesson
  ActivityTypeId = 2,
  MainActivityId = 3,
  // The ContentJson is an ARRAY of exercise objects
  ContentJson = @"[
      {
        ""spotlightLetter"": ""க்"",
        ""words"": [
          { ""text"": ""கொக்கு"" }, { ""text"": ""பாக்கு"" }, { ""text"": ""நாக்கு"" },
          { ""text"": ""தக்காளி"" }, { ""text"": ""சக்கரம்"" }
        ]
      },
      {
        ""spotlightLetter"": ""ங்"",
        ""words"": [
          { ""text"": ""சங்கு"" }, { ""text"": ""நங்கூரம்"" }, { ""text"": ""குரங்கு"" },
          { ""text"": ""பழங்கள்"" }, { ""text"": ""தங்கம்"" }
        ]
      },
      {
        ""spotlightLetter"": ""ச்"",
        ""words"": [
          { ""text"": ""பச்சை"" }, { ""text"": ""எலுமிச்சை"" }, { ""text"": ""பூச்சி"" },
          { ""text"": ""குச்சி"" }, { ""text"": ""நீச்சல்"" }
        ]
      },
      {
        ""spotlightLetter"": ""ஞ்"",
        ""words"": [
          { ""text"": ""இஞ்சி"" }, { ""text"": ""ஊஞ்சல்"" }, { ""text"": ""மஞ்சள்"" },
          { ""text"": ""குஞ்சு"" }, { ""text"": ""பஞ்சு"" }
        ]
      }
    ]"
}

// ...




      );
    }
  }
}