using dockerapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dockerapi.Maps
{
    public class WishListMap
    {
        public WishListMap(EntityTypeBuilder<WishList> entityBuilder)
        {
            entityBuilder.HasKey(x => x.id);
            entityBuilder.ToTable("wishlist");

            entityBuilder.Property(x => x.id).HasColumnName("id");
            entityBuilder.Property(x => x.tituloProduto).HasColumnName("titulo_produto");
            entityBuilder.Property(x => x.descProduto).HasColumnName("desc_produto");
            entityBuilder.Property(x => x.linkProduto).HasColumnName("link_produto");
            entityBuilder.Property(x => x.fotoProduto).HasColumnName("foto_produto");
            entityBuilder.Property(x => x.comprouOuGanhouItem).HasColumnName("comprou_ganhou_item");

        }
    }
}

