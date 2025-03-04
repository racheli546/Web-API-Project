using ProjectCore;

    public interface IAuthService
    {
        
        string GenerateJwtToken(User user); 
    }

