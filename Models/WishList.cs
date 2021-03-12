using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dockerapi.Models
{
    public class WishList
    {
        public int id { get; set; }

        [ForeignKey("id_usuario")]
        public Usuario usuario { get; set; }

        public string tituloProduto { get; set; }

        public string descProduto { get; set; }

        public string linkProduto { get; set; }

        public string fotoProduto { get; set; }

        public bool comprouOuGanhouItem { get; set; }

        public WishList()
        {
            comprouOuGanhouItem = false;
        }

    }
}
