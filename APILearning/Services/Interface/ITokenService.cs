using APILearning.Entities;

namespace APILearning.Services.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
