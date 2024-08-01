using Google;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http; 
using WebApplication6.Models;

namespace WebApplication6.Controllers.Api
{
    [RoutePrefix("api/question")]
    public class QuestionController : ApiController
    {
        private AppdbContext db = new AppdbContext();

        [HttpGet]
        [Route("get-daily-question/{userId}")]
        public IHttpActionResult GetDailyQuestion(int userId)
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            // Mark previous day's question as inactive
           MarkPreviousDayQuestionAsInactive(yesterday);

            // Find the question scheduled for today
            var dailyQuestion = db.Questions
                .Where(q => DbFunctions.TruncateTime(q.QuestionDate) == today && q.IsActive)
                .Select(q => new
                {
                    Question = q,
                    AnswerOptions = db.AnswerOptions.Where(a => a.QuestionId == q.QuestionId).ToList(),
                    AnswerKeys = db.AnswerKeys.Where(k => k.QuestionId == q.QuestionId).ToList(),
                    
                })
                .FirstOrDefault();

            if (dailyQuestion == null)
            {
                // Handle case where no question is available for today
                return NotFound(); // Or any appropriate response
            }
            // return Ok(new { HasAnswered = true, DailyQuestion = dailyQuestion });
            var userResponse = db.UserResponse
                .FirstOrDefault(ur => ur.UserId == userId && ur.QuestionId == dailyQuestion.Question.QuestionId);
            if (userResponse != null)
            {
                return NotFound();

            }
            else
            {
            return Ok(dailyQuestion);

            }

        }

        private void MarkPreviousDayQuestionAsInactive(DateTime date)
        {
            var previousDayQuestion = db.Questions
                .Where(q => DbFunctions.TruncateTime(q.QuestionDate) == date && q.IsActive)
                .FirstOrDefault();

            if (previousDayQuestion != null)
            {
                previousDayQuestion.IsActive = false;
                db.Entry(previousDayQuestion).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
