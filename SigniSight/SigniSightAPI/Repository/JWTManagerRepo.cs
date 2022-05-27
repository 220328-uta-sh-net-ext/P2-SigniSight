using Microsoft.IdentityModel.Tokens;
using SigniSightModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SigniSightAPI.Repository
{
  public class JWTManagerRepo : IJWTManagerRepo
  {

        private IConfiguration configuration;

        public JWTManagerRepo(IConfiguration configuration)
        {
          this.configuration = configuration;//instantiating configuration
        }

        public Tokens Authenticate(User use)
        {
              /*if (!UserRecords.Any(a => a.Key == user.UserName && a.Value == user.Password)) //Lambdas Expression
              {
                return null;
              }*/

              var tokenhandler = new JwtSecurityTokenHandler();
              var tokenKey = Encoding.UTF32.GetBytes(configuration["JWT:Key"]);

              var tokenDescriptor = new SecurityTokenDescriptor
              {
                Subject = new System.Security.Claims.ClaimsIdentity(
                             new Claim[]
                             {
                                new Claim(ClaimTypes.Name, User.Username)
                             }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256), //SecurityAlgorithms.HmacSha256 converts token in unreadable format/encryption for security

              };

          var token = tokenhandler.CreateToken(tokenDescriptor);
          return new Tokens { ValidToken = tokenhandler.WriteToken(token) };

        }


  }



}
