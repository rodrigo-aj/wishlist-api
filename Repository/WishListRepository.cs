using dockerapi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dockerapi.Repository
{
    public class WishListRepository
    {
        private readonly ApiDbContext context;

        public WishListRepository(ApiDbContext _context)
        {
            context = _context;
        }

        public IEnumerable<WishList> GetAll()
        {
            return (from n in context.WishList
                    join c in context.Usuario on n.usuario.id equals c.id

                    select new WishList
                    {
                        id = n.id,
                        tituloProduto = n.tituloProduto,
                        descProduto = n.descProduto,
                        comprouOuGanhouItem = n.comprouOuGanhouItem,
                        usuario = n.usuario
                    }).ToList();
        }

        public WishList GetById(long id)
        {
            var lista = context.WishList.First(w => w.id == id);

            context.Entry(lista).CurrentValues.SetValues(lista);

            return lista;
        }

        public WishList GetRandom()
        {
            return (from u in context.WishList
                    join c in context.Usuario on u.usuario.id equals c.id
                    orderby new Random().Next()
                    select u).First();
        }

        public WishList Save(WishList wishList)
        {
            wishList.usuario = context.Usuario.Where(x => x.id == wishList.usuario.id).First();
            context.WishList.Add(wishList);
            context.SaveChanges();

            return this.GetById(wishList.id);
        }

        public WishList marcouItemComoGanho(long id)
        {
            WishList lista = GetById(id);

            if (lista.comprouOuGanhouItem)
            {
                return null;
            }

            lista.comprouOuGanhouItem = true;
            context.Entry(lista).State = EntityState.Modified;
            context.SaveChanges();

            return lista;
        }
    }
}
