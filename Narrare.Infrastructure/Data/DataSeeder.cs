using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Narrare.Domain.Entities;
using Narrare.Domain.Enums;

namespace Narrare.Infrastructure.Data;
// Seed datan är gjord av ai
public class DataSeeder
{
    private readonly NarrareDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public DataSeeder(NarrareDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!await _context.Database.CanConnectAsync())
        {
            throw new InvalidOperationException(
                "Kan inte ansluta till NarrareDb.");
        }

        var users = new List<User>
        {
            CreateUser("AdminMachAdmin", "AdminMachAdmin", 34,
                "uddevalla", "Backenddew", UserRole.Admin),

            CreateUser("VisningVilma", "VisningVilma", 34,
                "uddevalla", "dew", UserRole.User),

            CreateUser("AnnaAndersson", "AnnaAndersson", 29,
                "Uddevalla", "Sjuksköterska", UserRole.User),
            CreateUser("ErikEriksson", "ErikEriksson", 42,
                "Trollhättan", "Snickare", UserRole.User),
            CreateUser("SaraSvensson", "SaraSvensson", 35,
                "Vänersborg", "Lärare", UserRole.User),
            CreateUser("JohanJohansson", "JohanJohansson", 31,
                "Göteborg", "Elektriker", UserRole.User),
            CreateUser("EmmaKarlsson", "EmmaKarlsson", 27,
                "Uddevalla", "Ekonom", UserRole.User),
            CreateUser("OscarNilsson", "OscarNilsson", 46,
                "Lysekil", "Kock", UserRole.User),
            CreateUser("MajaLarsson", "MajaLarsson", 33,
                "Stenungsund", "Förskollärare", UserRole.User),
            CreateUser("LukasOlsson", "LukasOlsson", 38,
                "Trollhättan", "Mekaniker", UserRole.User),
            CreateUser("ElsaPersson", "ElsaPersson", 25,
                "Göteborg", "Grafisk designer", UserRole.User),
            CreateUser("WilliamKarlsson", "WilliamKarlsson", 51,
                "Uddevalla", "Fastighetsskötare", UserRole.User),
            CreateUser("AliceEriksson", "AliceEriksson", 30,
                "Kungälv", "Projektledare", UserRole.User),
            CreateUser("NoahGustafsson", "NoahGustafsson", 44,
                "Ljungskile", "Rörmokare", UserRole.User),
            CreateUser("FrejaPettersson", "FrejaPettersson", 36,
                "Vänersborg", "Undersköterska", UserRole.User),
            CreateUser("HugoJonsson", "HugoJonsson", 28,
                "Göteborg", "IT-tekniker", UserRole.User),
            CreateUser("EllaLindberg", "EllaLindberg", 40,
                "Uddevalla", "Butikschef", UserRole.User),
            CreateUser("AdamJakobsson", "AdamJakobsson", 32,
                "Orust", "Målare", UserRole.User),
            CreateUser("OliviaMagnusson", "OliviaMagnusson", 47,
                "Stenungsund", "Tandsköterska", UserRole.User),
            CreateUser("LeoOlofsson", "LeoOlofsson", 26,
                "Trollhättan", "Student", UserRole.User),
            CreateUser("AstridLindgren", "AstridLindgren", 55,
                "Lysekil", "Trädgårdsmästare", UserRole.User),
            CreateUser("ViktorBerg", "ViktorBerg", 39,
                "Göteborg", "Ingenjör", UserRole.User),
            CreateUser("IdaAxelsson", "IdaAxelsson", 34,
                "Uddevalla", "Administratör", UserRole.User),
            CreateUser("AlbinBengtsson", "AlbinBengtsson", 43,
                "Munkedal", "Busschaufför", UserRole.User),
            CreateUser("NoraLundberg", "NoraLundberg", 29,
                "Kungälv", "Frisör", UserRole.User),
            CreateUser("FilipLind", "FilipLind", 37,
                "Vänersborg", "Plåtslagare", UserRole.User),
            CreateUser("WilmaBjörk", "WilmaBjörk", 41,
                "Uddevalla", "Inredare", UserRole.User)
        };
        
        var existingUsernames = await _context.Users
            .Select(user => user.Username)
            .ToListAsync();

        var newUsers = users
            .Where(user => !existingUsernames.Contains(user.Username))
            .ToList();


        if (newUsers.Count > 0)
        {
            await _context.Users.AddRangeAsync(newUsers);
            await _context.SaveChangesAsync();
        }


