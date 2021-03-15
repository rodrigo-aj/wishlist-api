using dockerapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dockerapi.Maps
{
    public class UsuarioMap
    {
        public UsuarioMap(EntityTypeBuilder<Usuario> entityBuilder)
        {
            entityBuilder.HasKey(x => x.id);
            entityBuilder.ToTable("usuario");

            //entityBuilder.Property(x => x.id).HasColumnName("id");
            //entityBuilder.Property(x => x.nome).HasColumnName("nome");
            //entityBuilder.Property(x => x.email).HasColumnName("email");
        }
    }
}
