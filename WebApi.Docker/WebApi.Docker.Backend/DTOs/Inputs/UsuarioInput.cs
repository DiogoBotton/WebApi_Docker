using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApi.Docker.Backend.DTOs.Inputs
{
    [DataContract]
    public class UsuarioInput
    {
        [Required]
        [DataMember]
        public string Nome { get; set; }
        [Required]
        [DataMember]
        public string Email { get; set; }
        [Required]
        [DataMember]
        public string Senha { get; set; }
    }
}
