using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TechTest_KawanLama.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TechTest_KawanLama.Data
{
    public class DBContext : DbContext
    {
        private readonly IWebHostEnvironment _env;

        public DBContext (DbContextOptions<DBContext> options, IWebHostEnvironment env)
            : base(options)
        {
            _env = env;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SequenceNumber>().HasData(SeedSequenceNumberData());
        }

        private List<SequenceNumber> SeedSequenceNumberData()
        {
            var jsonFilePath = $"{_env.WebRootPath}/data/";

            var sequenceNumbers = new List<SequenceNumber>();
            using (StreamReader r = new(jsonFilePath + @"SequenceNumber.json"))
            {
                string json = r.ReadToEnd();
                sequenceNumbers = JsonConvert.DeserializeObject<List<SequenceNumber>>(json);
            }
            return sequenceNumbers;
        }

        public DbSet<TechTest_KawanLama.Models.User> User { get; set; } = default!;
        public DbSet<TechTest_KawanLama.Models.ToDo> ToDos { get; set; } = default!;
        public DbSet<TechTest_KawanLama.Models.SequenceNumber> SequenceNumbers { get; set; } = default!;

    }
}
