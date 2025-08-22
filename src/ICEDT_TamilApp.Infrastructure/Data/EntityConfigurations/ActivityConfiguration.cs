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

      // === Activity for Preschool, Lesson 2: My Family Flashcards ===
    new Activity
    {
      ActivityId = 1, // Ensure this ID is unique
      LessonId = 1,    // Belongs to "மழலையர் நிலை - பாடம் 02"
      Title = "உடல் உறுப்புகள் (நான்) : Flashcards",
      SequenceOrder = 1,
      ActivityTypeId = 2,  // Reusing the general recognition type ID
      MainActivityId = 3,  // "Learning"
                           // The ContentJson is an array of exercises (flashcards)
      ContentJson = @"[
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""அம்மா"",
        ""imageUrl"": ""malaiyar/lesson2/amma.jpg"",
        ""audioUrl"": ""malaiyar/lesson2/amma_sound.mp3""
      },
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""அப்பா"",
        ""imageUrl"": ""malaiyar/lesson2/appa.jpg"",
        ""audioUrl"": ""https://your-bucket.s.region.amazonaws.com/malaiyar/lesson2/appa_sound.mp3""
      },
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""அக்கா"",
        ""imageUrl"": ""malaiyar/lesson2/akka.jpg"",
        ""audioUrl"": ""malaiyar/lesson2/akka_sound.mp3""
      },
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""அண்ணா"",
        ""imageUrl"": ""malaiyar/lesson2/anna.jpg"",
        ""audioUrl"": ""malaiyar/lesson2/anna_sound.mp3""
      },
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""தம்பி"",
        ""imageUrl"": ""malaiyar/lesson2/thambi.jpg"",
        ""audioUrl"": ""malaiyar/lesson2/thambi_sound.mp3""
      },
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""தங்கை"",
        ""imageUrl"": ""malaiyar/lesson2/thangai.jpg"",
        ""audioUrl"": ""malaiyar/lesson2/thangai_sound.mp3""
      }
    ]"
    },

new Activity   // : Vocabulary Spotlight
{
  ActivityId = 1, // Ensure this ID is unique
  LessonId = 1,   // Belongs to "மழலையர் நிலை - பாடம் 01"
  Title = "உயிர் எழுத்து சொற்கள் - அ",
  SequenceOrder = 5, // A new activity in this lesson
  ActivityTypeId = 19, // Our new, dedicated ID for this component
  MainActivityId = 3,  // "Learning"
  ContentJson = @"{
      ""title"": ""உயிர் எழுத்து"",
      ""spotlightLetter"": ""அ"",
      ""items"": [
        { ""text"": ""அம்மா"", ""imageUrl"": ""malaiyar/lesson1/amma.jpg"", ""audioUrl"": ""malaiyar/lesson1/amma_sound.mp3"" },
        { ""text"": ""அரிசி"", ""imageUrl"": ""malaiyar/lesson1/arisi.jpg"", ""audioUrl"": ""malaiyar/lesson1/arisi_sound.mp3"" },
        { ""text"": ""அன்னம்"", ""imageUrl"": ""malaiyar/lesson1/annam.jpg"", ""audioUrl"": ""malaiyar/lesson1/annam_sound.mp3"" },
        { ""text"": ""அடுப்பு"", ""imageUrl"": ""malaiyar/lesson1/aduppu.jpg"", ""audioUrl"": ""malaiyar/lesson1/aduppu_sound.mp3"" },
        { ""text"": ""அருவி"", ""imageUrl"": ""malaiyar/lesson1/aruvi.jpg"", ""audioUrl"": ""malaiyar/lesson1/aruvi_sound.mp3"" }
      ]
    }"
},


