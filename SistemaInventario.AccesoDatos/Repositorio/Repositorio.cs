using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;

        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task agregar(T entidad)
        {
           await dbSet.AddAsync(entidad);
        }

        public async Task<T> Obtener(int id)
        {
           return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null,
         Func<IQueryable<T>, IOrderedQueryable<T>> ordeBY = null, string includePropiedades = null, bool istracking = true)
        
        {
            IQueryable<T> query = dbSet;
            if(filtro != null)
            {
                query = query.Where(filtro);
            }

            if(includePropiedades != null)
            {
                foreach(var incluirPro in includePropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirPro);
                }
            }
            if(ordeBY != null)
            {
                query = ordeBY(query);
            }

            if (istracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }


        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null,
            bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirPro in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirPro);
                }
            }
            

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

       

        public void remover(T entidad)
        {
            dbSet.Remove(entidad);
        }

        public void removerRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad);
        }

       
    }
}
