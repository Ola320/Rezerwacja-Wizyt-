using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppSession> Sessions { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.Entity<DoctorSpecialization>()
                .HasKey(ds => new { ds.DoctorId, ds.SpecializationId });

            modelBuilder.Entity<DoctorSpecialization>()
                .HasOne(ds => ds.Doctor)
                .WithMany(d => d.DoctorSpecializations)   
                .HasForeignKey(ds => ds.DoctorId);

            modelBuilder.Entity<DoctorSpecialization>()
                .HasOne(ds => ds.Specialization)
                .WithMany(s => s.DoctorSpecializations)
                .HasForeignKey(ds => ds.SpecializationId);

       
            modelBuilder.Entity<Slot>()
                .HasOne(s => s.Doctor)
                .WithMany(d => d.Slots)
                .HasForeignKey(s => s.DoctorId);

    
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Patient)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PatientId);

            
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Slot)
                .WithOne(s => s.Booking)
                .HasForeignKey<Booking>(b => b.SlotId);

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                 .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Pesel)
                .IsUnique();

            modelBuilder.Entity<AppSession>()
                .HasIndex(s => s.Token)
                .IsUnique();

            modelBuilder.Entity<AppSession>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { DoctorId = 1, Name = "Jan", Surname = "Kowalski", Email = "jan@clinic.com", Phone = "500111222" },
                new Doctor { DoctorId = 2, Name = "Anna", Surname = "Nowak", Email = "anna@clinic.com", Phone = "500333444" },
                new Doctor { DoctorId = 3, Name = "Marek", Surname = "Lis", Email = "marek@clinic.com", Phone = "500555666" },
                new Doctor { DoctorId = 4, Name = "Ewa", Surname = "Zawisza", Email = "ewa@clinic.com", Phone = "500777888" },
                new Doctor { DoctorId = 5, Name = "Piotr", Surname = "Zieliński", Email = "piotr@clinic.com", Phone = "501222333" },
                new Doctor { DoctorId = 6, Name = "Katarzyna", Surname = "Mazur", Email = "katarzyna@clinic.com", Phone = "502333444" },
                new Doctor { DoctorId = 7, Name = "Tomasz", Surname = "Wójcik", Email = "tomasz@clinic.com", Phone = "503444555" },
                new Doctor { DoctorId = 8, Name = "Joanna", Surname = "Piotrowska", Email = "joanna@clinic.com", Phone = "504555666" },
                new Doctor { DoctorId = 9, Name = "Adam", Surname = "Konieczny", Email = "adam@clinic.com", Phone = "505666777" },
                new Doctor { DoctorId = 10, Name = "Magdalena", Surname = "Lewandowska", Email = "magda@clinic.com", Phone = "506777888" }
            );

            modelBuilder.Entity<Specialization>().HasData(
                new Specialization { SpecializationId = 1, SpecializationName = "Kardiolog" },
                new Specialization { SpecializationId = 2, SpecializationName = "Dermatolog" },
                new Specialization { SpecializationId = 6, SpecializationName = "Urolog" },
                new Specialization { SpecializationId = 7, SpecializationName = "Fizjoterapeuta" },
                new Specialization { SpecializationId = 3, SpecializationName = "Laryngolog" },
                new Specialization { SpecializationId = 4, SpecializationName = "Okulista" },
                new Specialization { SpecializationId = 5, SpecializationName = "Neurolog" }
            );

            modelBuilder.Entity<DoctorSpecialization>().HasData(
                new DoctorSpecialization { DoctorId = 1, SpecializationId = 1 },
                new DoctorSpecialization { DoctorId = 1, SpecializationId = 6 },
                new DoctorSpecialization { DoctorId = 2, SpecializationId = 2 },
                new DoctorSpecialization { DoctorId = 1, SpecializationId = 7 }, 
                new DoctorSpecialization { DoctorId = 2, SpecializationId = 6 }, 
                new DoctorSpecialization { DoctorId = 3, SpecializationId = 1 }, 
                new DoctorSpecialization { DoctorId = 4, SpecializationId = 3 }, 
                new DoctorSpecialization { DoctorId = 5, SpecializationId = 4 }, 
                new DoctorSpecialization { DoctorId = 6, SpecializationId = 2 }, 
                new DoctorSpecialization { DoctorId = 7, SpecializationId = 5 }, 
                new DoctorSpecialization { DoctorId = 8, SpecializationId = 3 }, 
                new DoctorSpecialization { DoctorId = 9, SpecializationId = 4 }, 
                new DoctorSpecialization { DoctorId = 10, SpecializationId = 1 }
            );


            modelBuilder.Entity<Patient>().HasData(
                new Patient { PatientId = 1, Name = "Ola", Surname = "Olszewska", Email = "ola@ex.com", Phone = "501000999", Address = "Warszawa" },
                new Patient { PatientId = 2, Name = "Piotr", Surname = "Zieliński", Email = "piotr@ex.com", Phone = "501000888", Address = "Kraków" }
            );

          
            //modelBuilder.Entity<Slot>().HasData(
            //new Slot { SlotId = 1, DoctorId = 1, StartAt = new DateTime(2025, 11, 10, 9, 0, 0, DateTimeKind.Utc), EndAt = new DateTime(2025, 11, 10, 9, 30, 0, DateTimeKind.Utc) },
            //new Slot { SlotId = 2, DoctorId = 1, StartAt = new DateTime(2025, 11, 10, 9, 30, 0, DateTimeKind.Utc), EndAt = new DateTime(2025, 11, 10, 10, 0, 0, DateTimeKind.Utc) },
            //new Slot { SlotId = 3, DoctorId = 2, StartAt = new DateTime(2025, 11, 10, 9, 0, 0, DateTimeKind.Utc), EndAt = new DateTime(2025, 11, 10, 9, 30, 0, DateTimeKind.Utc) }
            //);
            modelBuilder.Entity<Slot>()
                .HasIndex(s => new { s.DoctorId, s.StartAt })
                .IsUnique();
        }
    }
}