        await SeedPostsAsync();
        await SeedCommentsAsync();
        await SeedLikesAsync();
    }

    

    private async Task SeedPostsAsync()
    {
        var users = await _context.Users
            .OrderBy(user => user.Id)
            .ToListAsync();
        
            var categories = await _context.Categories
    .OrderBy(category => category.Id)
    .ToListAsync();

        if (users.Count == 0)
        {
            throw new InvalidOperationException(
                "Inga användare finns för testinläggen.");
        }

        if (categories.Count == 5)
        {
            throw new InvalidOperationException(
                "Alla fem kategorier måste finnas i databasen.");
        }

        var postTitles = new Dictionary<int, string[]>
        {
            
            [7] = new[]
            {
            "Hur skapar man ett mysigt vardagsrum?",
            "Välja rätt soffa för familjen",
            "Smart möblering i ett litet vardagsrum",
            "Vilken färg passar på vardagsrumsväggarna?",
            "Belysning för både vardag och fest",
            "Matta eller trägolv i vardagsrummet?",
            "Bokhyllor som blir en del av inredningen",
            "Skapa mer förvaring i vardagsrummet",
            "Gardiner som förändrar hela rummet",
            "Inreda vardagsrummet i skandinavisk stil",
            "TV på väggen eller på en bänk?",
            "Hur får man ett rum att kännas större?",
            "Kombinera gamla möbler med modern design",
            "Inredning som fungerar med husdjur",
            "Skapa en trivsam läshörna",
            "Vilket soffbord passar bäst?",
            "Förnya vardagsrummet utan renovering",
            "Dela era bästa vardagsrumstips"
        },
            [3] = new[]
            {
            "Planera en köksrenovering från grunden",
            "Vilken bänkskiva håller bäst?",
            "Smarta lösningar för små kök",
            "Öppen planlösning eller separat kök?",
            "Förvaring som gör köket mer praktiskt",
            "Är en köksö värd investeringen?",
            "Belysning ovanför köksbänken",
            "Renovera köket med begränsad budget",
            "Vilket golv passar bäst i köket?",
            "Organisera skafferiet på ett smart sätt",
            "Var ska diskmaskinen placeras?",
            "Välja färg på köksluckorna",
            "Hur mycket arbetsyta behövs?",
            "Köksfläkt med eller utan frånluft?",
            "Förnya köket utan att byta allt",
            "Praktiska hörnskåp i köket",
            "Planera eluttag vid köksrenovering",
            "Dela era bästa kökstips"
        },
            [4] = new[]
            {
            "Skapa ett lugnt och harmoniskt sovrum",
            "Vilken madrass passar bäst?",
            "Förvaring i ett litet sovrum",
            "Mörkläggningsgardiner som fungerar",
            "Belysning för en behaglig kväll",
            "Måla sovrummet i en mörk färg",
            "Var ska sängen stå?",
            "Garderob med skjutdörrar eller vanliga dörrar?",
            "Göra sovrummet mer ljudisolerat",
            "Sängbord med smart förvaring",
            "Välja rätt storlek på sängen",
            "Ventilation och temperatur i sovrummet",
            "Textilier som gör sovrummet ombonat",
            "Förnya sovrummet med små medel",
            "Förvaring under sängen",
            "Tapet eller färg bakom sängen?",
            "Göra gästrummet mer användbart",
            "Dela era bästa sovrumstips"
        },
            [5] = new[]
            {
            "Planera en badrumsrenovering",
            "Kakel eller våtrumsmatta?",
            "Förvaring i ett litet badrum",
            "Bra belysning vid badrumsspegeln",
            "Välja kommod till badrummet",
            "Duschhörna eller badkar?",
            "Förebygga fuktproblem i badrummet",
            "Praktiska hyllor i duschen",
            "Är golvvärme värt kostnaden?",
            "Välja fogfärg till kaklet",
            "Ventilation i äldre badrum",
            "Förnya badrummet utan totalrenovering",
            "Tvättmaskin i ett litet badrum",
            "Smart förvaring för handdukar",
            "Välja blandare till handfatet",
            "Badrum med begränsad golvyta",
            "Göra badrummet lättare att städa",
            "Dela era bästa badrumstips"
        },
            [6] = new[]
            {
            "Planera trädgården inför våren",
            "Lättskötta växter för nybörjare",
            "Skapa en trivsam uteplats",
            "Odla grönsaker i pallkrage",
            "Vilka perenner blommar länge?",
            "Förbättra jorden i trädgården",
            "Plantera en häck som insynsskydd",
            "Bygga en enkel kompost",
            "Vattning under torra sommardagar",
            "Skapa en rabatt med olika höjder",
            "Belysning längs trädgårdsgången",
            "Odla kryddor nära köket",
            "Planera en liten köksträdgård",
            "Ta hand om gräsmattan på våren",
            "Välja material till uteplatsen",
            "Skapa en trädgård för pollinatörer",
            "Förvara trädgårdsredskap smart",
            "Dela era bästa trädgårdstips"
        }
        };

        var existingTitles = await _context.Posts
            .Select(post => post.Title)
            .ToListAsync();

        var existingTitleSet = new HashSet<string>(
            existingTitles,
            StringComparer.Ordinal);

        var newPosts = new List<Post>();
        var postNumber = 0;

        foreach (var category in categories)
        {
            if (!postTitles.TryGetValue(category.Id, out var titles))
            {
                continue;
            }

            foreach (var title in titles)
            {
                if (!existingTitleSet.Add(title))
                {
                    continue;
                }
                
                var user = users[postNumber % users.Count];

                newPosts.Add(new Post
                {
                    Title = title,
                    Content =
                        $"Hej! Jag vill diskutera {title.ToLowerInvariant()} " +
                        "och höra hur andra har löst det hemma. " +
                        "Vilka erfarenheter, tips och idéer kan ni dela med er av?",

                    CreatedAt = DateTime.UtcNow.AddDays(
                        -(postNumber % 180)),

                    UpdatedAt = null,
                    UserId = user.Id,
                    CategoryId = category.Id,
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedAt = null
                });

                postNumber++;
            }
        }

        if (newPosts.Count > 0)
        {
            await _context.Posts.AddRangeAsync(newPosts);
            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedCommentsAsync()
    {
        var users = await _context.Users
            .OrderBy(user => user.Id)
            .ToListAsync();

        var posts = await _context.Posts
            .OrderBy(post => post.Id)
            .ToListAsync();

        if (users.Count == 0 || posts.Count == 0)
        {
            throw new InvalidOperationException(
                "Användare och inlägg krävs för att skapa kommentarer.");
        }

        var commentTexts = new Dictionary<int, string[]>
        {
            [7] = new[]
            {
            "Vi gjorde något liknande hemma och det blev riktigt bra.",
            "Jag skulle välja en lösning som ger mer förvaring.",
            "Bra idé! Det gör stor skillnad för helhetsintrycket.",
            "Vi valde en ljusare färg och rummet kändes större.",
            "Tänk också på belysningen när ni planerar.",
            "Det kan vara värt att mäta ordentligt innan ni börjar."
        },
            [3] = new[]
            {
            "Vi renoverade vårt kök förra året och planeringen var viktigast.",
            "Jag rekommenderar att jämföra flera olika material.",
            "Bra arbetsbelysning gör verkligen stor skillnad.",
            "Vi valde extra förvaring och är väldigt nöjda.",
            "Glöm inte att planera eluttagen från början.",
            "Det är klokt att ta in offerter innan arbetet börjar."
        },
            [4] = new[]
            {
            "Mörkläggningsgardiner gjorde stor skillnad hemma hos oss.",
            "Vi valde förvaring under sängen för att spara plats.",
            "En varm belysning gör rummet mer ombonat.",
            "Jag tycker att lugna färger fungerar bäst här.",
            "Mät rummet noga innan ni väljer möbler.",
            "Vi fick plats med mer genom att använda väggarna."
        },
            [5] = new[]
            {
            "Kontrollera alltid att materialet passar för våtrum.",
            "Vi valde en kommod med lådor och fick bättre förvaring.",
            "Bra ventilation är viktigt för att undvika fuktproblem.",
            "Det lönar sig att planera placeringen noggrant.",
            "Vi valde lättskötta material och är nöjda.",
            "Ta gärna in hjälp med tätskikt och våtrumsarbeten."
        },
            [6] = new[]
            {
            "Vi provade detta i trädgården och fick ett fint resultat.",
            "Välj gärna växter som passar jordmånen där du bor.",
            "Jag skulle planera bevattningen redan från början.",
            "Perenner gör det lättare att få en återkommande blomning.",
            "Vi använder pallkragar och tycker att de är praktiska.",
            "Tänk på hur mycket sol platsen får under dagen."
        }
        };

        var existingComments = await _context.Comments
            .Select(comment => new
            {
                comment.UserId,
                comment.PostId,
                comment.Content
            })
            .ToListAsync();

        var existingKeys = new HashSet<string>(
            existingComments.Select(comment =>
                $"{comment.UserId}|{comment.PostId}|{comment.Content}"));

        var newComments = new List<Comment>();

        for (var i = 0; i < 200; i++)
        {
            var post = posts[i % posts.Count];
            var user = users[(i * 7 + 1) % users.Count];

            if (!commentTexts.TryGetValue(
                    post.CategoryId, out var texts))
            {
                throw new InvalidOperationException(
                    $"Saknar kommentarstexter för kategori {post.CategoryId}.");
            }

            var content = texts[(i / posts.Count) % texts.Length];

            // Kontrollera både befintliga och nya kommentarer.
            var key = $"{user.Id}|{post.Id}|{content}";

            if (!existingKeys.Add(key))
            {
                continue;
            }

            newComments.Add(new Comment
            {
                Content = content,
                CreatedAt = post.CreatedAt.AddHours(i + 1),
                UpdatedAt = null,
                UserId = user.Id,
                PostId = post.Id,
                IsDeleted = false,
                DeletedByUserId = null,
                DeletedAt = null
            });
        }

        if (newComments.Count > 0)
        {
            await _context.Comments.AddRangeAsync(newComments);
            await _context.SaveChangesAsync();
        }
    }



    private async Task SeedLikesAsync()
    {
        // Seed:a bara om tabellen är tom.
        // Befintliga gilla-markeringar ska inte skrivas över
        // vid varje omstart av API:t.
        if (await _context.Likes.AnyAsync())
        {
            return;
        }

        var users = await _context.Users
            .OrderBy(user => user.Id)
            .ToListAsync();

        var posts = await _context.Posts
            .Where(post => !post.IsDeleted)
            .ToListAsync();

        var comments = await _context.Comments
            .Where(comment => !comment.IsDeleted)
            .ToListAsync();

        if (users.Count == 0 || posts.Count == 0 || comments.Count == 0)
        {
            throw new InvalidOperationException(
                "Användare, inlägg och kommentarer krävs för gilla-markeringar.");
        }

        const int targetPostLikes = 300;
        const int targetCommentLikes = 150;

        var postCandidates = (
            from post in posts
            from user in users
            select new { post, user }
        )
        .OrderBy(_ => Random.Shared.Next())
        .Take(targetPostLikes)
        .ToList();

        var commentCandidates = (
            from comment in comments
            from user in users
            select new { comment, user }
        )
        .OrderBy(_ => Random.Shared.Next())
        .Take(targetCommentLikes)
        .ToList();

        if (postCandidates.Count < targetPostLikes ||
            commentCandidates.Count < targetCommentLikes)
        {
            throw new InvalidOperationException(
                "Det finns inte tillräckligt många unika kombinationer.");
        }

        var newLikes = new List<Like>();

        foreach (var candidate in postCandidates)
        {
            newLikes.Add(new Like
            {
                UserId = candidate.user.Id,
                PostId = candidate.post.Id,
                CommentId = null,
                CreatedAt = DateTime.UtcNow.AddMinutes(
                    -Random.Shared.Next(0, 60 * 24 * 30))
            });
        }

        foreach (var candidate in commentCandidates)
        {
            newLikes.Add(new Like
            {
                UserId = candidate.user.Id,
                PostId = null,
                CommentId = candidate.comment.Id,
                CreatedAt = DateTime.UtcNow.AddMinutes(
                    -Random.Shared.Next(0, 60 * 24 * 30))
            });
        }

        await _context.Likes.AddRangeAsync(newLikes);
        await _context.SaveChangesAsync();
    }




    private User CreateUser(
        string username,
        string password,
        int age,
        string location,
        string occupation,
        UserRole role)
    {
        var user = new User
        {
            Username = username,
            PasswordHash = string.Empty,
            Age = age,
            Location = location,
            Occupation = occupation,
            ProfileImageUrl = null,
            IsProfileComplete = true,
            ShowAge = true,
            ShowLocation = true,
            ShowOccupation = true,
            CreatedAt = DateTime.UtcNow,
            IsBlocked = false,
            Role = role
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        return user;
    }
}
