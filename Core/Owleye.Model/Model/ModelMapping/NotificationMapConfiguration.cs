using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Owleye.Model.Model.ModelMapping
{
    public class NotificationMapConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.EmailAddress).HasMaxLength(200);
            builder.Property(t => t.PhoneNumber).HasMaxLength(20);
        }
    }
}
