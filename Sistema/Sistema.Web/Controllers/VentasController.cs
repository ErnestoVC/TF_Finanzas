using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Ventas;
using Sistema.Web.Models.Ventas.Venta;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public VentasController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Ventas/Listar
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<VentaViewModel>> Listar()
        {
            var venta = await _context.Ventas
                .Include(v => v.usuario)
                .Include(v => v.persona)
                .OrderByDescending(v => v.idventa)
                .Take(100)
                .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                num_documento=v.persona.num_documento,
                direccion=v.persona.direccion,
                telefono=v.persona.telefono,
                email=v.persona.email,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                dias = v.dias,
                procen_interes = v.procen_interes,
                intereses = v.intereses,
                total_venta = v.total_venta,
                total_deuda = v.total_deuda,
                tipo_interes = v.tipo_interes,
                estado = v.estado
            });

        }
        /*
        // GET: api/Ventas/VentasMes12
        [Authorize(Roles = "Almacenero,Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<ConsultaViewModel>> VentasMes12()
        {
            var consulta = await _context.Ventas
                .GroupBy(v=>v.fecha_hora.Month)
                .Select(x=> new { etiqueta=x.Key, valor =x.Sum(v=>v.total_deuda)})
                .OrderByDescending(x =>x.etiqueta)
                .Take(12)
                .ToListAsync();

            return consulta.Select(v => new ConsultaViewModel
            {
                etiqueta=v.etiqueta.ToString(),
                valor=v.valor
            });

        }*/


        // GET: api/Ventas/ListarFiltro/texto
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<VentaViewModel>> ListarFiltro([FromRoute] string texto)
        {
            var venta = await _context.Ventas
                .Include(v => v.usuario)
                .Include(v => v.persona)
                .Where(v => v.tipo_interes.Contains(texto))
                .OrderByDescending(v => v.idventa)
                .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                num_documento = v.persona.num_documento,
                direccion = v.persona.direccion,
                telefono = v.persona.telefono,
                email = v.persona.email,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                dias = v.dias,
                procen_interes = v.procen_interes,
                intereses = v.intereses,
                total_venta = v.total_venta,
                total_deuda = v.total_deuda,
                tipo_interes = v.tipo_interes,
                estado = v.estado
            });

        }
        /*
        // GET: api/Ventas/ConsultaFechas/FechaInicio/FechaFin
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{FechaInicio}/{FechaFin}")]
        public async Task<IEnumerable<VentaViewModel>> ConsultaFechas([FromRoute] DateTime FechaInicio,DateTime FechaFin)
        {
            var venta = await _context.Ventas
                .Include(v => v.usuario)
                .Include(v => v.persona)
                .Where(i=>i.fecha_hora>=FechaInicio)
                .Where(i => i.fecha_hora <= FechaFin)
                .OrderByDescending(v => v.idventa)
                .Take(100)
                .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                num_documento = v.persona.num_documento,
                direccion = v.persona.direccion,
                telefono = v.persona.telefono,
                email = v.persona.email,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                dias = v.dias,
                porcen_interes = v.porcen_interes,
                intereses = v.intereses,
                total_venta = v.total_venta,
                total_deuda = v.total_deuda,
                tipo_interes = v.tipo_interes,
                estado = v.estado
            });

        }*/

        // GET: api/Ventas/ListarDetalles
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{idventa}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles([FromRoute] int idventa)
        {
            var detalle = await _context.DetallesVentas
                .Include(a => a.articulo)
                .Where(d => d.idventa == idventa)
                .ToListAsync();

            return detalle.Select(d => new DetalleViewModel
            {
                idarticulo = d.idarticulo,
                articulo = d.articulo.nombre,
                cantidad = d.cantidad,
                precio = d.precio,
                descuento=d.descuento
            });

        }

        // POST: api/Ventas/Crear
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Venta venta = new Venta
            {
                idcliente = model.idcliente,
                idusuario = model.idusuario,
                dias = model.dias,
                procen_interes = model.procen_interes,
                intereses = model.intereses,
                total_venta = model.total_venta,
                total_deuda = model.total_deuda,
                tipo_interes = model.tipo_interes,
                estado = "Aceptado"
            };


            try
            {
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                var id = venta.idventa;
                foreach (var det in model.detalles)
                {
                    DetalleVenta detalle = new DetalleVenta
                    {
                        idventa = id,
                        idarticulo = det.idarticulo,
                        cantidad = det.cantidad,
                        precio = det.precio,
                        descuento=det.descuento
                    };
                    _context.DetallesVentas.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/Ventas/Anular/1
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var venta = await _context.Ventas.FirstOrDefaultAsync(v => v.idventa == id);

            if (venta == null)
            {
                return NotFound();
            }

            venta.estado = "Anulado";

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }


        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.idventa == id);
        }
    }
}