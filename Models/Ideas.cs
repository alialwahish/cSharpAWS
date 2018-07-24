using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;


namespace cSharpBelt{

    public class Ideas{

        [Key]
        [Required]
        public int IdeasId { get; set; }

      

        [Required]
        public int NumOfLikes {get;set;}

             
       
        [Required]
        [MinLength(2)]
        public string Description {get;set;}
        
        public int UserId {get;set;}


        public int? LikesId {get;set;}

        public List<Likes> Likes {get;set;}

        public Ideas(){

            NumOfLikes =0;
            Likes= new List<Likes>();
        }




    }






    
}