using Microsoft.EntityFrameworkCore;
using protecta.laft.api.Models;

namespace protecta.laft.api.Repository.DB {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options) {

        }

        public DbSet<Carga> Cargas { get; set; }
        public DbSet<Registro> Registros { get; set; }
        public DbSet<SbsReport> SbsReports { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Periodo> Periodos { get; set; }

        public DbSet<Correo> Correos { get; set; }

        public DbSet<Alertas> Alerta { get; set; }
        public DbSet<Correo_OC> Correo_OC { get; set; }

        public DbSet<Feriado> Feriados { get; set; }
        public DbSet<UserStatus> UsersStatus { get; set; }
        public DbSet<Menu> Menus { get; set; }        
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Forms> Forms { get; set; }
        public DbSet<Aplicacion> Aplicaciones { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Senial> Seniales { get; set; }
        public DbSet<HistoriaRegistro> HistoriaRegistros { get; set; }
        public DbSet<SenialAplicacion> SenialAplicaciones { get; set; }
        public DbSet<SenialAplicacionProducto> SenialAplicacionProductos { get; set; }
        public DbSet<RegistroAplicacion> RegistroAplicaciones { get; set; }
        public DbSet<RegistroAplicacionProducto> RegistroAplicacionProductos { get; set; }

    }
}
