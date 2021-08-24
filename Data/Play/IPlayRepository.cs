using System.Threading.Tasks;

namespace dynamodb_test.Data.Play
{
    public interface IPlayRepository<T> where T : class
    {
        Task<T> RetrieveByHashAndRange(string hash, string range);
        Task<T> Save(string player, string game, int score);
        Task<T> Update(T entity, string player, string game, int? score);
        Task<bool> Delete(T entity);
    }
}