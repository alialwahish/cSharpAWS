
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;


namespace cSharpBelt
{

    public class User
    {


        [Key]
        [Required]
        public int UserId { get; set; }


        [Required(ErrorMessage = "Field Required")]
        [RegularExpression("^[a-zA-Z ]*$"
    , ErrorMessage = "Letters Only!")]
        [MinLength(2)]

        public string Name { get; set; }

        [Required(ErrorMessage = "Field Required")]
        [RegularExpression("^[a-zA-Z ]*$"
    , ErrorMessage = "Letters Only!")]
        [MinLength(2)]
        public string Alias { get; set; }

        [Required(ErrorMessage = "Field Required")]
        [EmailAddress]

        public string Email { get; set; }

        [Required(ErrorMessage = "Field Required")]
        [MinLength(8)]
        
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password Don't match")]
        public string Confirm_Password { get; set; }


       
        public int? IdeasId { get; set; }

        public List<Ideas> Ideas {get;set;}
        
        public int? LikesId {get;set;}
        
        public List<Likes> Likes { get; set; }

        public User (){
            Ideas = new List<Ideas>();
            Likes = new List<Likes>();
        }

    }

}