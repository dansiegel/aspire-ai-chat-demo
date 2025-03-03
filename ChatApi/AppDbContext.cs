using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class SampleDbContext(DbContextOptions<SampleDbContext> options) : DbContext(options)
{
    public required DbSet<SomeModel> SomeModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<SomeModel>(entity =>
        {
            entity.HasData(
                new SomeModel { SomeProperty = "Some value" },
                new SomeModel { SomeProperty = "Another value" },
                new SomeModel { SomeProperty = "Yet another value" }
            );
        });
    }
}

public class SomeModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string SomeProperty { get; set; }
}

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public required DbSet<Conversation> Conversations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.Entity<Conversation>()
            .HasPartitionKey(c => c.Id)
            .ToContainer("conversations");
    }
}

public class ConversationChatMessage
{
    public required Guid Id { get; set; }
    public required string Role { get; set; }
    public required string Text { get; set; }
}

public class Conversation
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required List<ConversationChatMessage> Messages { get; set; } = [];
}
