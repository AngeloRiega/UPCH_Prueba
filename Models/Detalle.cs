using System;
using System.Collections.Generic;

namespace UPCH_Prueba.Models;

public partial class Detalle
{
    public int DetalleId { get; set; }

    public int? UserId { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? Ciudad { get; set; }

    public string? Estado { get; set; }

    public string? CodigoPostal { get; set; }

    public virtual Usuario? User { get; set; }
}
