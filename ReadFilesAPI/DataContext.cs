using Microsoft.EntityFrameworkCore;
using ReadFilesAPI.Models;

namespace ReadFilesAPI
{
    public class DataContext : DbContext

    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<FileDTO> Files { get; set; }
    }
}
