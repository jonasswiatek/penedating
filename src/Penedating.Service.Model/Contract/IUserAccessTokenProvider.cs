namespace Penedating.Service.Model.Contract
{
    public interface IUserAccessTokenProvider
    {
        void SetUserAccessToken(UserAccessToken accessToken);
        UserAccessToken GetAccessToken();
        bool TryGetAccessToken(out UserAccessToken accessToken);
        void DestroyAccessToken();
    }
}