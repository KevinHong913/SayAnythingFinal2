using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SayAnything
{
    public class QuestionSet
    {
        public class Question
        {
            public Question(string qs)
            {
                Qs = qs;
            }

            public string Qs { private set; get; }
        }

        public QuestionSet()
        {
            QSet = new List<Question>
            {
                new Question("What is the worst place to have a first date?"),
                new Question("Who's the most annoying celebrity in show business?"),
                new Question("What's the most important invention of the past century?"),
                new Question("What would be the coolest thing to teach a monkey?"),
                new Question("What would be the best thing to do on the moon?"),
                new Question("If you could have a BIG anything, what would it be?"),
                new Question("What would be the coolest thing to have at a mansion?"),
                new Question("Who’s the most overrated band of all time?"),
                new Question("What’s the worst thing to say to a cop after getting pulled over?")
            };
        }

        public List<Question> QSet { set; get; }
        
    }
}