// === Activity for Preschool, Lesson 2: My Family Flashcards ===
new Activity
{
  ActivityId = 19, // Ensure this ID is unique
  LessonId = 2,    // Belongs to "மழலையர் நிலை - பாடம் 02"
  Title = "எனது குடும்பம் Flashcards",
  SequenceOrder = 1,
  ActivityTypeId = 2,  // Reusing the general recognition type ID
  MainActivityId = 3,  // "Learning"
                       // The ContentJson is an array of exercises (flashcards)
  ContentJson = @"[
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""அம்மா"",
        ""imageUrl"": ""malaiyar/lesson2/amma.jpg"",
        ""audioUrl"": ""malaiyar/lesson2/amma_sound.mp3""
      },
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""அப்பா"",
        ""imageUrl"": ""malaiyar/lesson2/appa.jpg"",
        ""audioUrl"": ""https://your-bucket.s.region.amazonaws.com/malaiyar/lesson2/appa_sound.mp3""
      },
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""அக்கா"",
        ""imageUrl"": ""malaiyar/lesson2/akka.jpg"",
        ""audioUrl"": ""malaiyar/lesson2/akka_sound.mp3""
      },
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""அண்ணா"",
        ""imageUrl"": ""malaiyar/lesson2/anna.jpg"",
        ""audioUrl"": ""malaiyar/lesson2/anna_sound.mp3""
      },
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""தம்பி"",
        ""imageUrl"": ""malaiyar/lesson2/thambi.jpg"",
        ""audioUrl"": ""malaiyar/lesson2/thambi_sound.mp3""
      },
      {
        ""title"": ""எனது குடும்பம் (My Family)"",
        ""word"": ""தங்கை"",
        ""imageUrl"": ""malaiyar/lesson2/thangai.jpg"",
        ""audioUrl"": ""malaiyar/lesson2/thangai_sound.mp3""
      }
    ]"
},



// === Activity for Kindergarten, Lesson 7:MediaSpot ===
new Activity
{
    ActivityId = 13, // Ensure this ID is unique
    LessonId = 17,   // Belongs to "சிறுவர் நிலை - பாடம் 07"
    Title = "மெய்யெழுத்துகள் படங்களுடன் (தொடர்ச்சி)",
    SequenceOrder = 1,
    ActivityTypeId = 2,  // The ID for Spotlight components
    MainActivityId = 3,  // "Learning"
    ContentJson = @"[
      {
        ""spotlightLetter"": ""ப்"",
        ""items"": [
          { ""text"": ""சீப்பு"", ""imageUrl"": ""siruvar/lesson7/seeppu.jpg"" },
          { ""text"": ""பப்பாசி"", ""imageUrl"": ""siruvar/lesson7/pappasi.jpg"" },
          { ""text"": ""கப்பல்"", ""imageUrl"": ""siruvar/lesson7/kappal.jpg"" }
        ]
      },
      {
        ""spotlightLetter"": ""ம்"",
        ""items"": [
          { ""text"": ""மரம்"", ""imageUrl"": ""siruvar/lesson7/maram.jpg"" },
          { ""text"": ""மாம்பழம்"", ""imageUrl"": ""siruvar/lesson7/maampazham.jpg"" },
          { ""text"": ""பாம்பு"", ""imageUrl"": ""siruvar/lesson7/paambu.jpg"" }
        ]
      },
      {
        ""spotlightLetter"": ""ய்"",
        ""items"": [
          { ""text"": ""நாய்"", ""imageUrl"": ""siruvar/lesson7/naai.jpg"" },
          { ""text"": ""தாய்"", ""imageUrl"": ""siruvar/lesson7/thaai.jpg"" },
          { ""text"": ""மாங்காய்"", ""imageUrl"": ""siruvar/lesson7/maangaai.jpg"" }
        ]
      },
      {
        ""spotlightLetter"": ""ர்"",
        ""items"": [
          { ""text"": ""ஏர்"", ""imageUrl"": ""siruvar/lesson7/aer.jpg"" },
          { ""text"": ""வேர்"", ""imageUrl"": ""siruvar/lesson7/vaer.jpg"" },
          { ""text"": ""ஆசிரியர்"", ""imageUrl"": ""siruvar/lesson7/aasiriyar.jpg"" }
        ]
      }
    ]"
}

// ...



      );
    }
  }
}