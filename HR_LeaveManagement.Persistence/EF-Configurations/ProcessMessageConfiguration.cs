using HR_LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_LeaveManagement.Persistence.EF_Configurations
{
    public class ProcessMessageConfiguration
        : IEntityTypeConfiguration<ProcessedMessage>
    {
        public void Configure(EntityTypeBuilder<ProcessedMessage> builder)
        {
            builder.ToTable("ProcessedMessages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.MessageId)
                .IsRequired();

            builder.Property(x => x.ProcessedAt)
                .IsRequired();

            builder.HasIndex(x => x.MessageId)
                .IsUnique();
        }
    }
}
