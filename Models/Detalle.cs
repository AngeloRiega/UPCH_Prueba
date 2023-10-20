using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UPCH_Prueba.Models;

public partial class Detalle
{
    [Key]
    public int DetalleId { get; set; }

    public int UserId { get; set; }

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Provincia { get; set; } = null!;

    public string CodigoPostal { get; set; } = null!;

    [JsonIgnore]
    public virtual Usuario? User { get; set; }
}