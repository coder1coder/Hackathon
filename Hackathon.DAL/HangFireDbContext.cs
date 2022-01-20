using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL;

public class HangFireDbContext: DbContext
{
    public HangFireDbContext(DbContextOptions<HangFireDbContext> options) : base(options)
    {
    }
}