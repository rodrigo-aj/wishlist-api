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
        public object Get()
        {
            return _context.Usuario.Select((x) => new
            {
                id = x.id,
                nome = x.nome,
                email = x.email
            }).ToList();
        }

        /// <summary>
        /// Retorna um usuário a partir do ID
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public object GetById(long id)
        {
            var usuarioPesquisado = _context.Usuario.Where(y => y.id == id);

            return usuarioPesquisado.Select((x) => new
            {
                id = x.id,
                nome = x.nome,
                email = x.email
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

