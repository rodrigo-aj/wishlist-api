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
            var lista = (from n in context.WishList
                         join c in context.Usuario on n.usuario.id equals c.id
                         where n.id == id
                         select new WishList
                         {
                             id = n.id,
                             tituloProduto = n.tituloProduto,
                             descProduto = n.descProduto,
                             comprouOuGanhouItem = n.comprouOuGanhouItem,
                             usuario = n.usuario
                         }).AsNoTracking().FirstOrDefault();

            context.Entry(lista).CurrentValues.SetValues(lista);

            return lista;
        }

        public WishList GetRandom()
        {
            return (from n in context.WishList
                    join c in context.Usuario on n.usuario.id equals c.id
                    orderby new Random().Next()

                    select new WishList
                    {
                        id = n.id,
                        tituloProduto = n.tituloProduto,
                        descProduto = n.descProduto,
                        comprouOuGanhouItem = n.comprouOuGanhouItem,
                        usuario = n.usuario
                    }).FirstOrDefault();
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

            this.alterarRegistro(lista);

            return lista;
        }

        public void alterarRegistro(WishList lista)
        {
            context.Entry(lista).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
