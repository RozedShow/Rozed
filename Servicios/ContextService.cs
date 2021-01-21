using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Data;
using SqlKata.Execution;
using SqlKata.Compilers;
using System.Linq;

namespace Servicios
{

    public abstract class ContextService
    {
        protected readonly RChanContext _context;
        protected readonly HashService hashService;
        protected QueryFactory db;


        public ContextService(
            RChanContext context,
            HashService hashService
        )
        {
            this._context = context;
            this.hashService = hashService;
            db = new QueryFactory(context.Database.GetDbConnection(), new PostgresCompiler());
        }
    }
}
