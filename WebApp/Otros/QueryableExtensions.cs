
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace WebApp
{
    public static class QueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
        private static readonly FieldInfo QueryModelGeneratorField = typeof(QueryCompiler).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryModelGenerator");
        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);

            string sql = command.CommandText;
            return sql;
        }

        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);

         public static IQueryable<DenunciaModel> UltimasNoRevisadas(this IQueryable<DenunciaModel> denuncias) {
            return denuncias
                .Where(d => d.Estado == EstadoDenuncia.NoRevisada)
                .OrderByDescending(d => d.Creacion)
                .Include(d => d.Hilo)
                .Include(d => d.Usuario)
                .Include(d => d.Comentario)
                .Include(d => d.Comentario.Media)
                .Include(d => d.Comentario.Usuario)
                .Include(d => d.Hilo.Media)
                .Include(d => d.Hilo.Usuario);
        }

         public static IQueryable<BaneoModel> BansActivos(this IQueryable<BaneoModel> baneos, string usuarioId, string ip) {
            var ahora = DateTimeOffset.Now;
            return baneos
                    .OrderByDescending(b => b.Expiracion)
                    .Where(b => b.Visto)
                    .Where(b => b.Expiracion > ahora)
                    .Where(b => b.UsuarioId == usuarioId || b.Ip == ip);
        }

         public static async Task<bool> EstaBaneado(this IQueryable<BaneoModel> baneos, string usuarioId, string ip) {
            return (await baneos
                .BansActivos(usuarioId, ip)
                .FirstOrDefaultAsync()) != null;
        }
         public static IQueryable<ComentarioViewModelMod> AViewModelMod(this IQueryable<ComentarioModel> comentarios) {
            return comentarios.Select(c => new ComentarioViewModelMod
                {
                    HiloId = c.HiloId,
                    UsuarioId = c.UsuarioId,
                    Contenido = c.Contenido,
                    Id = c.Id,
                    Creacion = c.Creacion,
                    Media = c.Media,
                    Estado = c.Estado,
                    Rango = c.Rango,
                    Nombre = c.Nombre,
                });
        }

    }
    
}