using dockerapi.Models;
using System.Collections.Generic;
using System.Linq;

namespace dockerapi.Repository
{
    public class UsuarioRespository
    {
        private readonly ApiDbContext context;

        public UsuarioRespository(ApiDbContext _context)
        {
            context = _context;
        }

        public IEnumerable<Usuario> GetAll()
        {
            return (from u in context.Usuario

                    select new Usuario
                    {
                        id = u.id,
                        nome = u.nome,
                        email = u.email
                    }).ToList();
        }

        public Usuario GetById(long id)
        {
            return (from u in context.Usuario
                    where u.id == id
                    select u).First();
        }

        public Usuario Save(Usuario usuario)
        {
            context.Usuario.Add(usuario);

            context.SaveChanges();

            return this.GetById(usuario.id);

        }

    }
}