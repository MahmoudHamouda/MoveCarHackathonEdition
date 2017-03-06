using System.Data.Entity;

namespace MoveCarHackathonEdition.MySqlNS
{
    public class MySQLdb : DbContext
    {
        public MySQLdb()
            : base("name=MySQLdb")
        {
        }

        public virtual DbSet<car_forgot_password> car_forgot_password { get; set; }
        public virtual DbSet<car_notification> car_notification { get; set; }
        public virtual DbSet<car_request> car_request { get; set; }
        public virtual DbSet<car_user> car_user { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<car_forgot_password>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<car_notification>()
                .Property(e => e.MOBILE_ID)
                .IsUnicode(false);

            modelBuilder.Entity<car_notification>()
                .Property(e => e.NOTIFICATION_MSG)
                .IsUnicode(false);

            modelBuilder.Entity<car_request>()
                .Property(e => e.PLATE_NUMBER)
                .IsUnicode(false);

            modelBuilder.Entity<car_request>()
                .Property(e => e.STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<car_request>()
                .Property(e => e.CAR_USERNAME)
                .IsUnicode(false);

            modelBuilder.Entity<car_request>()
                .HasMany(e => e.car_notification)
                .WithOptional(e => e.car_request)
                .HasForeignKey(e => e.REQUEST_ID);

            modelBuilder.Entity<car_user>()
                .Property(e => e.CAR_USERNAME)
                .IsUnicode(false);

            modelBuilder.Entity<car_user>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<car_user>()
                .Property(e => e.MOBILE_ID)
                .IsUnicode(false);
        }
    }
}