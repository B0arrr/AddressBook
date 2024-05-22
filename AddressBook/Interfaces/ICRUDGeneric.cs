using System.Linq.Expressions;

namespace AddressBook.Interfaces;

public interface ICRUDGeneric<TModel, TDto>
{
    Task<IEnumerable<TDto>> GetAll(Expression<Func<TModel, bool>>? where = null, params string[] includes);
    Task<TDto?> GetBy(Expression<Func<TModel, bool>> predicate, params string[] includes);
    Task<TDto> Add(TDto dto, params Expression<Func<TModel, object>>[] references);
    Task<TDto> Update(TDto dto, Expression<Func<TModel, bool>>? where = null, params Expression<Func<TModel, object>>[] references);
    Task<TDto?> Delete(int id);
}