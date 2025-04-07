using Microsoft.EntityFrameworkCore;
using PriceAnalysis.DAL.Models;
using PriceAnalysis.DAL.Models.Reference;
using PriceAnalysis.DAL.Models.Support;

namespace PriceAnalysis.DAL;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }

    public DbSet<TestEntity> testEntity { get; set; }
    public DbSet<SectionEntity> sectionEntity { get; set; }
    public DbSet<DocFilesEntity> docFilesEntity { get; set; }
    public DbSet<SpecificationEntity> specificationEntity { get; set; }
    public DbSet<ChapterForSpecificationEntity> chapterForSpecificationEntity { get; set; }
    public DbSet<UnitOfMeasurementEntity> unitOfMeasurementEntity { get; set; }
    public DbSet<SupplierEntity> supplierEntity { get; set; }
    public DbSet<KPFilesEntity> kpFilesEntity { get; set; }

    public DbSet<SynchPriceEntity> synchPriceEntity { get; set; }
    public DbSet<SynchSpecificationEntity> synchSpecificationEntity { get; set; }
    public DbSet<SynchSpecificationAndKPPrepEntity> synchSpecificationAndKPPrepEntity { get; set; }
}
