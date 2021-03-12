using dockerapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dockerapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public WishListController(ApiDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as wishList cadastradas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Get()
        {
            var query = (from n in _context.WishList
                         join c in _context.Usuario on n.usuario.id equals c.id


                         select new
                         {
                             n.id,
                             n.tituloProduto,
                             n.descProduto,
                             n.comprouOuGanhouItem,
                             c.nome,
                             c.email
                         });

            return query.ToList();
        }

        /// <summary>
        /// Retorna uma wishList a partir do ID
        /// </summary>
        /// <param name="id">ID da wishList</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public object GetById(long id)
        {
            var query = (from n in _context.WishList
                         join c in _context.Usuario on n.usuario.id equals c.id
                         where n.id == id

                         select new
                         {
                             n.id,
                             n.tituloProduto,
                             n.descProduto,
                             n.comprouOuGanhouItem,
                             c.nome,
                             c.email
                         });

            return query;
        }

        /// <summary>
        /// Cadastra uma wishList
        /// </summary>
        /// <param name="wishlist">WishList</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WishList wishlist)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                Usuario user = _context.Usuario.Where(x => x.id == wishlist.usuario.id).First();
                wishlist.usuario = user;
                _context.WishList.Add(wishlist);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = wishlist.id }, wishlist);

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Marca um item como ganho/comprado
        /// </summary>
        /// <param name="id">ID da wishList</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> PostGanhouItemWishList(long id)
        {
            WishList lista = _context.WishList.Where(y => y.id == id).First();

            if (lista == null)
            {
                return StatusCode(400);
            }

            lista.comprouOuGanhouItem = true;
            _context.Entry(lista).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return StatusCode(200);
        }

        /// <summary>
        /// Retorna um item de randomicamente
        /// </summary>
        /// <returns></returns>
        [HttpGet("random")]
        public object GetRadom()
        {
            Random rnd = new Random();

            var query = (from n in _context.WishList
                         join c in _context.Usuario on n.usuario.id equals c.id
                         orderby rnd.Next()
                         //where n.id == id

                         select new
                         {
                             n.id,
                             n.tituloProduto,
                             n.descProduto,
                             n.comprouOuGanhouItem,
                             c.nome,
                             c.email
                         });

            return query.First();
        }
    }
}