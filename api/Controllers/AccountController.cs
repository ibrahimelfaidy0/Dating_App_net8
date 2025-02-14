using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Entities;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace api.Controllers
{
    public class AccountController(AppDbcontext _dbcontext,ItokenService tokenservice):BaseApiController
    {
        

        [HttpPost]
       [Route("register")]
       public async  Task< ActionResult<UserDto>> register(RegisterDto registerDto)

        {
          try
          {
            //   var isolduser= await _dbcontext.Users.FirstOrDefaultAsync(ou=>ou.Name==registerDto.username);
                
               bool userisexist= await userexist(registerDto.username.ToLower());

               if(userisexist)
                   return BadRequest("invalid user name becaues its old user name orridy exist ");
             
                using var hmac=new HMACSHA512();


                  AppUser userforsaving=new AppUser(){
                    
                  UserName =registerDto.username.ToLower(),
                  PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),           
                  PasswordSalt=hmac.Key
                  };



             await   _dbcontext.Users.AddAsync(userforsaving);

               await   _dbcontext.SaveChangesAsync();
                  
                

               string token=tokenservice.CreateToken(userforsaving);

               UserDto userDto=new UserDto()
               {
                UserName=userforsaving.UserName,
                Token=token
               };

                return Ok(userDto);
          }

          catch(Exception ex )
          {
                   return BadRequest(ex.Message);
          }
       }
       

       [HttpPost]
       [Route("Login")]

       public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
       {
          

          AppUser olduser=await _dbcontext.Users.FirstOrDefaultAsync(u=>u.UserName==loginDto.username.ToLower());
            
          HMACSHA512 hmac=new HMACSHA512(olduser.PasswordSalt);

            if(olduser==null)
               return Unauthorized("invalid user name");
            
            byte[] oldpasswordhashed=olduser.PasswordHash;

            byte[] newpasswordhashed=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));



            for (var i = 0; i <oldpasswordhashed.Length; i++)
            {
               if(oldpasswordhashed[i]!=newpasswordhashed[i])
                   return Unauthorized("invalid password");
            }
             
               //   instead of for loop u can

               //           if(!olduser.PasswordHash.SequenceEqual(newpasswordhashed))
               //   return Unauthorized("invalid password");
            
               


        return Ok(new UserDto(){
       UserName=olduser.UserName,
       Token=tokenservice.CreateToken(olduser)

        });
       }


       private async Task< bool> userexist(string appusername)
       {
          AppUser olduser=await _dbcontext.Users.FirstOrDefaultAsync(u=>u.UserName==appusername);
            
            if(olduser!=null)
                   return true;
            else
               return false;
       }
        
    }
}

