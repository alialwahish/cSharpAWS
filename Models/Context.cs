using Microsoft.EntityFrameworkCore;

namespace cSharpBelt{

public class MyContext : DbContext
{

    public MyContext(DbContextOptions<MyContext>options): base(options){}

    public DbSet<User> users {get;set;}

    public DbSet<Ideas> ideas {get;set;}

    public DbSet<Likes> likes {get;set;}
}



}