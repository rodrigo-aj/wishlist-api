using dockerapi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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

        private static IHostingEnvironment _environment;

        public WishListController(ApiDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable Get()
        {
            return (from n in _context.WishList
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

        }

        /// <summary>
        /// Retorna um item a partir do ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IQueryable GetById(long id)
        {
            return (from n in _context.WishList
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

            return (from n in _context.WishList
                    join c in _context.Usuario on n.usuario.id equals c.id
                    orderby rnd.Next()

                    select new
                    {
                        n.id,
                        n.tituloProduto,
                        n.descProduto,
                        n.comprouOuGanhouItem,
                        c.nome,
                        c.email
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Faz o upload de uma imagem para o item correspondente
        /// </summary>
        /// <param name="arquivo">Imagem</param>
        /// <param name="id">ID do item</param>
        /// <returns></returns>
        [HttpPost("{id}/uploadFotoItem")]
        public async Task<IActionResult> PostUploadFoto([FromBody] IFormFile arquivo, long id)
        {
            try
            {
                string ext = Path.GetExtension(arquivo.FileName);

                if (!Directory.Exists(_environment.WebRootPath + "\\imagens\\"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\imagens\\");
                }

                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\imagens\\" + arquivo.FileName))
                {
                    await arquivo.CopyToAsync(filestream);

                    filestream.Flush();
                }

                return StatusCode(200);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }
    }
}