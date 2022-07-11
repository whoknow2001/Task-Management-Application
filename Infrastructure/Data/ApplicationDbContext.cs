﻿using Domain.Entities.Projects;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Histories;
using Domain.Entities.Tasks;
using Domain.Entities.Labels;
using Domain.Entities.ListTasks;
using MediatR;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IMediator _mediator;

        public ApplicationDbContext(IMediator mediator)
        {
            _mediator = mediator;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ListTask> ListTasks { get; set; }

        public DbSet<Domain.Entities.Tasks.Task> Tasks { get; set; }

        public DbSet<Todo> Todos { get; set; }

        public DbSet<ProjectMember> ProjectMembers { get; set; }

        public DbSet<TaskMember> TaskMembers { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Label> Labels { get; set; }

        public DbSet<History> Histories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Server=VT-PGH;Database=TaskManagementDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<Project>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<ListTask>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<Domain.Entities.Tasks.Task>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<ProjectMember>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<TaskMember>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<Label>().HasQueryFilter(p => !p.IsDelete);
        }

        public async System.Threading.Tasks.Task SaveEntitiesAsync()
        {
            await SaveChangesAsync();
            await _mediator.DispatchDomainEventsAsync(this);
        }
    }
}