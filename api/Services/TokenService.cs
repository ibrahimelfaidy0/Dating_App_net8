using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Entities;
using api.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace api.Services
{
    public class TokenService(IConfiguration config) : ItokenService
    {
        public string CreateToken(AppUser user)
        {

            //start step 4    (for Signing the token (imagin it part 3 of token string (header,payload(claims),Signing)))
            var tokenkey =config["TokenKey"]?? throw new Exception("cant acess token key from appsetting");
            if(tokenkey.Length<64) throw new Exception ("token must be longer than 64");

            var key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);  //this also resposable for 
                                                                                         // part one header as it contain algorthem
                                                                                                
            //end step 4

            //start step 3  
            var claims =new List<Claim>                
            {
                new (ClaimTypes.NameIdentifier,user.UserName)
            };


           
             //start step 2   (for payload the token (imagin it part 2 payload (claims) of token string (header,payload(claims),Signing)))
            var tokenDescription=new SecurityTokenDescriptor  //step 2 this all data needed to add in 1 
            {
               Subject= new ClaimsIdentity(claims), //from step 3
               SigningCredentials=creds,  //from step 4


              Expires=DateTime.UtcNow.AddDays(7)
             
            };



              //start step 1
            var tokenhandler = new JwtSecurityTokenHandler();      //step 1 I MUST create object from 
                                                                       //JwtSecurityTokenHandler and add in it all data
              var token = tokenhandler.CreateToken(tokenDescription);

            return tokenhandler.WriteToken(token);

            
        }
    }
}

//very good expantion from jpt 
// How JWT Tokens are Created (Step-by-Step Guide for Beginners)
// A JWT (JSON Web Token) is like a digital pass that proves who you are. It is used for authentication and secure data exchange between a client (e.g., a web app) and a server (e.g., an API).

// 🛠️ JWT Structure
// A JWT consists of three parts, separated by dots (.):

// css
// Copy
// Edit
// Header.Payload.Signature
// Each part has a specific role:

// Header → Contains metadata like the algorithm used for signing.
// Payload (Claims) → Holds user information (e.g., username, role).
// Signature → Ensures the token has not been tampered with.
// 🔹 Step-by-Step Explanation of Code
// Let's break down how a JWT is created in C#.

// csharp
// Copy
// Edit
// public class TokenService(IConfiguration config) : ItokenService
// {
//     public string CreateToken(AppUser user)
//     {
//         // Step 4: Create Header & Signature
//         var tokenkey = config["TokenKey"] ?? throw new Exception("Can't access token key from appsettings");
//         if (tokenkey.Length < 64) throw new Exception("Token must be longer than 64");

//         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));
//         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

//         // Step 3: Create Payload (Claims)
//         var claims = new List<Claim>
//         {
//             new (ClaimTypes.NameIdentifier, user.UserName)
//         };

//         // Step 2: Create Token Descriptor (Combining Claims & Signature)
//         var tokenDescription = new SecurityTokenDescriptor
//         {
//             Subject = new ClaimsIdentity(claims),
//             SigningCredentials = creds,
//             Expires = DateTime.UtcNow.AddDays(7)
//         };

//         // Step 1: Generate the JWT Token
//         var tokenhandler = new JwtSecurityTokenHandler();
//         var token = tokenhandler.CreateToken(tokenDescription);
//         return tokenhandler.WriteToken(token);
//     }
// }
// 🛠️ Step-by-Step Breakdown
// 🔹 Step 4: Create Header & Signature (Part 3 of JWT)
// Step 4 consists of two parts:

// 📌 Part 1 - Creating the Header
// The header defines how the JWT is structured and which security algorithm is used.

// json
// Copy
// Edit
// {
//   "alg": "HS256",
//   "typ": "JWT"
// }
// 📌 How the Header is Created in Code
// The JwtSecurityTokenHandler automatically generates the header based on our signing algorithm:

// csharp
// Copy
// Edit
// var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));
// var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
// Encoding.UTF8.GetBytes(tokenkey) → Converts the key into a format that can be used for signing.
// SigningCredentials → Specifies the algorithm HS256 (HMAC-SHA256) for securing the token.
// After encoding with Base64Url, the header becomes:

// nginx
// Copy
// Edit
// eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9
// 📌 Part 2 - Creating the Signature
// The signature is used to verify that the token is authentic.

// csharp
// Copy
// Edit
// var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
// The SecurityAlgorithms.HmacSha256Signature ensures that no one can modify the token without knowing the secret key.
// The signature is created by encoding the header + payload and hashing it with the secret key.
// 🔹 Step 3: Create the Payload (Part 2 of JWT)
// The payload contains user information (claims) that the token carries.

// csharp
// Copy
// Edit
// var claims = new List<Claim>
// {
//     new (ClaimTypes.NameIdentifier, user.UserName)
// };
// Claims are key-value pairs inside the token.
// Here, we store the username of the authenticated user.
// 📌 After encoding (Base64Url), the payload looks like this:

// nginx
// Copy
// Edit
// eyJuYW1laWQiOiJKb2huIERvZSIsImV4cCI6MTY3MzcxNzIwMH0
// 🔹 Step 2: Create the Token Descriptor
// The SecurityTokenDescriptor combines:

// User Claims (payload)
// Signing Credentials (signature)
// Expiration (to prevent unlimited token use)
// csharp
// Copy
// Edit
// var tokenDescription = new SecurityTokenDescriptor
// {
//     Subject = new ClaimsIdentity(claims),
//     SigningCredentials = creds,
//     Expires = DateTime.UtcNow.AddDays(7)
// };
// 🔹 Step 1: Generate the JWT (Combining Everything)
// Now, we combine the header, payload, and signature to generate the final JWT string.

// csharp
// Copy
// Edit
// var tokenhandler = new JwtSecurityTokenHandler();
// var token = tokenhandler.CreateToken(tokenDescription);
// return tokenhandler.WriteToken(token);
// This produces a JWT like this:

// nginx
// Copy
// Edit
// eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9
// .eyJuYW1laWQiOiJKb2huIERvZSIsImV4cCI6MTY3MzcxNzIwMH0
// .q9c9XQHQ9-pYbnCoYrVYtkDmCeXkwMsWvLQoHdKuJmE
// 🔹 Summary
// JWT Part	Code Responsible	Description
// Header	JwtSecurityTokenHandler (Auto-generated)	Defines metadata like algorithm (HS256) and token type (JWT).
// Payload	Claims + SecurityTokenDescriptor	Contains user info (e.g., user.UserName) and expiration time.
// Signature	SigningCredentials with SecurityKey	Secures the token using HMAC-SHA256 and a secret key.
// 🔹 Why Use JWTs?
// ✅ Secure → Protects user data with encryption.
// ✅ Fast Authentication → No need to query a database for every request.
// ✅ Widely Used → Works for mobile apps, websites, and APIs.

// 🔹 Want to See How JWTs Work?
// You can decode a JWT using jwt.io. Copy and paste a token to see its header, payload, and signature.

// 🔹 Real-World Example
// When you log in to a website, the server sends back a JWT token.
// Your browser or mobile app stores the token.
// Every time you request data, the token is sent along with it.
// The server checks if the token is valid before allowing access.
// 🎯 Example:
// A bank’s website gives a JWT token when you log in. The token lets you check your balance, but you can’t change someone else’s balance because it knows who you are.

// 🚀 Next Steps
// 🔹 Try adding more claims (e.g., email, role).
// 🔹 Implement token validation in your API.
// 🔹 Explore refresh tokens to extend JWT lifetime securely.

// 🔥 Hope this helps! Let me know if you need more details! 🚀