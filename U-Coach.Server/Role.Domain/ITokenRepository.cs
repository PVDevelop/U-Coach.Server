using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public interface ITokenRepository
    {
        void Insert(Token token);

        bool TryGet(TokenId id, out Token token);
    }
}
