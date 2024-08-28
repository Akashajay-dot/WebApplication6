﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication6.Models;

namespace WebApplication6.Controllers.Api
{
    public class adminApproveController : ApiController
    { 
         private readonly AppdbContext _context;

    public adminApproveController()
    {
        _context = new AppdbContext();
    }

    [HttpGet]
    [Route("api/adminApprove")]
    public IHttpActionResult GetUnansweredQuestions()
    {
        var questions = _context.Questions
            .Where(q => q.IsActive )
            .ToList();


        return Ok(questions);
    }
    
    }
}
