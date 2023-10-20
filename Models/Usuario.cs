using System;
using System.Collections.Generic;

namespace UPCH_Prueba.Models;

public partial class Usuario
{
    public int UserId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Detalle> Detalles { get; set; } = new List<Detalle>();
}
