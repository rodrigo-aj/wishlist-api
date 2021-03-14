using dockerapi.Models;
using dockerapi.Repository;
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
        private readonly UsuarioRespository usuarioRespository;

        //TODO: DEIXAR O ENDPOINT DE /POST Usuario igual ao de WishList.

        public UsuarioController(UsuarioRespository _usuarioRespository, ApiDbContext context)
        {
            usuarioRespository = _usuarioRespository;
        }

        /// <summary>
        /// Retorna todos os usuários cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            return usuarioRespository.GetAll(); ;
        }

        /// <summary>
        /// Retorna um usuário a partir do ID
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Usuario GetById(long id)
        {
            return usuarioRespository.GetById(id);
        }

        /// <summary>
        /// Cadastra um usuário
        /// </summary>
        /// <param name="usuario">Informações do usuário</param>
        /// <returns></returns>
        [HttpPost]
        public Usuario Post([FromBody] Usuario usuario)
        {
            try
            {
                return usuarioRespository.Criar(usuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

