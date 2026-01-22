using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class Clue
    {
        public required string UserId { get; set;}
        public required string Username {  get; set;}
        public required string ClueWord {  get; set;}
        public required DateTime TimeStamp { get; set;} 
   }
}
