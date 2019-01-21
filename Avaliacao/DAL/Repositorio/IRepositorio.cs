using System;
using System.Linq;
using System.Linq.Expressions;

namespace Avaliacao.DAL.Repositorio
{
    public interface IRepositorio<T> where T : class
    {
        IQueryable<T> GetTodos();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        IQueryable<T> OrderBy(IQueryable<T> query, string key, bool ascending);
        T ProcurarKey(params object[] key);
        T Primeiro(Expression<Func<T, bool>> predicate);
        void Adicionar(T entity);
        void Atualizar(T entity);
        void Deletar(Func<T, bool> predicate);
        void Commit();
        void Dispose();
    }
}