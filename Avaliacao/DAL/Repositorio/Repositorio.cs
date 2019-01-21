using ExpressionBuilder.Generics;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Avaliacao.DAL.TrataErroEntityValidationException;

namespace Avaliacao.DAL.Repositorio
{
    public class Repositorio<T> : IRepositorio<T>, IDisposable where T : class
    {
        private db_EntitiesContext Context;

        public Repositorio()
        {
            Context = new db_EntitiesContext();
        }

        public IQueryable<T> GetTodos()
        {
            return Context.Set<T>();
        }

        public async Task<List<T>> GetTodosAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
                return await Context.Set<T>()
                            .AsNoTracking()
                            .ToListAsync<T>();
            else
                return await Context.Set<T>()
                    .Where(predicate)
                    .AsNoTracking()
                    .ToListAsync<T>();

        }

        public IQueryable<T> OrderBy(IQueryable<T> query, string key, bool ascending = true)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return query;
            }

            var lambda = (dynamic)CreateExpressionOrder<T>(key);

            return ascending
                ? Queryable.OrderBy(query, lambda)
                : Queryable.OrderByDescending(query, lambda);
        }

        //Os métodos abaixo CreateExpressionOrder, 

        public LambdaExpression CreateExpressionOrder<T>(string propertyName)
        {
            var param = Expression.Parameter(typeof(T), "x");

            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return Expression.Lambda(body, param);
        }

        //https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.equal?view=netframework-4.7.2
        //https://stackoverflow.com/questions/15977908/creating-a-linq-expression-where-parameter-equals-object
        //https://www.codeproject.com/Articles/1079028/Build-Lambda-Expressions-Dynamically
        //https://stackoverflow.com/questions/24405020/expression-like-in-c-sharp


        // OS métodos CreateExpressionFind, Like funcionam, porém existem muitos casos para pesquisa eu teria que implementar vários operadores
        // e isso atrasaria a entrega do projeto
        // Para não perder tempo, adicionei um projeto de que faz exatamente o que eu quero
        // Projeto ExpressionBuilder
        /*
        public Expression<Func<T, bool>> CreateExpressionFind<T>(string propertyName, string valueSearch)
        {

            //var param = Expression.Parameter(typeof(T), "x");

            //ParameterExpression pe = Expression.Parameter(typeof(T), propertyName);

            // ***** Where(company => (company.ToLower() == "coho winery" || company.Length > 16)) *****  
            // Create an expression tree that represents the expression 'company.ToLower() == "coho winery"'.  
            //Expression left = Expression.Call(pe, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
            //Expression right = Expression.Constant(valueSearch);

            // var body = Expression.Equal(left, right);


            var parameter = Expression.Parameter(typeof(T), "x");
            var member = Expression.Property(parameter, propertyName); //x.Id
            Expression left = Expression.Call(member, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
            Expression right = Expression.Constant(valueSearch);

            var body = Expression.Equal(left, right);  
            var finalExpression = Expression.Lambda<Func<T, bool>>(body, parameter);

            return Expression.Lambda<Func<T, bool>>(body, parameter); //Expression.Lambda<Func<T, bool>>(body, new ParameterExpression[] { pe });

        }


        public Expression<Func<T, bool>> Like<T>(string propertyName, string queryText)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var getter = Expression.Property(parameter, propertyName);
            //ToString is not supported in Linq-To-Entities, throw an exception if the property is not a string.
            if (getter.Type != typeof(string))
                throw new ArgumentException("A propriedade deve ser uma string!");
            //string.Contains with string parameter.
            var stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsCall = Expression.Call(getter, stringContainsMethod,
                Expression.Constant(queryText, typeof(string)));

            return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
        }
        */


       


        public IQueryable<T> Get(Expression<Func<T, bool>> predicate=null)
        {
            if (predicate != null)
                return Context.Set<T>().Where(predicate);
            else
                return Context.Set<T>();
        }

        public int Contar(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return Context.Set<T>().Where(predicate).Count();
            else
                return Context.Set<T>().Count();
        }

        public T ProcurarKey(params object[] key)
        {
            return Context.Set<T>().Find(key);
        }

        public T Primeiro(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate).FirstOrDefault();
        }
        public void Adicionar(T entity)
        {
            Context.Set<T>().Add(entity);            
        }

        public void AdicionarRange(List<T> entityList)
        {
            Context.Set<T>().AddRange(entityList);
        }

        public void Atualizar(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }
        public void Deletar(Func<T, bool> predicate)
        {
            Context.Set<T>()
           .Where(predicate).ToList()
           .ForEach(del => Context.Set<T>().Remove(del));
        }
        public void Commit()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var newException = new FormattedDbEntityValidationException(e);
                throw newException;
            }
        }
        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}