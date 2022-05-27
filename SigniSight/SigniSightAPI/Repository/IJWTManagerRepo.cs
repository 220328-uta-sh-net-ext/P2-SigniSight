using SigniSightModel

namespace SigniSightAPI.Repository
{
  public interface IJWTManagerRepo
  {
    Tokens Authenticate(User use);

  }
}
