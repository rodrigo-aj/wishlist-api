using dockerapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dockerapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public UsuarioController(ApiDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os usuários cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable Get()
        {
            return (from u in _context.Usuario
                    select new
                    {
                        u.id,
                        u.nome,
                        u.email
                    });
        }

        /// <summary>
        /// Retorna um usuário a partir do ID
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IQueryable GetById(long id)
        {
            return (from u in _context.Usuario
                    where u.id == id
                    select new
                    {
                        u.id,
                        u.nome,
                        u.email
                    });
        }

        /// <summary>
        /// Cadastra um usuário
        /// </summary>
        /// <param name="usuario">Informações do usuário</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.Usuario.Add(usuario);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = usuario.id }, usuario);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }
    }
}

