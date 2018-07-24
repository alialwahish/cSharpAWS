using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;


namespace cSharpBelt
{
    public class Likes{
        [Key]
        public int LikesId {set;get;}

        public int UserId {get;set;}
        public User User {get;set;}

        public int IdeasId {get;set;}
        public Ideas Ideas {get;set;}

        




    }







}