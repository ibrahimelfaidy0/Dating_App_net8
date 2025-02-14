using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using api.Entities;
using api.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{

    [Authorize]
    public class UsersController(AppDbcontext context) : BaseApiController
    {

        private AppDbcontext _context = context;







        // [HttpGet]
        // public async Task<ActionResult<string>> GetUsers()
        // {

        //     return Ok("hi2");
        // }


              [AllowAnonymous]
              [HttpGet]

           public  ActionResult<IEnumerable<AppUser>> GET()
           {

              List<AppUser> users=  _context.Users.ToList();


                 return Ok(users);
           }              

        
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> getUserById(int id)
        {

            AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("no user with thiss id");
            }
        }

        

        [HttpPost]
        public async Task<IActionResult>   adduser([FromBody]AppUser newuser)
        {
            _context.Users.Add(newuser);
            _context.SaveChanges();

            return Ok();

        } 



    }
}