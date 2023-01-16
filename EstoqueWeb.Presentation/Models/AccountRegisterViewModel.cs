﻿using System.ComponentModel.DataAnnotations;

namespace EstoqueWeb.Presentation.Models
{
    public class AccountRegisterViewModel
    {
        [MinLength(6, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, Informe o nome.")]
        public string? Nome { get; set; }

        [EmailAddress(ErrorMessage = "Por favor, informe um endereço de email válido.")]
        [Required(ErrorMessage = "Por favor, Informe o email.")]
        public string? Email { get; set; }

        [MinLength(6, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(20, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, Informe a senha.")]
        public string? Senha { get; set; }

        [Compare("Senha", ErrorMessage = "Senhas não conferem.")]
        [Required(ErrorMessage = "Por favor, confirme a senha.")]
        public string? SenhaConfirmacao { get; set; }
    }
}
